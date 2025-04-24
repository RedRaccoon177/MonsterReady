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
    // 오픈 리스트  = 경로 후보들만 담아두는 우선순위 큐 or 리스트
    Queue<Node> _openList;
    // 각 노드에 대한 g, h, f 값 + 부모 정보를 저장하는 데이터 저장소
    Dictionary<Node, NodeFGH> _nodeDataDict = new Dictionary<Node, NodeFGH>();
    // 방문 노드 저장용
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
    /// 목적지 설정
    /// </summary>
    //public void SetDestination()
    //{
    //    var tableList = GameManager._instance._tables;
    //    var counterList = GameManager._instance._counters;
    //    var grillList = GameManager._instance._grills;
    //    if (_pickUpObject == NpcPickUpObject.None)
    //    {
    //        // PickUp목적지 , 그릴 , 테이블 , 카운터 3
    //    }
    //    // PutDown 목적지 ,
    //    // 그릴 -> 카운터 1,3
    //    // 테이블 -> 쓰레기통
    //    // 카운터3 -> 카운터2
    //
    //}
    public void SetPutDownDestination()
    {
        switch (_pickUpObject)
        {
            case NpcPickUpObject.Meat:
                // 카운터 1 or 카운터3
                break;
            case NpcPickUpObject.Trash:
                // 쓰레기통
                break;
            case NpcPickUpObject.MeatSat:
                // 카운터 2
                break;
            case NpcPickUpObject.None:
                // idel로
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
        // 그릴 OR 테이블 OR 카운터3
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
            //목적지까지 이동
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
