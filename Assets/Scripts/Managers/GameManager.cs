using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance {  get; private set; }
    [Header("�׸�")] public Grill[] _grill;
    [Header("���̺�")] public Table[] _table;
    [Header("��ȣ ���� ������Ʈ")] public InteractionObject[] _interactObjs; // ī���� , ���̺� , ȭ�ε��� ��� �迭 - level �� ���� ������ ���� ���
    [Header("���� ������ �� ������Ʈ")] public int[] _GroundMoney;

    [Header("���� ��")] public Door _isOpenDoor;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    //��� ������Ʈ Ȱ��ȭ �� ��Ȱ��ȭ
    public void ActiveAllObject()
    {
        //�� Ȱ��ȭ
        _isOpenDoor.SetActiveObj(true);
    }

    // ��ȣ �ۿ� ������Ʈ �迭���� ���鼭 Ȱ��ȭ , ��Ȱ��ȭ ����
    public void CreateInteractionObject()
    {
        for (int i=0; i< _interactObjs.Length; i++)
        {
            if (_interactObjs[i].objectIsActive == true)
            {
                _interactObjs[i].gameObject.SetActive(true);
            }
            else
            {
                _interactObjs[i].gameObject.SetActive(false);
            }
        }
    }
}