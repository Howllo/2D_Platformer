using System.Collections;
using Character;
using MiscScripts;
using UnityEngine;

namespace Enemy.Vulture
{
    public class VultureColliderTrigger : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D boxCollider2D;
        [SerializeField] private int scoreOnKill;
        [SerializeField] private float playerJumpForce;
        [SerializeField] private VultureScript vultureScript;
        [SerializeField] private AudioClip clip;

        private void Start()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            vultureScript = gameObject.transform.parent.GetComponent<VultureScript>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player") )
            {
                vultureScript.hasKilled = true;
                vultureScript.SetState(VultureScript.MovementState.EMovingIdle);
                boxCollider2D.enabled = false;
                vultureScript.GetAnimator().SetBool("Dead", true);
                StartCoroutine(WaitForAnimation());
                AudioManager.Instance.PlaySoundEffect(clip);
                Rigidbody2D rb = PlayerCharacter.Instance.playerController.GetRb();
                rb.velocity = new Vector3(-rb.velocity.x, rb.velocity.y * playerJumpForce);
                PlayerCharacter.Instance.AddPlayerScore(scoreOnKill);
            }
        }

        private IEnumerator WaitForAnimation()
        {
            yield return new WaitForSeconds(0.5f);
            vultureScript.DisableObject();
        }

        public BoxCollider2D GetCollider()
        {
            return boxCollider2D;
        }
    }
}