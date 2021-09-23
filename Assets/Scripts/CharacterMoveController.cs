using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{

    [Header("Player Anim State")]
    [SerializeField]
    private bool isJumping;
    [SerializeField]
    private bool isOnGround;

    [Header("Player Stats")]
    [SerializeField]
    private float moveAccel;
    [SerializeField]
    [Range(10,20)]
    private float speed;
    [SerializeField]
    private float jumpAccel;

    [Header("Ground Raycast")]
    [SerializeField]
    private float groundRaycastDistance;
    [SerializeField]
    private LayerMask groundLayerMask;

    [Header("Camera")]
    [SerializeField]
    private CameraFollow cameraFollow;

    [Header("Scoring")]
    [SerializeField]
    private ScoreController score;
    [SerializeField]
    private float scoringRatio;
    [SerializeField]
    private float lastPositionX;

    [Header("GameOver")]
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private float fallPositionY;

    private Rigidbody2D rb;
    private Animator anim;
    private CharacterSoundController sound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sound = GetComponent<CharacterSoundController>();
    }

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.Start)
        {
            if (Input.GetKeyDown("space"))
            {
                if (isOnGround)
                {
                    isJumping = true;
                    sound.PlaySFX(sound.JumpSound);
                }
            }

            anim.SetBool("isOnGround", isOnGround);

            CalculateScore();

            if(speed < 20) 
                IncerementSpeed();

            if (transform.position.y < fallPositionY)
            {
                GameOver();
            }
        }
    }

    float counter = 0;
    void IncerementSpeed()
    {
        counter += Time.deltaTime;

        if (counter >= 15)
        {
            speed++;
            counter = 0;
        }
    }

    void CalculateScore()
    {
        int distancePassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
        int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);

        if (scoreIncrement > 0)
        {
            score.IncreaseCurrentScore(scoreIncrement);
            lastPositionX += distancePassed;
        }
    }

    void GameOver()
    {
        GameManager.Instance.GameState = GameState.GameOver;
        GameManager.Instance.StopBackgroundSound();
        sound.PlaySFX(sound.GameOverSound);
        score.FinishScoring();
        cameraFollow.enabled = false;
        gameOverScreen.SetActive(true);
        enabled = false;
    }

    void FixedUpdate()
    {
        if(GameManager.Instance.GameState == GameState.Start)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);

            if (hit)
            {
                if (!isOnGround && rb.velocity.y <= 0) isOnGround = true;
            }
            else
                isOnGround = false;

            Vector2 velocityVector = rb.velocity;
            velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, speed);

            if (isJumping)
            {
                velocityVector.y += jumpAccel;
                isJumping = false;
            }

            rb.velocity = velocityVector;
        }
    }

    void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.red);
    }
}
