using UnityEngine;

public class CameraBobbing : MonoBehaviour
{
    private static CameraBobbing _instance;
    public static CameraBobbing Instance { get { return _instance; } }

    public float OffsetY { get; private set; } = 0f;

    [SerializeField]
    private Transform _headTransform;
    [SerializeField]
    private Transform _renderingCamTrans;

    [Header("Bobbing Config")]

    [SerializeField]
    private float _bobFrequency = 5f;
    [SerializeField]
    private float _bobHorizontalAmp = 0.1f;
    [SerializeField]
    private float _bobVerticalAmp = 0.1f;
    [SerializeField] [Range(0, 1)]
    private float _bobSmoothing = 0.1f;

    [SerializeField]
    private bool _isMoving = false;

    private float _walkingTime;

    private Vector3 _targetCamPos;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    private void Update()
    {
        HeadBob();
    }

    private void HeadBob()
    {
        if (!_isMoving)
            _walkingTime = 0f;
        else
            _walkingTime += Time.deltaTime;

        _targetCamPos = _headTransform.position + CalculateOffset(_walkingTime);

        _renderingCamTrans.position = Vector3.Lerp(_renderingCamTrans.position, _targetCamPos, _bobSmoothing);

        if ((_renderingCamTrans.position - _targetCamPos).sqrMagnitude <= 0.000001f)
            _renderingCamTrans.position = _targetCamPos;
    }

    private Vector3 CalculateOffset(float time)
    {
        Vector3 offset = Vector3.zero;

        if (time > 0f)
        {
            float horzOffset = Mathf.Cos(time * _bobFrequency) * _bobHorizontalAmp;
            float vertOffset = Mathf.Sin(time * _bobFrequency * 2) * _bobVerticalAmp;

            offset = _headTransform.right * horzOffset + _headTransform.up * vertOffset;
            OffsetY = vertOffset;
        }

        return offset;
    }

    public void SetMovingState(bool state) => _isMoving = state;
}