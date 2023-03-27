using MiscScripts;
using UnityEngine;

namespace MainMenu
{
    public class PlayAmbiance : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;
    
        // Start is called before the first frame update
        void Start()
        {
            AudioManager.Instance.PlayAmbiance(clip, true);
        }
    }
}