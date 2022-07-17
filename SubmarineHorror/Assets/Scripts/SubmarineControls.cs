using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubmarineControls : MonoBehaviour
{
    private CameraManager cameraManager;

    [SerializeField]
    private Transform subObj;
    
    private Rigidbody subRb;

    private AudioSource waterNoise;

    [SerializeField]
    private GameObject[] movementButtons;

    [SerializeField]
    private Transform orienDisplay;
    [SerializeField]
    private TextMeshPro xDisplay;
    [SerializeField]
    private TextMeshPro yDisplay;
    [SerializeField]
    private TextMeshPro degDisplay;

    [SerializeField]
    private bool debugging = false;

    private bool movingFor = false;
    private bool movingBack = false;
    private bool rotRight = false;
    private bool rotLeft = false;

    private Vector3 startingPos = new Vector3Int(43, 3, 32);

    private const float ACCELERATION_SPEED = 25f;
    private const float MAX_SPEED = 4.5f;
    private const float MAX_ROT_SPEED = 0.5f;
    private const float BUTTON_MIN_Y = -0.759f;
    private const float SOUND_THRESHOLD = 0.1f;


    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        subRb = subObj.GetComponent<Rigidbody>();
        waterNoise = transform.Find("WaterNoise").GetComponent<AudioSource>();

        if (!debugging)
        {
            subRb.transform.position = startingPos;
        }
    }

    private void FixedUpdate()
    {
        Controls();
        ComputeSound();
    }

    private void Controls()
    {
        if (movingFor)
        {
            subRb.drag = 0.6f;
            subRb.AddForce(subObj.forward * ACCELERATION_SPEED);
            cameraManager.ClearScreen();
        }
        else if (movingBack)
        {
            subRb.drag = 0.6f;
            subRb.AddForce(-subObj.forward * ACCELERATION_SPEED);
            cameraManager.ClearScreen();
        }
        else if (rotRight)
        {
            subRb.AddTorque(Vector3.up);
            cameraManager.ClearScreen();
        }
        else if (rotLeft)
        {
            subRb.AddTorque(-Vector3.up);
            cameraManager.ClearScreen();
        }
        else
        {
            subRb.drag = 1.3f; // Helps slow down the sub faster
        }

        subRb.velocity = Vector3.ClampMagnitude(subRb.velocity, MAX_SPEED);
        subRb.angularVelocity = Vector3.ClampMagnitude(subRb.angularVelocity, MAX_ROT_SPEED);

        float subRoundedX = (float)Mathf.RoundToInt(subObj.position.x * 100f) / 100f;
        float subRoundedY = (float)Mathf.RoundToInt(subObj.position.z * 100f) / 100f;
        // In reality it's the Z axis, but the player never changes the Z
        float subRoundedRot = (float)Mathf.RoundToInt(subObj.eulerAngles.y * 100f) / 100f;

        xDisplay.SetText(subRoundedX.ToString("000.00"));
        yDisplay.SetText(subRoundedY.ToString("000.00"));
        degDisplay.SetText(subRoundedRot.ToString("000.00"));
        orienDisplay.rotation = Quaternion.Euler(90f, subObj.eulerAngles.y, 0f);
    }

    private void ComputeSound()
    {
        if (subRb.velocity.magnitude > SOUND_THRESHOLD || subRb.angularVelocity.magnitude > SOUND_THRESHOLD)
        {
            if (!waterNoise.isPlaying)
            {
                waterNoise.Play();
            }
            
            if (subRb.velocity.magnitude > SOUND_THRESHOLD)
            {
                float newPitch = Mathf.Clamp(subRb.velocity.magnitude / 2f, 0f, 1.2f);
                waterNoise.pitch = newPitch;
            }
            else if (subRb.angularVelocity.magnitude > SOUND_THRESHOLD)
            {
                float newPitch = Mathf.Clamp(subRb.angularVelocity.magnitude / 2f, 0f, 1.2f);
                waterNoise.pitch = newPitch;
            }
        }
        else
        {
            waterNoise.Pause();
        }
    }

    public void ToggleControl(string type, bool state, GameObject btn)
    {
        if (type == "disableall")
        {
            movingFor = false;
            movingBack = false;
            rotRight = false;
            rotLeft = false;

            foreach (GameObject button in movementButtons)
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
                    movingFor = state;
                    break;
                case "backwards":
                    movingBack = state;
                    break;
                case "right":
                    rotRight = state;
                    break;
                case "left":
                    rotLeft = state;
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
