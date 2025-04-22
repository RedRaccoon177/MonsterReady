using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine;

public class Banner : MonoBehaviour
{
    // 배너 광고 인스턴스를 담는 변수
    private BannerView _bannerView;

    // 플랫폼별 테스트 광고 ID
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
    private string _adUnitId = "unused";
#endif

    // 게임 시작 시 호출됨
    void Start()
    {
        // 광고 SDK 초기화
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // 초기화 완료 후 배너 광고 생성 및 로드
            CreateBannerView();
            LoadAd();
            ListenToAdEvents();
        });
    }

    /// <summary>
    /// 배너 광고 뷰를 생성하는 함수 (화면 상단에 고정됨)
    /// </summary>
    public void CreateBannerView()
    {
        // 기존 배너가 있으면 제거
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }

        // 새 배너 뷰 생성 (크기: 320x50, 위치: 상단)
        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Top);
    }

    /// <summary>
    /// 광고 요청을 보내고 배너 광고를 로드하는 함수
    /// </summary>
    public void LoadAd()
    {
        AdRequest request = new AdRequest();
        _bannerView.LoadAd(request);
    }

    /// <summary>
    /// 배너 광고의 수명 주기 이벤트 등록
    /// </summary>
    private void ListenToAdEvents()
    {
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("배너 광고 로드 완료!");
        };

        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("배너 광고 로드 실패: " + error);
        };

        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("배너 광고 클릭됨!");
        };

        // 필요 시 다른 이벤트도 여기서 추가 등록 가능
    }

    /// <summary>
    /// 오브젝트 제거 시 배너 광고 제거 (메모리 누수 방지)
    /// </summary>
    void OnDestroy()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }
    }
}
