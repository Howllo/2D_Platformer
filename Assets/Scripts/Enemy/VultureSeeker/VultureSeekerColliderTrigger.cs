using System.Collections;
using Character;
using MiscScripts;
using Pathfinding;
using UnityEngine;

namespace Enemy.VultureSeeker
{
    public class VultureSeekerColliderTrigger : MonoBehaviour
    {
        [SerializeField] private int scoreOnKill;
        [SerializeField] private float playerJumpForce;
        [SerializeField] private BoxCollider2D boxCollider2D;
        [SerializeField] private CircleCollider2D parentCircleCol;
        [SerializeField] private AIPath aiPath;
        [SerializeField] private Animator anim;
        [SerializeField] private AudioClip clip;
        [SerializeField] private GameObject parentObject;
        [SerializeField] private VultureSeeker seeker;
        private static readonly int Dead = Animator.StringToHash("Dead");
        private Rigidbody2D playerRb;
        private bool isDead;

        private void Start()
        {
            if(PlayerCharacter.Instance)
                playerRb = PlayerCharacter.Instance.playerController.GetRb();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player") && !isDead && !seeker.isDead)
            {
                seeker.isDead = true;
                isDead = true;
                boxCollider2D.enabled = false;
                parentCircleCol.enabled = false;
                aiPath.enabled = false;
                anim.SetBool(Dead, true);
                StartCoroutine(WaitForAnimation());
                AudioManager.Instance.PlaySoundEffect(clip);
                playerRb.velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y * playerJumpForce);
                PlayerCharacter.Instance.AddPlayerScore(scoreOnKill);
            }
        }

        private IEnumerator WaitForAnimation()
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(parentObject);
        }
    }
}