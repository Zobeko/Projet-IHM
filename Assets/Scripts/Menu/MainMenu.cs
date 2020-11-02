using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel = null;
    
    [SerializeField] private GameObject mainMenu = null;


    

    void Awake()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
    


    public void play() 
    {
        mainMenu.SetActive(false);
        Cursor.visible = false;
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
