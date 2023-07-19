using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [SerializeField] private AudioClip[] _footstepClips;

    private AudioSource _stepSource;

    private Vector3 _lastPlrPos;

    private bool _canPlayStep = true;

    private void Awake() 
    {
        _stepSource = GetComponent<AudioSource>();
        _lastPlrPos = transform.position;
    }

    private void Update() 
    {
        if (PlayerIsMoving())
        {
            CameraBobbing.Instance.SetMovingState(true);
            PlayFootstep();
        }
        else
            CameraBobbing.Instance.SetMovingState(false);

        _lastPlrPos = transform.position;
    }

    private void PlayFootstep()
    {
        if (CameraBobbing.Instance.OffsetY < -.09f && _canPlayStep)
        {
            _stepSource.clip = _footstepClips[Random.Range(0, _footstepClips.Length)];
            _stepSource.pitch = Random.Range(.9f, 1.1f);
            _stepSource.Play();

            _canPlayStep = false;
        }

        if (CameraBobbing.Instance.OffsetY > -.09f)
            _canPlayStep = true;
    }

    private bool PlayerIsMoving() => _lastPlrPos != transform.position;
}
