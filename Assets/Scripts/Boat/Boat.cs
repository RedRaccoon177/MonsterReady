using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    // NPC,고기생성위치
    [SerializeField] Transform meatBoxPos;
    [SerializeField] Transform custormerPos;    
    [SerializeField] Transform endPoint;    
    public float rotationSpeed = 90f; // 초당 회전 속도 (도/초)
    private Rigidbody rb;
    WaitForSeconds orderWaitTime;

    // 주문 변수
    TestHam counter;
    List<GameObject> meatBoxList;
    private int orderMeetBoxCount;
    private int maxOrderMeetBoxCount;
    private int currentMeetBoxCount;
    private int maxMeetBoxCount;
    
    public int CurrentMeetBoxCount
    {
        get
        {
            return currentMeetBoxCount;
        }
        set
        {
            currentMeetBoxCount = Mathf.Clamp(value, 0, maxMeetBoxCount);
        }
    }

    // 이동 변수
    private int currentPointIndex;
    public BoatSpawaner boatSpawaner;

    private void Start()
    {
        maxOrderMeetBoxCount = 4;
    }

    // 세팅
    public void Init(BoatSpawaner spawner,GameObject customer,Transform endPoint)
    {
        this.endPoint = endPoint;
        Instantiate(customer, custormerPos);
        this.boatSpawaner = spawner;
        transform.position = boatSpawaner.pointList[0].position;
        boatSpawaner.isVisited[0] = true;
        StartCoroutine(MoveRoutine());
    }
    // 이동 (포인트 따라)
    IEnumerator MoveRoutine()
    {
        while (currentPointIndex < boatSpawaner.pointList.Count - 1)
        {
            int nextIndex = currentPointIndex + 1;
            if (!boatSpawaner.isVisited[nextIndex])
            {
                boatSpawaner.isVisited[currentPointIndex] = false;
                boatSpawaner.isVisited[nextIndex] = true;
                Vector3 start = transform.position;
                Vector3 end = boatSpawaner.pointList[nextIndex].position;
                Vector3 dir = (end - start).normalized;
                Quaternion targetRot = Quaternion.LookRotation(dir);
                float duration = 1.5f;
                float elapsed = 0f;
                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / duration);
                    transform.position = Vector3.Lerp(start, end, t);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, t);
                    yield return null;
                }
                transform.position = end;
                currentPointIndex = nextIndex;
            }
            else
            {
                yield return null;
            }
        }
        StartCoroutine(Order());
    }
    void LeavePointArr()
    {
        boatSpawaner.isVisited[currentPointIndex] = false;
        transform.position = endPoint.transform.position;
    }
    IEnumerator Order()
    {
        Random.Range(1, maxOrderMeetBoxCount);
        // 피자를 모두 받앗을 시
        while (currentMeetBoxCount >= orderMeetBoxCount)
        {
            //LeavePointArr();
            yield return orderWaitTime;
            int beforeMeat = currentMeetBoxCount;
            int neededMeat = orderMeetBoxCount - beforeMeat;
            if (neededMeat > 0)
            {
                int receivedMeat = counter.MinusMeatBox(neededMeat);

                if (receivedMeat > 0)
                {
                    AddMeatBox(receivedMeat);                         // 고기 오브젝트 생성
                }
            }
        }
    }
    int MinusMeatBox(int meatBox)
    {
        CurrentMeetBoxCount -= meatBox;
        //UpdateMeatBoxDisplay(CurrentMeetBoxCount);
        return CurrentMeetBoxCount;
    }
    void AddMeatBox(int meatBox)
    {
        CurrentMeetBoxCount += meatBox;
        //UpdateMeatBoxDisplay(CurrentMeetBoxCount);
    }
    //void UpdateMeatBoxDisplay(float meatBox)
    //{
    //
    //    while (_boneList.Count < currentBone)
    //    {
    //        GameObject meat = _meatPool.GetBone();
    //        meat.transform.SetParent(_meatSpawnLocation, false);
    //        meat.transform.localPosition = GetStackPosition(_boneList.Count);
    //        meat.transform.localRotation = Quaternion.identity;
    //        meat.transform.localScale = _bonePrefab.transform.localScale;
    //
    //        _boneList.Add(meat);
    //    }
    //    while (_boneList.Count > currentBone)
    //    {
    //        GameObject lastMeat = _boneList[_boneList.Count - 1];
    //        _boneList.RemoveAt(_boneList.Count - 1);
    //        _meatPool.ReturnToPool(lastMeat);
    //    }
    //
    //}
    //Vector3 GetStackPosition(int index)
    //{
    //    return new Vector3(0, index * _stackHeight, 0);
    //}
}
