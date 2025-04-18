using TMPro;
using UnityEngine;

public class GoldTextUI : MonoBehaviour
{
    public TMP_Text _goldText;

    //���� ���� �����ؾ� �� �ʿ伺�� �������� ���� �߰�
    void OnEnable()
    {
        PlayerController.OnGoldChanged += UpdateGoldText;
    }

    void Start()
    {
        UpdateGoldText(PlayerController._instance._Gold); // �ʱ�ȭ
    }

    void UpdateGoldText(int goldAmount)
    {
        _goldText.text = goldAmount.ToString();
    }
}

 