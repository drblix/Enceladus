using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorEvent : MonoBehaviour
{
    private GameObject[] dummyBlockers = new GameObject[4];

    private enum BlockerOrigin
    {
        Front,
        Right,
        Back,
        Left
    }

    [SerializeField]
    private BlockerOrigin blockerOrigin = BlockerOrigin.Front;

    [SerializeField] [Range(0.1f, 1f)]
    private float blockersDistance = 0.1f;

    [SerializeField] [Range(1f, 10f)]
    private float duration = 2f;

    private bool used = false;

    private void Awake() 
    {
        Transform subObj = GameObject.FindGameObjectWithTag("SubObj").transform;
        dummyBlockers[0] = subObj.Find("FrontBlocker").gameObject;
        dummyBlockers[1] = subObj.Find("RightBlocker").gameObject;
        dummyBlockers[2] = subObj.Find("BackBlocker").gameObject;
        dummyBlockers[3] = subObj.Find("LeftBlocker").gameObject;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!used)
        {
            used = true;
            GameObject chosenBlocker = dummyBlockers[(int)blockerOrigin];

            if (chosenBlocker.name.Contains("Front"))
            {
                chosenBlocker.transform.localPosition = new Vector3(0f, 0f, 1f + blockersDistance);
            }
            else if (chosenBlocker.name.Contains("Right"))
            {
                chosenBlocker.transform.localPosition = new Vector3(1f + blockersDistance, 0f, 0f);
            }
            else if (chosenBlocker.name.Contains("Back"))
            {
                chosenBlocker.transform.localPosition = new Vector3(0f, 0f, -1f - blockersDistance);
            }
            else if (chosenBlocker.name.Contains("Left"))
            {
                chosenBlocker.transform.localPosition = new Vector3(-1f - blockersDistance, 0f, 0f);
            }

            StartCoroutine(StartEvent(chosenBlocker));
        }
    }

    private IEnumerator StartEvent(GameObject chosenBlocker)
    {
        chosenBlocker.SetActive(true);
        yield return new WaitForSeconds(duration);
        chosenBlocker.SetActive(false);
    }
}
