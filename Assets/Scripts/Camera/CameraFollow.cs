using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // ���� ��� (�÷��̾�)
    public Transform target;

    // ī�޶� ���� ���� ��ġ ������ (ī�޶� ���� ���� 8, ���� 14, �ڷ� 12 ������ ��ġ)
    public Vector3 offset = new Vector3(-8, 14f, -12f);

    void FixedUpdate()
    {
        // ��ǥ ��ġ = ��� ��ġ + ������
        Vector3 desiredPosition = target.position + offset;

        // ���� ī�޶� ��ġ ����
        transform.position = desiredPosition;

        // ��� �ٶ󺸱� (ī�޶� �÷��̾ ����)
        transform.LookAt(target);
    }
}
