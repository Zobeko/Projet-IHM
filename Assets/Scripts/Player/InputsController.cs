using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InputsController : MonoBehaviour
{
    
    [SerializeField] private Vector2 playerSpeed;
    [SerializeField] private float gravityUp;
    [SerializeField] private float gravityDown;
    [SerializeField] private float maxSpeed;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float jumpForce;
    [SerializeField] private float raycastDistance;
    [SerializeField] private LayerMask layerNotTraversablePlatforms;
    [SerializeField] private LayerMask layerTraversablePlatforms;
    [SerializeField] private Vector2 playerPosition;
    public Vector2 PlayerPosition
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    private bool isGrounded = false;

    private int jumpsCounter = 0;



    void Start()
    {
        playerSpeed = Vector2.zero;
    }

    void Update()
    {
        

        RaycastCollision();



        //Gere les déplacements horizontaux du joueur
        HorizMovementsInputs();

        //Gere le saut et double saut
        JumpInputs();

        //Update la position du joueur à chaque frame
        UpdatePosition();
    }


    private void PlayerGravity()
    {
        if(playerSpeed.y > 0)
        {
            playerSpeed.y -= gravityUp;
        }
        else if (playerSpeed.y <= 0)
        {
            playerSpeed.y -= gravityDown;
        }
        
    }

    private void UpdatePosition()
    {
        PlayerPosition += playerSpeed * Time.deltaTime;
    }

    

    private void HorizMovementsInputs()
    {

        //Calcul des déplacements horizontaux en fonction des inputs joystick gauche
        float horizMvt = Input.GetAxis("Horizontal");

        


        if(horizMvt >= 0.1f)
        {
            playerSpeed.x = horizMvt * maxSpeed;
        }
        else if (horizMvt <= -0.1f )
        {
            playerSpeed.x = horizMvt * maxSpeed;
        }
        else
        {
            playerSpeed.x = 0;
        }

        
        //Gere le saut simple
        

        //playerSpeed = speed * maxSpeed;
        
    }

    private void JumpInputs()
    {
        if (Input.GetButtonDown("Jump") && jumpsCounter < maxJumps)
        {
            playerSpeed.y = jumpForce;
            jumpsCounter++;
        }


        //playerSpeed = speed * maxSpeed;
    }

    private void RaycastCollision()
    {
        //Raycast vers le bas pour detecter collisions plateformes
        if(Physics2D.Raycast(PlayerPosition, Vector2.down, transform.lossyScale.y, layerNotTraversablePlatforms + layerTraversablePlatforms))
        {
            isGrounded = true;
            jumpsCounter = 0;
            playerSpeed.y = Mathf.Max(0, playerSpeed.y);
        }
        else
        {
            isGrounded = false;
            PlayerGravity();
        }
        
        if(Physics2D.Raycast(PlayerPosition, Vector2.up, transform.lossyScale.y, layerNotTraversablePlatforms))
        {
            playerSpeed.y = -gravityDown;
            PlayerGravity();
            
        }

    }

}
