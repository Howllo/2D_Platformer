using System.Collections;
using MiscScripts;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Character
{
    public class PlayerCharacter : MonoBehaviour, IDataPersistence
    {
        #region Variables
        
        public static PlayerCharacter Instance { get; private set; }
        
        [Header("Character Stats")] 
        [SerializeField] private float playerMaxHealth = 100;
        [SerializeField] private float playerCurrentHealth;
        [SerializeField] private PlayerUserInterface pui;
        [SerializeField] private AudioClip takeDamageClip;
        [SerializeField] private AudioClip deathSoundClip;
        [SerializeField] private string characterName;

        [Header("Player Score")] 
        [SerializeField] private int playerScore;
        
        [Header("Required Misc.")]
        [SerializeField] private Vector3 respawnPoint;
        [SerializeField] private float maxFallDistance = -50.0f;
        [SerializeField] private GameObject playerHUD;
        public int level;
        public PlayerController playerController;
        private SpriteRenderer spriteRenderer;

        [Header("Teleportation")]
        [SerializeField] private ParticleSystem teleport;
        [SerializeField] private AudioClip despawnClip;
        [SerializeField] private float teleportLength = 1.5f;

        [Header("Spawn")] 
        [SerializeField] private ParticleSystem partCheckPoint;
        public Collider2D currentSpawn;
        public RespawnPoint rp;
        private string rp_guid;
        private static readonly int Damage = Animator.StringToHash("Damage");

        [Header("Death")]
        [SerializeField] private int gameOverSceneIndex;

        #endregion
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        void Start()
        {
            playerCurrentHealth = playerMaxHealth;
            playerController = GetComponent<PlayerController>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            // Disable at Start
            spriteRenderer.enabled = false;
            playerController.GetRb().simulated = false;
            playerHUD.SetActive(false);
            playerController.state = PlayerController.ECharacterState.ECharStunned;
        }
        
        void Update()
        {
            if(playerController.state != PlayerController.ECharacterState.ECharDead)
                CheckPlayerDead();
            
            if (playerController.state != PlayerController.ECharacterState.ECharStunned)
            {
                if (transform.position.y < maxFallDistance)
                    OutOfBounds();
            }
        }

        /// <summary>
        /// Alternative to Start.
        /// Used to start the character full during levels.
        /// </summary>
        public void StartCharacter()
        {
            // Get Level
            Scene scene = SceneManager.GetActiveScene();
            level = scene.buildIndex;
            
            // Visibility
            playerController.GetRb().simulated = true;
            spriteRenderer.enabled = true;
            playerHUD.SetActive(true);
            
            // Sound Source
            AudioManager.Instance.Stop();
            AudioManager.Instance.PlayDefaultMusic();

            // UI
            pui.healthBar.maxValue = playerMaxHealth;
            pui.SetHealthBar(playerCurrentHealth, playerMaxHealth);
            
            // Respawn Point
            if(!string.IsNullOrEmpty(rp_guid) || RespawnReferenceSingleton.Instance)
                rp = RespawnReferenceSingleton.Instance.GetCheckPoint(rp_guid);
            
            // Vulture Spawner
            GetComponent<SpawnVulture>().enabled = true;
            StartCoroutine(GetComponent<SpawnVulture>().StartVultureSpawn());
        }


        public void StartCharacterLevel2()
        {
            Scene scene = SceneManager.GetActiveScene();
            
            // Set Save Button to False due to Level 2
            if (scene.buildIndex == 4)
            {
                pui.errorPopup.SetActive(true);
                pui.errorPopupText.SetText(pui.errorPopupTextWarning);
                pui.saveGame.gameObject.SetActive(false);
            }
        }

        private void OutOfBounds()
        {
            Rigidbody2D rb = playerController.GetRb();
            float gravityScale = rb.gravityScale;
            rb.velocity = new Vector2(0.0f, 0.0f);
            playerController.state = PlayerController.ECharacterState.ECharStunned;
            rb.gravityScale = 0;
            AudioManager.Instance.PlaySoundEffect(despawnClip);
            InvokeTeleport();
            spriteRenderer.enabled = false;
            if (SceneManager.GetActiveScene().buildIndex == 3)
                playerCurrentHealth -= 10.0f;
            else
                playerCurrentHealth -= 100000.0f;
            CheckPlayerDead();
            pui.SetHealthBar(playerCurrentHealth, playerMaxHealth);
            StartCoroutine(TeleportCheckPoint(gravityScale, rb));
        }

        /// <summary>
        /// Invoke the teleport particle effect.
        /// </summary>
        private void InvokeTeleport()
        {
            teleport.transform.position = transform.position;
            Instantiate(teleport);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Spawn") && col != currentSpawn)
            {
                currentSpawn = col;
                respawnPoint = col.gameObject.transform.position;
            }
        }

        IEnumerator TeleportCheckPoint(float gravityScale, Rigidbody2D rb)
        {
            yield return new WaitForSeconds(teleportLength);
            transform.position = respawnPoint;
            rb.gravityScale = gravityScale;
            spriteRenderer.enabled = true;
            rp.Spawning();
            StartCoroutine(ReEnableControls(1.0f));
        }

        IEnumerator ReEnableControls(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            playerController.state = PlayerController.ECharacterState.ECharNormal;
        }

        #region Game Over Section

        private void GameOver()
        {
            SceneLoader.Instance.finalScore = playerScore;
            Destroy(gameObject);
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.StopAmbiance();
            AudioManager.Instance.PlaySoundEffect(deathSoundClip);
            StartCoroutine(StopAllAudio());
            SceneLoader.Instance.gameBeaten = false;
            SceneLoader.Instance.LoadScene(gameOverSceneIndex);
        }

        private IEnumerator StopAllAudio()
        {
            yield return new WaitForSeconds(2.5f);
            AudioManager.Instance.Stop();
        }
        
        #endregion
        
        #region Player Take Damage

        public void TakeDamage(float damage)
        {
            if (playerController.state == PlayerController.ECharacterState.ECharDashing) return;
            
            playerController.anim.SetBool(Damage, true);
            CheckPlayerDead();
            playerCurrentHealth -= damage;
            AudioManager.Instance.PlaySoundEffect(takeDamageClip);
            pui.SetHealthBar(playerCurrentHealth, playerMaxHealth);
            StartCoroutine(StopAnimation());
        }

        private void CheckPlayerDead()
        {
            if (playerCurrentHealth <= 0)
            {
                playerController.state = PlayerController.ECharacterState.ECharDead;
                GameOver();
            }
        }

        IEnumerator StopAnimation()
        {
            yield return new WaitForSeconds(0.05f);
            playerController.anim.SetBool(Damage, false);
            
        }

        #endregion

        #region Data Persistence
        
        public void LoadData(SaveData data)
        {
            characterName = data.characterName;
            level = data.level;
            playerMaxHealth = data.maxHealth;
            playerCurrentHealth = data.currentHealth;
            playerScore = data.playerScore;
            spriteRenderer.flipX = data.flipX;
            transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
            if (RespawnReferenceSingleton.Instance != null)
                rp = RespawnReferenceSingleton.Instance.GetCheckPoint(data.respawnPointGuid);
            respawnPoint = new Vector3(data.respawnPoint[0], data.respawnPoint[1], data.respawnPoint[2]);
            rp_guid = data.respawnPointGuid;
            
            // Set UI
            pui.displayNameText.SetText(characterName);
            pui.playerScore.SetText(playerScore.ToString());
            pui.SetHealthBar(playerCurrentHealth, playerMaxHealth);
        }

        public void SaveData(ref SaveData data)
        {
            // Character Data
            data.characterName = characterName;
            data.level = level;
            data.maxHealth = playerMaxHealth;
            data.currentHealth = playerCurrentHealth;
            data.playerScore = playerScore;
            
            // Character Look FLip X
            data.flipX = spriteRenderer.flipX;
            
            // Position
            data.position = new float[3];
            var position = transform.position;
            data.position[0] = position.x;
            data.position[1] = position.y;
            data.position[2] = position.z;
            
            // Respawn
            data.respawnPointGuid = rp.objectGuid;
            data.respawnPoint = new float[3];
            data.respawnPoint[0] = respawnPoint.x;
            data.respawnPoint[1] = respawnPoint.y;
            data.respawnPoint[2] = respawnPoint.z;
        }

        #endregion
        
        public void AddPlayerScore(int score)
        {
            playerScore += score;
            pui.SetPlayerScore(playerScore);
        }
        
        public PlayerUserInterface GetPlayerUserInterface()
        {
            return pui;
        }

        #region Getters / Setters
        
        public float GetPlayerMaxHealth()
        {
            return playerMaxHealth;
        }

        public float GetPlayerCurrentHealth()
        {
            return playerMaxHealth;
        }

        public string GetPlayerName()
        {
            return characterName;
        }

        public int GetPlayerScore()
        {
            return playerScore;
        }

        public SpriteRenderer GetSpriteRender()
        {
            return spriteRenderer;
        }
        
        public void SetPlayerName(string incomingName)
        {
            name = incomingName;
            characterName = incomingName;
            pui.displayNameText.SetText(incomingName);
        }
        
        #endregion
    }
}