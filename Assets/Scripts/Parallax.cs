using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    public class Parallax : MonoBehaviour
    {
        public Transform[] backgrounds;
        public float smooth;
        private float calculatedSmooth = 0f;

        float[] backgroundsDepth;

        [SerializeField] Player player;

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
            for (int i = 0; i < backgrounds.Length; i++)
            {
                //get movement (deltaTime on X axis) multiplied to depth
                float parallax = Time.deltaTime * backgroundsDepth[i];
                float newX = backgrounds[i].position.x + parallax;

                //smooth lerp
                Vector2 newPosition = new Vector2(newX, backgrounds[i].position.y);
                if(player.isActiveAndEnabled)
                {
                    calculatedSmooth = Mathf.Lerp(0.2f, 1f, Mathf.InverseLerp(-5f, 5f, player.MovementSpeed)); 
                    //Debug.Log("Player Movement: " + player.MovementSpeed);
                    //Debug.Log("Calculated Smooth : " + calculatedSmooth * smooth);
                }
                
                //backgrounds[i].position = Vector2.Lerp(backgrounds[i].position, newPosition, smooth * Time.deltaTime);
                backgrounds[i].position = Vector2.Lerp(backgrounds[i].position, newPosition, calculatedSmooth * Time.deltaTime);
            }
        }
    }

}