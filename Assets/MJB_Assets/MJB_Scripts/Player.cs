using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자를 움직이고 점프하게 하고 싶다.
// 무한 점프를 막고 싶다.
// 에니메이션을 사용하고 싶다.
// 충돌시 넘어짐을 구현하고 싶다.
// 충돌시 플레이어가 돌아가는 문제를 해결하고 싶다.
// 플레이어의 피격 기능을 구현하고 싶다.
// 플레이어에게 넛백 기능을 구현하고 싶다.
// 플레이어가 물체의 속도가 0이 아닐때는 데미지를 입지 않는다.
// 플레이어가 부드러운 회전각으로 이동하고 싶다.
// 플레이어와 카메라가 같은 방향으로 이동하고 싶다.
// 플레이어가 Ragdoll 충돌을 하고 싶다.
// 플레이어가 G 키를 누르면 자살 기능을 구현하고 싶다.
// 플레이어가 Cntrl 키를 누르면 구르는 기능을 구현하고 싶다. - 구르기를 할 때 구르기 방향으로 간다.
// 플레이어가 죽을때, 던질때, 점프할때 소리를 내고싶다.
// 플레이어 점프를 구현하고 싶다.
// 플레이어가 트랩에 부딪혔을 때 움직이지 못하고 안 구르게 만들고 싶다.
public class Player : MonoBehaviour
{
    public const float ZERO_VALUE = 0;
    public const string TRAP_NAME = "Trap";
    public const string ENEMY_NAME = "Enemy";
    public const string FLOOR_NAME = "Floor";
    public const string IS_JUMPING_NAME = "isJumping";
    public const string IS_JUMPED_NAME = "isJumped";
    public const float FALL_OFF_MAX_VALUE = -20f;
    public const float FALL_VELOCITY_MAX_VALUE = -5.5f;
    public const float MOVE_MIN_MAGNITUDE = 0.1f;
    public const float MOVE_DAMPING_TIME = 0.08f;
    public const float DAMAGED_MIN_VELOCITY = 0.1f;
    // 적의 플레이어를 가져온다.
    // 스크립트를 가져오고 싶다.
    private GameObject enemy;

    public float speed = 5f;
    public float jumpPower = 5f;
    public float damagedPower = 5f;

    //일정 시간이 되면 일어난다
    public float timeToWakeUp = 3f;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private float hAxis;
    private float vAxis;

    private bool getJumpingButton;
    private bool getRollingKey;
    private bool getSuicideKey;
    private bool isRolling;
    private bool isJumping;
    private bool isGrounded;
    private bool isRagdolled;
    private bool isFalling;

    private Vector3 moveVector;
    private Rigidbody mainRigidbody;
    private Animator animator;
    public Transform mainCamera;
    public AudioSource activeSoundEffect;

    void Awake()
    {
        mainRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        // 애너미를 찾자
        enemy = GameObject.Find("Enemy");
    }
    void FreezeRotation()
    {

        mainRigidbody.angularVelocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        FreezeRotation();
        Move();
    }

    void Update()
    {
        InitInput();
        Jump();
        Suicide();
        Roll();
        FallDown();
        WakeUp();
        DamagedFromEnemy();

    }

    void DamagedFromEnemy()
    {
        if (enemy.GetComponent<SSB_Enemy_New>().m_state == SSB_Enemy_New.EnemyState.Attack)
        {
            StartCoroutine(OnDamage(Vector3.zero));
        }
    }

