using Character;
using MiscScripts;
using UnityEngine;

namespace UserInterface
{
    public class Interact : MonoBehaviour
    {
        public UniqueGemPickup gemScript;
        private bool isInteractable = false;
        private bool hasActivated = false;
        private CircleCollider2D circleCollider;
        
        private void Awake()
        {
            var position = gemScript.transform.position;
            transform.position = new Vector3(position.x, position.y - 1.0f, position.z);
        }
        
        private void Update()
        {
            if (!isInteractable) return;
            if (Input.GetKeyDown(KeyCode.F) && !hasActivated && gemScript)
            {
                hasActivated = true;
                gemScript.StartUniqueGem();
                PlayerCharacter.Instance.GetPlayerUserInterface().interactObject.SetActive(false);
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                PlayerCharacter.Instance.GetPlayerUserInterface().interactObject.SetActive(true);
                isInteractable = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            PlayerCharacter.Instance.GetPlayerUserInterface().interactObject.SetActive(false);
            isInteractable = false;
        }
    }
}
