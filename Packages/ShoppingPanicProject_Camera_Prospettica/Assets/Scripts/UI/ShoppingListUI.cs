using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField]
        private Text scoreText;

        private List<ItemIconUI> iconsList;

        private GameManager gameManager;

        public void OnScoreUpdated(int newScore)
        {
            scoreText.text = "Score: " + newScore.ToString();
        }

        private void Awake()
        {
            iconsList = new List<ItemIconUI>();
            gameManager = FindObjectOfType<GameManager>();
        }

        private void Start()
        {
            gameManager.onCurrentScoreUpdate += OnScoreUpdated;
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

