using System;
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
        [Range(0, 100)]
        public float itemOutOfCart;

        [Header("FirstIteration")]
        public float speed;

        [Header("SecondIteration")]
        public float acceleration;
        bool pressedInput;

        Rigidbody2D rb;
        Animator animator;
        Cart cart;
        ChildCollision cartTrigger;

        AudioSource audioSource;
        bool playerMoving;


        public float MovementSpeed { get { return rb.velocity.x; } }

        //for sprites shopping cart
        Transform itemsParent;
        float lastSpriteY = -0.4f;
        int spriteIndex = -1;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            cart = GetComponentInChildren<Cart>();
            audioSource = GetComponent<AudioSource>();
            cartTrigger = cart.GetComponentInChildren<ChildCollision>();

            itemsParent = transform.Find("Cart").Find("Items");
        }

        void Update()
        {
            CheckEndSound();

            if (firstIteration)
                NormalMovement();
            else
                AccelerationMovement();
            TestAnimazione();
        }

        private void TestAnimazione()
        {
            animator.SetFloat("animationSpeed", Mathf.Clamp(Mathf.Abs(rb.velocity.x), 0f, 5f));
        }

        #region private API

        #region movement

        void NormalMovement()
        {
            //move right or left
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, 0);

            TestSound_FirstIteration();
        }

        void AccelerationMovement()
        {
            //push player
            if (Input.GetAxisRaw("Horizontal") > 0.1f)
            {
                //only if input is not just pressed
                if (pressedInput == false)
                {
                    rb.AddForce(Vector2.right * acceleration);
                    pressedInput = true;

                    StartSound();
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < -0.1f)
            {
                //only if input is not just pressed
                if (pressedInput == false)
                {
                    rb.AddForce(Vector2.left * acceleration);
                    pressedInput = true;

                    StartSound();
                }
            }

            //check if release input
            CheckReleasedInput(Input.GetAxisRaw("Horizontal"));
        }

        void CheckReleasedInput(float axis)
        {
            //if axis is 0, then player released input
            if (pressedInput && axis == 0)
                pressedInput = false;
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

        void PickObject_SecondIteration(GameObject itemObject)
        {
            //call function in cart
            ShoppingItem objectDetails = itemObject.GetComponent<CollectibleItem>().GetItemDetails();
            cart.ItemObtained(objectDetails);
            if (ObjectsAddWeight)
            {
                rb.mass = Mathf.Clamp(rb.mass + objectDetails.Weight, 1f, 25f);
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
        }

        void CheckObjectPosition(Transform itemObject)
        {
            //how much can go out of the cart
            float percentage = itemObject.GetComponent<Collider2D>().bounds.size.x / 100 * itemOutOfCart;

            //check if the object is out to the left or to the right of the cart
            if (itemObject.localPosition.x + percentage < -0.9f)
            {
                //out to the left
                SetRiskyObject(itemObject);
            }
            else if (itemObject.localPosition.x - percentage > 0.9f)
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

            foreach (Transform child in itemsParent)
            {
                FallenObject(child);
            }

            LoseGame();
        }

        void DisableEverything()
        {
            //disable this script
            this.enabled = false;

            //disable cart trigger
            cartTrigger.enabled = false;
            
            //set to 0 speed and sound
            rb.velocity = Vector2.zero;
            CheckEndSound();
        }

        void FallenObject(Transform item)
        {
            //remove childCollision and parent just to be sure
            item.GetComponent<ChildCollision>().enabled = false;
            item.parent = null;

            //get or add rigidbody
            Rigidbody2D itemRb = item.GetComponent<Rigidbody2D>();
            if (itemRb == null)
                itemRb = item.gameObject.AddComponent<Rigidbody2D>();

            //push rigidbody in random direction
            itemRb.AddForce(Random.insideUnitCircle * 100);
        }

        void LoseGame()
        {
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

        #endregion


        #region public API

        public void OnChildTriggerEnter(ChildCollision child, Collider2D other)
        {
            //only if collectible item
            if (other.gameObject.layer != LayerMask.NameToLayer("CollectibleItem")) return;

            //only if on the child
            if (other.transform.position.y < child.transform.position.y) return;

            //pick object
            if (firstIteration)
            {
                PickObject(other);
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

        #endregion
    }
}
