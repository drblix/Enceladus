using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    private PlayerMovement plrMovement;

    [SerializeField]
    private Transform headTransform;
    [SerializeField]
    private Transform renderingCamTrans;

    [Header("Bobbing Config")]

    [SerializeField]
    private float bobFrequency = 5f;
    [SerializeField]
    private float bobHorizontalAmp = 0.1f;
    [SerializeField]
    private float bobVerticalAmp = 0.1f;
    [SerializeField] [Range(0, 1)]
    private float bobSmoothing = 0.1f;

    [SerializeField]
    private bool isMoving = false;
    private float walkingTime;
    private Vector3 targetCamPos;

    private void Awake() 
    {
        plrMovement = GetComponent<PlayerMovement>();
        bobFrequency = plrMovement.PlayerSpeed * 1.5f;
        
    }

    private void Update()
    {
        HeadBob();
    }

    private void HeadBob()
    {
        if (!isMoving)
        {
            walkingTime = 0f;
        }
        else
        {
            walkingTime += Time.deltaTime;
        }

        targetCamPos = headTransform.position + CalculateOffset(walkingTime);

        renderingCamTrans.position = Vector3.Lerp(renderingCamTrans.position, targetCamPos, bobSmoothing);

        if ((renderingCamTrans.position - targetCamPos).magnitude <= 0.001f)
        {
            renderingCamTrans.position = targetCamPos;
        }
    }

    private Vector3 CalculateOffset(float time)
    {
        Vector3 offset = Vector3.zero;

        if (time > 0f)
        {
            float horzOffset = Mathf.Cos(time * bobFrequency) * bobHorizontalAmp;
            float vertOffset = Mathf.Sin(time * bobFrequency * 2) * bobVerticalAmp;

            offset = headTransform.right * horzOffset + headTransform.up * vertOffset;
        }

        return offset;
    }

    public void SetMovingState(bool state)
    {
        isMoving = state;
    }
}