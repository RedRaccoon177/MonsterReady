using UnityEngine;

public enum ObjectType
{
    Table,Counter,Grill,Expand
}

public class BaseObject : MonoBehaviour
{
    public string _keyName;
    public bool _isActive;
    public ObjectType _objectType;
    

    public bool isActive() 
    {
        return _isActive;
    }
    public void DeActive()
    {
        _isActive = false;
        gameObject.SetActive(false);
    }

    public void OnActive()
    {
        Vector3 halfExtents = new Vector3(0.6f, 0.6f, 0.6f);
        Collider[] hits = Physics.OverlapBox(gameObject.transform.position, halfExtents,Quaternion.identity,LayerMask.GetMask("Node"));
        foreach (var hit in hits)
        {
            hit.gameObject.GetComponent<Node>()._isWalkale = false;
        }
        _isActive = true;
        gameObject.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Vector3 center = transform.position;
        Vector3 halfExtents = new Vector3(1f, 1f, 1f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, halfExtents * 2);
    }
}
// 레벨을 가지고 있는 테이블 , 카운터 ,그릴에 상속
public interface ILevelable 
{
    string GetKey();
    int GetLevel();
    int SetLevel(int level);
    void LevelUp();
}

// NPC가 목적지로 설정 가능한 오브젝트
public interface INpcDestination
{
    string GetKey();
    void SettingNode(); // 노드 세팅
    bool HasStack();
    int GetStackCount();
    void OnDestination();
    void OffDestination();
    bool IsDestination();
}


