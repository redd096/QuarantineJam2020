using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform[] backgrounds;
    public float smooth;

    float[] backgroundsDepth;

    void Start()
    {
        //get depths
        backgroundsDepth = new float[backgrounds.Length];
        for (int i = 0; i < backgroundsDepth.Length; i++)
        {
            backgroundsDepth[i] = backgrounds[i].position.z * -1;
        }
    }

    void Update()
    {
        //foreach background
        for(int i = 0; i < backgrounds.Length; i++)
        {
            //get movement (deltaTime on X axis) multiplied to depth
            float parallax = Time.deltaTime * backgroundsDepth[i];
            float newX = backgrounds[i].position.x - parallax;

            //smooth lerp
            Vector2 newPosition = new Vector2(newX, backgrounds[i].position.y);
            backgrounds[i].position = Vector2.Lerp(backgrounds[i].position, newPosition, smooth * Time.deltaTime);
        }
    }
}
