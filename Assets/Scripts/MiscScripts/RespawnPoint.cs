using System;
using Character;
using SaveSystem;
using UnityEngine;

namespace MiscScripts
{
    public class RespawnPoint : MonoBehaviour
    {
        [Header("Checkpoint")]
        public string objectGuid;
        [SerializeField] private AudioClip checkPointClip;
        [SerializeField] private ParticleSystem checkParticle;
        [SerializeField] private bool startingPoint = false;
        private Collider2D coll;
        private PlayerCharacter player;

        [Header("Spawning")] 
        [SerializeField] private ParticleSystem spawn;
    
        private void Start()
        {
            coll = GetComponent<Collider2D>();
            player = PlayerCharacter.Instance;
            if(startingPoint && DataPersistenceManager.Instance)
                DataPersistenceManager.Instance.RevistAllObjectTypeData();
        }

        public void Spawning()
        {
            spawn.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 0.1f, player.transform.position.z);
            Instantiate(spawn);
            if(!AudioManager.Instance.GetMusicSource().isPlaying)
                AudioManager.Instance.PlayMusicEffect(AudioManager.Instance.defaultMusic, true);
        }
    
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player") && player.currentSpawn != coll)
            {
                player.rp = this;
                if(!startingPoint)
                {
                    checkParticle.transform.position = col.gameObject.transform.position;
                    Instantiate(checkParticle);
                    AudioManager.Instance.PlaySoundEffect(checkPointClip);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 center = transform.position;
            Gizmos.DrawWireSphere(center, 0.9f);
        }
    }
}
