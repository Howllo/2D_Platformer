using System.Collections;
using UnityEngine;

public class DestroyTheCurrentObject : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyTheObject());
    }

    IEnumerator DestroyTheObject()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }
}
