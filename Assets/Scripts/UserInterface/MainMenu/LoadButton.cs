using Character;
using MiscScripts;
using SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.MainMenu
{
    public class LoadButton : MonoBehaviour
    {
        private Button loadButton;
        
        void Start()
        {
            loadButton = GetComponent<Button>();
            loadButton.onClick.AddListener(OnClickLoad);
            if (DataPersistenceManager.Instance.newGame)
                loadButton.gameObject.SetActive(false);
        }

        private void OnClickLoad()
        {
            ImportantCharacterReferenceHolder ICRH =
                PlayerCharacter.Instance.gameObject.GetComponent<ImportantCharacterReferenceHolder>();
            ICRH.saveGameButton.gameObject.SetActive(true);
            ICRH.quitButton.gameObject.SetActive(true);
            DataPersistenceManager.Instance.LoadGame();
            SceneLoader.Instance.LoadScene(DataPersistenceManager.Instance.GetData().level);
        }
    }
}
