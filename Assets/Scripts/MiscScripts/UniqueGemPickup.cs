using Character;
using SaveSystem;
using UnityEngine;
using UserInterface;

namespace MiscScripts
{
    public class UniqueGemPickup : ItemsPickup
    {
        private PlayCinematic playCinematic;
        [SerializeField] private GameObject interactObject;
        
        private void Start()
        {
            playCinematic = GetComponent<PlayCinematic>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();
            anim = GetComponent<Animator>();

            // Disable Asset
            spriteRenderer.enabled = false;
            boxCollider2D.enabled = false;
            anim.enabled = false;

            if (DataPersistenceManager.Instance.newGame)
            {
                Interact interact = interactObject.GetComponent<Interact>();
                interact.gemScript = this;
                Instantiate(interactObject);
            }
        }

        public void StartUniqueGem()
        {
            spriteRenderer.enabled = true;
            boxCollider2D.enabled = true;
            anim.enabled = true;
        }

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                hasPickUp = true;
                boxCollider2D.enabled = false;
                anim.enabled = false;
                spriteRenderer.enabled = false;
                PlayerCharacter.Instance.AddPlayerScore(playerScoreReward);
                AudioManager.Instance.PlaySoundEffect(clip);
                playCinematic.StartCinematic();
            }
        }
        
        public override void LoadData(SaveData data)
        {
            bool dataIn = false;
            if (data.pickupObjectHash.ContainsKey(objectGuid))
            {
                dataIn = data.pickupObjectHash[objectGuid];
            }

            if (dataIn)
            {
                hasPickUp = true;
                boxCollider2D.enabled = false;
                spriteRenderer.enabled = false;
                anim.enabled = false;
            }
            else
            {
                hasPickUp = false;
                Interact interact = interactObject.GetComponent<Interact>();
                interact.gemScript = this;
                Instantiate(interactObject);
            }
        }

        public override void SaveData(ref SaveData data)
        {
            PickUpObjects puo;
            puo.guid = objectGuid;
            puo.hasPickup = hasPickUp;
            data.pickupObjects.Add(puo);
        }
    }
}