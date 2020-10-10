using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InputsController : MonoBehaviour
{
    
    [SerializeField] private Vector2 playerSpeed;
    [SerializeField] private float gravityUp;
    [SerializeField] private float gravityDown;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float dashForce;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask layerNotTraversablePlatforms;
    [SerializeField] private LayerMask layerTraversablePlatforms;

    [SerializeField] private float sprintFactor;

    [SerializeField] private Vector2 playerPosition;
    public Vector2 PlayerPosition
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public bool isGrounded = false;

    private int jumpsCounter = 0;

    void Start()
    {
        playerSpeed = Vector2.zero;
    }

    void Update()
    {  
        //Gere les déplacements horizontaux du joueur
        HorizMovementsInputs();

        //Gere le saut et double saut
        JumpInputs();

        //Gere le sprint
        SprintInput();

        //Gere le dash
        DashInput();

        //Gere la gravité
        PlayerGravity();

        //Gere les collisions avec les plateformes
        RaycastCollision();

        //Update la position du joueur à chaque frame
        UpdatePosition();

        Debug.Log(PlayerPosition);
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
    }

    private void JumpInputs()
    {
        if (Input.GetButtonDown("Jump") && jumpsCounter < maxJumps)
        {
            playerSpeed.y = jumpForce;
            jumpsCounter++;
            isGrounded = false;
        }


        //playerSpeed = speed * maxSpeed;
    }

    private void SprintInput()
    {
        if (Input.GetButton("Sprint"))
        {
            playerSpeed.x *= sprintFactor;
        }
    }

    private void DashInput()
    {
        if (Input.GetButtonDown("Dash"))
        {
            if (playerSpeed.x >= 0)
            {
                playerSpeed.x += dashForce;
            }
            else
            {
                playerSpeed.x += -dashForce;
            }
        }
    }

    //Retourne la norme d'un vector2
    private float norme(Vector2 v)
    {
        return Mathf.Sqrt(v.x * v.x + v.y * v.y);
    }

    /* test la collision d'une seule face définie par 2 points A et B avec les plateformes
    les vecteurs a et b sont données dans le referentiel du personnage
    si il y a eu une collision, renvoie vraie et replace le personnage */
    private bool testOneFaceCollisions(Vector2 a, Vector2 b)
    {
        Vector2 middle = (a + b) * 0.5f;

        RaycastHit2D hitA = Physics2D.Raycast(
            PlayerPosition + a,
            playerSpeed,
            norme(playerSpeed) * Time.deltaTime,
            layerNotTraversablePlatforms + layerTraversablePlatforms);

        RaycastHit2D hitB = Physics2D.Raycast(
            PlayerPosition + b,
            playerSpeed,
            norme(playerSpeed) * Time.deltaTime,
            layerNotTraversablePlatforms + layerTraversablePlatforms);

        RaycastHit2D hitMiddle = Physics2D.Raycast(
            PlayerPosition + middle,
            playerSpeed,
            norme(playerSpeed) * Time.deltaTime,
            layerNotTraversablePlatforms + layerTraversablePlatforms);

        if (hitA || hitB || hitMiddle)
        {
            float distanceToHit = Mathf.Min(hitMiddle.distance, hitA.distance, hitB.distance);
            playerPosition += playerSpeed.normalized * distanceToHit;
            return true;
        }

        return false;
    }

    private void RaycastCollision()
    {
        float width = 0.5f; //GetComponent<BoxCollider2D>().size.x;
        float height = 0.5f;

        //bottom
        if (playerSpeed.y < 0 && testOneFaceCollisions(new Vector2(-width, -height), new Vector2(width, -height)))
        {
            isGrounded = true;
            jumpsCounter = 0;
            playerSpeed.y = 0;
        }
        else
        {
            isGrounded = false;
        }

        //left
        if (playerSpeed.x < 0 && testOneFaceCollisions(new Vector2(-width, -height), new Vector2(-width, height)))
        {
            jumpsCounter = 0;
            playerSpeed.x = 0;
        }

        //right
        if (playerSpeed.x > 0 && testOneFaceCollisions(new Vector2(width, -height), new Vector2(width, height)))
        {
            jumpsCounter = 0;
            playerSpeed.x = 0;
        }

        //top
        if (Physics2D.Raycast(PlayerPosition, Vector2.up, transform.lossyScale.y, layerNotTraversablePlatforms))
        {
            playerSpeed.y = -gravityDown;
        }

    }



}
