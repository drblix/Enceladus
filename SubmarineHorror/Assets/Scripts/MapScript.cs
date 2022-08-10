using UnityEngine;
using UnityEngine.InputSystem;

public class MapScript : MonoBehaviour
{
    private PlayerMovement plrMovement;

    private RectTransform mapRect;
    private AudioSource audioSrc;

    private const float OPEN_TIME = 1f;

    private bool mapOpen = false;

    private void Awake() 
    {
        plrMovement = FindObjectOfType<PlayerMovement>();

        mapRect = transform.Find("Map").GetComponent<RectTransform>();
        audioSrc = mapRect.GetComponent<AudioSource>();

        mapRect.localPosition = new Vector2(0f, -430f);
    }

    private void Update() 
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame && !LeanTween.isTweening(mapRect) && plrMovement.playerCanInteract || (mapOpen && Keyboard.current.tabKey.wasPressedThisFrame && !LeanTween.isTweening(mapRect)))
        {
            audioSrc.pitch = Random.Range(0.8f, 1.15f);

            if (!mapOpen)
            {
                mapOpen = true;
                Cursor.lockState = CursorLockMode.None;
                mapRect.LeanMoveLocalY(0f, OPEN_TIME).setEaseOutQuint();
                audioSrc.Play();
            }
            else
            {
                mapOpen = false;
                Cursor.lockState = CursorLockMode.Locked;
                mapRect.LeanMoveLocalY(-430f, OPEN_TIME - 0.5f).setEaseInQuint();
                audioSrc.Play();
            }

            plrMovement.playerCanInteract = !mapOpen;
        }
    }
}
