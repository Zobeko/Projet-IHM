using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadlyPlatform : MonoBehaviour
{

    [SerializeField] private int numberOfRaycasts = 0;
    public float tolerance = 0f;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private GameObject player;

    private float width = 0f;
    private float height = 0f;


    void Awake()
    {
        width = this.gameObject.GetComponent<BoxCollider2D>().bounds.size.x;
        height = this.gameObject.GetComponent<BoxCollider2D>().bounds.size.y;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Raycasts();
    }

    private void Raycasts()
    {
        for (int i = 0; i < numberOfRaycasts; i++)
        {
            Vector2 raycastOriginWidth = new Vector2((this.transform.position.x - (width / 2) + i * (width / (numberOfRaycasts - 1))), this.transform.position.y);
            Vector2 raycastOriginHeight = new Vector2(this.transform.position.x, this.transform.position.y - (height / 2) + i * (height / (numberOfRaycasts - 1)));

            if (Physics2D.Raycast(raycastOriginWidth, Vector2.up, (height / 2) + tolerance, playerLayerMask) || Physics2D.Raycast(raycastOriginWidth, Vector2.down, (height / 2) + tolerance, playerLayerMask) || Physics2D.Raycast(raycastOriginHeight, Vector2.right, (width / 2) + tolerance, playerLayerMask) || Physics2D.Raycast(raycastOriginHeight, Vector2.left, (width / 2) + tolerance, playerLayerMask))
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                player.GetComponent<InputsController>().Die();
               // audioSource.PlayOneShot(deathAudioClip);
            }



        }

    }
}
