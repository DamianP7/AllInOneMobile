using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllInOneMobile
{
	public struct Reward
	{
		public double amount;
		public string type;
	}

	public class Ads : Singleton<Ads>
	{
		// TODO: temp
		public AllInOneMobileSettings AllInOneMobileSettings;
		private static bool initialized;

		private void Start()
		{
			Initialize();
		}

		public void Initialize()
		{
			if (initialized)
				return;

			MobileAds.Initialize(initStatus => { });
			InitializeBanner();
			InitializeInterstitial();
			InitializeRewarded();
			initialized = true;
		}
		#region Banner
		/// <summary>
		/// Called when a banner is ready to show.
		/// </summary>
		public Action OnBannerLoaded;
		/// <summary>
		/// Called when can't load banner.
		/// </summary>
		public Action OnBannerFailedToLoad;
		/// <summary>
		/// Called when a banner is opening - pause game or load async scene here.
		/// </summary>
		public Action OnBannerOpened;
		/// <summary>
		/// Called when a banner is closing - unpause game or open scene.
		/// </summary>
		public Action OnBannerClosed;
		/// <summary>
		/// Called when player click a banner and leave the application.
		/// </summary>
		public Action OnBannerLeavingApplication;

		private BannerView bannerView;
		private bool requestedBanner = false;

		private void InitializeBanner()
		{
			this.RequestBanner();
		}

		private void RequestBanner()
		{
#if UNITY_ANDROID
            string adUnitId = AllInOneMobileSettings.AndroidBaner;
#elif UNITY_IPHONE
            string adUnitId = AllInOneMobileSettings.Instance.IOSBaner;
#else
			string adUnitId = "unexpected_platform";
#endif

			// Create a banner.
			this.bannerView = new BannerView(adUnitId, AllInOneMobileSettings.bannerSize, AllInOneMobileSettings.adPosition);

			// Called when an ad request has successfully loaded.
			this.bannerView.OnAdLoaded += this.HandleOnBannerLoaded;
			// Called when an ad request failed to load.
			this.bannerView.OnAdFailedToLoad += this.HandleOnBannerFailedToLoad;
			// Called when an ad is clicked.
			this.bannerView.OnAdOpening += this.HandleOnBannerOpened;
			// Called when the user returned from the app after an ad click.
			this.bannerView.OnAdClosed += this.HandleOnBannerClosed;
			// Called when the ad click caused the user to leave the application.
			this.bannerView.OnAdLeavingApplication += this.HandleOnBannerLeavingApplication;


			// Create an empty ad request.
			AdRequest request = new AdRequest.Builder().Build();
			requestedBanner = true;

			// Load the banner with the request.
			this.bannerView.LoadAd(request);
		}

		/// <summary>
		/// Show banner.
		/// </summary>
		public void ShowBanner()
		{
			if (!requestedBanner)
				RequestBanner();
			this.bannerView.Show();
		}

		// TODO: delete it
		public void RequestBanner(AdPosition adPos)
		{
#if UNITY_ANDROID
			string adUnitId = AllInOneMobileSettings.AndroidBaner;
#elif UNITY_IPHONE
            string adUnitId = AllInOneMobileSettings.Instance.IOSBaner;
#else
			string adUnitId = "unexpected_platform";
#endif
			if (bannerView != null)
				DestroyBanner();

			// Create a banner.
			this.bannerView = new BannerView(adUnitId, AllInOneMobileSettings.bannerSize, adPos);

			// Called when an ad request has successfully loaded.
			this.bannerView.OnAdLoaded += this.HandleOnBannerLoaded;
			// Called when an ad request failed to load.
			this.bannerView.OnAdFailedToLoad += this.HandleOnBannerFailedToLoad;
			// Called when an ad is clicked.
			this.bannerView.OnAdOpening += this.HandleOnBannerOpened;
			// Called when the user returned from the app after an ad click.
			this.bannerView.OnAdClosed += this.HandleOnBannerClosed;
			// Called when the ad click caused the user to leave the application.
			this.bannerView.OnAdLeavingApplication += this.HandleOnBannerLeavingApplication;


			// Create an empty ad request.
			AdRequest request = new AdRequest.Builder().Build();
			requestedBanner = true;

			// Load the banner with the request.
			this.bannerView.LoadAd(request);
		}

		/// <summary>
		/// Hide opened banner.
		/// </summary>
		public void HideBanner()
		{
			this.bannerView.Hide();
		}

		/// <summary>
		/// When finished with banner - destroy it.
		/// </summary>
		public void DestroyBanner()
		{
			this.bannerView.Destroy();
		}

		private void HandleOnBannerLoaded(object sender, EventArgs args)
		{
			OnBannerLoaded?.Invoke();
			MonoBehaviour.print("HandleAdLoaded event received");
		}

		private void HandleOnBannerFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
			OnBannerFailedToLoad?.Invoke();
			MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "	+ args.Message);
		}

		private void HandleOnBannerOpened(object sender, EventArgs args)
		{
			OnBannerOpened?.Invoke();
			MonoBehaviour.print("HandleAdOpened event received");
		}

		private void HandleOnBannerClosed(object sender, EventArgs args)
		{
			OnBannerClosed?.Invoke();
			MonoBehaviour.print("HandleAdClosed event received");
		}

		private void HandleOnBannerLeavingApplication(object sender, EventArgs args)
		{
			OnBannerLeavingApplication?.Invoke();
			MonoBehaviour.print("HandleAdLeavingApplication event received");
		}
		#endregion

		#region Interstitial
		/// <summary>
		/// Called when an interstitial ad is ready to show.
		/// </summary>
		public Action OnInterstitialLoaded;
		/// <summary>
		/// Called when can't load interstitial ad.
		/// </summary>
		public Action OnInterstitialFailedToLoad;
		/// <summary>
		/// Called when an interstitial ad is opening - pause game or load async scene here.
		/// </summary>
		public Action OnInterstitialOpened;
		/// <summary>
		/// Called when an interstitial ad is closing - unpause game or open scene.
		/// </summary>
		public Action OnInterstitialClosed;
		/// <summary>
		/// Called when player click an interstitial ad and leave the application.
		/// </summary>
		public Action OnInterstitialLeavingApplication;

		private InterstitialAd interstitial;

		private void InitializeInterstitial()
		{
			this.RequestInterstitial();
		}

		// TODO: think when need an ad request 
		// TODO: reuse interstitial object on android?  https://developers.google.com/admob/unity/interstitial
		private void RequestInterstitial()
		{
#if UNITY_ANDROID
            string adUnitId = AllInOneMobileSettings.AndroidInterstitial;
#elif UNITY_IPHONE
            string adUnitId = AllInOneMobileSettings.Instance.IOSInterstitial;
#else
			string adUnitId = "unexpected_platform";
#endif

			// Initialize an InterstitialAd.
			this.interstitial = new InterstitialAd(adUnitId);

			// Called when an ad request has successfully loaded.
			this.interstitial.OnAdLoaded += HandleOnAdLoaded;
			// Called when an ad request failed to load.
			this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
			// Called when an ad is shown.
			this.interstitial.OnAdOpening += HandleOnAdOpened;
			// Called when the ad is closed.
			this.interstitial.OnAdClosed += HandleOnAdClosed;
			// Called when the ad click caused the user to leave the application.
			this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

			// Create an empty ad request.
			AdRequest request = new AdRequest.Builder().Build();
			// Load the interstitial with the request.
			this.interstitial.LoadAd(request);

		}

		private void HandleOnAdLoaded(object sender, EventArgs args)
		{
			OnInterstitialLoaded?.Invoke();
			MonoBehaviour.print("HandleAdLoaded event received");
		}

		private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
			OnInterstitialFailedToLoad?.Invoke();
			MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
		}

		private void HandleOnAdOpened(object sender, EventArgs args)
		{
			OnInterstitialOpened?.Invoke();
			MonoBehaviour.print("HandleAdOpened event received");
		}

		private void HandleOnAdClosed(object sender, EventArgs args)
		{
			OnInterstitialClosed?.Invoke();
			MonoBehaviour.print("HandleAdClosed event received");
			interstitial.Destroy();	// TODO: check it
		}

		private void HandleOnAdLeavingApplication(object sender, EventArgs args)
		{
			OnInterstitialLeavingApplication?.Invoke();
			MonoBehaviour.print("HandleAdLeavingApplication event received");
		}

		// TODO: TryShowIntestitial()
		public void ShowInterstitial()
		{
			if (this.interstitial.IsLoaded())
			{
				this.interstitial.Show();
			}
		}

		#endregion

		#region Rewarded
		/// <summary>
		/// Called when a rewarded ad is ready to show.
		/// </summary>
		public Action OnRewardedLoaded;
		/// <summary>
		/// Called when can't load a rewarded ad.
		/// </summary>
		public Action OnRewardedFailedToLoad;
		/// <summary>
		/// Called when a rewarded ad is opening - pause game or load async scene here.
		/// </summary>
		public Action OnRewardedOpened;
		/// <summary>
		/// Called when a rewarded ad starts to play.
		/// </summary>
		public Action OnRewardedStarted;
		/// <summary>
		/// Called when a rewarded ad is closing - unpause game or open scene.
		/// </summary>
		public Action OnRewardedClosed;
		/// <summary>
		/// Called when player watched ad - reward player here.
		/// </summary>
		public Action OnRewardedReward;
		/// <summary>
		/// Called when player click a rewarded ad and leave the application.
		/// </summary>
		public Action OnRewardedLeavingApplication;

		private Reward reward;
		private RewardBasedVideoAd rewardBasedVideo;

		private void InitializeRewarded()
		{
			// Get singleton reward based video ad reference.
			this.rewardBasedVideo = RewardBasedVideoAd.Instance;

			// Called when an ad request has successfully loaded.
			rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
			// Called when an ad request failed to load.
			rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
			// Called when an ad is shown.
			rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
			// Called when the ad starts to play.
			rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
			// Called when the user should be rewarded for watching a video.
			rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
			// Called when the ad is closed.
			rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
			// Called when the ad click caused the user to leave the application.
			rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

			this.RequestRewardBasedVideo();
		}

		private void RequestRewardBasedVideo()
		{
#if UNITY_ANDROID
            string adUnitId = AllInOneMobileSettings.androidRewarded;
#elif UNITY_IPHONE
            string adUnitId = AllInOneMobileSettings.Instance.IOSRewarded;
#else
			string adUnitId = "unexpected_platform";
#endif

			// Create an empty ad request.
			AdRequest request = new AdRequest.Builder().Build();
			// Load the rewarded video ad with the request.
			this.rewardBasedVideo.LoadAd(request, adUnitId);
		}

		private void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
		{
			OnRewardedLoaded?.Invoke();
			MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
		}

		private void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
			OnRewardedFailedToLoad?.Invoke();
			MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
		}

		private void HandleRewardBasedVideoOpened(object sender, EventArgs args)
		{
			OnRewardedOpened?.Invoke();
			MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
		}

		private void HandleRewardBasedVideoStarted(object sender, EventArgs args)
		{
			OnRewardedStarted?.Invoke();
			MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
		}

		private void HandleRewardBasedVideoClosed(object sender, EventArgs args)
		{
			OnRewardedClosed?.Invoke();

			this.RequestRewardBasedVideo();
			MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
		}

		private void HandleRewardBasedVideoRewarded(object sender, GoogleMobileAds.Api.Reward args)
		{
			reward.amount = args.Amount;
			reward.type = args.Type;

			OnRewardedReward?.Invoke();

			string type = args.Type;
			double amount = args.Amount;
			MonoBehaviour.print("HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
		}

		private void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
		{
			OnRewardedLeavingApplication?.Invoke();
			MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
		}

		// TODO: TryShowRewarded()
		public void ShowRewarded()
		{
			if (this.rewardBasedVideo.IsLoaded())
			{
				this.rewardBasedVideo.Show();
			}
		}

		/// <summary>
		/// Returns last reward.
		/// </summary>
		/// <returns></returns>
		public Reward GetReward()
		{
			return reward;
		}

		/// <summary>
		/// Trying get last reward. If player hasn't watched ad yet, return the new structure.
		/// </summary>
		/// <param name="reward"></param>
		/// <returns></returns>
		public bool TryGetReward(out Reward reward)
		{
			if (this.reward.type != null)
			{
				reward = this.reward;
				return true;
			}
			else
			{
				reward = new Reward();
				return false;
			}
		}
	#endregion
	}
}

// https://developers.google.com/admob/unity/interstitial



	// TODO: Create ad manager on scene (sub-menu)