using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerRaycast : MonoBehaviour
{
    private static PlayerRaycast _instance;
    public static PlayerRaycast Instance { get { return _instance; } }

    private CameraManager _camManager;

    [SerializeField] private Transform _plrCam;

    [SerializeField] private Image _plrCursor;

    [SerializeField] private Color _defaultColor, _interactColor;

    private int _controlMask;

    private const float RAY_DISTANCE = 4f;

    private void Awake()
    {
        _camManager = FindObjectOfType<CameraManager>();

        _controlMask = LayerMask.GetMask("SubControls");

        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    private void Update()
    {
        Ray camRay = new (_plrCam.position, _plrCam.forward);

        if (Physics.Raycast(camRay, out RaycastHit hitInfo, RAY_DISTANCE, _controlMask))
        {
            _plrCursor.color = _interactColor;
        
            if (Mouse.current.leftButton.isPressed)
            {
                string btnName = hitInfo.collider.name;
                GameObject btn = hitInfo.collider.gameObject;

                if (btnName == "ForwardButton")
                {
                    SubmarineControls.Instance.ToggleControl("forward", true, btn);
                }
                else if (btnName == "BackButton")
                {
                    SubmarineControls.Instance.ToggleControl("backwards", true, btn);
                }
                else if (btnName == "RightButton")
                {
                    SubmarineControls.Instance.ToggleControl("right", true, btn);
                }
                else if (btnName == "LeftButton")
                {
                    SubmarineControls.Instance.ToggleControl("left", true, btn);
                }
                else if (btnName == "CamButton")
                {
                    StartCoroutine(_camManager.ButtonPressed());
                }
            }
            else
            {
                SubmarineControls.Instance.ToggleControl("disableall", false, null);
            }
        }
        else
        {
            SubmarineControls.Instance.ToggleControl("disableall", false, null);
            _plrCursor.color = _defaultColor;
        }
    }
}