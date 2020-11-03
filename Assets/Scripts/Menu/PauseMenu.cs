using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel = null;
    [SerializeField] private GameObject pauseMenu = null;
    private bool isPauseActivated = false;

    [SerializeField] private GameObject audioButton = null;


    void Update()
    {
        escapeButton();
    }

    public void ResumeButton()
    {
        pauseMenu.SetActive(false);
        isPauseActivated = false;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void AudioButton() {
        Debug.Log("ferljgeruoh");
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
    
    public void options()
    {
        Cursor.visible = true;
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

    private void escapeButton()
    {
        if (Input.GetButtonDown("Escape"))
        {

            if (!isPauseActivated)
            {
                isPauseActivated = true;
                pauseMenu.SetActive(true);
                //Time.timeScale = 0f;
                Cursor.visible = false;
            }
            else
            {
                isPauseActivated = false;
                pauseMenu.SetActive(false);
                Time.timeScale = 1f;
                Cursor.visible = true;
            }
        }
    }
}
