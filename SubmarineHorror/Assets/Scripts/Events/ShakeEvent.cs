using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ShakeEvent : MonoBehaviour
{
    [SerializeField] private bool _enabled = true;

    [SerializeField] private float _shakeDuration = 0.5f;
    [SerializeField] private float _shakeMagnitude = 0.25f;
    [SerializeField] private float _shakeDelay = 0f;

    [SerializeField] private bool _shakeFadeOut = false;

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
            StartCoroutine(PlayerMovement.Instance.ShakeCamera(_shakeDuration, _shakeMagnitude, _shakeFadeOut, _shakeDelay));
        }
    }
}
