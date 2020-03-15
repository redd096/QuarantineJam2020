using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    public class Player : MonoBehaviour
    {
        public Sprite[] spriteItems;

        public bool firstIteration;

        [Header("FirstIteration")]
        public float speed;

        [Header("SecondIteration")]
        public float acceleration;
        bool pressedInput;

        Rigidbody2D rb;
        Cart cart;

        //for sprites shopping cart
        Transform spritesParent;
        float lastSpriteY = -0.7f;
        int spriteIndex = -1;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            cart = GetComponentInChildren<Cart>();

            spritesParent = transform.Find("Cart").Find("Items");
        }

        void Update()
        {
            if (firstIteration)
                NormalMovement();
            else
                AccelerationMovement();
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
                return new Vector2(1.7f, lastSpriteY);
            }
            else if (spriteIndex == 1)
            {
                return new Vector2(1.8f, lastSpriteY);
            }
            else if (spriteIndex == 2)
            {
                lastSpriteY -= 0.1f;
                return new Vector2(2.2f, lastSpriteY);
            }
            else if (spriteIndex == 3)
            {
                return new Vector2(1.8f, lastSpriteY);
            }
            else if (spriteIndex == 4)
            {
                return new Vector2(2, lastSpriteY);
            }
            else
            {
                return new Vector2(2, lastSpriteY);
            }
        }

        #endregion

        #region second iteration

        void PickObject_SecondIteration(GameObject itemObject)
        {
            //stick on cart
            StickObject(itemObject);

            //call function in cart
            cart.ItemObtained(itemObject.GetComponent<CollectibleItem>().GetItemDetails());

            //check if out of the cart
            CheckObjectPosition(itemObject.transform);
        } 
        
        void StickObject(GameObject itemObject)
        {
            //remove rigidbody and reset layer (so it can collide with other collectibleItems)
            Destroy(itemObject.GetComponent<Rigidbody>());
            itemObject.layer = LayerMask.NameToLayer("Default");

            //set parent and add childCollision
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
