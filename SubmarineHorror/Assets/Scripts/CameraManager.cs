using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private ObjectiveChecker objChecker;

    private Transform camButton;
    private AudioSource photoSFX;
    private MeshRenderer lightBitRenderer;
    private MeshRenderer camDisplayRenderer;
    private Camera subCam;

    private bool canPress = true;

    private void Awake() 
    {
        objChecker = FindObjectOfType<ObjectiveChecker>();
        camButton = transform.Find("CamButton");
        photoSFX = camButton.GetComponent<AudioSource>();
        lightBitRenderer = transform.Find("LightBit").GetComponent<MeshRenderer>();
        camDisplayRenderer = transform.Find("CamDisplay").GetComponent<MeshRenderer>();
        subCam = GameObject.FindGameObjectWithTag("SubmarineCamera").GetComponent<Camera>();
    }

    
    public IEnumerator ButtonPressed()
    {
        if (!canPress) { yield break; }

        canPress = false;
        camDisplayRenderer.material.SetColor("_Color", Color.black);

        camButton.LeanMoveLocalZ(-4.42f, 0.75f);
        lightBitRenderer.material.SetColor("_Color", Color.red);
        lightBitRenderer.material.SetColor("_EmissionColor", Color.red);
        StartCoroutine(TakePhoto());
        yield return new WaitForSeconds(.25f);
        camButton.LeanMoveLocalZ(-4.35f, 0.75f);
        yield return new WaitForSeconds(4f);
        lightBitRenderer.material.SetColor("_Color", Color.green);
        lightBitRenderer.material.SetColor("_EmissionColor", Color.green);

        canPress = true;
    }

    private IEnumerator TakePhoto()
    {
        photoSFX.Play();
        yield return new WaitForSeconds(2.75f);
        photoSFX.Stop();
        camDisplayRenderer.material.SetColor("_Color", Color.white);
        subCam.Render(); // Renders single frame

        objChecker.CheckObjectives();
    }

    public void ClearScreen()
    {
        camDisplayRenderer.material.SetColor("_Color", Color.black);
    }
}
