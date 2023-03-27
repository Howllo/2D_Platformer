using Character;
using SaveSystem;
using UnityEngine;

namespace MiscScripts
{
    public class ItemsPickup : MonoBehaviour, IDataPersistence
    {
        public string objectGuid;
        public AudioClip clip;
        public int playerScoreReward = 10;
        protected BoxCollider2D boxCollider2D;
        protected SpriteRenderer spriteRenderer;
        [SerializeField] protected bool hasPickUp;
        protected Animator anim;
        private bool isAnimNotNull;

        private void Start()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            hasPickUp = false;

            if (gameObject.GetComponent<Animator>() != null)
                anim = GetComponent<Animator>();
            isAnimNotNull = anim != null;
        }

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player") && !hasPickUp)
            {
                PlayerCharacter.Instance.AddPlayerScore(playerScoreReward);
                AudioManager.Instance.PlaySoundEffect(clip);
                DisableObject();
            }
        }

        private void DisableObject()
        {
            hasPickUp = true;
            boxCollider2D.enabled = false;
            spriteRenderer.enabled = false;
            if (isAnimNotNull)
                anim.enabled = false;
        }

        public virtual void LoadData(SaveData data)
        {
            bool dataIn = false;
            if(data.pickupObjectHash.Count > 0)
                dataIn = data.pickupObjectHash[objectGuid];
            
            if (dataIn)
            {
                hasPickUp = true;
                boxCollider2D.enabled = false;
                spriteRenderer.enabled = false;
                if (isAnimNotNull)
                    anim.enabled = false;
            }
            else
            {
                hasPickUp = false;
                boxCollider2D.enabled = true;
                spriteRenderer.enabled = true;
                if (isAnimNotNull)
                    anim.enabled = true;
            }
        }

        public virtual void SaveData(ref SaveData data)
        {
            PickUpObjects puo;
            puo.guid = objectGuid;
            puo.hasPickup = hasPickUp;
            data.pickupObjects.Add(puo);
        }
    }
}
