using System.Collections;
using Character;
using UnityEngine;

namespace MiscScripts
{
    public class PlayCinematic : MonoBehaviour
    {
        public ParticleSystem[] particleSystemsToPlay;
        public AudioClip[] soundClips;
        public float[] waitTimes;
        public PlayerCharacter character;
        public GameObject locationToMove;
        public bool makeCharacterInvisible;
        public bool disableCharacter;
        private int i = 0;
        
        public void StartCinematic()
        {
            character = PlayerCharacter.Instance;
            if (makeCharacterInvisible)
                character.GetSpriteRender().enabled = false;
            if (disableCharacter)
            {
                character.playerController.state = PlayerController.ECharacterState.ECharStunned;
                character.playerController.GetRb().velocity = new Vector2(0.0f, 0.0f);
                character.playerController.GetRb().simulated = false;
            }
            particleSystemsToPlay[i].transform.position = character.transform.position;
            Instantiate(particleSystemsToPlay[i]);
            AudioManager.Instance.PlaySoundEffect(soundClips[i]);
            StartCoroutine(WaitForParticleEffect());
        }

        IEnumerator WaitForParticleEffect()
        {
            yield return new WaitForSeconds(waitTimes[i]);
            i++;
            character.gameObject.transform.position = locationToMove.transform.position;
            if (particleSystemsToPlay.Length > 1)
            {
                particleSystemsToPlay[i].transform.position = character.transform.position;
                Instantiate(particleSystemsToPlay[i]);
            }
            if(soundClips.Length > 1)
                AudioManager.Instance.PlaySoundEffect(soundClips[i]);
            character.GetSpriteRender().enabled = true;
            character.playerController.state = PlayerController.ECharacterState.ECharNormal;
            character.playerController.GetRb().simulated = true;
            AudioManager.Instance.StopAmbiance();
            AudioManager.Instance.PlayMusicEffect(AudioManager.Instance.defaultMusic, true);
        }
    }
}