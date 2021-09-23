using UnityEngine;

public class ScoreController : MonoBehaviour
{
    
    [Header("Score Highlight")]
    [SerializeField]
    private int scoreHighlightRange;
    [SerializeField]
    private CharacterSoundController sound;

    private int currentScore = 0;
    private int lastScoreHighlight = 0;

    public float GetCurrentScore() => currentScore;

    private void Start() => currentScore = lastScoreHighlight = 0;
    public void FinishScoring() { if (currentScore > ScoreData.highScore) { ScoreData.highScore = currentScore; } }

    public void IncreaseCurrentScore(int increment)
    {
        currentScore += increment;

        if (currentScore - lastScoreHighlight > scoreHighlightRange)
        {
            sound.PlaySFX(sound.ScoreHighLightSound);
            lastScoreHighlight += scoreHighlightRange;
        }
    }

}
