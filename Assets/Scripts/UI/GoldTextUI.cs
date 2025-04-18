using TMPro;
using UnityEngine;

public class GoldTextUI : MonoBehaviour
{
    public TMP_Text _goldText;

    void OnEnable()
    {
        PlayerController.OnGoldChanged += UpdateGoldText;
    }
    void Start()
    {
        UpdateGoldText(PlayerController._instance._Gold); // √ ±‚»≠
    }

    void UpdateGoldText(int goldAmount)
    {
        _goldText.text = goldAmount.ToString();
    }
}

 