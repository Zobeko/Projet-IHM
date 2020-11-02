using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel = null;
    [SerializeField] private GameObject inputsPanel = null;

    [SerializeField] private GameObject mainMenu = null;


    

    void Awake()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;
        Time.timeScale = 1f;

    }
    
    void update() 
    {
        Debug.Log("a");
        if(Input.GetButton("A")) {
            Debug.Log("a");
            play();
        }
    }


    public void play() 
    {
        mainMenu.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void options()
    {
        optionsPanel.SetActive(true);
    }

    public void quitoptionsButton()
    {
        optionsPanel.SetActive(false);
    }

    public void InputsPanel()
    {
        inputsPanel.SetActive(true);
    }

    public void quitInputsPanel()
    {
        inputsPanel.SetActive(false);
    }

    public void quit() 
    {
        Application.Quit();
    }

    

    
}
