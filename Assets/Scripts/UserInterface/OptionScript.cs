using Character;
using MiscScripts;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UserInterface
{
    public class OptionScript : MonoBehaviour
    {
        [Header("Music")]
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Button incrementUpMusic;
        [SerializeField] private Button incrementDownMusic;
        [SerializeField] private TextMeshProUGUI percentageMusicText;
    
        [Header("Effect")]
        [SerializeField] private Slider effectSlider;
        [SerializeField] private Button incrementUpEffect;
        [SerializeField] private Button incrementDownEffect;
        [SerializeField] private TextMeshProUGUI percentageEffectText;

        [Header("Ambiance")] 
        [SerializeField] private Slider ambianceSlider;
        [SerializeField] private Button incrementUpAmbiance;
        [SerializeField] private Button incrementDownAmbiance;
        [SerializeField] private TextMeshProUGUI percentageAmbianceText;
    
        [Header("Other")]
        [SerializeField] private Button quitGameButton;
        [SerializeField] private Button saveGameButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private GameObject errorPopup;
        [SerializeField] private AudioClip successfulSave;
        
        [Header("Values")] 
        [SerializeField] private float incrementAmount = 0.1f;
    
        // Start is called before the first frame update
        void Start()
        {
            // Music
            incrementUpMusic.onClick.AddListener(IncrementUpMusicButton);
            incrementDownMusic.onClick.AddListener(IncrementDownMusicButton);
            musicSlider.onValueChanged.AddListener(OnMusicSliderUpdate);
        
            // Effect
            incrementUpEffect.onClick.AddListener(IncrementUpEffectButton);
            incrementDownEffect.onClick.AddListener(IncrementDownEffectButton);
            effectSlider.onValueChanged.AddListener(OnEffectSliderUpdate);
        
            // Ambiance
            incrementUpAmbiance.onClick.AddListener(IncrementUpAmbianceButton);
            incrementDownAmbiance.onClick.AddListener(IncrementDownAmbianceButton);
            ambianceSlider.onValueChanged.AddListener(OnAmbianceSliderUpdate);
        
            // Quit
            quitGameButton.onClick.AddListener(OnQuitButtonClick);
        
            // Save
            saveGameButton.onClick.AddListener(SaveGame);
            
            // Close
            closeButton.onClick.AddListener(OnCloseButtonClick);

            Scene sceneManager = SceneManager.GetActiveScene();
            if (sceneManager.buildIndex == 1)
            {
                saveGameButton.gameObject.SetActive(false);
                quitGameButton.gameObject.SetActive(false);
            }
        }
    
        private void IncrementUpMusicButton()
        {
            if (musicSlider.value + incrementAmount <= 1.0f)
                musicSlider.value += incrementAmount;
            else
                musicSlider.value = 1.0f;
        }

        private void IncrementDownMusicButton()
        {
            if (musicSlider.value - incrementAmount <= 1.0f)
                musicSlider.value -= incrementAmount;
            else
                musicSlider.value = 0.0f;
        }

        public void OnMusicSliderUpdate(float value)
        {
            musicSlider.value = value;
            AudioManager.Instance.GetMusicSource().volume = value;
            percentageMusicText.SetText( (int)(value * 100) + "%");
        }

        private void IncrementUpEffectButton()
        {
            if (effectSlider.value + incrementAmount <= 1.0f)
                effectSlider.value += incrementAmount;
            else
                effectSlider.value = 1.0f;
        }

        private void IncrementDownEffectButton()
        {
            if (effectSlider.value - incrementAmount <= 1.0f)
                effectSlider.value -= incrementAmount;
            else
                effectSlider.value = 0.0f;
        }
    
        public void OnEffectSliderUpdate(float value)
        {
            effectSlider.value = value;
            AudioManager.Instance.GetEffectSource().volume = value;
            percentageEffectText.SetText((int)(value * 100) + "%");
        }

        private void IncrementUpAmbianceButton()
        {
            if (ambianceSlider.value + incrementAmount <= 1.0f)
                ambianceSlider.value += incrementAmount;
            else
                ambianceSlider.value = 1.0f;
        }

        private void IncrementDownAmbianceButton()
        {
            if (ambianceSlider.value - incrementAmount <= 1.0f)
                ambianceSlider.value -= incrementAmount;
            else
                ambianceSlider.value = 0.0f;
        }
    
        public void OnAmbianceSliderUpdate(float value)
        {
            ambianceSlider.value = value;
            AudioManager.Instance.GetAmbianceSource().volume = value;
            percentageAmbianceText.SetText((int)(value * 100) + "%");
        }

        private void SaveGame()
        {
            if(!PlayerCharacter.Instance.playerController.isGrounded)
                errorPopup.SetActive(true);
            else
            {
                DataPersistenceManager.Instance.SaveGame();
                AudioManager.Instance.PlaySoundEffect(successfulSave);
            }
        }
    
        private void OnQuitButtonClick()
        {
            Application.Quit();
        }

        private void OnCloseButtonClick()
        {
            Time.timeScale = 1;
            if (PlayerCharacter.Instance.playerController)
                PlayerCharacter.Instance.playerController.state = PlayerController.ECharacterState.ECharNormal;
        }
    }
}