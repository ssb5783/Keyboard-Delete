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
public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float jumpPower = 5f;

    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float hAxis;
    float vAxis;

    bool getJumpingButton;
    bool getRollingKey;
    bool getSuicideKey;
    bool isRolling;
    bool isJumping;

    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;
    private Vector3 moveVector;
    private Rigidbody mainRigidbody;
    private Animator animator;
    public GameObject playerRig;
    public CapsuleCollider mainColider;
    public Transform mainCamera;

    // 점프 오디오 소스
    public AudioSource activeSoundEffect;
    public AudioSource deathSoundEffect;

    void Awake()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        mainRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // 레그돌 초기 비활성화
        DisableRagdoll();
    }

    // 충돌 물리를 제어하고 싶다.
    void FreezeRotation()
    {

        mainRigidbody.angularVelocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        FreezeRotation();
    }

    void Update()
    {
        InitInput();
        Move();
        Jump();
        Suicide();
        Roll();
        FallDown();
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
        moveVector = new Vector3(hAxis, 0, vAxis).normalized;

        if (moveVector.magnitude >= 0.1f)
        {
            MoveSmooth(moveVector);

        }
        // 블랜드된 에니메이터를 발생시킨다.크기를 발생시킨다.
        animator.SetFloat("isRunning", moveVector.magnitude);
        

    }

    void MoveSmooth(Vector3 moveVector)
    {
        // 플레이어의 방향 각 값을 알고싶다. (좌표에 해당하는 방향을 만든다, 각을 도로 만든다.) - 카메라와 플레이어가 함께 향하는 방향
        float targetAngle = Mathf.Atan2(moveVector.x, moveVector.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;

        // 플레이어가 부드러운 각 이동을 만들고 싶다. (현재 지정 각도, 타겟 각도, 속도, 걸리는 시간)
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        // 플레이어의 회전을 만든다. (y축을 기준으로 회전한다.)
        transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

        //카메라의 회전 방향을 고려한 앞 방향으로 이동을 하고싶다.
        Vector3 cameraMoveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        transform.position += cameraMoveDir * speed * Time.deltaTime;
    }

    void Jump()
    {
        // 무한 점프를 막아야 한다. 점프 버튼을 누르고 점프를 하지 않았을 때, 구르기 도중에 실행되지 않아야 한다.
        if (getJumpingButton && !isJumping && !isRolling)
        {
            mainRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            animator.SetBool("isJumped", true);
            // 활동 소리를 플레이 한다.
            activeSoundEffect.Play();
            animator.SetTrigger("isJumping");
            isJumping = true;
        }
    }    
    
    void Roll()
    {
        // shift키를 누르고 점프를 하지 않았을 때 실행한다. 구르기 도중에 실행되지 않는다.
        if (getRollingKey && !isJumping && moveVector != Vector3.zero && !isRolling)
        {

            speed *= 2;
            animator.SetTrigger("isRolling");
            activeSoundEffect.Play();
            isRolling = true;

            // 구르기 실행 시간차를 생성하여 연속 누르는 것을 막는다.
            Invoke("RollOut", 1.5f);
        }
    }

    // 구르는 것이 끝나는 것을 지정
    void RollOut()
    {
        speed *= 0.5f;
        isRolling = false;
    }

    // 바닥에 닿았을 때 점프를 초기화 한다.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            animator.SetBool("isJumped", false);
            isJumping = false;

        }
        //적이랑 충돌하면 체력을 깍는다.
        if (collision.gameObject.tag == "Trap")
        {
            Vector3 collisionVector = transform.position - collision.transform.position;

            // 물체의 속도가 일정 이상일 때 데미지를 입는다.
            if (collision.rigidbody.velocity.magnitude > 1f)
            {
                StartCoroutine(OnDamage(collisionVector));

            }

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
        if (PlayerHP.instance.HP > 0)
        {
            //print("[애니메이션] 플레이어가 넘어진다.");
            mainRigidbody.AddForce(collisionVector * 5, ForceMode.Impulse);
        }

        else
        {
            //print("[UI] 죽는 로직이 생성되고 다시 시작한다.");
            mainRigidbody.AddForce(collisionVector * 5, ForceMode.Impulse);
            EnableRagdoll();
            GameManager.instance.GameOver();
        }
    }

    // G키를 눌렀을 때 자살 기능을 구현하고 싶다.
    void Suicide()
    {
        if (getSuicideKey)
        {
            EnableRagdoll();
            GameManager.instance.GameOver();
        }
    }

    // 플레이어가 특정 위치까지 떨어지면 레그돌 후 종료한다.
    void FallDown()
    {
        if (mainRigidbody.position.y < -20f)
        {
            EnableRagdoll();
            GameManager.instance.GameOver();
        }
    }
    private void DisableRagdoll()
    {
        // 플레이어 하위 바디, 충돌체 접근
        ApproachSubPhyscisCollision(false, true);
        ApproachParentPhysicsCollision(true, false);
        animator.enabled = true;

    }

    private void EnableRagdoll()
    {
        ApproachSubPhyscisCollision(true, false);
        ApproachParentPhysicsCollision(false, true);
        deathSoundEffect.Play();
        animator.enabled = false;
    }

    // 모든 하위 부위의 Collider와 Rigidbody에 접근하고 싶다.
    private void ApproachSubPhyscisCollision(bool isColliderEnabled, bool isKinematicEnabled)
    {
        foreach (var collider in ragdollColliders)
        {
            collider.enabled = isColliderEnabled;
        }

        foreach (var rigidBody in ragdollRigidbodies)
        {
            // 모든 하위 부위가 물리적 힘을 받지 않는가? - false 받는다.
            rigidBody.isKinematic = isKinematicEnabled;
        }
    }
    private void ApproachParentPhysicsCollision(bool isColliderEnabled, bool isKinematicEnabled)
    {
        mainColider.enabled = isColliderEnabled;
        mainRigidbody.isKinematic = isKinematicEnabled;
    }
}
