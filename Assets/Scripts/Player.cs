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

    void Start()
    {
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
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) // 스페이스바나 마우스를 사용할 경우. (0)은 좌클릭 모바일의 경우 터치
                {
                    // 게임 재시작
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

        float angle = Mathf.Clamp((rb.velocity.y * 10f), -90, 90);
        float lerpAngle = Mathf.Lerp(rb.velocity.y, angle, Time.deltaTime * 5f);
        transform.rotation = Quaternion.Euler(0, 0, lerpAngle);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (godMode)
            return;

        if (isDead)
            return;

        animator.SetInteger("IsDie", 1);
        isDead = true;
        deathCooldown = 1f;
    }
}