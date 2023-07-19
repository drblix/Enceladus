using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement _instance;
    public static PlayerMovement Instance { get { return _instance; } }

    public bool PlayerCanInteract { get; set; } = true;
    public bool CurrentlyShaking { get; private set; } = false;
    public float PlayerSpeed { get; private set; } = 2.5f;

    [SerializeField] private Transform _plrCamera;

    private const float MAX_CAMERA_ANGLE = 80f;

    private PlayerInput _plrInput;

    private CharacterController _charController;

    private float _xRotation = 0f;

    private float _lockedYAxisConst;


    private void Awake() 
    {
        _plrInput = GetComponent<PlayerInput>();
        _charController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        _lockedYAxisConst = transform.position.y;

        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    private void Update()
    {
        if (PlayerCanInteract)
        {
            Movement();
            CameraMovement();
        }
    }

    private void Movement()
    {
        float vertical = _plrInput.actions["Vertical"].ReadValue<float>();
        float horizontal = _plrInput.actions["Horizontal"].ReadValue<float>();

        Vector3 movementVector = (transform.forward * vertical + transform.right * horizontal).normalized;
        _charController.Move(PlayerSpeed * Time.deltaTime * movementVector);
        transform.position = new Vector3(transform.position.x, _lockedYAxisConst, transform.position.z); // Locks player's y-position
    }

    private void CameraMovement()
    {
        float mouseX = _plrInput.actions["MouseX"].ReadValue<float>() * PlayerSettings.mouseSensitivity;
        float mouseY = _plrInput.actions["MouseY"].ReadValue<float>() * PlayerSettings.mouseSensitivity;

        if (!PlayerSettings.mouseInverted)
            _xRotation -= mouseY;
        else
            _xRotation += mouseY;

        _xRotation = Mathf.Clamp(_xRotation, -MAX_CAMERA_ANGLE, MAX_CAMERA_ANGLE);

        _plrCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(transform.up * mouseX);
    }

    /// <summary>
    /// Shakes the player's camera for a specified amount of time and magnitude
    /// </summary>
    /// <param name="duration">Duration of the shake (in seconds)</param>
    /// <param name="magnitude">Strength of the shake</param>
    /// <param name="fadeOut">Should the shake fade out? (default is true)</param>
    /// <returns></returns>
    public IEnumerator ShakeCamera(float duration, float magnitude, bool fadeOut, float delay)
    {
        // Prevents running method twice
        if (CurrentlyShaking) { yield break; }
        CurrentlyShaking = true;

        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        Vector3 originalPos = _plrCamera.localPosition;

        float elapsed = 0f;
        float startingMagnitude = magnitude;

        while (elapsed < duration)
        {
            float newX = Random.Range(-1f, 1f) * startingMagnitude;
            float newY = Random.Range(-1f, 1f) * startingMagnitude;

            _plrCamera.localPosition = new Vector3(newX, newY, originalPos.z);

            elapsed += Time.deltaTime;

            if (fadeOut)
                startingMagnitude = Mathf.Lerp(magnitude, 0f, EasingFunctions.EaseOutCirc(elapsed / duration));

            yield return new WaitForEndOfFrame();
        }
        
        _plrCamera.localPosition = originalPos;

        CurrentlyShaking = false;
    }
}

// Class for handling all of the player's settings
// Each setting is static to preserve over scenes
public static class PlayerSettings
{
    public enum ResolutionTypes
    {
        x64,
        x128,
        x256,
        x512,
    }

    public static bool mouseInverted = true;
    public static bool motionBlur = true;
    public static float mouseSensitivity = 0.1f;
    public static ResolutionTypes resolutionType = ResolutionTypes.x256;
}
