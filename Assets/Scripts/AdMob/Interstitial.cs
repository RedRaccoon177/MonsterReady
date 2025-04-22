using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;

public class Interstitial : MonoBehaviour
{
    // 전면 광고 인스턴스를 담는 변수
    private InterstitialAd _interstitialAd;

    // 플랫폼별 광고 유닛 ID 설정 (Google 테스트용 ID)
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
    private string _adUnitId = "unused";
#endif

    // 게임 실행 시 최초 실행되는 함수
    void Start()
    {
        // Google AdMob SDK 초기화
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log("AdMob SDK 초기화 완료!");

            // 광고 미리 로드
            LoadInterstitialAd();

            // 테스트용: 1초 후 광고 강제 표시
            Invoke(nameof(TestShowAd), 1f);
        });
    }

    // 테스트용 광고 호출 함수 (정상 작동 여부 확인용)
    private void TestShowAd()
    {
        ShowInterstitialAd();
    }

    /// <summary>
    /// 전면 광고 로드 함수
    /// </summary>
    public void LoadInterstitialAd()
    {
        // 기존 광고가 있다면 제거
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        // 광고 요청 객체 생성
        AdRequest adRequest = new AdRequest();

        // 광고 로드 시작
        InterstitialAd.Load(_adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError($"전면 광고 로드 실패: {error}");
                return;
            }

            Debug.Log("전면 광고 로드 완료!");
            _interstitialAd = ad;

            // 광고 이벤트 등록
            RegisterEventHandlers(ad);

            // 광고 닫힌 후 다음 광고 자동 로드 설정
            RegisterReloadHandler(ad);
        });
    }

    /// <summary>
    /// 전면 광고 보여주기
    /// </summary>
    public void ShowInterstitialAd()
    {
        Debug.Log("ShowInterstitialAd() 호출됨");

        // 광고가 null이면 로드가 안 된 상태
        if (_interstitialAd == null)
        {
            Debug.LogWarning("_interstitialAd == null");
            return;
        }

        // 광고가 준비되지 않은 경우
        if (!_interstitialAd.CanShowAd())
        {
            Debug.LogWarning("광고는 있음, 하지만 CanShowAd() == false");
            return;
        }

        // 광고 정상 표시
        Debug.Log("광고 조건 충족, Show() 실행");
        _interstitialAd.Show();
    }

    /// <summary>
    /// 광고 수명 주기 이벤트 등록
    /// </summary>
    private void RegisterEventHandlers(InterstitialAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"광고 수익 발생: {adValue.Value} {adValue.CurrencyCode}");
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
            Debug.Log("전면 광고 열림 (전체화면)");
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("전면 광고 닫힘");
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError($"전면 광고 전체화면 열기 실패: {error}");
        };
    }

    /// <summary>
    /// 광고 닫히거나 실패 시 자동 재로드 설정
    /// </summary>
    private void RegisterReloadHandler(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("광고 닫힘 → 다음 광고 로드 시작");
            LoadInterstitialAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError($"전체화면 열기 실패 → 다음 광고 로드 시작: {error}");
            LoadInterstitialAd();
        };
    }

    /// <summary>
    /// 오브젝트 파괴 시 광고 정리 (메모리 누수 방지)
    /// </summary>
    void OnDestroy()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
    }
}
