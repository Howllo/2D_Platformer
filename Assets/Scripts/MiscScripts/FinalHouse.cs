using Character;
using UnityEngine;

namespace MiscScripts
{
    public class FinalHouse : MonoBehaviour
    {
        [SerializeField] private int gameOverIndex;
    
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                SceneLoader.Instance.gameBeaten = true;
                PlayerCharacter.Instance.GetComponent<SpawnVulture>().enabled = false;
                SceneLoader.Instance.LoadScene(gameOverIndex);
            }
        }
    }
}