using System.Collections;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MiscScripts
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }
        public Slider slider;
        public TextMeshProUGUI text;
        public GameObject loadingScreen;
        public bool gameBeaten;
        public int finalScore;
        public bool newLevel;
        
        private void Awake()
        {
            if(Instance != null)
                Destroy(gameObject);
            Instance = this;
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            text.SetText("Loading...0%");
        }

        public void LoadScene(int buildIndex)
        {
            if(loadingScreen)
                loadingScreen.SetActive(true);
            StartCoroutine(LoadAsync(buildIndex));
        }

        private void SetText(float inFloat)
        {
            text.SetText("Loading..." + inFloat * 100 + "%");
        }
        
        IEnumerator LoadAsync(int index)
        {
            if(index > 3)
                newLevel = true;
            
            AsyncOperation load = SceneManager.LoadSceneAsync(index);
            while (!load.isDone && index != 2)
            {
                SetText(load.progress);
                slider.value = load.progress;
                yield return null;
            }

            if (load.isDone)
            {
                if ( index > 2 && !DataPersistenceManager.Instance.newGame )
                {
                    if (!newLevel)
                    {
                        DataPersistenceManager.Instance.LoadGame();
                    }
                    else
                    {
                        DataPersistenceManager.Instance.NewLevel();
                    }
                    
                    StartCoroutine(DisableLoadingScreen());
                }
                if(loadingScreen)
                    loadingScreen.SetActive(false);
            }
        }

        IEnumerator DisableLoadingScreen()
        {
            yield return new WaitUntil(() => DataPersistenceManager.Instance.isLoading == false);
            loadingScreen.SetActive(false);
        }
    }
}
