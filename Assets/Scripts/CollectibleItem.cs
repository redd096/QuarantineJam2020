using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    //[RequireComponent(typeof(Rigidbody2D))]
    public class CollectibleItem : MonoBehaviour
    {
        private SpriteRenderer sprite;
        private BoxCollider2D boxCollider;
        private Rigidbody2D rb;
        private float defaultMass;
        private GameManager gameManager;

        ShoppingItem itemDetail;

        protected internal float reward;

        private void Update()
        {
            if (gameManager)
            {
                rb.gravityScale = gameManager.fallingItemsTimeScale;
            }
        }

        public void LoadFromShoppingItem(ShoppingItem item, GameManager gameManager)
        {
            this.gameManager = gameManager;
            itemDetail = item;
            sprite = GetComponent<SpriteRenderer>();
            boxCollider = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            rb.mass = item.Weight;

            sprite.sprite = item.Sprite;
            defaultMass = item.Weight;
            reward = item.BaseReward;
            boxCollider.size = sprite.size * item.ColliderSizeFactor;
        }

        public ShoppingItem GetItemDetails()
        {
            return itemDetail;
        }
    }
}

