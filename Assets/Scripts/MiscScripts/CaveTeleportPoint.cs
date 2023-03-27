using System;
using Character;
using UnityEngine;

namespace MiscScripts
{
    public class CaveTeleportPoint : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Vector3 center = transform.position;
            Gizmos.DrawWireSphere(center, 0.9f);
        }
    }
}