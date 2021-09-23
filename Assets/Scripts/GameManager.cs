using UnityEngine;

public enum GameState
{
    Ready,
    Start,
    GameOver,
    Win
}

public class GameManager : MonoBehaviour
{

    [Header("BGM")]
    [SerializeField]
    private AudioSource bgmSource;

    [Header("GameState")]
    [SerializeField]
    private GameState gameState;

    private static GameManager instance = null;
    public static GameManager Instance { get { if (instance == null) instance = FindObjectOfType<GameManager>(); return instance; } }

    public GameState GameState
    {
        get => gameState;
        set => gameState = value;
    }
    public void ChangeStateToStartOnClick() => gameState = GameState.Start;
    public void StopBackgroundSound() => bgmSource.Stop();
}
