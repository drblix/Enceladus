using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class PlayerRaycast : MonoBehaviour
{
    private SubmarineControls subControls;
    private CameraManager camManager;

    [SerializeField]
    private Transform plrCam;

    [SerializeField]
    private Image plrCursor;

    [SerializeField]
    private Color defaultColor;
    [SerializeField]
    private Color interactColor;

    private int controlMask;

    private const float RAY_DISTANCE = 4f;

    private void Awake()
    {
        subControls = FindObjectOfType<SubmarineControls>();
        camManager = FindObjectOfType<CameraManager>();

        controlMask = LayerMask.GetMask("SubControls");
    }

    private void FixedUpdate()
    {
        Ray camRay = new Ray(plrCam.position, plrCam.forward);

        if (Physics.Raycast(camRay, out RaycastHit hitInfo, RAY_DISTANCE, controlMask))
        {
            plrCursor.color = interactColor;
        
            if (Mouse.current.leftButton.isPressed)
            {
                string btnName = hitInfo.collider.name;
                GameObject btn = hitInfo.collider.gameObject;

                if (btnName == "ForwardButton")
                {
                    subControls.ToggleControl("forward", true, btn);
                }
                else if (btnName == "BackButton")
                {
                    subControls.ToggleControl("backwards", true, btn);
                }
                else if (btnName == "RightButton")
                {
                    subControls.ToggleControl("right", true, btn);
                }
                else if (btnName == "LeftButton")
                {
                    subControls.ToggleControl("left", true, btn);
                }
                else if (btnName == "CamButton")
                {
                    StartCoroutine(camManager.ButtonPressed());
                }
            }
            else
            {
                subControls.ToggleControl("disableall", false, null);
            }
        }
        else
        {
            subControls.ToggleControl("disableall", false, null);
            plrCursor.color = defaultColor;
        }
    }
}