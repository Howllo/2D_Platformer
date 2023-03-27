using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using MiscScripts;
using UnityEngine;

public class BridgeScript : MonoBehaviour
{
    [SerializeField] private CircleCollider2D circleCollider2D;
    [SerializeField] private Sprite sprite;
    [SerializeField] private AudioClip clip;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<GameObject> bridgePieces = new List<GameObject>();
    [SerializeField] private float timeBetweenGameObjects;
    [SerializeField] private bool checkForPlayerInput;
    private int i = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var go in bridgePieces)
        {
            go.SetActive(false);
        }
    }

    private void Update()
    {
        if(!checkForPlayerInput) return;
        if (Input.GetKeyDown(KeyCode.F))
        {
            circleCollider2D.enabled = false;
            spriteRenderer.sprite = sprite;
            AudioManager.Instance.PlaySoundEffect(clip);
            StartCoroutine(BuildBridge());
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerCharacter.Instance.GetPlayerUserInterface().interactObject.SetActive(true);
            checkForPlayerInput = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCharacter.Instance.GetPlayerUserInterface().interactObject.SetActive(false);
            checkForPlayerInput = false;
        }
    }

    private IEnumerator BuildBridge()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenGameObjects);
            bridgePieces[i].SetActive(true);
            i++;
            if (i == bridgePieces.Count)
                break;
        }
    }
}
