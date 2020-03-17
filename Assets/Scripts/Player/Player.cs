using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    public class Player : MonoBehaviour
    {
        public Sprite[] spriteItems;
        public AudioClip inizioCarrello;
        public AudioClip loopCarrello;

        public bool firstIteration;
        public bool ObjectsAddWeight;
        public float multiplierMass = 0.25f;
        [Range(0, 150)]
        public float itemOutOfCartLeft;
        [Range(0, 150)]
        public float itemOutOfCartRight;
        [Range(0f, 1000f)]
        public float spintaEsplosione;
        bool objectsFalling;
        public bool modifierLoseOnRed;
        public KeyCode[] rightKeyCodeList;
        public KeyCode[] leftKeyCodeList;

        [Header("FirstIteration")]
        public float speed;
        protected internal float actualSpeed;

        [Header("SecondIteration")]
        public float acceleration;
        protected internal float actualAcceleration;
        bool pressedInput;
        public GameObject vfx;

        Rigidbody2D rb;
        Animator animator;
        Cart cart;
        Transform leftLimit, rightLimit;

        AudioSource audioSource;
        bool playerMoving;

        private List<GameObject> itemsInCart = new List<GameObject>();
        public float MovementSpeed { get { return rb.velocity.x; } }

        //for sprites shopping cart
        Transform itemsParent;
        float lastSpriteY = -0.4f;
        int spriteIndex = -1;

        ButtonTextUI leftButton, rightButton;
        KeyCode rightKeyCode, leftKeyCode;
        Coroutine changeKeyCodes;

        GameManager gm;

        void Start()
        {
            gm = FindObjectOfType<GameManager>();

            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            cart = GetComponentInChildren<Cart>();
            audioSource = GetComponent<AudioSource>();
            Transform limits = cart.transform.Find("Limits");
            leftLimit = limits.Find("LeftLimit");
            rightLimit = limits.Find("RightLimit");

            actualSpeed = speed;
            actualAcceleration = acceleration;

            itemsParent = cart.transform.Find("Items");
            //add already inside objects - so when everything fall down, they fall too
            Transform items_alreadyInside = itemsParent.Find("AlreadyInside");
            foreach(Transform child in items_alreadyInside)
            {
                itemsInCart.Add(child.gameObject);
            }

            leftButton = GameObject.Find("MoveLeftTutorial").GetComponent<ButtonTextUI>();
            rightButton = GameObject.Find("MoveRightTutorial").GetComponent<ButtonTextUI>();

            StartKeyCodeList();            
        }

        void Update()
        {
            CheckEndSound();

            if (firstIteration)
            {
                NormalMovement();
            }
            else
            {
                AccelerationMovement();
            }

            SetAnimationSpeed();
            SetVFX();

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.K))
            {
                gm.PauseGame();
            }
        }

        private void SetAnimationSpeed()
        {
            animator.SetFloat("animationSpeed", Mathf.Clamp(Mathf.Abs(rb.velocity.x), 0f, 5f));
        }

        private void SetVFX()
        {
            if(Mathf.Abs(rb.velocity.x) > 4f)
            {
                vfx.SetActive(true);
            }
            else
            {
                vfx.SetActive(false);
            }
        }

        #region private API

        #region movement

        void NormalMovement()
        {
            //move right or left
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * actualSpeed, 0);
            TestSound_FirstIteration();
        }

        void AccelerationMovement()
        {
            //touch
            float touchPosition = 0;
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touchPosition = Input.GetTouch(0).position.x;
            }

            //push player
            if (Input.GetKeyDown(rightKeyCode) || touchPosition > Screen.width /2 + 10)
            {
                rb.AddForce(Vector2.right * actualAcceleration);

                StartSound();
            }
            else if (Input.GetKeyDown(leftKeyCode) || touchPosition < Screen.width / 2 - 10)
            {
                rb.AddForce(Vector2.left * actualAcceleration);

                StartSound();
            }
        }

        #endregion

        #region pick object

        #region first iteration

        void PickObject(Collider2D other)
        {
            //add sprite in shopping cart
            AddSprite();

            //call function in cart
            cart.ItemObtained(other.gameObject.GetComponent<CollectibleItem>().GetItemDetails());

            //destroy picked object
            Destroy(other.gameObject);
        }

        void AddSprite()
        {
            //get sprite index and position
            spriteIndex++;
            Vector2 spriteLocalPosition = GetLocalPosition();

            //instantiate and add sprite
            GameObject spriteInstantiated = new GameObject("Item " + spriteIndex);
            spriteInstantiated.AddComponent<SpriteRenderer>().sprite = spriteItems[spriteIndex];

            //set Transform
            spriteInstantiated.transform.parent = itemsParent;
            spriteInstantiated.transform.localPosition = spriteLocalPosition;
            spriteInstantiated.transform.localScale = new Vector2(0.4f, 0.4f);

            //reset index when reached last
            if (spriteIndex >= spriteItems.Length - 1)
                spriteIndex = -1;
        }

        Vector2 GetLocalPosition()
        {
            lastSpriteY += 0.3f;

            if (spriteIndex == 0)
            {
                lastSpriteY -= 0.3f;
                return new Vector2(-0.2f, lastSpriteY);
            }
            else if (spriteIndex == 1)
            {
                return new Vector2(-0.1f, lastSpriteY);
            }
            else if (spriteIndex == 2)
            {
                lastSpriteY -= 0.1f;
                return new Vector2(0.3f, lastSpriteY);
            }
            else if (spriteIndex == 3)
            {
                return new Vector2(-0.1f, lastSpriteY);
            }
            else if (spriteIndex == 4)
            {
                return new Vector2(0.1f, lastSpriteY);
            }
            else
            {
                return new Vector2(0.1f, lastSpriteY);
            }
        }

        #endregion

        #region second iteration

        void PickModifier(GameObject itemObject)
        {
            //add to cart, so it add the modifier
            ShoppingItem objectDetails = itemObject.GetComponent<CollectibleItem>().GetItemDetails();
            cart.ItemObtained(objectDetails);

            //destroy modifier
            Destroy(itemObject);
        }

        void PickObject_SecondIteration(GameObject itemObject)
        {
            //call function in cart
            ShoppingItem objectDetails = itemObject.GetComponent<CollectibleItem>().GetItemDetails();
            cart.ItemObtained(objectDetails);
            if (ObjectsAddWeight)
            {
                float addedMass = objectDetails.Weight * multiplierMass;
                rb.mass = Mathf.Clamp(rb.mass + addedMass, 1f, 25f);
            }

            //stick on cart
            StickObject(itemObject);

            //check if out of the cart
            CheckObjectPosition(itemObject.transform);
        }

        void StickObject(GameObject itemObject)
        {
            //remove script and rigidbody
            Destroy(itemObject.GetComponent<CollectibleItem>());
            Destroy(itemObject.GetComponent<Rigidbody2D>());

            //reset layer so can collide with others collectible item
            itemObject.layer = LayerMask.NameToLayer("Default");

            //set parent and add child collision
            itemObject.transform.parent = itemsParent;
            itemObject.AddComponent<ChildCollision>();
            itemsInCart.Add(itemObject);
        }

        void CheckObjectPosition(Transform itemObject)
        {
            //how much can go out of the cart
            float sizeX = itemObject.GetComponent<Collider2D>().bounds.size.x;
            float half = sizeX / 2;
            float left = sizeX / 100 * itemOutOfCartLeft;
            float right = sizeX / 100 * itemOutOfCartRight;

            //centralPosition - half = leftPoint of the object -> add right to be safe

            //check if the object is out to the left or to the right of the cart
            if (itemObject.position.x - half + left < leftLimit.position.x)
            {
                //out to the left
                SetRiskyObject(itemObject);
            }
            else if (itemObject.position.x + half - right > rightLimit.position.x)
            {
                //out to the right
                SetRiskyObject(itemObject);
            }
        }

        void SetRiskyObject(Transform itemObject)
        {
            //color red and set risky
            itemObject.GetComponent<SpriteRenderer>().color = Color.red;
            itemObject.GetComponent<ChildCollision>().risky = true;
        }

        #endregion

        #endregion

        #region everything fall

        void EverythingFall()
        {
            DisableEverything();

            foreach (GameObject item in itemsInCart)
            {
                FallenObject(item);
            }
            
            StartCoroutine(LoseGame());
        }

        void DisableEverything()
        {
            //disable this script
            this.enabled = false;

            //disable possibility to pick objects
            objectsFalling = true;

            //disable cart trigger
            //Destroy(cartTrigger.GetComponent<Collider2D>());

            //set to 0 speed and relatives
            rb.velocity = Vector2.zero;
            CheckEndSound();
            SetAnimationSpeed();

            //reset list in the cart
            cart.ClearChecklist();
        }

        void FallenObject(GameObject item)
        {
            //remove childCollision and parent just to be sure
            //Destroy(item.GetComponent<Collider2D>());
            //item.transform.parent = null;

            //get or add rigidbody
            Rigidbody2D itemRb = item.GetComponent<Rigidbody2D>();
            if (itemRb == null)
                itemRb = item.AddComponent<Rigidbody2D>();

            //push rigidbody in random direction
            itemRb.AddForce(Random.insideUnitCircle * spintaEsplosione);
        }

        IEnumerator LoseGame()
        {
            yield return new WaitForSeconds(2f);

            //end game
            LevelTimer levelTimer = FindObjectOfType<LevelTimer>();
            if (levelTimer)
                levelTimer.endGame = true;
        }

        #endregion

        #region sound

        void TestSound_FirstIteration()
        {
            //if not moving and press to start movement
            if (playerMoving == false && (Input.GetAxis("Horizontal") > 0.1f || Input.GetAxis("Horizontal") < -0.1f))
            {
                playerMoving = true;
                StartSound();
            }
        }

        void StartSound()
        {
            audioSource.clip = inizioCarrello;
            audioSource.Play();
            Invoke("LoopSound", inizioCarrello.length);
        }

        void LoopSound()
        {
            audioSource.clip = loopCarrello;
            audioSource.loop = true;
            audioSource.Play();
        }

        void CheckEndSound()
        {
            if (rb.velocity.x > -0.15f && rb.velocity.x < 0.15f)
            {
                audioSource.loop = false;
                audioSource.Stop();
                playerMoving = false;
            }
        }

        #endregion

        #region randomize keys

        void StartKeyCodeList()
        {
            //remove duplicates from lists
            rightKeyCodeList = RemoveDuplicateArray(rightKeyCodeList);
            leftKeyCodeList = RemoveDuplicateArray(leftKeyCodeList);

            //set defaults
            DefaultKeyCodes();
        }

        T[] RemoveDuplicateArray<T>(T[] array)
        {
            //use Set that has only unique elements
            HashSet<T> set = new HashSet<T>(array);

            //copy into an array and return it
            T[] result = new T[set.Count];
            set.CopyTo(result);

            return result;
        }        

        IEnumerator ChangeKeyCodes()
        {
            //change until left is different from right
            while (leftKeyCode == rightKeyCode)
            {
                //if no elements in left list, randomize right key code (if no elements in right list, we have A for left and D for right, so no problem)
                if (leftKeyCodeList.Length < 1)
                {
                    //if right list has only one element, than it is A. Than we force left to be D instead of A (so we invert default keys)
                    if (rightKeyCodeList.Length == 1)
                    {
                        leftKeyCode = KeyCode.D;
                        break;
                    }

                    //randomize right
                    rightKeyCode = rightKeyCodeList[Random.Range(0, rightKeyCodeList.Length)];
                }
                else
                {
                    //if right list has no elements and left list has only one element, than it is D. Then we force right to be A instead of D (so we invert)
                    if(rightKeyCodeList.Length < 1 && leftKeyCodeList.Length == 1)
                    {
                        rightKeyCode = KeyCode.A;
                        break;
                    }

                    //otherwise randomize left key code
                    leftKeyCode = leftKeyCodeList[Random.Range(0, leftKeyCodeList.Length)];
                }

                yield return null;
            }

            //set finals
            changeKeyCodes = null;

            SetFinalKeys();
        }

        void SetFinalKeys()
        {
            leftButton.SetButton(leftKeyCode);
            rightButton.SetButton(rightKeyCode);
        }

        #endregion

        #endregion


        #region public API

        public void OnChildTriggerEnter(ChildCollision child, Collider2D other)
        {
            //only if collectible item
            if (other.gameObject.layer != LayerMask.NameToLayer("CollectibleItem")) return;

            //only if on the child
            if (other.transform.position.y < child.transform.position.y) return;

            //only if not falling objects
            if (objectsFalling) return;

            //pick object
            if (firstIteration)
            {
                PickObject(other);
            }
            else
            {
                //is a modifier or normal object?
                bool isModifier = other.gameObject.GetComponent<CollectibleItem>().GetItemDetails().Modifiers.Length > 0;

                if (isModifier)
                {
                    //if could lose on risky child, then everything fall down - else pick modifier
                    if (modifierLoseOnRed && child.risky)
                        EverythingFall();
                    else
                        PickModifier(other.gameObject);
                }
                else
                {
                    //if child was risky, then everything fall down - else pick object
                    if (child.risky)
                        EverythingFall();
                    else
                        PickObject_SecondIteration(other.gameObject);
                }
            }
        }

        public void RandomizeKeyCodes()
        {
            //check if right and left have some keycodes in list to randomize, otherwise set default D and A
            if (rightKeyCodeList.Length < 1)
                rightKeyCode = KeyCode.D;
            else
                rightKeyCode = rightKeyCodeList[Random.Range(0, rightKeyCodeList.Length)];

            if (leftKeyCodeList.Length < 1)
                leftKeyCode = KeyCode.A;
            else
                leftKeyCode = leftKeyCodeList[Random.Range(0, leftKeyCodeList.Length)];

            //stop if is running
            if (changeKeyCodes != null)
                StopCoroutine(changeKeyCodes);

            //be sure left is not the same of right
            changeKeyCodes = StartCoroutine(ChangeKeyCodes());
        }

        public void DefaultKeyCodes()
        {
            leftKeyCode = KeyCode.A;
            rightKeyCode = KeyCode.D;

            SetFinalKeys();
        }

        #endregion
    }
}
