using AllInOneMobile;
using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAds : MonoBehaviour
{
    public Ads ads;

    public Text bannerText;
    public Text interstitialText;
    public Text rewardedText;

    void Start()
    {
        ads.OnBannerClosed += BannerClosed;
        ads.OnBannerFailedToLoad += BannerFailedToLoad;
        ads.OnBannerLoaded += BannerLoaded;
        ads.OnBannerOpened += BannerOpened;

        ads.OnInterstitialClosed += InterstitialClosed;
        ads.OnInterstitialFailedToLoad += InterstitialFailedToLoaded;
        ads.OnInterstitialLoaded += InterstitialLoaded;
        ads.OnInterstitialOpened += InterstitialOpened;

        ads.OnRewardedClosed += RewardedClosed;
        ads.OnRewardedFailedToLoad += RewardedFailedToLoaded;
        ads.OnRewardedLoaded += RewardedLoaded;
        ads.OnRewardedOpened += RewardedOpened;

        bannerText.text = "Banner: Loading";
        interstitialText.text = "Interstitial: Loading";
        rewardedText.text = "Rewarded: Loading";
    }
    
    public void SetBanner()
    {
        ads.RequestBanner();
        ads.ShowBanner();
    }

    public void HideBanner()
    {
        ads.HideBanner();
    }

    public void ShowInterstitial()
    {
        ads.ShowInterstitial();
    }

    public void ShowRewarded()
    {
        ads.ShowRewarded();
    }
    public void BannerLoaded()
    {
        bannerText.text = "Banner: Loaded";
    }

    public void BannerFailedToLoad()
    {
        bannerText.text = "Banner: Failed to load";
    }

    public void BannerOpened()
    {
        bannerText.text = "Banner: Opened";
    }

    public void BannerClosed()
    {
        bannerText.text = "Banner: Closed";
    }

    public void InterstitialLoaded()
    {
        interstitialText.text = "Interstitial: Loaded";
    }

    public void InterstitialFailedToLoaded()
    {
        interstitialText.text = "Interstitial: Failed to load";
    }

    public void InterstitialOpened()
    {
        interstitialText.text = "Interstitial: Opened";
    }

    public void InterstitialClosed()
    {
        interstitialText.text = "Interstitial: Closed";
    }

    public void RewardedLoaded()
    {
        rewardedText.text = "Rewareded: Loaded";
    }

    public void RewardedFailedToLoaded()
    {
        rewardedText.text = "Rewareded: Failed to load";
    }

    public void RewardedOpened()
    {
        rewardedText.text = "Rewareded: Opened";
    }

    public void RewardedClosed()
    {
        rewardedText.text = "Rewareded: Closed";
    }

    public void Services()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ServicesTest");
    }
    
    public void Shop()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ShopTest");
    }
}
