using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    public class Parallax : MonoBehaviour
    {
        public Transform[] backgroundsList;
        public float speed;
        public float smooth;
        private float calculatedSmooth = 0f;

        Transform[] backgrounds;
        float[] backgroundsDepth;

        [SerializeField] Player player;
        Camera cam;

        void Start()
        {
            cam = Camera.main;

            int length = backgroundsList.Length;
            backgrounds = new Transform[length * 2];
            backgroundsDepth = new float[length * 2];

            for (int i = 0; i < length; i++)
            {
                Transform tr = backgroundsList[i];

                //add to backgrounds and create clone
                backgrounds[i] = tr;
                backgrounds[length + i] = Instantiate(tr.gameObject).transform;

                //move the clone to the right of the original
                backgrounds[length + i].position = new Vector3(tr.position.x + tr.GetComponent<SpriteRenderer>().bounds.size.x, tr.position.y, tr.position.z);

                //get depth and clone depth too
                backgroundsDepth[i] = tr.position.z * -1;
                backgroundsDepth[length + i] = tr.position.z * -1;
            }
        }

        void Update()
        {
            //foreach background
            for (int i = 0; i < backgrounds.Length; i++)
            {
                //get movement (speed on X axis) multiplied to depth
                float parallax = speed * Time.deltaTime * backgroundsDepth[i];
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

                if(IsOutOfScreen(backgrounds[i]))
                {
                    LoopBackground(backgrounds[i]);
                }
            }
        }

        bool IsOutOfScreen(Transform background)
        {
            //get the rightest point of the background
            float rightPosition = background.position.x + background.GetComponent<SpriteRenderer>().bounds.extents.x;

            Vector3 viewportPosition = cam.WorldToViewportPoint(new Vector3(rightPosition, background.position.y, background.position.z));
            if(viewportPosition.x < 0)
            {
                return true;
            }

            return false;
        }

        void LoopBackground(Transform background)
        {
            //sposta il clone a destra
            float newXPosition = background.position.x + background.GetComponent<SpriteRenderer>().bounds.size.x * 2;
        }
    }

}