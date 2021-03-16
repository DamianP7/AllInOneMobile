#if ADS
using GoogleMobileAds.Api;
#endif
using System;
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
		static bool initialized;
		AllInOneMobileSettings allInOneMobileSettings;
#if ADS
		public override void Awake()
		{
			allInOneMobileSettings = AllInOneMobileSettings.Instance;
		}

		void Start()
		{
			Initialize();
		}

		public void Initialize()
		{
			if (!AllInOneMobileSettings.Instance.useAdMob)
			{
				Debug.LogError("Ads are disabled.");
				return;
			}
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

		BannerView bannerView;
		bool requestedBanner = false;

		void InitializeBanner()
		{
			RequestBanner();
		}

		public void RequestBanner()
		{
			if (!AllInOneMobileSettings.Instance.useAdMob)
			{
				Debug.LogError("Ads are disabled.");
				return;
			}
#if UNITY_ANDROID
			string adUnitId = allInOneMobileSettings.AndroidBaner;
#else
			string adUnitId = "unexpected_platform";
#endif

			// Create a banner.
			bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
			//bannerView = new BannerView(adUnitId, allInOneMobileSettings.bannerSize, allInOneMobileSettings.adPosition);

			// Called when an ad request has successfully loaded.
			bannerView.OnAdLoaded += this.HandleOnBannerLoaded;
			// Called when an ad request failed to load.
			bannerView.OnAdFailedToLoad += this.HandleOnBannerFailedToLoad;
			// Called when an ad is clicked.
			bannerView.OnAdOpening += this.HandleOnBannerOpened;
			// Called when the user returned from the app after an ad click.
			bannerView.OnAdClosed += this.HandleOnBannerClosed;
			// Called when the ad click caused the user to leave the application.
			bannerView.OnAdLeavingApplication += this.HandleOnBannerLeavingApplication;


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
			if (!AllInOneMobileSettings.Instance.useAdMob)
			{
				Debug.LogError("Ads are disabled.");
				return;
			}
			if (!requestedBanner)
				RequestBanner();
			bannerView.Show();
		}

		/// <summary>
		/// Hide opened banner.
		/// </summary>
		public void HideBanner()
		{
			bannerView.Hide();
		}

		/// <summary>
		/// When finished with banner - destroy it.
		/// </summary>
		public void DestroyBanner()
		{
			this.bannerView.Destroy();
		}

		void HandleOnBannerLoaded(object sender, EventArgs args)
		{
			Debug.Log("HandleAdLoaded event received");
			OnBannerLoaded?.Invoke();
		}

		void HandleOnBannerFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
			Debug.Log("HandleFailedToReceiveAd event received with message: " + args.Message);
			OnBannerFailedToLoad?.Invoke();
		}

		void HandleOnBannerOpened(object sender, EventArgs args)
		{
			Debug.Log("HandleAdOpened event received");
			OnBannerOpened?.Invoke();
		}

		void HandleOnBannerClosed(object sender, EventArgs args)
		{
			Debug.Log("HandleAdClosed event received");
			OnBannerClosed?.Invoke();
		}

		void HandleOnBannerLeavingApplication(object sender, EventArgs args)
		{
			Debug.Log("HandleAdLeavingApplication event received");
			OnBannerLeavingApplication?.Invoke();
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

		InterstitialAd interstitial;

		void InitializeInterstitial()
		{
			this.RequestInterstitial();
		}

		void RequestInterstitial()
		{
#if UNITY_ANDROID
			string adUnitId = allInOneMobileSettings.AndroidInterstitial;
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
			interstitial.LoadAd(request);
		}

		void HandleOnAdLoaded(object sender, EventArgs args)
		{
			Debug.Log("HandleAdLoaded event received");
			OnInterstitialLoaded?.Invoke();
		}

		void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
			Debug.Log("HandleFailedToReceiveAd event received with message: " + args.Message);
			OnInterstitialFailedToLoad?.Invoke();
		}

		void HandleOnAdOpened(object sender, EventArgs args)
		{
			Debug.Log("HandleAdOpened event received");
			OnInterstitialOpened?.Invoke();
		}

		void HandleOnAdClosed(object sender, EventArgs args)
		{
			Debug.Log("HandleAdClosed event received");
			OnInterstitialClosed?.Invoke();
		}

		void HandleOnAdLeavingApplication(object sender, EventArgs args)
		{
			Debug.Log("HandleAdLeavingApplication event received");
			OnInterstitialLeavingApplication?.Invoke();
		}

		public void ShowInterstitial()
		{
			if (!AllInOneMobileSettings.Instance.useAdMob)
			{
				Debug.LogError("Ads are disabled.");
				return;
			}
			if (interstitial == null)
				RequestInterstitial();
			if (interstitial.IsLoaded())
				interstitial.Show();
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

		Reward reward;
		RewardBasedVideoAd rewardBasedVideo;

		void InitializeRewarded()
		{
			// Get singleton reward based video ad reference.
			rewardBasedVideo = RewardBasedVideoAd.Instance;

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

			RequestRewardBasedVideo();
		}

		void RequestRewardBasedVideo()
		{
#if UNITY_ANDROID
			string adUnitId = allInOneMobileSettings.androidRewarded;
#else
			string adUnitId = "unexpected_platform";
#endif

			// Create an empty ad request.
			AdRequest request = new AdRequest.Builder().Build();
			// Load the rewarded video ad with the request.
			rewardBasedVideo.LoadAd(request, adUnitId);
		}

		void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
		{
			Debug.Log("HandleRewardBasedVideoLoaded event received");
			OnRewardedLoaded?.Invoke();
		}

		void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
			Debug.Log("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
			OnRewardedFailedToLoad?.Invoke();
		}

		void HandleRewardBasedVideoOpened(object sender, EventArgs args)
		{
			Debug.Log("HandleRewardBasedVideoOpened event received");
			OnRewardedOpened?.Invoke();
		}

		void HandleRewardBasedVideoStarted(object sender, EventArgs args)
		{
			Debug.Log("HandleRewardBasedVideoStarted event received");
			OnRewardedStarted?.Invoke();
		}

		void HandleRewardBasedVideoClosed(object sender, EventArgs args)
		{
			Debug.Log("HandleRewardBasedVideoClosed event received");
			OnRewardedClosed?.Invoke();

			RequestRewardBasedVideo();
		}

		void HandleRewardBasedVideoRewarded(object sender, GoogleMobileAds.Api.Reward args)
		{
			reward.amount = args.Amount;
			reward.type = args.Type;

			OnRewardedReward?.Invoke();

			string type = args.Type;
			double amount = args.Amount;
			Debug.Log("HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
		}

		void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
		{
			Debug.Log("HandleRewardBasedVideoLeftApplication event received");
			OnRewardedLeavingApplication?.Invoke();
		}

		public void ShowRewarded()
		{
			if (rewardBasedVideo == null)
				RequestRewardBasedVideo();
			if (rewardBasedVideo.IsLoaded())
				rewardBasedVideo.Show();
		}

		/// <summary>
		/// Returns last reward.
		/// </summary>
		/// <returns></returns>
		public Reward GetReward()
		{
			if (!AllInOneMobileSettings.Instance.useAdMob)
			{
				Debug.LogError("Ads are disabled.");
				return new Reward();
			}
			return reward;
		}

		/// <summary>
		/// Trying get last reward. If player hasn't watched ad yet, return the new structure.
		/// </summary>
		/// <param name="reward"></param>
		/// <returns></returns>
		public bool TryGetReward(out Reward reward)
		{
			if (!AllInOneMobileSettings.Instance.useAdMob)
			{
				Debug.LogError("Ads are disabled.");
				reward = new Reward();
				return false;
			}
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

#endif
	}
}