using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CollectibleDestroyer : MonoBehaviour
    {
        private BoxCollider2D boxCollider;

        // Start is called before the first frame update
        void Start()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<CollectibleItem>() != null)
            {
                Destroy(other.gameObject);
            }
        }
        
    }
}

