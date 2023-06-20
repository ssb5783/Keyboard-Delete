using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 일정 시간이 되면 레그돌 후 일어나고 싶다.
public class PlayerRagdoll : MonoBehaviour
{
    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;
    private Rigidbody mainRigidbody;
    private Animator animator;
    public CapsuleCollider mainColider;
    public AudioSource deathSoundEffect;

    // 싱글톤 생성
    public static PlayerRagdoll instance;

    private void Awake()
    {
        instance = this;
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        mainRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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
}
