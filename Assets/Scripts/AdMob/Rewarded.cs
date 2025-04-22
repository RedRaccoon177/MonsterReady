using System;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class Rewarded : MonoBehaviour
{
    // ������ ���� �ν��Ͻ��� ���� ����
    private RewardedAd _rewardedAd;

    // �÷����� ���� �׽�Ʈ ���� ID ����
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917"; // Android �׽�Ʈ ���� ID
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/1712485313"; // iOS �׽�Ʈ ���� ID
#else
    private string _adUnitId = "unused";
#endif

    // ���� ���� �� �� �� ����Ǵ� �ʱ�ȭ �Լ�
    void Start()
    {
        // AdMob SDK �ʱ�ȭ
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log("AdMob �ʱ�ȭ �Ϸ�");

            // ���� �ε�
            LoadRewardedAd();

            // 1�� �� ���� �õ� (���� �غ�ƴ��� Ȯ�ο� �׽�Ʈ)
            Invoke("TestAdShow", 1f);
        });
    }

    // �׽�Ʈ������ ������ ���� ���� �Լ�
    void TestAdShow()
    {
        Debug.Log("�׽�Ʈ ���� ȣ�� �õ�");
        ShowRewardedAd();
    }

    /// <summary>
    /// ������ ���� �ε��ϴ� �Լ�
    /// </summary>
    public void LoadRewardedAd()
    {
        // ������ �ε�� ���� ������ ����
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("������ ���� �ε� �õ�");

        // ���� ��û ��ü ����
        AdRequest request = new AdRequest();

        // ���� �ε�
        RewardedAd.Load(_adUnitId, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                // ���� �ε� ���� �� ���� ���
                Debug.LogError("������ ���� �ε� ����: " + error);
                return;
            }

            Debug.Log("������ ���� �ε� �Ϸ�");

            // �ε�� ���� ����
            _rewardedAd = ad;

            // �̺�Ʈ �ڵ鷯 ���
            RegisterEventHandlers(ad);

            // ���� ������ �� ���� ���� �ڵ� �ε��ϵ��� ����
            RegisterReloadHandler(ad);
        });
    }

    /// <summary>
    /// ������ ���� ����ڿ��� �����ִ� �Լ�
    /// </summary>
    public void ShowRewardedAd()
    {
        // ���� �ε�Ǿ� �ְ�, ������ ǥ���� �� �ִ� �������� Ȯ��
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Debug.Log("������ ���� ǥ��");

            // ���� ǥ�� �� ���� ó��
            _rewardedAd.Show((Reward reward) =>
            {
                // ���� ������ ���� ���� ���޵� (���⼭ ���� ���� ó��)
                Debug.Log("���� ���� - Ÿ��: " + reward.Type + ", ����: " + reward.Amount);

                // TODO: ���⿡ ���� ���� �� ���� ó�� ���� �ֱ� (��: ��� �߰�)
            });
        }
        else
        {
            // ���� ���� �غ� �� ���� ��
            Debug.LogWarning("������ ���� ���� �غ���� �ʾҽ��ϴ�");
        }
    }

    /// <summary>
    /// ���� ���� �ֱ� �� �߻��ϴ� �̺�Ʈ���� ����ϴ� �Լ�
    /// </summary>
    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("���� ���� ���� - " + adValue.Value + " " + adValue.CurrencyCode);
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
            Debug.Log("���� ��üȭ�� ����");
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("���� ����");
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("���� ��üȭ�� ���� ����: " + error);
        };
    }

    /// <summary>
    /// ���� �����ų� ���� �������� ��, ���� ���� �̸� �ε��ϴ� �Լ�
    /// </summary>
    private void RegisterReloadHandler(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("���� ���� - ���� ���� �̸� �ε�");
            LoadRewardedAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("���� ���� ���� - ���� ���� �̸� �ε�: " + error);
            LoadRewardedAd();
        };
    }

    // ������Ʈ �ı� �� �޸� ���� ������ ���� ����
    void OnDestroy()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
    }
}
