﻿using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AllInOneMobile
{
	/// <summary>
	/// Editor's tab responsible for displaying information about ads.
	/// </summary>
	public class AdsTab
	{
		enum BannerSize
		{
			Banner = 0,
			MediumRectangle = 1,
			IABBanner = 2,
			Leaderboard = 3,
			SmartBanner = 4,
			Custom = 99
		}
		public bool adsInstalled = false;

		AllInOneMobileSettings settings;

		bool adIDs;
		bool adBaner;
		bool adIntestitial;
		bool adRewarded;

		BannerSize bannerSize;
		int bannerWidth;
		int bannerHeight;

		bool groupEnabled;
		float defaultScale;



		public AdsTab(AllInOneMobileSettings settings)
		{
			this.settings = settings;
		}

		public void Save()
		{
			switch (bannerSize)
			{
				case BannerSize.Banner:
					settings.bannerSize = GoogleMobileAds.Api.AdSize.Banner;
					break;
				case BannerSize.MediumRectangle:
					settings.bannerSize = GoogleMobileAds.Api.AdSize.MediumRectangle;
					break;
				case BannerSize.IABBanner:
					settings.bannerSize = GoogleMobileAds.Api.AdSize.IABBanner;
					break;
				case BannerSize.Leaderboard:
					settings.bannerSize = GoogleMobileAds.Api.AdSize.Leaderboard;
					break;
				case BannerSize.SmartBanner:
					settings.bannerSize = GoogleMobileAds.Api.AdSize.SmartBanner;
					break;
				case BannerSize.Custom:
					settings.bannerSize = new GoogleMobileAds.Api.AdSize(bannerWidth, bannerHeight);
					break;
				default:
					break;
			}
		}

		public void ShowAds()
		{
			GUILayout.Label("Ads (AdMob)", EditorStyles.boldLabel);
			if (!adsInstalled)    // if can't find plugin - download button
			{
				if (GUILayout.Button("Download plugin"))
				{
					Application.OpenURL("https://github.com/googleads/googleads-mobile-unity/releases/tag/v5.3.0");
				}
				return;
			}
			settings.useAdMob = EditorGUILayout.Toggle(new GUIContent("Use AdMob"), settings.useAdMob);

			if (settings.useAdMob)
			{
				adIDs = EditorGUILayout.Foldout(adIDs, "Application ID", true);
				if (adIDs)
				{
					EditorGUI.indentLevel++;
					settings.androidID = EditorGUILayout.TextField(new GUIContent("Android ID"), settings.androidID);
					settings.iOSID = EditorGUILayout.TextField(new GUIContent("iOS ID"), settings.iOSID);

					if (settings.iOSID == "" && settings.androidID == "")
						EditorGUILayout.HelpBox("AdMob App ID will look similar to this sample ID: ca-app-pub-3940256099942544~3347511713", MessageType.Info);
					else if (!CheckAdsIDFormat(settings.iOSID) || !CheckAdsIDFormat(settings.androidID))
						EditorGUILayout.HelpBox("AdMob App ID will look similar to this sample ID: ca-app-pub-3940256099942544~3347511713", MessageType.Error);

					EditorGUI.indentLevel--;
				}

				adBaner = EditorGUILayout.Foldout(adBaner, "Baner", true);
				if (adBaner)
				{
					EditorGUI.indentLevel++;
					settings.useBaner = EditorGUILayout.Toggle(new GUIContent("Baners"), settings.useBaner);
					if (settings.useBaner)
					{
						BannerSettings();
					}
					EditorGUI.indentLevel--;
				}

				adIntestitial = EditorGUILayout.Foldout(adIntestitial, "Interstitial", true);
				if (adIntestitial)
				{
					EditorGUI.indentLevel++;
					settings.useInterstitial = EditorGUILayout.Toggle(new GUIContent("Interstitials"), settings.useInterstitial);
					if (settings.useInterstitial)
					{
						settings.androidInterstitial = EditorGUILayout.TextField(new GUIContent("Android"), settings.androidInterstitial);
						settings.iOSInterstitial = EditorGUILayout.TextField(new GUIContent("iOS"), settings.iOSInterstitial);

					}
					EditorGUI.indentLevel--;
				}

				adRewarded = EditorGUILayout.Foldout(adRewarded, "Rewarded", true);
				if (adRewarded)
				{
					EditorGUI.indentLevel++;
					settings.useRewarded = EditorGUILayout.Toggle(new GUIContent("Rewarded"), settings.useRewarded);
					if (settings.useRewarded)
					{
						settings.androidRewarded = EditorGUILayout.TextField(new GUIContent("Android"), settings.androidRewarded);
						settings.iOSRewarded = EditorGUILayout.TextField(new GUIContent("iOS"), settings.iOSRewarded);

					}
					EditorGUI.indentLevel--;
				}
			}
		}

		private void BannerSettings()
		{
			bool disabledFields;
			settings.androidBaner = EditorGUILayout.TextField(new GUIContent("Android"), settings.androidBaner);
			settings.iOSBaner = EditorGUILayout.TextField(new GUIContent("iOS"), settings.iOSBaner);

			EditorGUILayout.Separator();

			bannerSize = (BannerSize)EditorGUILayout.EnumPopup("Banner size", bannerSize);
			if (bannerSize != BannerSize.Custom)
			{
				disabledFields = true;
				bannerWidth = GetBannerSize(bannerSize).x;
				bannerHeight = GetBannerSize(bannerSize).y;
			}
			else
				disabledFields = false;
			EditorGUI.BeginDisabledGroup(disabledFields);
			bannerWidth = EditorGUILayout.IntField("Width", bannerWidth);
			bannerHeight = EditorGUILayout.IntField("Height", bannerHeight);
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Separator();

			settings.adPosition = (AdPosition)EditorGUILayout.EnumPopup("Banner position", settings.adPosition);
		}

		Vector2Int GetBannerSize(BannerSize bannerSize)
		{
			switch (bannerSize)
			{
				case BannerSize.Banner:
					return new Vector2Int(320, 50);
					break;
				case BannerSize.MediumRectangle:
					return new Vector2Int(300, 250);
					break;
				case BannerSize.IABBanner:
					return new Vector2Int(468, 60);
					break;
				case BannerSize.Leaderboard:
					return new Vector2Int(728, 90);
					break;
				case BannerSize.SmartBanner:
					return new Vector2Int(0, 0);
					break;
				default:
					return Vector2Int.zero;
					break;
			}
		}

		bool CheckAdsIDFormat(string id)
		{
			if (id == "")
				return true;
			else if (id.Length != 38)
				return false;
			else if (id[2] != '-')
				return false;
			else if (id[6] != '-')
				return false;
			else if (id[10] != '-')
				return false;
			else if (id[27] != '~')
				return false;
			else
				return true;
		}
	}

}