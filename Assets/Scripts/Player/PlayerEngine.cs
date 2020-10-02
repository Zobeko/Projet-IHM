using UnityEngine;

public class PlayerEngine : MonoBehaviour
{
    //Current player speed
    [SerializeField]
    private Vector2 playerSpeed;
    public Vector2 PlayerSpeed
    {
        get { return playerSpeed; }
        set
        {
            playerSpeed = value;
        }
    }


    //Current Player position
    [SerializeField]
    private Vector2 playerPosition;
    private Vector2 PlayerPosition
    {
        get { return this.transform.position; }
        set
        {
            this.transform.position = value;
        }
    }

    [SerializeField] float gravity;
    



    void Start()
    {
        PlayerSpeed = Vector2.zero;
    }

    void Update()
    {
        PlayerGravity();

        PlayerMovements();

    }

    private void PlayerGravity()
    {
        playerSpeed.y -= gravity * Time.deltaTime;
    }

    private void PlayerMovements()
    {
        PlayerPosition += PlayerSpeed;
    }
}
