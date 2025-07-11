using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsActivator : MonoBehaviour
{
    [field: SerializeField] public int _step { get; private set; }
    [SerializeField, Header("활성화 하고자 하는 오브젝트")]
    GameObject _ActivateObj;
    BaseObject _activeObjBaseScript;
    Coroutine _gageCor;

    [SerializeField, Header("비활성화 하고자 하는 오브젝트")]
    GameObject _DeactivateObj;
    BaseObject _deactivateObjScript;

    [SerializeField, Header("지불해야 할 총 골드")]
    int _maxPayGold;

    [SerializeField] int _currentPayGold;
    bool _isUnlock = false; // 해금 오브젝트 Unlock 여부
    public bool _isActive = false; // 해금 오브젝트 활성화 여부
    bool _playerInTrigger = false;

    [Header("Ui")] 
    [SerializeField] TextMeshProUGUI _payGoldText;
    [SerializeField] Image _payGoldProgress;

    private void OnEnable()
    {
        PlayerController.OnJoystickReleased += OnJoystickReleased;
    }

    private void OnDisable()
    {
        PlayerController.OnJoystickReleased -= OnJoystickReleased;
        if (_gageCor != null)
        {
            StopCoroutine(_gageCor);
        }
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
        _payGoldText.text = (_maxPayGold - _currentPayGold).ToString();
    }
    
    private void OnJoystickReleased()
    {
        if (_isUnlock) return;
        if (!_playerInTrigger)
        {
            if (_gageCor != null)
            {
                StopCoroutine(_gageCor);
            }
            return;
        }
        PlayerController player = PlayerController._instance;
        if (player == null) return;

        // 골드 지불
        if (_currentPayGold >= 0)
        {
            _gageCor = StartCoroutine(BuyGauge());
        }
    }

    IEnumerator BuyGauge()
    {
        int count = 0;
        while (_currentPayGold < _maxPayGold)
        {
            if (PlayerController._instance._playerGold <= 0)
                yield break;

            count++;
            PlayerController._instance.MinusGold(1);
            _currentPayGold += 1;
            _payGoldText.text = (_maxPayGold-_currentPayGold).ToString();
            float from = _payGoldProgress.fillAmount;
            float to = (float)_currentPayGold / _maxPayGold;
            yield return StartCoroutine(AnimateGauge(from, to, 0.1f));
        }
        UnlockObject();
        _isUnlock = true;
    }
    IEnumerator AnimateGauge(float from, float to, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            _payGoldProgress.fillAmount = Mathf.Lerp(from, to, t);
            yield return null;
        }
        _payGoldProgress.fillAmount = to; // 정확하게 도달 보정
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInTrigger = false;
        }
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