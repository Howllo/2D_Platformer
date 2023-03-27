using MiscScripts;
using UnityEngine;

public class LastHouseLevel1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            SceneLoader.Instance.LoadScene(4);
        }
    }
}
