using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AllInOneMobile
{
	/// <summary>
	/// Plugin main window editor
	/// </summary>
	public class MainWindow : EditorWindow
	{
		bool adsInstalled = false;
		const string adsPluginPath = "/Plugins/Android/googlemobileads-unity.aar";

		AllInOneMobileSettings settings;

		// -- Tabs
		AdsTab adsTab;

		[SerializeField] string myString = "Hello World";

		#region Open/Close
		[MenuItem("AllInOne Mobile/Main Settings")]
		public static void ShowWindow()
		{
			EditorWindow.GetWindow(typeof(MainWindow), false, "AllInOne Mobile Settings");
			//DebugWindow window = (DebugWindow)EditorWindow.GetWindow(typeof(DebugWindow), true, "My Empty Window");
		}

		private void OnFocus()
		{
			adsInstalled = System.IO.File.Exists(Application.dataPath + adsPluginPath);
			adsTab.adsInstalled = adsInstalled;

			//Debug.Log("oneb");
		}

		protected void OnEnable()
		{
			settings = AllInOneMobileSettings.Instance;

			adsTab = new AdsTab(settings);


			//defaultScale = Time.timeScale;

			//var data = EditorPrefs.GetString("Debugger", JsonUtility.ToJson(this, false));
			//JsonUtility.FromJsonOverwrite(data, this);
		}

		//protected void OnDisable()
		//{
		//	Time.timeScale = defaultScale;

		//	var data = JsonUtility.ToJson(this, false);
		//	EditorPrefs.SetString("Debugger", data);
		//}
		#endregion

		void OnGUI()
		{
			EditorUtility.SetDirty(this);

			//groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
			//Time.timeScale = EditorGUILayout.Slider("Time Scale", Time.timeScale, 0, 2);
			//EditorGUILayout.EndToggleGroup();

			adsTab.ShowAds();


			if (GUI.changed)
			{
				adsTab.Save();

				//OnSettingsChanged();
				settings.WriteSettingsToFile();
			}
		}

		
	}
}