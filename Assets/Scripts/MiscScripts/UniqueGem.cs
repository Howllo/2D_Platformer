using UnityEngine;

namespace MiscScripts
{
    public class UniqueGem : MonoBehaviour
    {
        public AudioClip firstClip;
        public ParticleSystem spark;
        public Animator anim;
        private bool hasPlayed = false;

        public void PlayFirstSound()
        {
            if(!hasPlayed)
                AudioManager.Instance.PlaySoundEffect(firstClip);
            hasPlayed = true;
        }

        public void PlayParticleEffect()
        {
            spark.transform.position = transform.position;
            Instantiate(spark);
        }

        public void StopAnimation()
        {
            anim.enabled = false;
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}