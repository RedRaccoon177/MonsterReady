using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // 따라갈 대상 (플레이어)
    public Transform target;

    // 카메라가 따라갈 때의 위치 오프셋 (카메라 기준 좌측 8, 위로 14, 뒤로 12 떨어진 위치)
    public Vector3 offset = new Vector3(-8, 14f, -12f);

    void FixedUpdate()
    {
        // 목표 위치 = 대상 위치 + 오프셋
        Vector3 desiredPosition = target.position + offset;

        // 실제 카메라 위치 갱신
        transform.position = desiredPosition;

        // 대상 바라보기 (카메라가 플레이어를 향함)
        transform.LookAt(target);
    }
}
