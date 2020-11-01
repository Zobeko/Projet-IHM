using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Vector2 speed;

    public Vector2 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    [SerializeField] float maxSpeed = 3;

     [SerializeField] private LayerMask layerNotTraversablePlatforms;
    [SerializeField] private LayerMask layerTraversablePlatforms;

    private float width=0;
    private float height=0;

    void Awake()
    {
        width = this.gameObject.GetComponent<BoxCollider2D>().bounds.size.x/2;
        height = this.gameObject.GetComponent<BoxCollider2D>().bounds.size.y/2;
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = new Vector2(maxSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 leftCorner = new Vector2( - width, - height);
        Vector2 rightCorner = new Vector2( width,  - height);

        float rayLength = 1;

        Debug.DrawRay(Position + leftCorner, Vector2.down * rayLength, Color.red);
        Debug.DrawRay(Position + rightCorner, Vector2.down * rayLength, Color.red);

        if (speed.x < 0 && 
        !Physics2D.Raycast(Position + leftCorner, Vector2.down, rayLength, layerNotTraversablePlatforms + layerTraversablePlatforms))
        {
            speed.x *= -1;   
        }
        else if (speed.x > 0 && 
        !Physics2D.Raycast(Position + rightCorner, Vector2.down, rayLength, layerNotTraversablePlatforms + layerTraversablePlatforms))
        {
            speed.x *= -1;    
        }

        Position += speed * Time.deltaTime;
    }
}
