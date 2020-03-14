using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Quaranteam
{
    public class ItemIconUI : MonoBehaviour
    {
        /// <summary>
        /// The item icon without the checkbox.
        /// </summary>
        private Image image;

        /// <summary>
        /// The checkbox icon.
        /// </summary>
        [SerializeField]
        private Image checkIcon;

        /// <summary>
        /// The item this icons refers to.
        /// </summary>
        ShoppingItem boundItem;

        private void Awake()
        {
            image = GetComponent<Image>();
            checkIcon.gameObject.SetActive(false);
        }

        public void BindToShoppingItem(Cart cart, ShoppingItem item)
        {
            boundItem = item;
            cart.onItemCollected += OnItemCollected;
            image.sprite = item.Sprite;
        }

        private void OnItemCollected(ShoppingItem collectedItem)
        {
            if (collectedItem == boundItem)
            {
                checkIcon.gameObject.SetActive(true);
            }
        }
    }
}

