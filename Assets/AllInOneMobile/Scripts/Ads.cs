using GoogleMobileAds.Api;
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
		AllInOneMobileSettings AllInOneMobileSettings = AllInOneMobileSettings.Instance;

		void Start()
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

		BannerView bannerView;
		bool requestedBanner = false;

		void InitializeBanner()
		{
			RequestBanner();
		}

		public void RequestBanner()
		{
#if UNITY_ANDROID
            string adUnitId = AllInOneMobileSettings.AndroidBaner;
#else
			string adUnitId = "unexpected_platform";
#endif

			// Create a banner.
			bannerView = new BannerView(adUnitId, AllInOneMobileSettings.bannerSize, AllInOneMobileSettings.adPosition);

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
			if (!requestedBanner)
				RequestBanner();
			bannerView.Show();
		}

		public void RequestBanner(AdPosition adPos = AdPosition.Bottom)
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
			this.bannerView = new BannerView(adUnitId, AdSize.Banner, adPos);
			//this.bannerView = new BannerView(adUnitId, AllInOneMobileSettings.bannerSize, adPos);

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
			OnBannerLoaded?.Invoke();
			Debug.Log("HandleAdLoaded event received");
		}

		void HandleOnBannerFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
			OnBannerFailedToLoad?.Invoke();
			Debug.Log("HandleFailedToReceiveAd event received with message: "	+ args.Message);
		}

		void HandleOnBannerOpened(object sender, EventArgs args)
		{
			OnBannerOpened?.Invoke();
			Debug.Log("HandleAdOpened event received");
		}

		void HandleOnBannerClosed(object sender, EventArgs args)
		{
			OnBannerClosed?.Invoke();
			Debug.Log("HandleAdClosed event received");
		}

		void HandleOnBannerLeavingApplication(object sender, EventArgs args)
		{
			OnBannerLeavingApplication?.Invoke();
			Debug.Log("HandleAdLeavingApplication event received");
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
            string adUnitId = AllInOneMobileSettings.AndroidInterstitial;
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
			OnInterstitialLoaded?.Invoke();
			Debug.Log("HandleAdLoaded event received");
		}

		void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
			OnInterstitialFailedToLoad?.Invoke();
			Debug.Log("HandleFailedToReceiveAd event received with message: " + args.Message);
		}

		void HandleOnAdOpened(object sender, EventArgs args)
		{
			OnInterstitialOpened?.Invoke();
			Debug.Log("HandleAdOpened event received");
		}

		void HandleOnAdClosed(object sender, EventArgs args)
		{
			OnInterstitialClosed?.Invoke();
			Debug.Log("HandleAdClosed event received");
			interstitial.Destroy();
		}

		void HandleOnAdLeavingApplication(object sender, EventArgs args)
		{
			OnInterstitialLeavingApplication?.Invoke();
			Debug.Log("HandleAdLeavingApplication event received");
		}

		public void ShowInterstitial()
		{
			if(interstitial == null)
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
            string adUnitId = AllInOneMobileSettings.androidRewarded;
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
			OnRewardedLoaded?.Invoke();
			Debug.Log("HandleRewardBasedVideoLoaded event received");
		}

		void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
			OnRewardedFailedToLoad?.Invoke();
			Debug.Log("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
		}

		void HandleRewardBasedVideoOpened(object sender, EventArgs args)
		{
			OnRewardedOpened?.Invoke();
			Debug.Log("HandleRewardBasedVideoOpened event received");
		}

		void HandleRewardBasedVideoStarted(object sender, EventArgs args)
		{
			OnRewardedStarted?.Invoke();
			Debug.Log("HandleRewardBasedVideoStarted event received");
		}

		void HandleRewardBasedVideoClosed(object sender, EventArgs args)
		{
			OnRewardedClosed?.Invoke();

			RequestRewardBasedVideo();
			Debug.Log("HandleRewardBasedVideoClosed event received");
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
			OnRewardedLeavingApplication?.Invoke();
			Debug.Log("HandleRewardBasedVideoLeftApplication event received");
		}

		public void ShowRewarded()
		{
			if(rewardBasedVideo == null)
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