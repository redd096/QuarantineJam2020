using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quaranteam
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ShoppingItemSpawner : MonoBehaviour
    {
        public GameObject collectibleItemPrefab;

        public List<ShoppingItem> itemsToSpawn;

        public float minDelay = 1f;

        public float maxDelay = 10f;

        private BoxCollider2D boundingBox;

        private GameManager gameManager;

        protected internal GameObject previewIconPrefab;

        protected internal float previewIconTime = 2f;

        private void Awake()
        {
            boundingBox = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            OnItemSpawned();
            gameManager = FindObjectOfType<GameManager>();
        }

        private IEnumerator SpawnAfterDelay(float delay, int itemIndex)
        {
            yield return new WaitForSeconds(delay);

            // Get random point along bounding box.
            float randomSpawnPoint = Random.Range(0f, 1f);
            float halfWidth = boundingBox.size.x * 0.5f;
            Vector2 spawnLocation = transform.position;
            spawnLocation.x = spawnLocation.x - halfWidth + randomSpawnPoint * boundingBox.size.x;

            ShoppingItem item = itemsToSpawn[itemIndex];
            GameObject previewIcon = Instantiate(previewIconPrefab, spawnLocation, Quaternion.identity);
            PreviewIconUI iconComponent = previewIcon.GetComponent<PreviewIconUI>();
            iconComponent.itemSprite.sprite = item.Sprite;

            yield return new WaitForSeconds(previewIconTime);
            Destroy(previewIcon);
            GameObject spawnedItem = Instantiate(collectibleItemPrefab, spawnLocation, Quaternion.identity);
            spawnedItem.GetComponent<CollectibleItem>().LoadFromShoppingItem(item, gameManager);

            OnItemSpawned();
        }

        private void OnItemSpawned()
        {
            float newDelay = Random.Range(minDelay, maxDelay);
            int newIndex = Random.Range(0, itemsToSpawn.Count);

            StartCoroutine(SpawnAfterDelay(newDelay, newIndex));
        }
    }
}

