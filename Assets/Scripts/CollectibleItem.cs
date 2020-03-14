using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class CollectibleItem : MonoBehaviour
    {
        private SpriteRenderer sprite;
        private BoxCollider2D boxCollider;
        private Rigidbody2D rb;

        protected internal float reward;

        public void LoadFromShoppingItem(ShoppingItem item)
        {
            sprite = GetComponent<SpriteRenderer>();
            boxCollider = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();

            sprite.sprite = item.Sprite;
            rb.mass = item.Weight;
            reward = item.BaseReward;
            boxCollider.size = sprite.size;
        }
    }
}

