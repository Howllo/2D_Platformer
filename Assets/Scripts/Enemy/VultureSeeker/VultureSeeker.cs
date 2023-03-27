using Character;
using Pathfinding;
using UnityEngine;

namespace Enemy.VultureSeeker
{
    public class VultureSeeker : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float damage;
        [SerializeField] private AIDestinationSetter aiDestinationSetter;
        [SerializeField] private AIPath aiPath;
        public bool isDead;
        
        private void Start()
        {
            if (PlayerCharacter.Instance)
                aiDestinationSetter.target = PlayerCharacter.Instance.transform;
        }

        private void Update()
        {
            SetSpriteRenderX();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player") && !isDead)
            {
                if(!isDead)
                    PlayerCharacter.Instance.TakeDamage(damage);
            }
        }
        
        private void SetSpriteRenderX()
        {
            if (aiPath.velocity.x < -0.001f)
                spriteRenderer.flipX = true;
            else if (aiPath.velocity.x > 0.001f)
                spriteRenderer.flipX = false;
        }
    }
}