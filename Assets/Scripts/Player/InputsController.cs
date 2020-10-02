using UnityEditor;
using UnityEngine;

public class InputsController : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpFactor;

    private PlayerAvatar playerAvatar;
    private PlayerEngine playerEngine;

    private Vector2 speed;

    void Awake()
    {
        playerAvatar = GetComponent<PlayerAvatar>();
        playerEngine = GetComponent<PlayerEngine>();
    }

    void Update()
    {
        HorizMovementsInputs();
    }

    private void HorizMovementsInputs()
    {

        //Calcul des déplacements horizontaux en fonction des inputs joystick gauche
        float horizMvt = Input.GetAxis("Horizontal");

        


        if(horizMvt >= 0.1f)
        {
            speed = Vector2.right *horizMvt;
        }
        else if (horizMvt <= -0.1f )
        {
            speed = Vector2.right * horizMvt;
        }
        else
        {
            speed = Vector2.right * 0;
        }

        if(playerEngine.PlayerSpeed.y > 0)
        {
            speed.y += jumpFactor;
        }
        //Gere le saut simple
        if (Input.GetButtonDown("Jump"))
        {
            speed = new Vector2(horizMvt, jumpForce);
        }

        playerEngine.PlayerSpeed = speed * playerAvatar.MaxSpeed * Time.deltaTime;
        
    }

    private void JumpInputs()
    {

        
    }

}
