using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Quaranteam
{
    public class Border : MonoBehaviour
    {
        Camera cam;
        List<Transform> borders = new List<Transform>();

        float lastScreenWidth, lastScreenHeight;

        private void Start()
        {
            cam = Camera.main;

            CreateBorders();
        }

        private void Update()
        {
            CheckChangedResolution();
        }

        #region start

        void CreateBorders()
        {
            InstantiateBorder("BorderRight");    
            InstantiateBorder("BorderLeft");

            OnScreenSizeChanged();
        }

        void InstantiateBorder(string name)
        {
            //instantiate, move and set size
            GameObject border = new GameObject(name);

            //add collider and parent
            border.AddComponent<BoxCollider2D>();
            border.transform.parent = transform;

            //add to list
            borders.Add(border.transform);
        }

        #endregion

        void CheckChangedResolution()
        {
            if (lastScreenWidth != Screen.width || lastScreenHeight != Screen.height)
            {
                lastScreenWidth = Screen.width;
                lastScreenHeight = Screen.height;
                OnScreenSizeChanged();
            }
        }

        Vector3 GetBorderScale(float depth)
        {
            //get size for the wall from the screen width and height
            Vector3 down = cam.ViewportToWorldPoint(new Vector3(0, 0, depth));
            Vector3 top = cam.ViewportToWorldPoint(new Vector3(1, 1, depth));

            Vector3 size = top - down;

            return new Vector3(size.x, size.y, 1);
        }

        void SetBorder(Transform border, Vector3 viewportPosition, Vector3 size)
        {
            border.position = cam.ViewportToWorldPoint(viewportPosition);
            border.localScale = size;
        }

        void OnScreenSizeChanged()
        {
            float depthScreen = cam.WorldToViewportPoint(transform.position).z;
            Vector3 size = GetBorderScale(depthScreen);

            SetBorder(borders[0], new Vector3(1.5f, 0.5f, depthScreen), size);
            SetBorder(borders[1], new Vector3(-0.5f, 0.5f, depthScreen), size);
        }
    }
}
