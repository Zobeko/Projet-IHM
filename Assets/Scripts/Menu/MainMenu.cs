using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel = null;

    public void play() 
    {
        this.gameObject.SetActive(false);
    }

    public void options()
    {
        optionsPanel.SetActive(true);
    }

    public void quitoptionsButton()
    {
        optionsPanel.SetActive(false);
    }

    public void quit() 
    {
        Application.Quit();
    }
}
