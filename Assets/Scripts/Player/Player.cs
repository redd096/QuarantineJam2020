using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool useNormalMovement;

    [Header("NormalMovement")]
    public float speed;

    [Header("AccelerationMovement")]
    public float acceleration;
    bool pressedInput;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (useNormalMovement)
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

    #region public API

    public void OnChildTriggerEnter(Transform child, Collider2D other)
    {
        if(child.CompareTag("Left"))
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

    #endregion
}
