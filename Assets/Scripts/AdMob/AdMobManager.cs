using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdMobManager : MonoBehaviour
{
    // 전면 광고 및 보상형 광고 스크립트 참조
    public Interstitial interstitial;
    public Rewarded rewarded;

    /// <summary>
    /// 게임 시작 버튼 눌렸을 때 호출됨 (전면 광고 표시용)
    /// 유니티 버튼의 OnClick에 연결해야 함
    /// </summary>
    public void OnGameStartButtonClicked()
    {
        Debug.Log("게임 시작 버튼 눌림 -> 광고 호출 시도");
        interstitial.ShowInterstitialAd();
    }

    /// <summary>
    /// 보상형 광고 버튼 눌렸을 때 호출됨
    /// 유니티 버튼의 OnClick에 연결해야 함
    /// </summary>
    public void OnRewardButtonClicked()
    {
        rewarded.ShowRewardedAd();
    }
}
