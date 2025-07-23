using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Boat : MonoBehaviour
{
    // NPC,고기생성위치
    [SerializeField] Transform meatBoxPos;
    [SerializeField] Transform custormerPos;    
    [SerializeField] Transform endPoint;    
    public float rotationSpeed = 90f; // 초당 회전 속도 (도/초)
    private Rigidbody rb;
    WaitForSeconds orderWaitTime;
    List<GameObject> boxList;

    // 주문 변수
    BoatPool _originPool;
    Counter counter;
    ObjectPooling objectPool;
    List<GameObject> meatBoxList;
    private int orderMeetBoxCount;
    private int maxOrderMeetBoxCount;
    public int currentMeetBoxCount;
    private int maxMeetBoxCount;
    private float stackHeight;
    
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
        counter = (Counter)GameManager._instance._baseObjectDict["counter2"];
        maxOrderMeetBoxCount = 4;
        maxMeetBoxCount = maxOrderMeetBoxCount;
        stackHeight = 1;
        orderWaitTime = new WaitForSeconds(2);
    }

    // 세팅
    public void Init(BoatSpawaner spawner,Transform endPoint,ObjectPooling objectPool)
    {
        this.objectPool = objectPool;
        this.endPoint = endPoint;
        this.boatSpawaner = spawner;
        transform.position = boatSpawaner._pointList[0].position;
        boatSpawaner._isVisited[0] = true;
        StartCoroutine(MoveRoutine());
    }
    IEnumerator MoveBoat(int nextIndex)
    {
        Vector3 start = transform.position;
        Vector3 end = boatSpawaner._pointList[nextIndex].position;
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
    // 이동 (포인트 따라)
    IEnumerator MoveRoutine()
    {
        while (currentPointIndex < boatSpawaner._pointList.Count-1)
        {
            int nextIndex = currentPointIndex + 1;
            if (boatSpawaner._isVisited[nextIndex] == false)
            {
                boatSpawaner._isVisited[currentPointIndex] = false;
                boatSpawaner._isVisited[nextIndex] = true;
                StartCoroutine(MoveBoat(nextIndex));
            }
            else
            {
                yield return null;
            }
        }
        StartCoroutine(Order());
        Debug.Log(gameObject.name);
    }
    void LeavePointArr()
    {
        boatSpawaner._isVisited[currentPointIndex] = false;
        boatSpawaner._boatCount--;
        StartCoroutine(MoveEndPoint());
    }
    IEnumerator MoveEndPoint()
    {
        Vector3 start = transform.position;
        Vector3 end = endPoint.transform.position;
        float duration = 3f;
        float elapsed = 0f;
        Vector3 dir = (end - start).normalized;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(start, end, t);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, t);
            yield return null;
        }
        gameObject.SetActive(false);
    }
    IEnumerator Order()
    {
        orderMeetBoxCount = Random.Range(1, maxOrderMeetBoxCount);
        UiManager._instance.ActiveMeatBoxOrdreUi(orderMeetBoxCount,true);
        // 피자를 모두 받앗을 시
        boxList = new List<GameObject> ();
        while (currentMeetBoxCount < orderMeetBoxCount)
        {
            yield return orderWaitTime;
            int beforeMeat = currentMeetBoxCount;
            int neededMeat = orderMeetBoxCount - beforeMeat;
            if (neededMeat > 0 && counter._objectInteration._IsInteration)
            {
                int receivedMeat = counter.MinusBox(neededMeat);
                if (receivedMeat > 0)
                {
                    AddMeatBox(receivedMeat);                         // 고기 오브젝트 생성
                    UiManager._instance.ActiveMeatBoxOrdreUi(neededMeat - receivedMeat, true);
                }
            }
        }
        UiManager._instance.ActiveMeatBoxOrdreUi(0,false);
        LeavePointArr();
    }
    int MinusMeatBox(int meatBox)
    {
        CurrentMeetBoxCount -= meatBox;
        UpdateMeatBoxDisplay(CurrentMeetBoxCount);
        return CurrentMeetBoxCount;
    }
    void AddMeatBox(int boxCount)
    {
        CurrentMeetBoxCount += boxCount;
        UpdateMeatBoxDisplay(CurrentMeetBoxCount);
    }
    void UpdateMeatBoxDisplay(float boxCount)
    {
        while (boxList.Count < boxCount)
        {
            GameObject box = objectPool.GetBox();
            box.transform.SetParent(meatBoxPos, false);
            box.transform.localPosition = GetStackPosition(boxList.Count);
            box.transform.localRotation = Quaternion.identity;
            box.transform.localScale = new Vector3(1,1,1);
            boxList.Add(box);
        }
        while (boxList.Count > boxCount)
        {
            GameObject lastMeat = boxList[boxList.Count - 1];
            boxList.RemoveAt(boxList.Count - 1);
            objectPool.ReturnToPool(lastMeat);
        }
    }
    Vector3 GetStackPosition(int index)
    {
        return new Vector3(0, index * stackHeight, 0);
    }
    public void SetOriginPool(BoatPool pool)
    {
        _originPool = pool;
    }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
        _originPool.ReturnObject(this);
    }
}
