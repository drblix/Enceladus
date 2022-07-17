using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SoundEvent : MonoBehaviour
{
    private SFXManager sfxManager;

    private enum OriginDirection
    {
        Forward = 0,
        ForwardRight = 1,
        Right = 2,
        Back = 3,
        Left = 4,
        ForwardLeft = 5
    }


    [Header("Clip Settings")]

    [SerializeField]
    private OriginDirection chosenDirection;

    [SerializeField] [Range(0, 5)]
    private int chosenClip;

    [SerializeField]
    private float clipDelay = 0f;

    [SerializeField] [Range(0f, 1f)]
    private float volume = 1f;

    [Header("Camera Settings")]

    [SerializeField]
    private bool shakeCamera = false;

    [SerializeField]
    private float shakeDuration = 0.5f;

    [SerializeField]
    private float shakeMagnitude = 0.25f;

    [SerializeField]
    private bool shakeFadeOut = false;


    private bool used = false;

    private void Awake() 
    {
        sfxManager = FindObjectOfType<SFXManager>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("SubObj") && !used)
        {
            used = true;
            sfxManager.PlaySpecificNoise(chosenClip, (int)chosenDirection, clipDelay, shakeCamera, shakeDuration, shakeMagnitude, shakeFadeOut, volume);
        }
    }
}
