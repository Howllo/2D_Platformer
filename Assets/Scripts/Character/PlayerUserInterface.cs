using MiscScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInterface;

namespace Character
{
    public class PlayerUserInterface : MonoBehaviour
    {
        public Slider healthBar;
        public GameObject loadingScreen;
        public GameObject settingsUI;
        public OptionScript settingsScript;
        public TextMeshProUGUI healthBarText;
        public TextMeshProUGUI playerScore;
        public TextMeshProUGUI displayNameText;
        public GameObject interactObject;
        public Button saveGame;

        [Header("ErrorPopup")] 
        public GameObject errorPopup;
        public TextMeshProUGUI errorPopupText;
        public string errorPopupTextWarning;
        public string errorOriginalTextWarning;

        private void Start()
        {
            playerScore.SetText(PlayerCharacter.Instance.GetPlayerScore().ToString());
            SceneLoader.Instance.loadingScreen = loadingScreen;
        }

        public void SetHealthBar(float currentHealth, float maxHealth)
        {
            if (currentHealth < 0)
            {
                healthBar.value = 0;
                healthBarText.SetText(0 + "/" + (int)maxHealth);
            }
            else
            {
                healthBar.value = currentHealth;
                healthBarText.SetText((int)currentHealth + "/" + (int)maxHealth);
            }
        }

        public void SetPlayerScore(int score)
        {
            playerScore.SetText(score.ToString());
        }

        public void LoadNewScene(int buildIndex)
        {
            loadingScreen.SetActive(true);
            SceneLoader.Instance.LoadScene(buildIndex);
        }
    }
}