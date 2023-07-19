using UnityEngine;

public class SubmarineDetectors : MonoBehaviour
{
    [SerializeField] private GameObject[] _detectors; // 0 - 3

    [SerializeField] private Transform _subTrans;

    private AudioSource _beepSource;

    private bool[] _detectorEnabled = new bool[4];
    private float[] _beepWait = new float[4], _detectorTimers = new float[4];

    private int _terrainLayer;

    private bool _raycasting = true;

    private const float RAY_DISTANCE = 7f;
    private const float BEEP_FACTOR = 6f;

    private void Awake()
    {
        _beepSource = transform.Find("BeepObj").GetComponent<AudioSource>();
        _terrainLayer = LayerMask.GetMask("MapTerrain");
        
        foreach (GameObject obj in _detectors)
            obj.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_raycasting)
        {
            if (Physics.Raycast(new Ray(_subTrans.position, _subTrans.forward), out RaycastHit hitInfo, RAY_DISTANCE, _terrainLayer))
            {
                if (hitInfo.collider)
                {
                    _detectorEnabled[0] = true;
                    _beepWait[0] = hitInfo.distance / BEEP_FACTOR;
                }
            }
            else
            {
                _detectorEnabled[0] = false;
            }

            if (Physics.Raycast(new Ray(_subTrans.position, _subTrans.right), out RaycastHit hitInfo2, RAY_DISTANCE, _terrainLayer))
            {
                if (hitInfo2.collider)
                {
                    _detectorEnabled[1] = true;
                    _beepWait[1] = hitInfo2.distance / BEEP_FACTOR;
                }
            }
            else
            {
                _detectorEnabled[1] = false;
            }

            if (Physics.Raycast(new Ray(_subTrans.position, -_subTrans.forward), out RaycastHit hitInfo3, RAY_DISTANCE, _terrainLayer))
            {
                if (hitInfo3.collider)
                {
                    _detectorEnabled[2] = true;
                    _beepWait[2] = hitInfo3.distance / BEEP_FACTOR;
                }
            }
            else
            {
                _detectorEnabled[2] = false;
            }

            if (Physics.Raycast(new Ray(_subTrans.position, -_subTrans.right), out RaycastHit hitInfo4, RAY_DISTANCE, _terrainLayer))
            {
                if (hitInfo4.collider)
                {
                    _detectorEnabled[3] = true;
                    _beepWait[3] = hitInfo4.distance / BEEP_FACTOR;
                }
            }
            else
            {
                _detectorEnabled[3] = false;
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (_detectorEnabled[i])
            {
                _detectorTimers[i] += Time.deltaTime;

                if (_detectorTimers[i] > _beepWait[i])
                {
                    if (_detectors[i].activeInHierarchy)
                    {
                        _detectors[i].SetActive(false);
                    }
                    else
                    {
                        _detectors[i].SetActive(true);
                        _beepSource.Play();
                    }

                    _detectorTimers[i] = 0f;
                }
            }
            else
                _detectors[i].SetActive(false);
        }

    }
}
