using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameScript : MonoBehaviour
{
    [SerializeField]
    private Button quitButton;

    private void Start()
    {
        quitButton = GetComponent<Button>();
        quitButton.onClick.AddListener(OnClickQuit);
    }

    void OnClickQuit()
    {
        Application.Quit();
    }
}