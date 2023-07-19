using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;
    public static CameraManager Instance { get { return _instance; } }

    private AudioSource _photoSFX;

    [SerializeField] private Transform _camButton;
    [SerializeField] private MeshRenderer _camDisplayRenderer, _lightBitRenderer;
    [SerializeField] private Camera _subCam;

    private bool _canPress = true;

    private void Awake() 
    {
        _photoSFX = _camButton.GetComponent<AudioSource>();

        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    public IEnumerator ButtonPressed()
    {
        if (!_canPress) { yield break; }

        _canPress = false;
        _camDisplayRenderer.material.SetColor("_Color", Color.black);

        Vector3 toPos = new (_camButton.transform.position.x, _camButton.transform.position.y, _camButton.transform.position.z - .045f);
        CoroutineManager.Start(_camButton.MoveToReturn(toPos, .75f, .35f, EasingFunctions.FunctionType.EaseOutSine));

        // StartCoroutine(_camButton.MoveTo(toPos, .75f, EasingFunctions.FunctionType.EaseOutSine));
        // StartCoroutine(_camButton.MoveToReturn(toPos, .75f, .35f, EasingFunctions.FunctionType.EaseOutQuart));

        _lightBitRenderer.material.SetColor("_Color", Color.red);
        _lightBitRenderer.material.SetColor("_EmissionColor", Color.red);

        StartCoroutine(TakePhoto());

        // _camButton.LeanMoveLocalZ(-4.35f, 0.75f);

        yield return new WaitForSeconds(4f);

        _lightBitRenderer.material.SetColor("_Color", Color.green);
        _lightBitRenderer.material.SetColor("_EmissionColor", Color.green);

        _canPress = true;
    }

    private IEnumerator TakePhoto()
    {
        _photoSFX.Play();
        yield return new WaitForSeconds(2.75f);
        _photoSFX.Stop();

        _camDisplayRenderer.material.SetColor("_Color", Color.white);
        _subCam.Render(); // Renders single frame

        ObjectiveChecker.Instance.CheckObjectives();
    }

    public void ClearScreen()
    {
        _camDisplayRenderer.material.SetColor("_Color", Color.black);
    }
}
