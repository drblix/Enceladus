using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput plrInput;
    private InputAction movementMap;

    private Transform plrCamera;
    private CharacterController cController;

    private float xRotation = 0f;

    private float lockedYAxisConst;

    private bool currentlyShaking = false;
    private bool lockMovement = false; // default is false

    private const float MOVEMENT_SPEED = 3.5f;
    private const float MAX_CAMERA_ANGLE = 80f;

    private void Awake() 
    {
        plrInput = GetComponent<PlayerInput>();

        plrCamera = transform.GetChild(0).GetChild(0);
        cController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        lockedYAxisConst = transform.position.y;
    }

    private void Update()
    {
        if (!lockMovement)
        {
            Movement();
            CameraMovement();
        }
    }

    private void Movement()
    {
        float vertical = plrInput.actions["Vertical"].ReadValue<float>();
        float horizontal = plrInput.actions["Horizontal"].ReadValue<float>();

        Vector3 movementVector = (transform.forward * vertical + transform.right * horizontal).normalized;
        cController.Move(movementVector * Time.deltaTime * MOVEMENT_SPEED);
        transform.position = new Vector3(transform.position.x, lockedYAxisConst, transform.position.z); // Locks player's y-position
    }

    private void CameraMovement()
    {
        float mouseX = plrInput.actions["MouseX"].ReadValue<float>() * PlayerSettings.mouseSensitivity;
        float mouseY = plrInput.actions["MouseY"].ReadValue<float>() * PlayerSettings.mouseSensitivity;

        if (!PlayerSettings.mouseInverted)
        {
            xRotation -= mouseY;
        }
        else
        {
            xRotation += mouseY;
        }

        xRotation = Mathf.Clamp(xRotation, -MAX_CAMERA_ANGLE, MAX_CAMERA_ANGLE);

        plrCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(transform.up * mouseX);
    }

    /// <summary>
    /// Shakes the player's camera for a specified amount of time and magnitude
    /// </summary>
    /// <param name="duration">Duration of the shake (in seconds)</param>
    /// <param name="magnitude">Strength of the shake</param>
    /// <param name="fadeOut">Should the shake fade out? (default is true)</param>
    /// <returns></returns>
    public IEnumerator ShakeCamera(float duration, float magnitude, bool fadeOut = true)
    {
        if (currentlyShaking) { yield break; }

        currentlyShaking = true;

        Vector3 originalPos = plrCamera.localPosition;

        float elapsed = 0f;
        duration = Mathf.Clamp(duration, 0f, 10f);

        if (fadeOut)
        {
            // Creating lambda expression to tween magnitude to 0
            LeanTween.value(gameObject, magnitude, 0f, duration).setEaseOutSine().setOnUpdate((float val) => { magnitude = val; });
        }

        while (elapsed < duration)
        {
            float newX = Random.Range(-1f, 1f) * magnitude;
            float newY = Random.Range(-1f, 1f) * magnitude;

            plrCamera.localPosition = new Vector3(newX, newY, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        
        plrCamera.localPosition = originalPos;

        currentlyShaking = false;
    }

    public void LockMovement(bool state)
    {
        lockMovement = state;
    }
}

public class PlayerSettings
{
    public static bool mouseInverted = true;
    public static float mouseSensitivity = 0.15f;
}
