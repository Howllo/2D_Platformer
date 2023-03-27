using System;
using Enemy.Vulture;
using MiscScripts;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EditorMenu
{
    // IT WORKS! HOLY SHIT! DOCUMENTATION IS ACTUALLY IMPORTANT!
    // https://docs.unity3d.com/ScriptReference/SerializedObject.html
    // This prevents so much copy & paste. Just place pickup items in the 
    // scene and click the button to generate a GUID to create unique
    // ID for each pickup item. This is used for the save system to
    // find them and decided which object to be disable or enabled.
    public class EditorScript : Editor
    {
        /// <summary>
        /// Generate GUID for objects that do not currently have a GUID.
        /// Used to prevent other object from losing their current GUID.
        /// </summary>
        
        [MenuItem("Tool/Generate Guid/Generate All GUID")]
        private static void GenerateAllGuid()
        {
            GenerateCheckpointGuid();
            GenerateCollectGuid();
            GenerateEnemies();
        }
        
        [MenuItem("Tool/Generate Guid/Generate Collect GUID")]
        private static void GenerateCollectGuid()
        {
            ItemsPickup[] allGo = FindObjectsOfType<ItemsPickup>();
            foreach (ItemsPickup go in allGo)
            {
                ItemsPickup item = go.GetComponent<ItemsPickup>();
                if (string.IsNullOrEmpty(item.objectGuid))
                {
                    string GUID = Guid.NewGuid() + "-" + Random.Range(0, 2000000);
                    var so = new SerializedObject(item);
                    so.FindProperty("objectGuid").stringValue = GUID;
                    so.ApplyModifiedProperties();
                }
            }
        }
        
        [MenuItem("Tool/Generate Guid/Generate Checkpoint GUID")]
        private static void GenerateCheckpointGuid()
        {
            RespawnPoint[] allGo = FindObjectsOfType<RespawnPoint>();
            foreach (RespawnPoint go in allGo)
            {
                if (string.IsNullOrEmpty(go.objectGuid))
                {
                    string GUID = Guid.NewGuid() + "-" + Random.Range(0, 2000000);
                    var so = new SerializedObject(go);
                    so.FindProperty("objectGuid").stringValue = GUID;
                    so.ApplyModifiedProperties();
                }
            }
        }
        
        [MenuItem("Tool/Generate Guid/Generate Enemy GUID")]
        private static void GenerateEnemies()
        {
            VultureScript[] allGo = FindObjectsOfType<VultureScript>();
            foreach (VultureScript go in allGo)
            {
                if (string.IsNullOrEmpty(go.objectGuid))
                {
                    string GUID = Guid.NewGuid() + "-" + Random.Range(0, 2000000);
                    var so = new SerializedObject(go);
                    so.FindProperty("objectGuid").stringValue = GUID;
                    so.ApplyModifiedProperties();
                }
            }
        }
        
        /// <summary>
        /// Used to generate every single object of that type.
        /// This will break old saves.
        /// </summary>
        
        [MenuItem("Tool/Danger Zone/Regenerate Guid/Regenerate All GUID")]
        private static void RegenerateAllGuid()
        {
            RegenerateCheckpointGuid();
            RegenerateCollectGuid();
            RegenerateEnemies();
        }
        
        [MenuItem("Tool/Danger Zone/Regenerate Guid/Regenerate Collect GUID")]
        private static void RegenerateCollectGuid()
        {
            GameObject[] allGo = GameObject.FindGameObjectsWithTag("Pickup");
            foreach (GameObject go in allGo)
            {
                string GUID = Guid.NewGuid() + "-" + Random.Range(0, 2000000);
                ItemsPickup item = go.GetComponent<ItemsPickup>();
                var so = new SerializedObject(item);
                so.FindProperty("objectGuid").stringValue = GUID;
                so.ApplyModifiedProperties();
            }
        }
        
        [MenuItem("Tool/Danger Zone/Regenerate Guid/Regenerate Checkpoint GUID")]
        private static void RegenerateCheckpointGuid()
        {
            RespawnPoint[] allGo = FindObjectsOfType<RespawnPoint>();
            foreach (RespawnPoint go in allGo)
            {
                string GUID = Guid.NewGuid() + "-" + Random.Range(0, 2000000);
                var so = new SerializedObject(go);
                so.FindProperty("objectGuid").stringValue = GUID;
                so.ApplyModifiedProperties();
            }
        }
        
        [MenuItem("Tool/Danger Zone/Regenerate Guid/Regenerate Enemy GUID")]
        private static void RegenerateEnemies()
        {
            VultureScript[] allGo = FindObjectsOfType<VultureScript>();
            foreach (VultureScript go in allGo)
            {
                string GUID = Guid.NewGuid() + "-" + Random.Range(0, 2000000);
                var so = new SerializedObject(go);
                so.FindProperty("objectGuid").stringValue = GUID;
                so.ApplyModifiedProperties();
            }
        }
    }
}