﻿using UnityEditor;
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
    [SerializeField] private Vector2 playerSpeed;
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
    private int jumpsCounter = 0;
    private float width=0;
    private float height=0;

    void Awake()
    {
        width = this.transform.lossyScale.x;
        height = this.transform.lossyScale.y;
    }
    void Start()
    {
        playerSpeed = Vector2.zero;
    }

    void Update()
    {
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
    private bool testOneFaceCollisions(Vector2 a, Vector2 b, LayerMask layerMask)
    {
        Vector2 middle = (a + b) * 0.5f;

        RaycastHit2D hitA = Physics2D.Raycast(
            PlayerPosition + a,
            playerSpeed,
            norme(playerSpeed) * Time.deltaTime,
            layerMask);

        RaycastHit2D hitB = Physics2D.Raycast(
            PlayerPosition + b,
            playerSpeed,
            norme(playerSpeed) * Time.deltaTime,
            layerMask);

        RaycastHit2D hitMiddle = Physics2D.Raycast(
            PlayerPosition + middle,
            playerSpeed,
            norme(playerSpeed) * Time.deltaTime,
            layerMask);

        if (hitA || hitB || hitMiddle)
        {
            //si collision avec le coté droit d'une plateforme
            if(Mathf.Max(hitMiddle.normal.x, hitA.normal.x, hitB.normal.x) > 0.8 && playerSpeed.x < 0)
            {
                playerSpeed.x = 0;
            }
            //si collision avec le coté gauche d'une plateforme
            if(Mathf.Min(hitMiddle.normal.x, hitA.normal.x, hitB.normal.x) < -0.8 && playerSpeed.x > 0)
            {
                playerSpeed.x = 0;
            }
            //collision avec le haut d'une plateforme
            if(Mathf.Max(hitMiddle.normal.y, hitA.normal.y, hitB.normal.y) > 0.8 && playerSpeed.y < 0)
            {
                playerSpeed.y = 0;
            } 
            //collision avec le bas d'une plateforme
            if(Mathf.Min(hitMiddle.normal.y, hitA.normal.y, hitB.normal.y) < -0.8 && playerSpeed.y > 0 && layerMask == layerNotTraversablePlatforms)
            {
                playerSpeed.y = 0;
            } 
            float distanceToHit = Mathf.Min(hitMiddle.distance, hitA.distance, hitB.distance);
            playerPosition += playerSpeed.normalized * distanceToHit;
            return true;
        }

        return false;
    }
    private void RaycastCollision()
    {

        //Collision avec le haut d'un plateforme
        if (playerSpeed.y < 0 && testOneFaceCollisions(new Vector2(-width, -height), new Vector2(width, -height), layerNotTraversablePlatforms+layerTraversablePlatforms))
        {
            isGrounded = true;
            jumpsCounter = 0;
        }
        else
        {
            isGrounded = false;
        }

        //left
        if (playerSpeed.x < 0 && testOneFaceCollisions(new Vector2(-width, -height), new Vector2(-width, height), layerNotTraversablePlatforms + layerTraversablePlatforms))
        {
            
            int dir = -1; //Mur à gauche du joueur donc dir = -1
            WallJumpInput(dir);
        }

        //right
        if (playerSpeed.x > 0 && testOneFaceCollisions(new Vector2(width, -height), new Vector2(width, height), layerNotTraversablePlatforms + layerTraversablePlatforms))
        {
            
            
            int dir = 1; //Mur à droite du joueur donc dir = 1
            WallJumpInput(dir);
        }

        //Collision avec le bas d'une plateforme
        if (playerSpeed.y > 0 && testOneFaceCollisions(new Vector2(width, height), new Vector2(-width, height), layerNotTraversablePlatforms))
        {
            playerSpeed.y = -gravityDown;
        }
    }


}
