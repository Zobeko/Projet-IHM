using UnityEngine;
using UnityEngine.UIElements;

public class PlatformMovements : MonoBehaviour
{
    public float speed = 0f;
    [SerializeField] private float amplitude = 0f;
    private Vector2 initialPosition = Vector2.zero;
    public Vector2 Position
    {
        get { return this.transform.position; }
        set { this.transform.position = value; }
    }

    private bool directionChange;
    public bool DirectionChange
    {
        get { return directionChange; }
        set
        {
            directionChange = value;
            speed *= -1;
        }
    }

    void Awake()
    {
        initialPosition = this.transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        if(Position.x >= (initialPosition.x + amplitude))
        {
            DirectionChange = false;
        }
        else if (Position.x <= (initialPosition.x - amplitude))
        {
            DirectionChange = true;
        }



        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Position += new Vector2(speed * Time.deltaTime, 0f);
    }
}
