using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Quaranteam
{
    [RequireComponent(typeof(Text))]
    public class ButtonTextUI : MonoBehaviour
    {
        /// <summary>
        /// The text that displays the button.
        /// </summary>
        private Text buttonText;

        public KeyCode boundKey;

        public Color keyPressedColor;

        // Start is called before the first frame update
        void Start()
        {
            buttonText = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(boundKey))
            {
                buttonText.color = keyPressedColor;
            }
            else
            {
                buttonText.color = Color.white;
            }
        }
    }
}

