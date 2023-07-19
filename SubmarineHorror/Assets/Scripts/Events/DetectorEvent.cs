using System.Collections;
using UnityEngine;

public class DetectorEvent : MonoBehaviour
{
    private GameObject[] _dummyBlockers = new GameObject[4];

    private enum BlockerOrigin
    {
        Front,
        Right,
        Back,
        Left
    }

    [SerializeField] private bool _enabled = true;

    [SerializeField] private BlockerOrigin _blockerOrigin = BlockerOrigin.Front;

    [SerializeField] [Range(0.1f, 1f)] private float _blockersDistance = 0.1f;

    [SerializeField] [Min(0f)] private float _duration = 2f;

    [SerializeField] [Min(0f)] private float _delay = 0f;

    private bool _used = false;



    private void Awake() 
    {
        if (!_enabled) Destroy(this);

        Transform subObj = GameObject.FindGameObjectWithTag("SubObj").transform;
        _dummyBlockers[0] = subObj.Find("FrontBlocker").gameObject;
        _dummyBlockers[1] = subObj.Find("RightBlocker").gameObject;
        _dummyBlockers[2] = subObj.Find("BackBlocker").gameObject;
        _dummyBlockers[3] = subObj.Find("LeftBlocker").gameObject;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!_used)
        {
            _used = true;
            GameObject chosenBlocker = _dummyBlockers[(int)_blockerOrigin];

            if (chosenBlocker.name.Contains("Front"))
            {
                chosenBlocker.transform.localPosition = new Vector3(0f, 0f, 1f + _blockersDistance);
            }
            else if (chosenBlocker.name.Contains("Right"))
            {
                chosenBlocker.transform.localPosition = new Vector3(1f + _blockersDistance, 0f, 0f);
            }
            else if (chosenBlocker.name.Contains("Back"))
            {
                chosenBlocker.transform.localPosition = new Vector3(0f, 0f, -1f - _blockersDistance);
            }
            else if (chosenBlocker.name.Contains("Left"))
            {
                chosenBlocker.transform.localPosition = new Vector3(-1f - _blockersDistance, 0f, 0f);
            }

            StartCoroutine(StartEvent(chosenBlocker));
        }
    }

    private IEnumerator StartEvent(GameObject chosenBlocker)
    {
        if (_delay > 0f)
            yield return new WaitForSeconds(_delay);

        chosenBlocker.SetActive(true);
        yield return new WaitForSeconds(_duration);
        chosenBlocker.SetActive(false);
    }
}
