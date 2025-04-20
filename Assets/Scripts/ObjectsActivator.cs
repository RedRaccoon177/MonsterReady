using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsActivator : MonoBehaviour
{
    [SerializeField, Header("활성화 하고자 하는 오브젝트")]
    GameObject _ActivateObj;

    [SerializeField, Header("비활성화 하고자 하는 오브젝트")]
    GameObject _DeactivateObj;

    [SerializeField, Header("지불해야 할 총 골드")]
    int _maxPayGold;

    [SerializeField] int _currentPayGold;
    bool _isActivated = false;

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
        _currentPayGold = _maxPayGold;
    }
    
    private void OnJoystickReleased()
    {
        if (_isActivated) return;
        if (!_playerInTrigger) return;

        PlayerController player = PlayerController._instance;
        if (player == null) return;

        // 골드 지불
        if (_currentPayGold > 0)
        {
            _currentPayGold = player.MinusGold(_currentPayGold);

            if (_currentPayGold <= 0)
            {
                Activate();
                _isActivated = true;
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

    void Activate()
    {
        _ActivateObj.SetActive(true);
        _DeactivateObj.SetActive(false);
    }
}