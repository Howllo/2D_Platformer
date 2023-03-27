using MiscScripts;
using UnityEngine;

public class AmbianceTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip ambianceClip;
    [SerializeField] private string triggerRequirement;
    [SerializeField] private bool playAmbianceLoop;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(triggerRequirement))
        {
            if (AudioManager.Instance)
            {
                AudioManager.Instance.PlayAmbiance(ambianceClip, playAmbianceLoop);
                AudioManager.Instance.GetMusicSource().Stop();
            }
        }
    }
}
