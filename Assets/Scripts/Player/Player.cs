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

        [Header("FirstIteration")]
        public float speed;

        [Header("SecondIteration")]
        public float acceleration;
        bool pressedInput;

        Rigidbody2D rb;
        Cart cart;
        AudioSource audioSource;
        bool test;

        //for sprites shopping cart
        Transform spritesParent;
        float lastSpriteY = -0.4f;
        int spriteIndex = -1;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            cart = GetComponentInChildren<Cart>();
            audioSource = GetComponent<AudioSource>();

            spritesParent = transform.Find("Cart").Find("Items");
        }

        void Update()
        {
            if (firstIteration)
                NormalMovement();
            else
                AccelerationMovement();

            TestSuoni();
        }

        #region private API

        #region movement

        void NormalMovement()
        {
            //move right or left
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, 0);
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
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < -0.1f)
            {
                //only if input is not just pressed
                if (pressedInput == false)
                {
                    rb.AddForce(Vector2.left * acceleration);
                    pressedInput = true;
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
            spriteInstantiated.transform.parent = spritesParent;
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
            cart.ItemObtained(itemObject.GetComponent<CollectibleItem>().GetItemDetails());

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

            //set parent
            itemObject.transform.parent = spritesParent;
            itemObject.AddComponent<ChildCollision>();
        }

        void CheckObjectPosition(Transform itemObject)
        {
            //check if the object is out to the left or to the right
            if(itemObject.localPosition.x < -0.9f)
            {
                //out left
            }
            else if(itemObject.localPosition.x > 0.9f)
            {
                //out right
            }
        }

        #endregion

        #endregion

        #region sound

        void TestSuoni()
        {
            float speed = rb.velocity.x;

            if (rb.velocity.x == 0)
            {
                audioSource.loop = false;
                audioSource.Stop();
                test = false;
            }

            if (test == false && (Input.GetAxis("Horizontal") > 0.1f || Input.GetAxis("Horizontal") < -0.1f))
            {
                test = true;
                audioSource.clip = inizioCarrello;
                audioSource.Play();
                Invoke("LoopSuono", inizioCarrello.length);
            }
        }

        void LoopSuono()
        {
            audioSource.clip = loopCarrello;
            audioSource.loop = true;
            audioSource.Play();
        }

        #endregion

        #endregion


        #region public API

        public void OnChildTriggerEnter(Transform child, Collider2D other)
        {
            //only if collectible item
            if (other.gameObject.layer != LayerMask.NameToLayer("CollectibleItem")) return;

            //pick object
            if (firstIteration)
            {
                PickObject(other);
            }
            else
            {
                PickObject_SecondIteration(other.gameObject);
            }
        }
        
        #endregion
    } 
}
