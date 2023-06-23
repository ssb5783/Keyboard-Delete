using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 일정 시간이 되면 레그돌 후 일어나고 싶다.
//플레이어가 부딪힌 방향에서 일어나고 싶다.
//플레이어가 레그돌 후 엉덩이가 부딪힌 포지션을 가져오고 싶다.
//플레이어가 레그돌 한 뼈와 일어나는 뼈를 가져와서 재배치하고 싶다.
//플레이어가 레그돌한 애니메이션 뼈대를 가져오고싶다.
//플레이어의 뼈대를 다시 원위치 시키고 싶다.
public class PlayerRagdoll : MonoBehaviour
{
    // 뼈대의 트랜스폼의 위치와 회전을 가져온다.
    public class BoneTransform
    {
        // 위치를 가져온다.
        public Vector3 Position { set; get; }

        // 회전 각을 가져온다.
        public Quaternion Rotation { set; get; }
    }


    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;
    private Rigidbody mainRigidbody;
    // 플레이어의 뼈를 가져온다.
    private Transform hipsBone;
    private Animator animator;
    public CapsuleCollider mainColider;
    public AudioSource deathSoundEffect;

    // 플레이어가 일어날때 뼈, 레그돌할때 뼈, 뼈의 위치를 가져온다.
    public BoneTransform[] standingUpBoneTransform;
    public BoneTransform[] ragdollBoneTransform;
    private Transform[] bones;

    //일정 시간이 되면 일어난다
    public float timeToWakeUp = 5f;


    //클립의 이름을 가져오고 싶다.
    [SerializeField]
    private string standingUpClipName;

    // 뼈대가 다시 완성하는데 걸리는 시간을 지정하고 싶다.
    [SerializeField]
    private float timeToResetBones;
    public float elaspedResetBonesTime;

    // 싱글톤 생성
    public static PlayerRagdoll instance;


    private void Awake()
    {
        instance = this;
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        mainRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        initBones();
    }


    void initBones()
    {
        hipsBone = animator.GetBoneTransform(HumanBodyBones.Hips);
        // 엉덩이 뼈의 모든 뼈대 트랜스폼을 가져온다.
        bones = hipsBone.GetComponentsInChildren<Transform>();
        // 일어날때 뼈와 , 레그돌할 때 벼의 길이들을 초기화 한다.
        standingUpBoneTransform = new BoneTransform[bones.Length];
        ragdollBoneTransform = new BoneTransform[bones.Length];

        // 뼈대의 배열안에 뼈대의 길이와 회전 각을 넣는다.
        for (int boneIndex = 0; boneIndex < bones.Length; boneIndex++)
        {
            standingUpBoneTransform[boneIndex] = new BoneTransform();
            ragdollBoneTransform[boneIndex] = new BoneTransform();
        }

        // 처음 뼈대의 시작 위치를 지정한다.
        PutAnimationStartBoneTransform(standingUpClipName, standingUpBoneTransform);

        DisableRagdoll();
    }
    public void DisableRagdoll()
    {
        // 플레이어 하위 바디, 충돌체 접근
        ApproachSubPhyscisCollision(false, true);
        ApproachParentPhysicsCollision(true, false);
        animator.enabled = true;
    }

    public void EnableRagdoll()
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

    public void TagPositionToHips()
    {
        //1.초기 엉덩이 위치를 가져온다.
        Vector3 originalHipsPosition = hipsBone.position;
        //2. 플레이어의 위치는 엉덩이 위치에 있다.
        transform.position = hipsBone.position;
        //3. 플레이어의 위치에서 밑 방향으로 레이케스트를 발사하여 받은 정보가 있다면
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo))
        {
            //4. 플레이어의 위치는 플레이어의 x축 방향, 부딪힌 y값, 플레이어의 z 포지션 값이다.
            transform.position = new Vector3(transform.position.x, hitInfo.point.y, transform.position.z);
        }
        // 부딪힌 엉덩이의 위치는 초기 위치로 지정한다.
        hipsBone.position = originalHipsPosition;

    }

    // 뼈대의 길이와 회전각을 각각 넣고 싶다.
    public void PutBoneTransform(BoneTransform[] boneTransforms)
    {
        // 뼈들의 길이 만큼 접근한다.
        for (int boneIndex = 0; boneIndex < bones.Length; boneIndex++)
        {
            // 뼈들의 트랜스폼 회전 각, 위치 값들을, 원래 뼈들의 이전 포지션과 회전값을 넣는다.
            boneTransforms[boneIndex].Position = bones[boneIndex].localPosition;
            boneTransforms[boneIndex].Rotation = bones[boneIndex].localRotation;
        }
    }


    // 플레이어가 넘어졌을 때 클립의 애니메이션 뼈대를 가져오고 싶다.
    public void PutAnimationStartBoneTransform(string clipName, BoneTransform[] boneTransforms)
    {
        // 애니메이션 실행하기 전의 샘플링을 현재 위치값으로 지정하고
        Vector3 positionBeforeSampling = transform.position;
        // 회전각 샘플링을 이전의 회전각으로 만든다.
        Quaternion rotationBeforeSampling = transform.rotation;

        // 애니메이터야 너가 가지고 있는 실행 애니메이터 콘트롤러야 너의 모든 애니메이션 클립을 가져와서
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            // 너가 지정한 클립이 이름이 존재한다면
            if (clip.name == clipName)
            {
                // 애니메이션의 처음 클립 샘플링을 해
                clip.SampleAnimation(gameObject, 0);
                // 일어날 때의 뼈대를 넣어
                PutBoneTransform(standingUpBoneTransform);
                break;
            }
        }
        // 샘플링 후 위치와 회전 값을 처음으로 만든다.
        transform.position = positionBeforeSampling;
        transform.rotation = rotationBeforeSampling;
    }

    //뼈를 재새팅하고 싶다.
    public void ResetBones()
    {
        // 경과 시간이 일정시간 흐르고
        elaspedResetBonesTime += Time.deltaTime;
        // 경과 백분율을 구한다. (흘러간 시간 / 내가 지정한 시간)
        float elaspedResetTimePercent = elaspedResetBonesTime / timeToResetBones;
        // 뼈의 길이에 반복하여
        for (int boneIndex = 0; boneIndex < bones.Length; boneIndex++)
        {
            // Lerp를 사용하여 레그돌, 일어날 때의 뼈의 위치와, 회전각 백분율을 만든다.
            bones[boneIndex].localPosition = Vector3.Lerp(
                ragdollBoneTransform[boneIndex].Position,
                standingUpBoneTransform[boneIndex].Position,
                elaspedResetTimePercent
                );

            bones[boneIndex].localRotation = Quaternion.Lerp(
                ragdollBoneTransform[boneIndex].Rotation,
                standingUpBoneTransform[boneIndex].Rotation,
                elaspedResetTimePercent
                );
        }

        // 경과 시간 비율이 1초를 넘긴다면
        if (elaspedResetTimePercent >= 1)
        {
            DisableRagdoll();
            // 레그돌을 비활성화 하고 일어나는 애니메이션을 실행한다.
            animator.SetTrigger("isStandingUp");
        }
    }
}
