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
            var collectibleItem = other.gameObject.GetComponent<CollectibleItem>();
            if (collectibleItem != null)
            {
                if (collectibleItem.GetItemDetails().LostSound) 
                    AudioSource.PlayClipAtPoint(collectibleItem.GetItemDetails().LostSound, other.gameObject.transform.position);
                Destroy(other.gameObject);
            }
        }
        
    }
}

