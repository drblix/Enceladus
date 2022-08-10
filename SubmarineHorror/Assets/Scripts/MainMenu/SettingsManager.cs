using UnityEngine;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI invertedText;
    [SerializeField]
    private TextMeshProUGUI blurText;
    [SerializeField]
    private TextMeshProUGUI sensitivityText;
    [SerializeField]
    private TextMeshProUGUI volumeText;
    [SerializeField]
    private TextMeshProUGUI resolutionText;

    private int selectedResolution = 2;

    private void Awake() 
    {
        UpdateSettingDisplays();
    }

    private void UpdateSettingDisplays()
    {
        if (PlayerSettings.mouseInverted)
        {
            invertedText.SetText("Inverted: Yes");
        }
        else
        {
            invertedText.SetText("Inverted: No");
        }

        if (PlayerSettings.motionBlur)
        {
            blurText.SetText("Motion blur: Yes");
        }
        else
        {
            blurText.SetText("Motion blur: No");
        }

        resolutionText.SetText(PlayerSettings.resolutionType.ToString());
        sensitivityText.SetText(PlayerSettings.mouseSensitivity.ToString());
        volumeText.SetText(AudioListener.volume.ToString());
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
        }
        else if (name == "sensAdd")
        {
            PlayerSettings.mouseSensitivity += 0.1f;
            PlayerSettings.mouseSensitivity = Mathf.Round((PlayerSettings.mouseSensitivity * 10f)) / 10f;
            PlayerSettings.mouseSensitivity = Mathf.Clamp(PlayerSettings.mouseSensitivity, 0.1f, 2f);
        }
        else if (name == "sensRemove")
        {
            PlayerSettings.mouseSensitivity -= 0.1f;
            PlayerSettings.mouseSensitivity = Mathf.Round((PlayerSettings.mouseSensitivity * 10f)) / 10f;
            PlayerSettings.mouseSensitivity = Mathf.Clamp(PlayerSettings.mouseSensitivity, 0.1f, 2f);
        }
        else if (name == "volumeAdd")
        {
            AudioListener.volume += 0.1f;
            AudioListener.volume = Mathf.Round((AudioListener.volume * 10f)) / 10f;
            AudioListener.volume = Mathf.Clamp01(AudioListener.volume);
        }
        else if (name == "volumeRemove")
        {
            AudioListener.volume -= 0.1f;
            AudioListener.volume = Mathf.Round((AudioListener.volume * 10f)) / 10f;
            AudioListener.volume = Mathf.Clamp01(AudioListener.volume);
        }
        else if (name == "resolutionUp")
        {
            selectedResolution++;
            selectedResolution = Mathf.Clamp(selectedResolution, 0, 3);
            PlayerSettings.resolutionType = (PlayerSettings.ResolutionTypes) selectedResolution;
        }
        else if (name == "resolutionDown")
        {
            selectedResolution--;
            selectedResolution = Mathf.Clamp(selectedResolution, 0, 3);
            PlayerSettings.resolutionType = (PlayerSettings.ResolutionTypes) selectedResolution;
        }

        UpdateSettingDisplays();
    }
}
