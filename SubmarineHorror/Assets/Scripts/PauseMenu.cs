using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private GameObject pauseMenu;

    [SerializeField]
    private Volume plrVolume;

    [SerializeField]
    private Camera renderingCam;

    [SerializeField]
    private RawImage gameDisplay;

    [SerializeField]
    private RenderTexture[] displayTextures;

    [SerializeField]
    private TextMeshProUGUI[] textDisplays;

    public bool canPauseGame = true;

    private void Awake()
    {
        Time.timeScale = 1f;
        playerMovement = FindObjectOfType<PlayerMovement>();

        pauseMenu = transform.Find("PauseMenu").gameObject;
        pauseMenu.SetActive(false);

        switch (PlayerSettings.resolutionType)
        {
            case PlayerSettings.ResolutionTypes.x64:
                renderingCam.targetTexture = displayTextures[0];
                gameDisplay.texture = displayTextures[0];
                break;
            case PlayerSettings.ResolutionTypes.x128:
                renderingCam.targetTexture = displayTextures[1];
                gameDisplay.texture = displayTextures[1];
                break;
            case PlayerSettings.ResolutionTypes.x256:
                renderingCam.targetTexture = displayTextures[2];
                gameDisplay.texture = displayTextures[2];
                break;
            case PlayerSettings.ResolutionTypes.x512:
                renderingCam.targetTexture = displayTextures[3];
                gameDisplay.texture = displayTextures[3];
                break;
            default:
                renderingCam.targetTexture = displayTextures[2];
                gameDisplay.texture = displayTextures[2];
                break;
        }

        UpdateSettingDisplays();

        if (plrVolume.profile.TryGet<MotionBlur>(out MotionBlur blur))
        {
            blur.active = PlayerSettings.motionBlur;
        }
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && canPauseGame)
        {
            if (Time.timeScale == 0f)
            {
                playerMovement.playerCanInteract = true;
                Cursor.lockState = CursorLockMode.Locked;
                pauseMenu.SetActive(false);
                ToggleAudioSources(false);
                Time.timeScale = 1f;
            }
            else
            {
                playerMovement.playerCanInteract = false;
                Cursor.lockState = CursorLockMode.None;
                pauseMenu.SetActive(true);
                ToggleAudioSources(true);
                Time.timeScale = 0f;
            }
        }
    }

    public void ButtonPressed(string name)
    {
        if (name == "inverted")
        {
            PlayerSettings.mouseInverted = !PlayerSettings.mouseInverted;
        }
        else if (name == "blur")
        {
            PlayerSettings.motionBlur = !PlayerSettings.motionBlur;

            if (plrVolume.profile.TryGet<MotionBlur>(out MotionBlur blur))
            {
                blur.active = PlayerSettings.motionBlur;
            }
        }
        else if (name == "sensUp")
        {
            PlayerSettings.mouseSensitivity += 0.1f;
            PlayerSettings.mouseSensitivity = Mathf.Round((PlayerSettings.mouseSensitivity * 10f)) / 10f;
            PlayerSettings.mouseSensitivity = Mathf.Clamp(PlayerSettings.mouseSensitivity, 0.1f, 2f);
        }
        else if (name == "sensDown")
        {
            PlayerSettings.mouseSensitivity -= 0.1f;
            PlayerSettings.mouseSensitivity = Mathf.Round((PlayerSettings.mouseSensitivity * 10f)) / 10f;
            PlayerSettings.mouseSensitivity = Mathf.Clamp(PlayerSettings.mouseSensitivity, 0.1f, 2f);
        }
        else if (name == "volUp")
        {
            AudioListener.volume += 0.1f;
            AudioListener.volume = Mathf.Round((AudioListener.volume * 10f)) / 10f;
            AudioListener.volume = Mathf.Clamp01(AudioListener.volume);
        }
        else if (name == "volDown")
        {
            AudioListener.volume -= 0.1f;
            AudioListener.volume = Mathf.Round((AudioListener.volume * 10f)) / 10f;
            AudioListener.volume = Mathf.Clamp01(AudioListener.volume);
        }
        else if (name == "resume")
        {
            playerMovement.playerCanInteract = true;
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.SetActive(false);
            ToggleAudioSources(false);
            Time.timeScale = 1f;
        }
        else if (name == "quit")
        {
            SceneManager.LoadScene(0);
        }

        UpdateSettingDisplays();
    }

    private void UpdateSettingDisplays()
    {
        if (PlayerSettings.mouseInverted)
        {
            textDisplays[0].SetText("Inverted: Yes");
        }
        else
        {
            textDisplays[0].SetText("Inverted: No");
        }

        if (PlayerSettings.motionBlur)
        {
            textDisplays[1].SetText("Motion blur: Yes");
        }
        else
        {
            textDisplays[1].SetText("Motion blur: No");
        }

        textDisplays[2].SetText(PlayerSettings.mouseSensitivity.ToString());
        textDisplays[3].SetText(AudioListener.volume.ToString());
    }

    private void ToggleAudioSources(bool state)
    {
        foreach (AudioSource source in FindObjectsOfType<AudioSource>())
        {
            if (state)
            {
                source.Pause();
            }
            else
            {
                source.UnPause();
            }
        }
    }
}