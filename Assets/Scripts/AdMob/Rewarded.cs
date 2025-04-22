using System;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class Rewarded : MonoBehaviour
{
    // 보상형 광고 인스턴스를 담을 변수
    private RewardedAd _rewardedAd;

    // 플랫폼에 따라 테스트 광고 ID 지정
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917"; // Android 테스트 광고 ID
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/1712485313"; // iOS 테스트 광고 ID
#else
    private string _adUnitId = "unused";
#endif

    // 게임 실행 시 한 번 실행되는 초기화 함수
    void Start()
    {
        // AdMob SDK 초기화
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log("AdMob 초기화 완료");

            // 광고 로드
            LoadRewardedAd();

            // 1초 후 광고 시도 (광고가 준비됐는지 확인용 테스트)
            Invoke("TestAdShow", 1f);
        });
    }

    // 테스트용으로 강제로 광고를 띄우는 함수
    void TestAdShow()
    {
        Debug.Log("테스트 광고 호출 시도");
        ShowRewardedAd();
    }

    /// <summary>
    /// 보상형 광고를 로드하는 함수
    /// </summary>
    public void LoadRewardedAd()
    {
        // 기존에 로드된 광고가 있으면 정리
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("보상형 광고 로드 시도");

        // 광고 요청 객체 생성
        AdRequest request = new AdRequest();

        // 광고 로드
        RewardedAd.Load(_adUnitId, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                // 광고 로드 실패 시 에러 출력
                Debug.LogError("보상형 광고 로드 실패: " + error);
                return;
            }

            Debug.Log("보상형 광고 로드 완료");

            // 로드된 광고 저장
            _rewardedAd = ad;

            // 이벤트 핸들러 등록
            RegisterEventHandlers(ad);

            // 광고 닫혔을 때 다음 광고 자동 로드하도록 설정
            RegisterReloadHandler(ad);
        });
    }

    /// <summary>
    /// 보상형 광고를 사용자에게 보여주는 함수
    /// </summary>
    public void ShowRewardedAd()
    {
        // 광고가 로드되어 있고, 실제로 표시할 수 있는 상태인지 확인
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Debug.Log("보상형 광고 표시");

            // 광고 표시 및 보상 처리
            _rewardedAd.Show((Reward reward) =>
            {
                // 광고 끝까지 보면 보상 지급됨 (여기서 실제 보상 처리)
                Debug.Log("보상 지급 - 타입: " + reward.Type + ", 수량: " + reward.Amount);

                // TODO: 여기에 실제 게임 내 보상 처리 로직 넣기 (예: 골드 추가)
            });
        }
        else
        {
            // 아직 광고 준비가 안 됐을 때
            Debug.LogWarning("보상형 광고가 아직 준비되지 않았습니다");
        }
    }

    /// <summary>
    /// 광고 수명 주기 중 발생하는 이벤트들을 등록하는 함수
    /// </summary>
    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("광고 수익 정보 - " + adValue.Value + " " + adValue.CurrencyCode);
        };

        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("광고 노출 기록됨");
        };

        ad.OnAdClicked += () =>
        {
            Debug.Log("광고 클릭됨");
        };

        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("광고 전체화면 열림");
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("광고 닫힘");
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("광고 전체화면 열기 실패: " + error);
        };
    }

    /// <summary>
    /// 광고가 닫히거나 열기 실패했을 때, 다음 광고를 미리 로드하는 함수
    /// </summary>
    private void RegisterReloadHandler(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("광고 닫힘 - 다음 광고 미리 로드");
            LoadRewardedAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("광고 열기 실패 - 다음 광고 미리 로드: " + error);
            LoadRewardedAd();
        };
    }

    // 오브젝트 파괴 시 메모리 누수 방지를 위한 정리
    void OnDestroy()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
    }
}
