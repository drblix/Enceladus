using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Transform camTransform;

    [SerializeField]
    private Transform enceladusTrans;

    private Vector3 defaultPos;
    private Vector3 defaultRot;
    
    private const float TRANS_TIME = 2.5f;


    private void Awake() 
    {
        Time.timeScale = 1f;
        defaultPos = new Vector3(458f, 334f, -1342f);
        defaultRot = new Vector3(10f, 0f, 0f);

        camTransform.position = defaultPos;
        camTransform.rotation = Quaternion.Euler(defaultRot);
    }

    private void Update() 
    {
        Vector3 rotationVector = new Vector3(0f, 0.05f, 0f);
        enceladusTrans.Rotate(rotationVector);
    }

    public void SettingsClicked()
    {
        MoveCamera(new Vector3(1133f, 268f, -246f), new Vector3(9.959f, -55.406f, -0.906f), TRANS_TIME);
    }

    public void BackBtnClicked()
    {
        MoveCamera(defaultPos, defaultRot, TRANS_TIME);
    }

    public void PlayButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButtonClicked()
    {
        Application.Quit();
    }

    public void MoveCamera(Vector3 newPos, Vector3 newRot, float time)
    {
        if (LeanTween.isTweening(camTransform.gameObject)) { return; }

        camTransform.LeanMove(newPos, time).setEaseOutSine();
        camTransform.LeanRotate(newRot, time).setEaseOutSine();
    }

    public void SizeUpUI(RectTransform rect)
    {
        Vector3 scaleVector = Vector3.one * 1.1f;
        rect.LeanScale(scaleVector, 0.1f);
    }

    public void SizeDownUI(RectTransform rect)
    {
        rect.LeanScale(Vector3.one, 0.1f);
    }
}
