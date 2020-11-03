using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SlidersManager : MonoBehaviour
{
    [SerializeField] private Slider graviteEnMonteeSlider = null;
    [SerializeField] private Slider graviteEnDescenteSlider = null;
    [SerializeField] private Slider vitesseMaximaleSlider = null;
    [SerializeField] private Slider forceDuSautSlider = null;
    [SerializeField] private Slider multiplicateurSprintSlider = null;
    [SerializeField] private Slider forceDuDashSlider = null;
    [SerializeField] private Slider toleranceSautSlider = null;
    [SerializeField] private Slider vitessePlateformesMouvantesSlider = null;
    [SerializeField] private Slider tolerancePlateformesSlider = null;
    [SerializeField] private Slider forceRebondissementSlider = null;


    [SerializeField] private GameObject player = null;
    private InputsController playerInputsController = null;

    [SerializeField] private GameObject movingPlatform = null;
    private PlatformMovements platformMovements = null;

    [SerializeField] private GameObject deadlyPlatform = null;
    private DeadlyPlatform deadlyPlatformScript = null;

    [SerializeField] private GameObject bouncingPlatform = null;
    private BouncingPlatform bouncingPlatformScript = null;

    
    void Awake()
    {
        playerInputsController = player.GetComponent<InputsController>();
        platformMovements = movingPlatform.GetComponent<PlatformMovements>();
        deadlyPlatformScript = deadlyPlatform.GetComponent<DeadlyPlatform>();
        bouncingPlatformScript = bouncingPlatform.GetComponent<BouncingPlatform>();

        
    }

    // Update is called once per frame
    void Update()
    {
        playerInputsController.gravityUp = graviteEnMonteeSlider.value;
        playerInputsController.gravityDown = graviteEnDescenteSlider.value;

        playerInputsController.maxSpeed = vitesseMaximaleSlider.value;
        playerInputsController.jumpForce = forceDuSautSlider.value;
        playerInputsController.sprintFactor = multiplicateurSprintSlider.value;
        playerInputsController.dashForce = forceDuDashSlider.value;
        playerInputsController.jumpTolerance = toleranceSautSlider.value;

        platformMovements.speed = vitessePlateformesMouvantesSlider.value;
        bouncingPlatformScript.tolerance = tolerancePlateformesSlider.value;
        deadlyPlatformScript.tolerance = tolerancePlateformesSlider.value;
        bouncingPlatformScript.bouncingForce = forceRebondissementSlider.value;


    }
}
