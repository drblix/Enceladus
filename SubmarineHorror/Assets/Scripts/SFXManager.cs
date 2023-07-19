using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private static SFXManager _instance;
    public static SFXManager Instance { get { return _instance; } }

    private PlayerMovement playerMovement;

    [SerializeField] private AudioSource[] audioSources;

    [SerializeField] private AudioClip[] audioClips;

    public enum AudioClips
    {
        CreepNoise1,
        CreepNoise2,
        CreepNoise3,
        CreepNoise4,
        CreepNoise5,
        CreepNoise6,
        MetalSlam
    }

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    private bool IsSourcePlaying()
    {
        foreach (AudioSource source in audioSources)
            if (source.isPlaying)
                return true;

        return false;
    }

    /// <summary>
    /// Plays a random noise
    /// </summary>
    /// <param name="shakeCamera">Should the camera shake with the audio?</param>
    /// <param name="shakeDuration">Length of the shake</param>
    /// <param name="shakeMagnitude">Strength of the shake</param>
    /// <param name="shakeFade">Should the shake fade out?</param>
    public void PlayRandomNoise(float volume = 1f)
    {
        if (IsSourcePlaying()) return;

        AudioClip clip = audioClips[Random.Range(0, audioClips.Length)];
        AudioSource source = audioSources[Random.Range(0, audioSources.Length)];

        source.clip = clip;
        source.volume = volume;
        source.Play();
    }

    /// <summary>
    /// Plays a specific noise at a specified location with needed delay
    /// </summary>
    /// <param name="clipNum">Clip to play</param>
    /// <param name="location">Location of where to play the clip (0-5)</param>
    /// <param name="delay">Delay before playing the clip</param>
    /// <param name="shakeCamera">Should the camera shake with the audio?</param>
    /// <param name="shakeDuration">Length of the shake</param>
    /// <param name="shakeMagnitude">Strength of the shake</param>
    /// <param name="shakeFade">Should the shake fade out?</param>
    public void PlaySpecificNoise(int clipNum, int location, float delay, float volume)
    {
        if (IsSourcePlaying()) { return; }

        AudioClip clip = audioClips[clipNum];
        AudioSource source = audioSources[location];

        if (clip && source)
        {
            source.clip = clip;
            source.volume = volume;

            if (delay > 0f)
            {
                source.PlayDelayed(delay);
            }
            else
            {
                source.Play();
            }
        }
    }
}
