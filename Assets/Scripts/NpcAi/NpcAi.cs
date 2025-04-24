using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum NpcState
{
   MoveToPickUp,
   MoveToPutDown,
   PickUp,
   PutDown,
   Idle
}
enum NpcPickUpObject
{
    Meat,
    Trash,
    MeatSat,
    None
}
public class NpcAi : MonoBehaviour
{
    public string _keyName;
    int _currentLevel;
    int _currentMaxLevel;
    float _moveSpeed;
    int _holdMaxAmount;
    bool _isUnlockNpc;
    // ���� ����Ʈ  = ��� �ĺ��鸸 ��Ƶδ� �켱���� ť or ����Ʈ
    Queue<Node> _openList;
    // �� ��忡 ���� g, h, f �� + �θ� ������ �����ϴ� ������ �����
    Dictionary<Node, NodeFGH> _nodeDataDict = new Dictionary<Node, NodeFGH>();
    // �湮 ��� �����
    HashSet<Node> visited = new HashSet<Node>();
    NpcState _npcState;
    NpcPickUpObject _pickUpObject;
    Vector3 _goalPos;

    private void Start()
    {
        _npcState = NpcState.Idle;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartNavMove();
        }
    }
    public void StartNavMove()
    {
    }
    public double Distance(Vector2 startIdxPos, Vector2 goalIdxPos)
    {
        var _dx = goalIdxPos.x - startIdxPos.x;
        var _dy = goalIdxPos.y - startIdxPos.y;
        return (1.2 * Mathf.Min(_dx, _dy)) + 1 * (Mathf.Max(_dx, _dy) - Mathf.Max(_dx, _dy));
    }


    /// <summary>
    /// ������ ����
    /// </summary>
    //public void SetDestination()
    //{
    //    var tableList = GameManager._instance._tables;
    //    var counterList = GameManager._instance._counters;
    //    var grillList = GameManager._instance._grills;
    //    if (_pickUpObject == NpcPickUpObject.None)
    //    {
    //        // PickUp������ , �׸� , ���̺� , ī���� 3
    //    }
    //    // PutDown ������ ,
    //    // �׸� -> ī���� 1,3
    //    // ���̺� -> ��������
    //    // ī����3 -> ī����2
    //
    //}
    public void SetPutDownDestination()
    {
        switch (_pickUpObject)
        {
            case NpcPickUpObject.Meat:
                // ī���� 1 or ī����3
                break;
            case NpcPickUpObject.Trash:
                // ��������
                break;
            case NpcPickUpObject.MeatSat:
                // ī���� 2
                break;
            case NpcPickUpObject.None:
                // idel��
                break;
        }
    }
    public void SetPickUpDestination()
    {
        var a = GameManager._instance._tables;
        for (int i=0; i<a.Length; i++)
        { 
            if(a[i]._trashCount > 0)
            {
                break;
            }
        }

        var b = GameManager._instance._grills;
        for (int i = 0; i < b.Length; i++)
        {
            if (b[i]._currentMeatCount > 0)
            {
                break;
            }
        }
        // �׸� OR ���̺� OR ī����3
    }
    public void ChangeState()
    {
        if (_npcState == NpcState.Idle)
        {
            SetPickUpDestination();
            _npcState = NpcState.MoveToPickUp;
        }
        else if (_npcState == NpcState.MoveToPickUp)
        {
            //���������� �̵�
        }
        else if (_npcState == NpcState.MoveToPutDown)
        {

        }
        else if (_npcState == NpcState.PickUp)
        {
            SetPutDownDestination();
        }
        else if (_npcState == NpcState.PutDown)
        {

        }
    }
}
