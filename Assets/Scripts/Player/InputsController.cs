using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class InputsController : MonoBehaviour
{
    [Header("Gravity")]
    [SerializeField] private float gravityUp;
    [SerializeField] private float gravityDown;

    [Header("Player attributes")]
    [SerializeField] private Vector2 playerPosition;
    public Vector2 PlayerPosition
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
    public Vector2 playerSpeed;
    [SerializeField] private float maxSpeed;

    [Header("Jump")]
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isWallJumping = false;
    [SerializeField] private float wallJumpDelay = 0;
    [SerializeField] private float wallJumpForceX = 0;
    [SerializeField] private float wallJumpForceY = 0;

    [Header("Sprint")]
    [SerializeField] private float sprintFactor;

    [Header("Dash")]
    [SerializeField] private float dashForce;

    [Header("LayerMasks")]
    [SerializeField] private LayerMask layerNotTraversablePlatforms;
    [SerializeField] private LayerMask layerTraversablePlatforms;


    private bool isGrounded = false;
    public int jumpsCounter = 0;
    private float width=0;
    private float height=0;

    void Awake()
    {
        width = this.gameObject.GetComponent<BoxCollider2D>().bounds.size.x/2;
        height = this.gameObject.GetComponent<BoxCollider2D>().bounds.size.y/2;
    }
    void Start()
    {
        playerSpeed = Vector2.zero;
    }

    void FixedUpdate() {

        //Gere les déplacements horizontaux du joueur
        if (!isWallJumping)
        {
            HorizMovementsInputs();
        }

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
    }

    void Update()
    {
        

        

        
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
    
    //La variable _dir vaut 1 si le mur est à la droite du joueur et -1 si il est à gauche (permet de calculer la bonne force à metre en X en fonction de la direction du mur)
    private void WallJumpInput(int _dir)
    {
        if (!isGrounded && Input.GetButtonDown("Jump"))
        {
            //On remet le jumpsCounter à 1 pour que, après un wall jump, le joueur puisse ressauter une fois de plus (meme s'il a deja fais double saut)
            jumpsCounter = 1;
            isWallJumping = true;
            //Utilisation de Incoke afin de mettre isWalllJumping à false "wallJumpDelay" secondes après le wall jump (utile pour bloquer inputs mvt horizontals)
            Invoke("setIsWallJumpingToFalse", wallJumpDelay);
            playerSpeed = new Vector2(-_dir*wallJumpForceX, wallJumpForceY);
        }
    }

    private void setIsWallJumpingToFalse()
    {
        isWallJumping = false;
    }

    //Retourne la norme d'un vector2
    private float norme(Vector2 v)
    {
        return Mathf.Sqrt(v.x * v.x + v.y * v.y);
    }

    /* test la collision d'une seule face définie par 2 points A et B avec les plateformes
    les vecteurs a et b sont données dans le referentiel du personnage
    si il y a eu une collision, renvoie vraie et replace le personnage et change sa vitesse */
    private bool TestOneFaceCollisions(Vector2 a, Vector2 b, LayerMask layerMask)
    {
        Vector2 middle = (a + b) * 0.5f;

        //vecteur normale à la face du joueur testée
        Vector2 playersNormal = middle.normalized;
        float normalSpeed = Vector2.Dot(playerSpeed, playersNormal);

        int n = 5;
        RaycastHit2D[] hits = new RaycastHit2D[n];
        Vector2 rayOrigin;
        float minimalDistanceToHit = -1;

        for (int k = 0; k < n; k++)
        {
            rayOrigin = ((float)k) * (b - a) / ((float)n - 1) + a;
        
            hits[k] = Physics2D.Raycast(
            PlayerPosition + rayOrigin,
            playersNormal,
            Mathf.Abs(normalSpeed) * Time.deltaTime,
            layerMask);

            Debug.DrawRay(playerPosition + rayOrigin, playersNormal * Mathf.Abs(normalSpeed) * Time.deltaTime, Color.blue);
        }
   
        foreach(RaycastHit2D hit in hits) 
        {
            if (hit && Vector2.Dot(hit.normal, playersNormal) < -0.5 
            && (minimalDistanceToHit > hit.distance || minimalDistanceToHit == -1)) {
                minimalDistanceToHit = hit.distance;
            }
        }

        if (minimalDistanceToHit > 0) {
            playerPosition += playerSpeed.normalized * minimalDistanceToHit;
            return true;
        }

        return false;
    }
    private void RaycastCollision()
    {

        //Collision avec le haut d'un plateforme
        if (playerSpeed.y < 0 && TestOneFaceCollisions(new Vector2(-width, -height), new Vector2(width, -height), layerNotTraversablePlatforms+layerTraversablePlatforms))
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
        if (playerSpeed.x < 0 && TestOneFaceCollisions(new Vector2(-width, -height), new Vector2(-width, height), layerNotTraversablePlatforms + layerTraversablePlatforms))
        {
            playerSpeed.x = 0; 
            int dir = -1; //Mur à gauche du joueur donc dir = -1
            WallJumpInput(dir);
        }

        //right
        if (playerSpeed.x > 0 && TestOneFaceCollisions(new Vector2(width, -height), new Vector2(width, height), layerNotTraversablePlatforms + layerTraversablePlatforms))
        {
            playerSpeed.x = 0;       
            int dir = 1; //Mur à droite du joueur donc dir = 1
            WallJumpInput(dir);
        }

        //Collision avec le bas d'une plateforme
        if (playerSpeed.y > 0 && TestOneFaceCollisions(new Vector2(width, height), new Vector2(-width, height), layerNotTraversablePlatforms))
        {
            playerSpeed.y = -gravityDown;
        }
    }


}