    void InitInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        getJumpingButton = Input.GetButtonDown("Jump");
        getSuicideKey = Input.GetKeyDown("g");
        getRollingKey = Input.GetKeyDown(KeyCode.LeftShift);
    }

    void Move()
    {
        moveVector = new Vector3(hAxis, ZERO_VALUE, vAxis);
        Vector3 normalizedMoveVector = moveVector.normalized;

        // 레그돌 하지 않았을때, 떨어지고 않았을때 움직일 수 있게 한다.
        if (!isRagdolled && !isFalling)
        {
            if (normalizedMoveVector.magnitude >= MOVE_MIN_MAGNITUDE)
            {
                MoveSmooth(normalizedMoveVector);
            }

            // 블랜드된 에니메이터를 발생시킨다.크기를 발생시킨다. 속도를 제어하고 시간마다 변경한다.
            float clamp01Velocity = Mathf.Clamp01(moveVector.magnitude);
            // 벡터 속도의 크기를 강제로 0 ~ 1로 변경한다.

            //땅에 있을때만 방향 키를 눌렀을 때 애니메이션을 실행한다.
            animator.SetFloat("isRunning", clamp01Velocity, MOVE_DAMPING_TIME, Time.deltaTime);

        }
    }

    void MoveSmooth(Vector3 moveVector)
    {
        // 플레이어의 방향 각 값을 알고싶다. (좌표에 해당하는 방향을 만든다, 각을 도로 만든다.) - 카메라와 플레이어가 함께 향하는 방향
        float targetAngle = Mathf.Atan2(moveVector.x, moveVector.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;

        // 플레이어가 부드러운 각 이동을 만들고 싶다. (현재 지정 각도, 타겟 각도, 속도, 걸리는 시간)
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        // 플레이어의 회전을 만든다. (y축을 기준으로 회전한다.)
        transform.rotation = Quaternion.Euler(ZERO_VALUE, smoothAngle, ZERO_VALUE);

        //카메라의 회전 방향을 고려한 앞 방향으로 이동을 하고싶다.
        Vector3 cameraMoveDir = Quaternion.Euler(ZERO_VALUE, targetAngle, ZERO_VALUE) * Vector3.forward;

        //transform.position += cameraMoveDir * speed * Time.deltaTime;
        mainRigidbody.MovePosition(transform.position + cameraMoveDir * speed * Time.deltaTime);
    }

    void Jump()
    {
        // 무한 점프를 막아야 한다. 점프 버튼을 누르고 점프를 하지 않았을 때, 구르기 도중에 실행되지 않아야 한다.
        if (getJumpingButton && !isJumping && !isRolling && !isRagdolled && !isFalling)
        {
            mainRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            animator.SetBool(IS_JUMPING_NAME, true);
            animator.SetTrigger(IS_JUMPED_NAME);
            // 활동 소리를 플레이 한다.
            activeSoundEffect.Play();
            isJumping = true;
            // 땅에 없다.
            isGrounded = false;
        }
    }

    void Roll()
    {
        // shift키를 누르고, 점프를 하지 않았고, 앞으로 안가고, 구르지 않았을 때 구른다. 땅에 있을때
        if (getRollingKey && !isJumping && moveVector != Vector3.zero && !isRolling && isGrounded)
        {

            speed *= 2;
            animator.SetTrigger("isRolling");
            activeSoundEffect.Play();
            isRolling = true;
            // 땅에있다.
            isGrounded = true;

            Invoke("RollOut", 1.5f);
        }
    }


    void RollOut()
    {
        speed *= 0.5f;
        isRolling = false;
    }

    // 바닥에 닿았을 때 점프를 초기화 한다.
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 collisionVector = transform.position - collision.transform.position;
        if (collision.gameObject.tag == FLOOR_NAME)
        {
            animator.SetBool(IS_JUMPING_NAME, false);
            isJumping = false;
            //땅에 있다.
            isFalling = false;
            isGrounded = true;
        }

        if (collision.gameObject.tag == TRAP_NAME)
        {
            StartCoroutine(OnDamage(collisionVector));
            animator.SetBool(IS_JUMPING_NAME, false);
            isJumping = false;
            isFalling = false;
            isGrounded = false;
        }

        //적이랑 충돌하면 체력을 깍는다.
        if (collision.gameObject.layer == LayerMask.NameToLayer("ThornWood"))
        {
                StartCoroutine(OnDamage(collisionVector));
        }

    }

    // 코루틴을 사용하여 피격 기능을 구현한다.
    IEnumerator OnDamage(Vector3 collisionVector)
    {
        // 시간 간격을 0.1초로 지정한다.
        yield return new WaitForSeconds(0.1f);
        collisionVector = collisionVector.normalized;
        collisionVector += Vector3.up;

        PlayerHP.instance.HP--;

        PlayerRagdoll.instance.EnableRagdoll();
        isRagdolled = true;
        if (PlayerHP.instance.HP > ZERO_VALUE)
        {
            //print("[애니메이션] 플레이어가 넘어진다.");
            mainRigidbody.AddForce(collisionVector * damagedPower, ForceMode.Impulse);
        }
        else
        {
            //print("[UI] 죽는 로직이 생성되고 다시 시작한다.");
            mainRigidbody.AddForce(collisionVector * damagedPower, ForceMode.Impulse);
            GameManager.instance.GameOver();
        }
    }

    void Suicide()
    {
        if (getSuicideKey)
        {
            PlayerRagdoll.instance.EnableRagdoll();
            GameManager.instance.GameOver();
        }
    }

    void FallDown()
    {
        // 낭떨어지에서 떨어질 때, 일정 속력 이상일 때
        if (mainRigidbody.position.y < FALL_OFF_MAX_VALUE || mainRigidbody.velocity.y < -20)
        {
            PlayerRagdoll.instance.EnableRagdoll();
            GameManager.instance.GameOver();
        }

        // 플레이어가 일정 속력에서 떨어질때 못움직이게 한다.
        if (mainRigidbody.velocity.y < FALL_VELOCITY_MAX_VALUE)
        {
            // 점프 모션을 실행한다. 떨어지는 모션
            animator.SetBool(IS_JUMPING_NAME, true);
            animator.SetTrigger(IS_JUMPED_NAME);
            isFalling = true;
        }
    }
    void WakeUp()
    {
        timeToWakeUp -= Time.deltaTime;
        if (timeToWakeUp <= ZERO_VALUE && isRagdolled)
        {
            PlayerRagdoll.instance.TagPositionToHips();
            PlayerRagdoll.instance.PutBoneTransform(PlayerRagdoll.instance.ragdollBoneTransform);
            PlayerRagdoll.instance.ResetBones();
            PlayerRagdoll.instance.elaspedResetBonesTime = ZERO_VALUE;
            timeToWakeUp = 3f;
            isRagdolled = false;
        }
    }
}
