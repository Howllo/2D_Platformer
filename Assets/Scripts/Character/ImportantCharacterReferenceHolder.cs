using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class ImportantCharacterReferenceHolder : MonoBehaviour
    {
        [Header("User Interface - Loading")]
        public Slider loadingBar;
        public TextMeshProUGUI textLoadValue;
        public GameObject loadingObject;

        [Header("User Interface - Settings")]
        public GameObject settingMenu;
        public Button quitButton;
        public Button saveGameButton;
    }
}
