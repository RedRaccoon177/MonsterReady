using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;

public class Interstitial : MonoBehaviour
{
    // ���� ���� �ν��Ͻ��� ��� ����
    private InterstitialAd _interstitialAd;

    // �÷����� ���� ���� ID ���� (Google �׽�Ʈ�� ID)
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
    private string _adUnitId = "unused";
#endif

    // ���� ���� �� ���� ����Ǵ� �Լ�
    void Start()
    {
        // Google AdMob SDK �ʱ�ȭ
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log("AdMob SDK �ʱ�ȭ �Ϸ�!");

            // ���� �̸� �ε�
            LoadInterstitialAd();

            // �׽�Ʈ��: 1�� �� ���� ���� ǥ��
            Invoke(nameof(TestShowAd), 1f);
        });
    }

    // �׽�Ʈ�� ���� ȣ�� �Լ� (���� �۵� ���� Ȯ�ο�)
    private void TestShowAd()
    {
        ShowInterstitialAd();
    }

    /// <summary>
    /// ���� ���� �ε� �Լ�
    /// </summary>
    public void LoadInterstitialAd()
    {
        // ���� ���� �ִٸ� ����
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        // ���� ��û ��ü ����
        AdRequest adRequest = new AdRequest();

        // ���� �ε� ����
        InterstitialAd.Load(_adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError($"���� ���� �ε� ����: {error}");
                return;
            }

            Debug.Log("���� ���� �ε� �Ϸ�!");
            _interstitialAd = ad;

            // ���� �̺�Ʈ ���
            RegisterEventHandlers(ad);

            // ���� ���� �� ���� ���� �ڵ� �ε� ����
            RegisterReloadHandler(ad);
        });
    }

    /// <summary>
    /// ���� ���� �����ֱ�
    /// </summary>
    public void ShowInterstitialAd()
    {
        Debug.Log("ShowInterstitialAd() ȣ���");

        // ���� null�̸� �ε尡 �� �� ����
        if (_interstitialAd == null)
        {
            Debug.LogWarning("_interstitialAd == null");
            return;
        }

        // ���� �غ���� ���� ���
        if (!_interstitialAd.CanShowAd())
        {
            Debug.LogWarning("����� ����, ������ CanShowAd() == false");
            return;
        }

        // ���� ���� ǥ��
        Debug.Log("���� ���� ����, Show() ����");
        _interstitialAd.Show();
    }

    /// <summary>
    /// ���� ���� �ֱ� �̺�Ʈ ���
    /// </summary>
    private void RegisterEventHandlers(InterstitialAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"���� ���� �߻�: {adValue.Value} {adValue.CurrencyCode}");
        };

        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("���� ���� ��ϵ�");
        };

        ad.OnAdClicked += () =>
        {
            Debug.Log("���� Ŭ����");
        };

        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("���� ���� ���� (��üȭ��)");
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("���� ���� ����");
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError($"���� ���� ��üȭ�� ���� ����: {error}");
        };
    }

    /// <summary>
    /// ���� �����ų� ���� �� �ڵ� ��ε� ����
    /// </summary>
    private void RegisterReloadHandler(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("���� ���� �� ���� ���� �ε� ����");
            LoadInterstitialAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError($"��üȭ�� ���� ���� �� ���� ���� �ε� ����: {error}");
            LoadInterstitialAd();
        };
    }

    /// <summary>
    /// ������Ʈ �ı� �� ���� ���� (�޸� ���� ����)
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
