using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel = null;
    [SerializeField] private GameObject pauseMenu = null;
    private bool isPauseActivated = false;


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
                Time.timeScale = 0f;
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
