using Character;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiscScripts
{
    public class SpawnPoint : MonoBehaviour
    {
        void Start()
        {
            if(SceneManager.GetActiveScene().buildIndex == 3)
                PlayerCharacter.Instance.StartCharacter();
            else
                PlayerCharacter.Instance.StartCharacterLevel2();
            
            if (DataPersistenceManager.Instance.newGame || SceneLoader.Instance.newLevel)
                PlayerCharacter.Instance.transform.position = transform.position;
            
            if (SceneManager.GetActiveScene().buildIndex == 4) return;
                DataPersistenceManager.Instance.RevistAllObjectTypeData();
        }
    
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Vector3 center = transform.position;
            Gizmos.DrawWireSphere(center, 0.9f);
        }
    }
}
