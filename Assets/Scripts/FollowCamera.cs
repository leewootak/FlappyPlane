using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    float offsetX;

    void Start()
    {
        if (target == null) return;

        offsetX = transform.position.x - target.position.x; // 처음 카메라 포지션과 타겟을 포지션 거리 저장
    }

    /// <summary>
    /// transform.position.x를 바로 바꾸려고 하면 오류가 생기기 때문에 변수에 저장했다가 사용하는 과정 필요
    /// </summary>
    void Update()
    {
        if (target == null) return;

        Vector3 pos = transform.position; // 내 위치 가져오기
        pos.x = target.position.x + offsetX; // 캐릭터 위치에서 offsetX만큼 떨어져서 이동
        transform.position = pos;
    }
}
