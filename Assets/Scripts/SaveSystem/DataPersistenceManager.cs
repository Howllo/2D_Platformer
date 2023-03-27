using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SaveSystem
{
    public class DataPersistenceManager : MonoBehaviour
    {
        public static DataPersistenceManager Instance { get; private set; }
        [SerializeField] private SaveData data;
        public bool newGame = true;
        public bool isLoading = true;
        private List<IDataPersistence> iDataPersistence;

        [Header("File Settings")]
        [SerializeField] private string fileName;
        private FileHandler fileHandler;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            data = null;
            fileHandler = new FileHandler(Application.persistentDataPath, fileName);
            data = fileHandler.Load();
            newGame = fileHandler.CheckForNewGame();
        }

        public void NewGame()
        {
            data = new SaveData();
            newGame = true;
        }

        public void LoadGame()
        {
            if(data == null)
                NewGame();

            // Create hash map for pickup objects.
            foreach (var pickupObject in data.pickupObjects)
            {
                if(data.pickupObjectHash.ContainsKey(pickupObject.guid))
                    continue;
                data.pickupObjectHash.Add(pickupObject.guid, pickupObject.hasPickup);
            }

            // Create hash map for enemies.
            foreach (var em in data.enemiesList)
            {
                if(data.enemiesListHash.ContainsKey(em.guid))
                    continue;
                data.enemiesListHash.Add(em.guid, em.hasKilled);
            }
            
            iDataPersistence = FindAllObjectTypeDataPersistence();
            foreach (var dataPersistence in iDataPersistence)
            {
                dataPersistence.LoadData(data);
            }
            isLoading = false;
        }

        public void SaveGame()
        {
            iDataPersistence = FindAllObjectTypeDataPersistence();
            data.pickupObjects.Clear();
            foreach (var dataPersistence in iDataPersistence)
            {
                dataPersistence.SaveData(ref data);
            }
            
            // Save Data
            fileHandler.Save(data);
        }

        // Deletes the old save and create new save. 
        public void NewLevel()
        {
            fileHandler.DeleteFile();
        }
        
        public void RevistAllObjectTypeData()
        {
            iDataPersistence = FindAllObjectTypeDataPersistence();
        }
        
        private List<IDataPersistence> FindAllObjectTypeDataPersistence()
        {
            IEnumerable<IDataPersistence> dataPersistence = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
            return new List<IDataPersistence>(dataPersistence);
        }

        public FileHandler GetFileHandler()
        {
            return fileHandler;
        }

        public SaveData GetData()
        {
            return data;
        }
    }
}