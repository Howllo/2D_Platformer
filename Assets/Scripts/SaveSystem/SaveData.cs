using System.Collections.Generic;

namespace SaveSystem
{
    [System.Serializable]
    public class SaveData
    {
        public string characterName;
        public int level;
        public float maxHealth;
        public float currentHealth;
        public int playerScore;
        public bool flipX;
        public float[] position;
        public string respawnPointGuid;
        public float[] respawnPoint;
        public float musicVolume;
        public float effectVolume;
        public float ambianceVolume;
        public List<PickUpObjects> pickupObjects = new List<PickUpObjects>();
        public List<Enemies> enemiesList = new List<Enemies>();
        public Dictionary<string, bool> pickupObjectHash = new Dictionary<string, bool>();
        public Dictionary<string, bool> enemiesListHash = new Dictionary<string, bool>();
    }
    
    [System.Serializable]
    public struct PickUpObjects
    {
        public string guid;
        public bool hasPickup;
    }

    [System.Serializable]
    public struct Enemies
    {
        public string guid;
        public bool hasKilled;
    }
}