﻿using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InputsController : MonoBehaviour
{
    
    [SerializeField] private Vector2 playerSpeed;
    [SerializeField] float gravityUp;
    [SerializeField] float gravityDown;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float raycastDistance;
    [SerializeField] private LayerMask layers;
    [SerializeField] private Vector2 playerPosition;
    public Vector2 PlayerPosition
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    private bool isGrounded = false;



    private Vector2 speed;

    void Start()
    {
        playerSpeed = Vector2.zero;
    }

    void Update()
    {
        

        RaycastCollision();

        //Gere la gravité appliquée au joueur
        PlayerGravity();

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
            playerSpeed.x = horizMvt;
        }
        else if (horizMvt <= -0.1f )
        {
            playerSpeed.x = horizMvt;
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
        if (Input.GetButtonDown("Jump"))
        {
            playerSpeed.y = jumpForce;
        }


        //playerSpeed = speed * maxSpeed;
    }

    private void RaycastCollision()
    {
        //RaycastHit2D hit;
        if(Physics2D.Raycast(PlayerPosition, Vector2.down, transform.lossyScale.y, layers))
        {
            isGrounded = true;

            gravityDown = 0;
            gravityUp = 0;
            playerSpeed.y = 0;
        }
        else
        {
            isGrounded = false;
        }


    }

}
