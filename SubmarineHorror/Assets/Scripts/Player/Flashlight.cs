using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    private PlayerMovement plrMovement;

    [SerializeField]
    private Transform transToFollow;
    private Light lightComp;
    private AudioSource audioSrc;

    [SerializeField]
    private float correctionSpeed = 2.5f;


    private void Awake() 
    {
        plrMovement = FindObjectOfType<PlayerMovement>();

        lightComp = GetComponent<Light>();
        audioSrc = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame && plrMovement.PlayerCanInteract)
        {
            transform.rotation = transToFollow.rotation;
            audioSrc.Play();
            lightComp.enabled = !lightComp.enabled;
        }

        if (lightComp.enabled)
        {
            UpdateTransform();
        }
    }

    private void UpdateTransform()
    {
        transform.position = transToFollow.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, transToFollow.rotation, Time.deltaTime * correctionSpeed);
    }
}
