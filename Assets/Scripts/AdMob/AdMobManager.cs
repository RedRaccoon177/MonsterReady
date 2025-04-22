using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdMobManager : MonoBehaviour
{
    // ���� ���� �� ������ ���� ��ũ��Ʈ ����
    public Interstitial interstitial;
    public Rewarded rewarded;

    /// <summary>
    /// ���� ���� ��ư ������ �� ȣ��� (���� ���� ǥ�ÿ�)
    /// ����Ƽ ��ư�� OnClick�� �����ؾ� ��
    /// </summary>
    public void OnGameStartButtonClicked()
    {
        Debug.Log("���� ���� ��ư ���� -> ���� ȣ�� �õ�");
        interstitial.ShowInterstitialAd();
    }

    /// <summary>
    /// ������ ���� ��ư ������ �� ȣ���
    /// ����Ƽ ��ư�� OnClick�� �����ؾ� ��
    /// </summary>
    public void OnRewardButtonClicked()
    {
        rewarded.ShowRewardedAd();
    }
}
