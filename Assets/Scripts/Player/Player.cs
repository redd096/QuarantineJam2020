using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    public class Player : MonoBehaviour
    {
        public bool firstIteration;

        [Header("FirstIteration")]
        public float speed;

        [Header("SecondIteration")]
        public float acceleration;
        bool pressedInput;

        Rigidbody2D rb;
        Cart cart;

        //for shopping cart
        GameObject[] shoppingItems;
        int pickedItems = -1;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            cart = GetComponentInChildren<Cart>();

            GetShoppingItems();
        }

        void Update()
        {
            if (firstIteration)
                NormalMovement();
            else
                AccelerationMovement();
        }

        #region private API

        void GetShoppingItems()
        {
            //get items in shopping cart
            Transform shoppingCart = transform.Find("Cart");
            Transform items = shoppingCart.Find("Items");

            shoppingItems = new GameObject[items.childCount];
            for (int i = 0; i < shoppingItems.Length; i++)
            {
                //get sprite and set it false
                shoppingItems[i] = items.GetChild(i).gameObject;
                shoppingItems[i].SetActive(false);
            }
        }

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

        void PickObject(Collider2D other)
        {
            //only if can pick other objects
            if (pickedItems >= shoppingItems.Length - 1) return;

            //active sprite in shopping cart
            pickedItems++;
            shoppingItems[pickedItems].SetActive(true);

            cart.ItemObtained(other.gameObject.GetComponent<CollectibleItem>().GetItemDetails());
            //ShoppingCart.ItemObtained(other.gameObject);

            //destroy picked object
            Destroy(other.gameObject);
        }

        void OnLeftCart(Collider2D other)
        {

        }

        void OnCenterCart(Collider2D other)
        {

        }

        void OnRightCart(Collider2D other)
        {

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
                if (child.CompareTag("OnlyOneTrigger"))
                    PickObject(other);
            }
            else
            {
                if (child.CompareTag("Left"))
                {
                    OnLeftCart(other);
                }
                else if (child.CompareTag("Center"))
                {
                    OnCenterCart(other);
                }
                else if (child.CompareTag("Right"))
                {
                    OnRightCart(other);
                }
            }
        }

        #endregion
    } 
}
