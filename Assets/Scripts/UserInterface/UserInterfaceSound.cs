using Character;
using MiscScripts;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class UserInterfaceSound : MonoBehaviour
    {
        public AudioClip clip;
        public Button button;

        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(StartSound);
        }

        private void StartSound()
        {
            AudioManager.Instance.PlaySoundEffect(clip);
        }
    }
}
