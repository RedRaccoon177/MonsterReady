using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public interface ICustomerState
{
    void Enter(CustomerAI customer);
    void Update(CustomerAI customer);
    void Exit(CustomerAI customer);
}

public class CustomerAI : MonoBehaviour
{
    ICustomerState _currentState;

    public void SetState(ICustomerState newState)
    {
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState.Enter(this);
    }

    void Update()
    {
        _currentState?.Update(this);
    }
}

public class MoveToCounterState : ICustomerState
{
    List<Node> _path;
    int _currentIndex;

    public void Enter(CustomerAI customer)
    {
        //// 길찾기 시작
        //Node start = FindClosestNode(customer.transform.position);
        //Node goal = FindTargetNode(); // 목적지에 해당하는 노드
        //_path = AStarPathfinder.FindPath(start, goal);
        //_currentIndex = 0;
    }

    public void Update(CustomerAI customer)
    {
        //if (_path == null || _currentIndex >= _path.Count)
        //{
        //    customer.SetState(new IdleState()); // 목적지 도착 처리
        //    return;
        //}

        //// 현재 타겟 노드로 이동
        //Node targetNode = _path[_currentIndex];
        //Vector3 targetPos = targetNode.transform.position;
        //float step = 3f * Time.deltaTime;
        //customer.transform.position = Vector3.MoveTowards(customer.transform.position, targetPos, step);

        //if (Vector3.Distance(customer.transform.position, targetPos) < 0.1f)
        //{
        //    _currentIndex++;
        //}
    }

    public void Exit(CustomerAI customer)
    {
        // 아무 것도 안 해도 됨
    }
}

public class WaitState : ICustomerState 
{
    public void Enter(CustomerAI customer)
    { 
    
    }

    public void Update(CustomerAI customer)
    {

    }

    public void Exit(CustomerAI customer)
    {

    }
}

public class OrderAndWait : ICustomerState
{
    public void Enter(CustomerAI customer)
    {

    }

    public void Update(CustomerAI customer)
    {

    }

    public void Exit(CustomerAI customer)
    {

    }
}

public class MoveToTable : ICustomerState
{
    public void Enter(CustomerAI customer)
    {

    }

    public void Update(CustomerAI customer)
    {

    }

    public void Exit(CustomerAI customer)
    {

    }
}

public class Eating : ICustomerState
{
    public void Enter(CustomerAI customer)
    {

    }

    public void Update(CustomerAI customer)
    {

    }

    public void Exit(CustomerAI customer)
    {

    }
}

public class GoingHome : ICustomerState
{
    public void Enter(CustomerAI customer)
    {

    }

    public void Update(CustomerAI customer)
    {

    }

    public void Exit(CustomerAI customer)
    {

    }
}