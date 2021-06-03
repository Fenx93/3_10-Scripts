using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip intro, deathClip, cards, click;
    public AudioSource musicSource, sfxSource;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = intro;
    }

    public void ClickSound()
    {
        sfxSource.clip = click;
        sfxSource.Play();
    }
}
