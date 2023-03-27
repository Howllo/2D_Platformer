using Character;
using UnityEngine;

namespace UserInterface.MainMenu
{
    public class SettingMainMenuScript : MonoBehaviour
    {
        public void OpenSettingMenu()
        {
            ImportantCharacterReferenceHolder irh = PlayerCharacter.Instance.gameObject.GetComponent<ImportantCharacterReferenceHolder>();
            irh.settingMenu.SetActive(true);
        }
    }
}
