using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : Trigger
{
    [SerializeField] private GameObject endScreen = null;

    override public void TriggerAction(InputsController player){
        endScreen.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
    }

    
}  
