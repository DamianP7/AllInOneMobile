using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AllInOneMobile
{
	/// <summary>
	/// Editor's tab responsible for displaying information about achievements.
	/// </summary>
	public class AchievementsTab
	{
		public bool achievementsInstalled = false;

		AllInOneMobileSettings settings;

		bool adIDs;

		bool groupEnabled;
		float defaultScale;

		public AchievementsTab(AllInOneMobileSettings settings)
		{
			this.settings = settings;
		}

		public void Save()
		{

		}

		public void ShowTab()
		{
			GUILayout.Label("Achievements", EditorStyles.boldLabel);
			if (!achievementsInstalled)    // if can't find plugin - download button
			{
				if (GUILayout.Button("Download plugin"))
				{
					Application.OpenURL("https://github.com/playgameservices/play-games-plugin-for-unity");
				}
				return;
			}
			settings.useAchievements = EditorGUILayout.Toggle(new GUIContent("Use Achievements"), settings.useAchievements);

			if (settings.useAchievements)
			{
				adIDs = EditorGUILayout.Foldout(adIDs, "Application ID", true);
				if (adIDs)
				{
					EditorGUI.indentLevel++;
					settings.androidID = EditorGUILayout.TextField(new GUIContent("Android ID"), settings.androidID);

					if (settings.androidID == "")
						EditorGUILayout.HelpBox("AdMob App ID will look similar to this sample ID: ca-app-pub-3940256099942544~3347511713", MessageType.Info);
					else if (!CheckAdsIDFormat(settings.androidID))
						EditorGUILayout.HelpBox("AdMob App ID will look similar to this sample ID: ca-app-pub-3940256099942544~3347511713", MessageType.Error);

					EditorGUI.indentLevel--;
				}

				//adBaner = EditorGUILayout.Foldout(adBaner, "Baner", true);
				//if (adBaner)
				//{
				//	EditorGUI.indentLevel++;
				//	settings.useBaner = EditorGUILayout.Toggle(new GUIContent("Baners"), settings.useBaner);
				//	if (settings.useBaner)
				//	{
				//		BannerSettings();
				//	}
				//	EditorGUI.indentLevel--;
				//}
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