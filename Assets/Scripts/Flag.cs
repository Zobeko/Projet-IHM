using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : Trigger
{
    override public void TriggerAction(InputsController player){
        player.Die();
    }
}  
