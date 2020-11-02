using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private float width; 

    private float height;

    [SerializeField] private Vector2 relativeMargin;

    [SerializeField] private Vector2 speed;


    public Vector2 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }



    // Start is called before the first frame update
    void Start()
    {
     height = GetComponent<Camera>().orthographicSize;
     width = GetComponent<Camera>().aspect * height;  
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = player.transform.position;
        Vector3 camPos = transform.position; 

        if (playerPos.y >= camPos.y + height*(1 - relativeMargin.y)) {
            camPos.y += speed.y * Time.deltaTime;
        }
        else if (playerPos.y <= camPos.y - height*(1 - relativeMargin.y)) {
            camPos.y -= speed.y * Time.deltaTime;
        }
        camPos.z = -10;
        transform.position = camPos;
    }
}
