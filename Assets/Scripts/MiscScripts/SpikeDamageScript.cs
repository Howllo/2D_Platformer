using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

public class SpikeDamageScript : MonoBehaviour
{
    public float damage = 10;
    public float timeToDamage = 0.5f;
    private bool takingDamage = false;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !takingDamage)
        {
            PlayerCharacter pc =  PlayerCharacter.Instance;
            //pc.pc.GetRB().velocity.x = new Vector3(0.0f, 0.0f, 0.0f);
            pc.TakeDamage(damage);
            takingDamage = true;
            StartCoroutine(TakeDamage());
        }
    }

    IEnumerator TakeDamage()
    {
        yield return new WaitForSeconds(timeToDamage);
        takingDamage = false;
    }
}
