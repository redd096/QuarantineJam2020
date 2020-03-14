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
    Rigidbody2D rb;

    //to keep inside camera
    Camera cam;
    Transform backLimit, frontLimit;

    void Start()
    {
        cam = Camera.main;

        Transform limits = transform.Find("Limits");
        backLimit = limits.Find("BackLimit");
        frontLimit = limits.Find("FrontLimit");

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

    bool IsOutScreen()
    {
        //if out of the screen
        if (cam.WorldToViewportPoint(backLimit.position).x < 0 || cam.WorldToViewportPoint(frontLimit.position).x > 1)
        {
            return true;
        }

        return false;
    }

    void NormalMovement()
    {
        Vector2 previousPosition = transform.position;

        //add speed
        float newX = transform.position.x + Input.GetAxis("Horizontal") * speed;
        transform.position = new Vector2(newX, transform.position.y);

        //keep inside camera
        if(IsOutScreen())
        {
            transform.position = previousPosition;
        }
    }

    void AccelerationMovement()
    {
        Vector2 previousPosition = transform.position;

        //push player
        if (Input.GetAxisRaw("Horizontal") > 0.1f)
            rb.AddForce(Vector2.right * acceleration);
        else if (Input.GetAxisRaw("Horizontal") < -0.1f)
            rb.AddForce(Vector2.left * acceleration);

        //keep inside camera
        if (IsOutScreen())
        {
            rb.velocity = Vector2.zero;
            transform.position = previousPosition;
        }
    }

    #endregion

    void OnLeftCart()
    {

    }

    void OnCenterCart()
    {

    }

    void OnRightCart()
    {

    }

    #endregion

    #region public API

    public void OnChildTriggerEnter(Transform child, Collider other)
    {
        if(child.CompareTag("Left"))
        {
            OnLeftCart();
        }
        else if (child.CompareTag("Center"))
        {
            OnCenterCart();
        }
        else if (child.CompareTag("Right"))
        {
            OnRightCart();
        }
    }

    #endregion
}
