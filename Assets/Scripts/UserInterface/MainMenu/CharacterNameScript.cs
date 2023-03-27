using Character;
using MiscScripts;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UserInterface.MainMenu
{
    public class CharacterNameScript : MonoBehaviour
    {
        [SerializeField] private Button confirm;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private int sceneIndex;
        
        // Start is called before the first frame update
        void Start()
        {
            nameText.SetText("");
            confirm.onClick.AddListener(SetPlayerName);
            PlayerCharacter.Instance.playerController.state = PlayerController.ECharacterState.ECharStunned;
        }

        private void SetPlayerName()
        {
            PlayerCharacter.Instance.SetPlayerName(nameText.text);
            PlayerCharacter.Instance.playerController.state = PlayerController.ECharacterState.ECharNormal;
            Destroy(gameObject);
            SceneManager.LoadScene(sceneIndex);
        }

        public void LoadScene()
        {
            ImportantCharacterReferenceHolder ICRH =
                PlayerCharacter.Instance.gameObject.GetComponent<ImportantCharacterReferenceHolder>();
            ICRH.saveGameButton.gameObject.SetActive(true);
            ICRH.quitButton.gameObject.SetActive(true);
            DataPersistenceManager.Instance.NewGame();
            SceneLoader.Instance.LoadScene(sceneIndex);
        }
    }
}
