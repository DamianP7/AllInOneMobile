using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AllInOneMobile.Editor
{
	/// <summary>
	/// Editor's tab responsible for displaying information about achievements.
	/// </summary>
	public class GameServicesTab
	{
		public bool achievementsInstalled = false;

		AllInOneMobileSettings settings;

		bool adIDs;

		bool groupEnabled;
		float defaultScale;
		Dictionary<string, string> achievements;
		Dictionary<string, string> leaderboards;

		public GameServicesTab(AllInOneMobileSettings settings)
		{
			this.settings = settings;
			FindIds();
		}

		public void ShowTab()
		{
			settings.useAchievements =
				EditorGUILayout.Toggle(new GUIContent("Enable Game Services"), settings.useAchievements);

			if (!settings.useAchievements) return;

			if (!achievementsInstalled) // if can't find plugin - download button
			{
				if (GUILayout.Button("Download Google Play Games plugin",GUILayout.Height(30)))
				{
					Application.OpenURL("https://github.com/playgameservices/play-games-plugin-for-unity");
				}

				return;
			}
			
			ShowIds();
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

		void FindIds()
		{
			string filePathAndName = "Assets/GPGSIds.cs";
			string line;
			StreamReader streamReader = new StreamReader(filePathAndName);

			achievements = new Dictionary<string, string>();
			leaderboards = new Dictionary<string, string>();

			while (!streamReader.EndOfStream)
			{
				string type, name, id;
				line = streamReader.ReadLine();
				if (line.Length == 0 || !line.Contains("string")) // check
					continue;
				line = line.Remove(0, 20 + line.IndexOf("public const string")); // remove type
				type = line.Substring(0,11); // get id type
				line = line.Remove(0, 12); // remove type
				name = line.Substring(0, line.IndexOf('=') - 1); // get name
				line = line.Remove(0, name.Length + 4); // remove name
				id = line.Substring(0, line.IndexOf('\"')); // get id

				Debug.Log(type);
				
				if (type == "leaderboard")
					leaderboards.Add(name, id);
				else
					achievements.Add(name, id);
			}
		}

		void ShowIds()
		{
			if (achievements.Count == 0 && leaderboards.Count == 0)
			{
				EditorGUILayout.HelpBox(
					"No generated items could be found.\n\n" +
					"Manually configure Google Play Games in:\n" +
					"Window/Google Play Games/Setup/Android Setup...", MessageType.Error);
				
				if (GUILayout.Button("More info..."))
				{
					Application.OpenURL("https://github.com/playgameservices/play-games-plugin-for-unity");
				}
			}
			
			if (achievements.Count > 0)
			{
				EditorGUILayout.LabelField("Achievements");
				EditorGUI.indentLevel++;
				foreach (KeyValuePair<string, string> achievement in achievements)
				{
					EditorGUILayout.BeginHorizontal();
					GUILayout.Label(achievement.Key, EditorStyles.boldLabel, GUILayout.Width(200));
					GUILayout.Label(achievement.Value, GUILayout.Width(200));
					
					EditorGUILayout.EndHorizontal();
				}

				EditorGUI.indentLevel--;
			}
			else if (leaderboards.Count > 0)
			{
				EditorGUILayout.HelpBox(
					$"Found {achievements.Count} achievements.", MessageType.Warning);
			}

			EditorGUILayout.Space(4);
			
			if (leaderboards.Count > 0)
			{
				EditorGUILayout.LabelField("Leaderboards");
				EditorGUI.indentLevel++;
				foreach (KeyValuePair<string, string> leaderboard in leaderboards)
				{
					EditorGUILayout.BeginHorizontal();
					GUILayout.Label(leaderboard.Key, EditorStyles.boldLabel, GUILayout.Width(200));
					GUILayout.Label(leaderboard.Value, GUILayout.Width(200));
					
					EditorGUILayout.EndHorizontal();
				}
				EditorGUI.indentLevel--;
			}
			else if (achievements.Count > 0)
			{
				EditorGUILayout.HelpBox(
					$"Found {leaderboards.Count} leaderboards.", MessageType.Warning);
			}
		}
	}
}