using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel = null;
    [SerializeField] private GameObject inputsPanel = null;

    [SerializeField] private GameObject mainMenu = null;
    
    [SerializeField] private GameObject audioButton = null;

    

    void Awake()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;


    }
    
    void update() 
    {
        Debug.Log("a");
        if(Input.GetButton("A")) {
            Debug.Log("a");
            play();
        }
    }

    public void AudioButton() {
        if (AudioListener.volume == 0)
        {
            audioButton.GetComponent<TextMeshProUGUI>().text = "Desactiver Son";
            AudioListener.volume = 1;
        }
        else {
            audioButton.GetComponent<TextMeshProUGUI>().text = "Activer Son";
            AudioListener.volume = 0;
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
