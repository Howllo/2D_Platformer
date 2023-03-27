using UnityEngine;

namespace MiscScripts
{
    public class AntiSpawnDestroy : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D col)
        {
            if(col.gameObject.CompareTag("Enemy_"))
                Destroy(col.gameObject);
        }
    }
}
