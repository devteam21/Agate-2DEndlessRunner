using UnityEngine;
using UnityEngine.UI;

public class UIScoreController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Text score;
    [SerializeField]
    private Text highScore;

    [Header("Score")]
    [SerializeField]
    private ScoreController scoreController;

    void Update()
    {
        score.text = scoreController.GetCurrentScore().ToString();
        highScore.text = ScoreData.highScore.ToString();
    }
}
