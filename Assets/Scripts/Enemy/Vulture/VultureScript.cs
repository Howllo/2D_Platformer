using System;
using System.Collections;
using Character;
using SaveSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.Vulture
{
    public class VultureScript : MonoBehaviour, IDataPersistence
    {
        public enum MovementState
        {
            EMovingUp,
            EMovingDown,
            EMovingIdle
        }

        public string objectGuid;
        public bool hasKilled = false;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BoxCollider2D trigger;
        [SerializeField] private float damage;
        [SerializeField] private float maxYDiff;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Animator anim;
        [SerializeField] private MovementState state;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float heightOffset;
        private MovementState lastState;
        private Vector3 startingPos;
        
        private VultureColliderTrigger vultureColliderTrigger;

        private void Start()
        {
            startingPos = transform.position;
            anim = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (Math.Abs(transform.position.y - 3.0f) < Single.Epsilon)
                state = MovementState.EMovingDown;
            else
                state = MovementState.EMovingUp;
            vultureColliderTrigger = gameObject.transform.GetChild(0).GetComponent<VultureColliderTrigger>();
        }

        private void Update()
        {
            if(state == MovementState.EMovingIdle) return;
        
            if (transform.position.y <= startingPos.y + maxYDiff && state == MovementState.EMovingUp)
            {
                transform.Translate((Vector3.up * moveSpeed) * Time.deltaTime);
            }
            else if (!GetGround() && state == MovementState.EMovingDown)
            {
                transform.Translate((Vector3.down * moveSpeed) * Time.deltaTime);
            }

            if (transform.position.y >= startingPos.y + maxYDiff)
            {
                state = MovementState.EMovingDown;
                StartCoroutine(RandomIdle(Random.Range(0.0f, 3.0f)));
            }
            else if (GetGround())
            {
                state = MovementState.EMovingUp;
                StartCoroutine(RandomIdle(Probability(Random.Range(0.0f, 100.0f))));
            }
        }

        private IEnumerator RandomIdle(float random)
        {
            lastState = state;
            state = MovementState.EMovingIdle;
            yield return new WaitForSeconds(random);
            state = lastState;
        }

        private float Probability(float random)
        {
            if (random <= 75.0f)
                return 1.0f;
            if (random >= 76.0f && random <= 95.0f)
                return 2.0f;
            if (random >= 96.0f)
                return 3.0f;
            return 1.0f;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player") && !hasKilled)
            {
                if(!hasKilled)
                    PlayerCharacter.Instance.TakeDamage(damage);
            }
        }

        private bool GetGround()
        {
           
            RaycastHit2D raycastHit2D =
                Physics2D.Raycast(trigger.bounds.center, Vector2.down, trigger.bounds.extents.y + heightOffset, groundMask);
            Color color;
            if (raycastHit2D.collider != null)
                color = Color.green;
            else
                color = Color.red;
            Debug.DrawRay(trigger.bounds.center, Vector2.down * (trigger.bounds.extents.y + heightOffset), color);
            return (bool)raycastHit2D.collider;
        }
        
        public void DisableObject()
        {
            state = MovementState.EMovingIdle;
            trigger.enabled = false;
            spriteRenderer.enabled = false;
            vultureColliderTrigger.GetCollider().enabled = false;
        }

        public void LoadData(SaveData data)
        {
            bool killed = false;
            if(data.enemiesListHash.ContainsKey(objectGuid))
                killed = data.enemiesListHash[objectGuid];
            if (killed)
            {
                hasKilled = killed;
                trigger.enabled = false;
                spriteRenderer.enabled = false;
                vultureColliderTrigger.GetCollider().enabled = false;
            }
        }

        public void SaveData(ref SaveData data)
        {
            Enemies em;
            em.hasKilled = hasKilled;
            em.guid = objectGuid;
            data.enemiesList.Add(em);
        }

        public Animator GetAnimator()
        {
            return anim;
        }

        public void SetState(MovementState state)
        {
            this.state = state;
        }
    }
}