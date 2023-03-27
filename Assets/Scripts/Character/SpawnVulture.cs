using System.Collections;
using UnityEngine;

namespace Character
{
    public class SpawnVulture : MonoBehaviour
    {
        [SerializeField] private GameObject vulture;
        [SerializeField] private float waitBeforeStart;
        [SerializeField] private float spawnTimer;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Camera cam;
        private bool isSpawning;
        private bool allowSpawn;
        private Vector3 posToSpawn;

        private void Update()
        {
            if (!isSpawning && allowSpawn)
            {
                posToSpawn = cam.ViewportToWorldPoint(offset);
                posToSpawn.z = 0.0f;
                StartCoroutine(StartSpawn());
            }
        }

        private IEnumerator StartSpawn()
        {
            isSpawning = true;
            vulture.transform.position = posToSpawn;
            Instantiate(vulture);
            yield return new WaitForSeconds(spawnTimer);
            isSpawning = false;
        }

        public IEnumerator StartVultureSpawn()
        {
            yield return new WaitForSeconds(waitBeforeStart);
            allowSpawn = true;
        }
    }
}