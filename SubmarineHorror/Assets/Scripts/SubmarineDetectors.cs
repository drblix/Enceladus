using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineDetectors : MonoBehaviour
{
    [SerializeField]
    private GameObject[] detectors; // 0 - 3

    [SerializeField]
    private Transform subTrans;

    private AudioSource beepSound;

    private bool[] detectorEnabled = new bool[4];
    private float[] beepWait = new float[4];

    private int terrainLayer;

    private bool raycasting = true;

    private const float RAY_DISTANCE = 7f;
    private const float BEEP_FACTOR = 6f;

    private void Awake()
    {
        beepSound = transform.Find("BeepObj").GetComponent<AudioSource>();
        terrainLayer = LayerMask.GetMask("MapTerrain");
        
        foreach (GameObject obj in detectors)
        {
            obj.SetActive(false);
        }

        StartCoroutine(ForwardDetector());
        StartCoroutine(RightDetector());
        StartCoroutine(BackDetector());
        StartCoroutine(LeftDetector());
    }

    private void FixedUpdate()
    {
        if (raycasting)
        {
            if (Physics.Raycast(new Ray(subTrans.position, subTrans.forward), out RaycastHit hitInfo, RAY_DISTANCE, terrainLayer))
            {
                if (hitInfo.collider)
                {
                    detectorEnabled[0] = true;
                    beepWait[0] = hitInfo.distance / BEEP_FACTOR;
                }
            }
            else
            {
                detectorEnabled[0] = false;
            }

            if (Physics.Raycast(new Ray(subTrans.position, subTrans.right), out RaycastHit hitInfo2, RAY_DISTANCE, terrainLayer))
            {
                if (hitInfo2.collider)
                {
                    detectorEnabled[1] = true;
                    beepWait[1] = hitInfo2.distance / BEEP_FACTOR;
                }
            }
            else
            {
                detectorEnabled[1] = false;
            }

            if (Physics.Raycast(new Ray(subTrans.position, -subTrans.forward), out RaycastHit hitInfo3, RAY_DISTANCE, terrainLayer))
            {
                if (hitInfo3.collider)
                {
                    detectorEnabled[2] = true;
                    beepWait[2] = hitInfo3.distance / BEEP_FACTOR;
                }
            }
            else
            {
                detectorEnabled[2] = false;
            }

            if (Physics.Raycast(new Ray(subTrans.position, -subTrans.right), out RaycastHit hitInfo4, RAY_DISTANCE, terrainLayer))
            {
                if (hitInfo4.collider)
                {
                    detectorEnabled[3] = true;
                    beepWait[3] = hitInfo4.distance / BEEP_FACTOR;
                }
            }
            else
            {
                detectorEnabled[3] = false;
            }
        }
    }

    private IEnumerator ForwardDetector()
    {
        while (true)
        {
            if (detectorEnabled[0])
            {
                detectors[0].SetActive(true);
                beepSound.Play();
                yield return new WaitForSeconds(beepWait[0]);
                detectors[0].SetActive(false);
                yield return new WaitForSeconds(beepWait[0]);
            }

            yield return null;
        }
    }

    private IEnumerator RightDetector()
    {
        while (true)
        {
            if (detectorEnabled[1])
            {
                detectors[1].SetActive(true);
                beepSound.Play();
                yield return new WaitForSeconds(beepWait[1]);
                detectors[1].SetActive(false);
                yield return new WaitForSeconds(beepWait[1]);
            }

            yield return null;
        }
    }

    private IEnumerator BackDetector()
    {
        while (true)
        {
            if (detectorEnabled[2])
            {
                detectors[2].SetActive(true);
                beepSound.Play();
                yield return new WaitForSeconds(beepWait[2]);
                detectors[2].SetActive(false);
                yield return new WaitForSeconds(beepWait[2]);
            }

            yield return null;
        }
    }

    private IEnumerator LeftDetector()
    {
        while (true)
        {
            if (detectorEnabled[3])
            {
                detectors[3].SetActive(true);
                beepSound.Play();
                yield return new WaitForSeconds(beepWait[3]);
                detectors[3].SetActive(false);
                yield return new WaitForSeconds(beepWait[3]);
            }

            yield return null;
        }
    }
}
