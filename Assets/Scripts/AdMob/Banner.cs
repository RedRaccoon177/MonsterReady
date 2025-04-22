using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine;

public class Banner : MonoBehaviour
{
    // ��� ���� �ν��Ͻ��� ��� ����
    private BannerView _bannerView;

    // �÷����� �׽�Ʈ ���� ID
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
    private string _adUnitId = "unused";
#endif

    // ���� ���� �� ȣ���
    void Start()
    {
        // ���� SDK �ʱ�ȭ
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // �ʱ�ȭ �Ϸ� �� ��� ���� ���� �� �ε�
            CreateBannerView();
            LoadAd();
            ListenToAdEvents();
        });
    }

    /// <summary>
    /// ��� ���� �並 �����ϴ� �Լ� (ȭ�� ��ܿ� ������)
    /// </summary>
    public void CreateBannerView()
    {
        // ���� ��ʰ� ������ ����
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }

        // �� ��� �� ���� (ũ��: 320x50, ��ġ: ���)
        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Top);
    }

    /// <summary>
    /// ���� ��û�� ������ ��� ���� �ε��ϴ� �Լ�
    /// </summary>
    public void LoadAd()
    {
        AdRequest request = new AdRequest();
        _bannerView.LoadAd(request);
    }

    /// <summary>
    /// ��� ������ ���� �ֱ� �̺�Ʈ ���
    /// </summary>
    private void ListenToAdEvents()
    {
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("��� ���� �ε� �Ϸ�!");
        };

        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("��� ���� �ε� ����: " + error);
        };

        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("��� ���� Ŭ����!");
        };

        // �ʿ� �� �ٸ� �̺�Ʈ�� ���⼭ �߰� ��� ����
    }

    /// <summary>
    /// ������Ʈ ���� �� ��� ���� ���� (�޸� ���� ����)
    /// </summary>
    void OnDestroy()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }
    }
}
