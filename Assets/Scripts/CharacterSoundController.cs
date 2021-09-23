using UnityEngine;

public class CharacterSoundController : MonoBehaviour
{
    [SerializeField]
    private AudioClip gameOverSound;
    [SerializeField]
    private AudioClip scoreHighlightSound;
    [SerializeField]
    private AudioClip jumpSound;
    [SerializeField]
    private AudioSource audioSource;

    public AudioClip ScoreHighLightSound => scoreHighlightSound;
    public AudioClip GameOverSound => gameOverSound;
    public AudioClip JumpSound => jumpSound;

    public void PlaySFX(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
