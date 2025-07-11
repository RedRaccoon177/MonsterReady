using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteration : MonoBehaviour
{
    [SerializeField] public bool _IsInteration;
    [SerializeField] bool _playerInTrigger;
    [SerializeField] public bool _IsExistNpc;
    [SerializeField] SpriteRenderer _regionImg;


    private void Awake()
    {
        _IsExistNpc =false;
    }

    private void OnEnable()
    {
        PlayerController.OnJoystickReleased += OnInteraction;
        PlayerController.OnJoystickRelePerformed += OffInteraction;
    }
    private void OnDisable()
    {
        PlayerController.OnJoystickReleased -= OnInteraction;
        PlayerController.OnJoystickRelePerformed -= OffInteraction;
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(_IsExistNpc == true) { return; }
        if (other.CompareTag("Player"))
        {
            _playerInTrigger = true;    
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(_IsExistNpc == true) { return; }
        if (other.CompareTag("Player"))
        {
            _playerInTrigger = false;    
        }
    }

    void OffInteraction()
    {
        if (_playerInTrigger == false){ return; }
        _IsInteration = false;
        _regionImg.color = Color.white;
    }
    void OnInteraction()
    {
        if (_playerInTrigger == false) { return; }
        _IsInteration = true;
        _regionImg.color = Color.yellow;
    }

    public void OnNpc()
    {
        PlayerController.OnJoystickReleased -= OnInteraction;
        PlayerController.OnJoystickRelePerformed -= OffInteraction;
        _IsExistNpc = true;
        _IsInteration = true;
        _regionImg.color = Color.yellow;
    }

}
