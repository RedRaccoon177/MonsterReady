using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsActivator : MonoBehaviour
{
    [SerializeField, Header("Ȱ��ȭ �ϰ��� �ϴ� ������Ʈ")]
    GameObject _ActivateObj;
    IActivable _iActive;
    [field: SerializeField] public int _step { get; private set; }

    [SerializeField, Header("��Ȱ��ȭ �ϰ��� �ϴ� ������Ʈ")]
    GameObject[] _DeactivateObj;

    [SerializeField, Header("�����ؾ� �� �� ���")]
    int _maxPayGold;

    [SerializeField] int _currentPayGold;
    bool _isUnlock = false; // �ر� ������Ʈ Unlock ����
    public bool _isActive = false; // �ر� ������Ʈ Ȱ��ȭ ����

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
            _iActive = _ActivateObj.GetComponent<IActivable>();
        }
        _currentPayGold = _maxPayGold;
    }
    
    private void OnJoystickReleased()
    {
        if (_isUnlock) return;
        if (!_playerInTrigger) return;

        PlayerController player = PlayerController._instance;
        if (player == null) return;

        // ��� ����
        if (_currentPayGold > 0)
        {
            _currentPayGold = player.SpendGold(_currentPayGold);

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
    ///  ������ �����Ǽ� �ر� �Ǿ��� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="step"></param>
    void UnlockObject()
    {
        _isActive = false;
        if (_iActive != null)
        {
            _iActive.OnActive();
        }
        for (int i=0; i< _DeactivateObj.Length; i++)
        {
            if (_DeactivateObj != null)
            {
                _DeactivateObj[i].SetActive(false);
            }
        }
        GameManager._instance.OnUnlockObject(_step);
    }

}