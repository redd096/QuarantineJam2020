using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    public class ShoppingListUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject achievementsPanel;

        [SerializeField]
        private GameObject shoppingListIconsPanel;

        [SerializeField]
        private GameObject achievementTextPrefab;

        [SerializeField]
        private GameObject itemIconPrefab;

        [SerializeField]
        private Cart cart;

        private List<ItemIconUI> iconsList;

        private void Awake()
        {
            iconsList = new List<ItemIconUI>();
        }

        public void AddRequiredShoppingItem(ShoppingItem shoppingItem)
        {
            GameObject newIcon = Instantiate(itemIconPrefab, shoppingListIconsPanel.transform);
            ItemIconUI iconUI = newIcon.GetComponent<ItemIconUI>();
            iconUI.BindToShoppingItem(cart, shoppingItem);
            iconsList.Add(iconUI);
        }

        public void ClearShoppingListIcons()
        {
            foreach (ItemIconUI icon in iconsList)
            {
                Destroy(icon.gameObject);
            }
        }
    }
}

