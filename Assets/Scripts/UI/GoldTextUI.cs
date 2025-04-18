using TMPro;
using UnityEngine;

public class GoldTextUI : MonoBehaviour
{
    public TMP_Text _goldText;

    //추후 구독 해제해야 할 필요성이 느껴지면 추후 추가
    void OnEnable()
    {
        PlayerController.OnGoldChanged += UpdateGoldText;
    }

    void Start()
    {
        UpdateGoldText(PlayerController._instance._Gold); // 초기화
    }

    void UpdateGoldText(int goldAmount)
    {
        _goldText.text = goldAmount.ToString();
    }
}

 