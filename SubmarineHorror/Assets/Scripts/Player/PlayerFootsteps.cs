using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    private CameraBobbing cameraBobbing;

    [SerializeField]
    private AudioClip[] footsteps;

    private AudioSource stepSource;

    private Vector3 lastPlrPos;

    [SerializeField]
    private float stepWaitTime = 0.5f;

    private bool canPlayStep = true;

    private void Awake() 
    {
        cameraBobbing = FindObjectOfType<CameraBobbing>();
        stepSource = GetComponent<AudioSource>();
        lastPlrPos = transform.position;
    }

    private void Update() 
    {
        if (PlayerIsMoving())
        {
            cameraBobbing.SetMovingState(true);
            PlayFootstep();
        }
        else
        {
            cameraBobbing.SetMovingState(false);
        }

        /*
        if (lastPlrPos != transform.position)
        {
            cameraBobbing.SetMovingState(true);

            if (!stepSource.isPlaying)
            {
                AudioClip newClip = footsteps[Random.Range(0, footsteps.Length)];
                stepSource.clip = newClip;
                stepSource.pitch = Random.Range(0.9f, 1.1f);
                stepSource.Play();
            }
        }
        else
        {
            cameraBobbing.SetMovingState(false);
        }
        */

        lastPlrPos = transform.position;
    }

    private void PlayFootstep()
    {
        if (canPlayStep)
        {
            canPlayStep = false;
            AudioClip clip = footsteps[Random.Range(0, footsteps.Length)];
            stepSource.clip = clip;
            stepSource.pitch = Random.Range(0.9f, 1.1f);
            stepSource.Play();
            
            StartCoroutine(CooldownStep());
        }
    }

    private IEnumerator CooldownStep()
    {
        yield return new WaitForSeconds(stepWaitTime);
        canPlayStep = true;
    }

    private bool PlayerIsMoving()
    {
        if (lastPlrPos != transform.position)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
