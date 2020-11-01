using UnityEngine;

public class BouncingPlatform : MonoBehaviour
{
    [SerializeField] private int numberOfRaycasts = 0;
    [SerializeField] private float raycastsDistanceOffset = 0;
    [SerializeField] private float bouncingForce = 0;
    [SerializeField] private GameObject player = null;
    [SerializeField] private LayerMask playerLayerMask;
    private InputsController playerInputController = null;
    


    private float width = 0;
    private float height = 0;

    void Awake()
    {
        width = this.GetComponent<BoxCollider2D>().bounds.size.x;
        height = this.GetComponent<BoxCollider2D>().bounds.size.y;

        
    }

    void Start()
    {
        playerInputController = player.GetComponent<InputsController>();
    }

    void Update()
    {
        RaycastsUp();
    }

    private void RaycastsUp()
    {
        for (int i = 0; i < numberOfRaycasts; i++)
        {
            Vector2 raycastOrigin = new Vector2((this.transform.position.x - (width / 2) + i * (width / (numberOfRaycasts-1))), this.transform.position.y);

            if(Physics2D.Raycast(raycastOrigin, Vector2.up, (height/2) + raycastsDistanceOffset, playerLayerMask))
            {
                playerInputController.playerSpeed.y = bouncingForce;
                playerInputController.jumpsCounter = 1; //Pour pouvoir faire un saut apres le saut dû à la plateforme
            }
            //Debug.DrawRay(raycastOrigin, Vector2.up, Color.red, Mathf.Infinity);

          
        }
        
    }
}
