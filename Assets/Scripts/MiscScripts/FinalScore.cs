using SaveSystem;
using TMPro;
using UnityEngine;

namespace MiscScripts
{
    public class FinalScore : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Awake()
        {
            DataPersistenceManager.Instance.NewLevel();
            
            if (!SceneLoader.Instance.gameBeaten)
            {
                GetComponent<TextMeshProUGUI>().SetText($"GAME OVER!\n The final score was: {SceneLoader.Instance.finalScore}.");
            }
            GetComponent<TextMeshProUGUI>().SetText($"CONGRATULATION YOU WON!\n The final score was: {SceneLoader.Instance.finalScore}.");
        }
    }
}
