using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator = null;
    Rigidbody2D rb = null;

    public float flapForce = 6f;
    public float forwardSpeed = 3f;
    public bool isDead = false;
    float deathCooldown = 0f;

    bool isFlap = false;

    public bool godMode = false;

    GameManager gameManager;

    void Start()
    {
        // 클래스 명으로 객체에 접근 (접근할 때 이미 클래스의 Awake에서 this로 객체를 저장해둔 상태이기 때문에 바로 접근 가능)
        gameManager = GameManager.Instance;

        animator = transform.GetComponentInChildren<Animator>();
        rb = transform.GetComponent<Rigidbody2D>();

        // 예외처리
        if (animator == null)
        {
            Debug.LogError("Not Founded Animator");
        }

        if (rb == null)
        {
            Debug.LogError("Not Founded Rigidbody");
        }
    }

    void Update()
    {
        if (isDead)
        {
            if (deathCooldown <= 0)
            {
                // 스페이스바나 마우스의 좌클릭을 사용할 경우. (0)은 모바일의 경우 터치
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    // 게임 재시작
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                    {
                        gameManager.RestartGame();
                    }
                }
            }
            else
            {
                deathCooldown -= Time.deltaTime;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                isFlap = true;
            }
        }
    }

    public void FixedUpdate()
    {
        if (isDead) return;

        Vector3 velocity = rb.velocity; // 가속도
        velocity.x = forwardSpeed;

        if (isFlap)
        {
            velocity.y += flapForce;
            isFlap = false;
        }

        rb.velocity = velocity; // Vector3는 구조체라 값을 가져오기만 한 것이고 값을 변환한게 아니기 때문에 실제 값을 넣어줌

        float angle = Mathf.Clamp((rb.velocity.y * 10f), -90, 90); // 회전 각도 제한
        float lerpAngle = Mathf.Lerp(rb.velocity.y, angle, Time.deltaTime * 5f); // 프레임 통일
        transform.rotation = Quaternion.Euler(0, 0, lerpAngle); // z축 회전 적용
    }

    /// <summary>
    /// 충돌 메서드
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (godMode) return;

        if (isDead) return;

        animator.SetInteger("isDie", 1); // 애니메이터에서 isDie 정수형 파라미터 1로 변경
        gameManager.GameOver();
        isDead = true;
        deathCooldown = 1f;
    }
}