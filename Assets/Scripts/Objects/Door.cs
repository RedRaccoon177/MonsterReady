using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject _storeDoor;
    [SerializeField] GameObject _UIPlate;

    int _payGold = 100;

    bool _isOpen;

    void Awake()
    {
        _isOpen = false;
        SetActiveObj(_isOpen);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _isOpen == false)
        {
            other.GetComponent<PlayerController>().SpendGold(_payGold);
            _isOpen = true;
            GameManager._instance.ActiveAllObject();
        }   
    }

    public void SetActiveObj(bool _isOpen)
    {
        _storeDoor.SetActive(_isOpen);
        _UIPlate.SetActive(!_isOpen);
    }
}
