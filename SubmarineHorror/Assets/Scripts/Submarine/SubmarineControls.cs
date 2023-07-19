using UnityEngine;
using TMPro;


public class SubmarineControls : MonoBehaviour
{
    private static SubmarineControls _instance;
    public static SubmarineControls Instance { get { return _instance; } }


    private CameraManager _cameraManager;

    [SerializeField] private Transform _subObj;
    [SerializeField] private Transform _orienDisplay;
    
    [SerializeField] private GameObject[] _movementButtons;

    [SerializeField] private TextMeshPro _xDisplay;
    [SerializeField] private TextMeshPro _yDisplay;
    [SerializeField] private TextMeshPro _degDisplay;

    [SerializeField] private bool _debugging = false;

    private Rigidbody _subRb;
    private AudioSource _waterNoise;

    private bool _movingFor = false, _movingBack = false, _rotRight = false, _rotLeft = false;

    private Vector3 startingPos = new Vector3Int(43, 3, 32);

    private const float ACCELERATION_SPEED = 25f;
    private const float MAX_SPEED = 4.5f;
    private const float MAX_ROT_SPEED = 0.5f;
    private const float BUTTON_MIN_Y = -0.759f;
    private const float SOUND_THRESHOLD = 0.1f;

    private void Awake()
    {
        _cameraManager = FindObjectOfType<CameraManager>();
        _subRb = _subObj.GetComponent<Rigidbody>();
        _waterNoise = transform.Find("WaterNoise").GetComponent<AudioSource>();

        if (!_debugging)
            _subRb.transform.position = startingPos;

        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    private void FixedUpdate()
    {
        Controls();
        ComputeSound();
    }

    private void Controls()
    {
        if (_movingFor)
        {
            _subRb.drag = 0.6f;
            _subRb.AddForce(_subObj.forward * ACCELERATION_SPEED);
            _cameraManager.ClearScreen();
        }
        else if (_movingBack)
        {
            _subRb.drag = 0.6f;
            _subRb.AddForce(-_subObj.forward * ACCELERATION_SPEED);
            _cameraManager.ClearScreen();
        }
        else if (_rotRight)
        {
            _subRb.AddTorque(Vector3.up);
            _cameraManager.ClearScreen();
        }
        else if (_rotLeft)
        {
            _subRb.AddTorque(-Vector3.up);
            _cameraManager.ClearScreen();
        }
        else
        {
            _subRb.drag = 1.3f; // Helps slow down the sub faster
        }

        _subRb.velocity = Vector3.ClampMagnitude(_subRb.velocity, MAX_SPEED);
        _subRb.angularVelocity = Vector3.ClampMagnitude(_subRb.angularVelocity, MAX_ROT_SPEED);

        float subRoundedX = Mathf.RoundToInt(_subObj.position.x * 100f) / 100f;

        // In reality it's the Z axis, but the player never changes the Z
        float subRoundedY = Mathf.RoundToInt(_subObj.position.z * 100f) / 100f;

        float subRoundedRot = Mathf.RoundToInt(_subObj.eulerAngles.y * 100f) / 100f;

        _xDisplay.SetText(subRoundedX.ToString("000.00"));
        _yDisplay.SetText(subRoundedY.ToString("000.00"));
        _degDisplay.SetText(subRoundedRot.ToString("000.00"));
        _orienDisplay.rotation = Quaternion.Euler(90f, _subObj.eulerAngles.y, 0f);
    }

    private void ComputeSound()
    {
        const float SQR_THRESHOLD = SOUND_THRESHOLD * SOUND_THRESHOLD;

        if (_subRb.velocity.sqrMagnitude > SQR_THRESHOLD || _subRb.angularVelocity.sqrMagnitude > SQR_THRESHOLD)
        {
            if (!_waterNoise.isPlaying)
            {
                _waterNoise.Play();
            }
            
            if (_subRb.velocity.sqrMagnitude > SQR_THRESHOLD)
            {
                float newPitch = Mathf.Clamp(SqrMagOperation(_subRb.velocity.sqrMagnitude) / 2f, 0f, 1.2f);
                _waterNoise.pitch = newPitch;
            }
            else if (_subRb.angularVelocity.sqrMagnitude > SQR_THRESHOLD)
            {
                float newPitch = Mathf.Clamp(SqrMagOperation(_subRb.angularVelocity.sqrMagnitude) * 3f, 0f, 1.2f);
                _waterNoise.pitch = newPitch;
            }
        }
        else
        {
            _waterNoise.Pause();
        }

        // Ludicrous work around for approximately calculating (_subRb.velocity.magnitude / 2f) with out actually using the sqrt function!
        // static float SqrMagOperation(float veloc) => ((veloc + 150f) * (veloc + 150f) / 600f) - 37.5f;
        static float SqrMagOperation(float veloc) => ((veloc + 686f) * (veloc + 686f) / 2800f) - 168.07f;
    }

    public void ToggleControl(string type, bool state, GameObject btn)
    {
        if (type == "disableall")
        {
            _movingFor = false;
            _movingBack = false;
            _rotRight = false;
            _rotLeft = false;

            foreach (GameObject button in _movementButtons)
            {
                if (!LeanTween.isTweening(button) && button.transform.localPosition.y < (BUTTON_MIN_Y + 0.01f))
                {
                    button.LeanMoveLocalY(button.transform.localPosition.y + 0.01f, 0.1f);
                }
            }

            return;
        }
        else
        {
            switch (type)
            {
                case "forward":
                    _movingFor = state;
                    break;
                case "backwards":
                    _movingBack = state;
                    break;
                case "right":
                    _rotRight = state;
                    break;
                case "left":
                    _rotLeft = state;
                    break;
                default:
                    Debug.LogError("Invalid control type");
                    break;
            }
        }

        if (!LeanTween.isTweening(btn) && btn.transform.localPosition.y > BUTTON_MIN_Y)
        {
            btn.LeanMoveLocalY(btn.transform.localPosition.y - 0.01f, 0.1f);
        }
    }
}
