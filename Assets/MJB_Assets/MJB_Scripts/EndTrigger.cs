using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 도달할때 종료 트리거를 발생시키고 싶다.
public class EndTrigger : MonoBehaviour
{

    // 호출 되었을 때 게임 완료를 발생시킨다.
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.CompleteGame();
        // 게임 오브젝트 플레이어를 삭제한다.
        Destroy(other.gameObject);
    }
}
