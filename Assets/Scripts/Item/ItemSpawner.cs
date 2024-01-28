using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.PanicBuying.Character
{
    public class ItemSpawner : MonoBehaviour
    {
        private ItemPositioner[] itemSpawners;
        public GameObject item;
        public int count;

        void Start()
        {
            itemSpawners = GetComponentsInChildren<ItemPositioner>();
            Invoke("SpawnItems", 3.0f);
        }

        public void SpawnItems()
        {
            for (int i = 0; i < count; i++)
            {
                int spawnerIdx = Random.Range(0, itemSpawners.Length);
                itemSpawners[spawnerIdx].spawnItem(item);
            }
        }
    }
}
