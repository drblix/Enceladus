using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraNoFog : MonoBehaviour
{
    [SerializeField]
    private bool allowFog = false;

    private void OnPreRender() 
    {
        RenderSettings.fog = allowFog;
    }

    private void OnPostRender() 
    {
        RenderSettings.fog = !allowFog;
    }
}
