using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsActivator : MonoBehaviour
{
    [field: SerializeField] public int _step { get; private set; }
    [SerializeField, Header("활성화 하고자 하는 오브젝트")]
    GameObject _ActivateObj;
    BaseObject _activeObjBaseScript;

    [SerializeField, Header("비활성화 하고자 하는 오브젝트")]
    GameObject _DeactivateObj;
    BaseObject _deactivateObjScript;

    [SerializeField, Header("지불해야 할 총 골드")]
    int _maxPayGold;

    [SerializeField] int _currentPayGold;
    bool _isUnlock = false; // 해금 오브젝트 Unlock 여부
    public bool _isActive = false; // 해금 오브젝트 활성화 여부

    bool _playerInTrigger = false;

    private void OnEnable()
    {
        PlayerController.OnJoystickReleased += OnJoystickReleased;
    }

    private void OnDisable()
    {
        PlayerController.OnJoystickReleased -= OnJoystickReleased;
    }

    void Start()
    {
        if (_ActivateObj != null)
        {
            _activeObjBaseScript = _ActivateObj.GetComponent<BaseObject>();
        }
        if (_DeactivateObj != null)
        {
            _deactivateObjScript = _DeactivateObj.GetComponent<BaseObject>();
        }
        _currentPayGold = _maxPayGold;
    }
    
    private void OnJoystickReleased()
    {
        if (_isUnlock) return;
        if (!_playerInTrigger) return;

        PlayerController player = PlayerController._instance;
        if (player == null) return;

        // 골드 지불
        if (_currentPayGold > 0)
        {
            _currentPayGold = player.MinusGold(_currentPayGold);

            if (_currentPayGold <= 0)
            {
                UnlockObject();
                _isUnlock = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _playerInTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _playerInTrigger = false;
    }


    /// <summary>
    ///  조건이 만족되서 해금 되엇을 때 실행되는 함수
    /// </summary>
    /// <param name="step"></param>
    void UnlockObject()
    {
        if (_activeObjBaseScript != null)
        {
            _activeObjBaseScript.OnActive();
        }
        if (_deactivateObjScript != null)
        {
            _deactivateObjScript.DeActive();
        }
        _isActive = false;
        GameManager._instance.OnUnlockObject(_step);
        gameObject.SetActive(false);
    }
}