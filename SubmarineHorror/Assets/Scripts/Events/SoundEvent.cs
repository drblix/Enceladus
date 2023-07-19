using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SoundEvent : MonoBehaviour
{
    private enum OriginDirection
    {
        Forward = 0,
        ForwardRight = 1,
        Right = 2,
        Back = 3,
        Left = 4,
        ForwardLeft = 5
    }

    [SerializeField] private bool _enabled = true;

    [SerializeField] private OriginDirection _chosenDirection;

    [SerializeField] private SFXManager.AudioClips _chosenClip;

    [SerializeField] private float _clipDelay = 0f;

    [SerializeField] [Range(0f, 1f)] private float _volume = 1f;

    private bool _used = false;

    private void Awake()
    {
        if (!_enabled) Destroy(this);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("SubObj") && !_used)
        {
            _used = true;
            SFXManager.Instance.PlaySpecificNoise((int)_chosenClip, (int)_chosenDirection, _clipDelay, _volume);
        }
    }
}
