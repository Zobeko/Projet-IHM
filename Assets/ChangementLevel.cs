using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangementLevel : Trigger
{
    override public void TriggerAction(InputsController player)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
