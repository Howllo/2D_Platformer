using System.Collections.Generic;
using UnityEngine;

namespace MiscScripts
{
    public class RespawnReferenceSingleton : MonoBehaviour
    {
        public static RespawnReferenceSingleton Instance { get; private set; }
        private readonly Dictionary<string, RespawnPoint> checkPoints = new Dictionary<string, RespawnPoint>();

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
            var temp = FindObjectsOfType<RespawnPoint>();
            foreach (var respawnPoint in temp)
            {
                checkPoints.Add(respawnPoint.objectGuid, respawnPoint);
            }
        }

        public RespawnPoint GetCheckPoint(string testStr)
        {
            return checkPoints[testStr];
        }
    }
}