using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Purchasing;

namespace AllInOneMobile
{
	public class InAppStoreTab
	{
		public bool inappsInstalled = false;

		AllInOneMobileSettings settings;

		bool products;

		bool groupEnabled;
		float defaultScale;

		public InAppStoreTab(AllInOneMobileSettings settings)
		{
			this.settings = settings;
			if (settings.products == null)
				settings.products = new List<InAppProduct>();
		}

		public void ShowTab()
		{
			GUILayout.Label("In-App Purchases", EditorStyles.boldLabel);
			if (!inappsInstalled)
			{
				EditorGUILayout.HelpBox(
					"First you must enable Unity In-App Purchasing in Window/Services",
					MessageType.Error);

				return;
			}

			settings.useInAppPurchases =
				EditorGUILayout.Toggle(new GUIContent("Use InApp-Purchases"), settings.useInAppPurchases);

			if (!settings.useAchievements) return;

			products = EditorGUILayout.Foldout(products, "Products", true);
			if (products)
			{
				EditorGUI.indentLevel++;
				
				for (int i = 0; i < settings.products.Count; i++)
				{
					EditorGUILayout.BeginVertical();
					settings.products[i].name =
						EditorGUILayout.TextField(new GUIContent("Name"), settings.products[i].name);
					settings.products[i].id = EditorGUILayout.TextField(new GUIContent("ID"),
						settings.products[i].id);
					EditorGUILayout.BeginHorizontal();
					settings.products[i].productType =
						(ProductType) EditorGUILayout.EnumPopup("Type", settings.products[i].productType, GUILayout.Width(300));
					if (GUILayout.Button("-", GUILayout.Width(20)))
					{
						settings.products.RemoveAt((i));
						i--;
					}

					EditorGUILayout.EndHorizontal();
					EditorGUILayout.EndVertical();
				}

				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("+", GUILayout.Width(50)))
				{
					settings.products.Add(new InAppProduct());
				}

				EditorGUILayout.Space(5);
				if (GUILayout.Button("Save products"))
				{
					GenerateProducts();
				}

				EditorGUI.indentLevel--;
			}
			
		}
		
		void GenerateProducts()
		{
			if (settings.products == null || settings.products.Count == 0)
				return;
			string enumName = "AddedProduct";
			string[] enumEntries = new string[settings.products.Count];
			for (int i = 0; i < settings.products.Count; i++)
			{
				if (settings.products[i].name.Length == 0)
				{
					Debug.LogError("Empty name!");
					return;
				}
				else if (settings.products[i].id.Length == 0)
				{
					Debug.LogError("Empty id!");
					return;
				}
				enumEntries[i] = settings.products[i].name.Replace(' ', '_');
			}
			string filePathAndName =
				"Assets/AllInOneMobile/Enums/" + enumName + ".cs";
		
			using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
			{
				streamWriter.WriteLine("public enum " + enumName);
				streamWriter.WriteLine("{");
				for (int i = 0; i < enumEntries.Length; i++)
				{
					streamWriter.WriteLine("\t" + enumEntries[i] + ",");
				}
		
				streamWriter.WriteLine("}");
			}
		
			AssetDatabase.Refresh();
		}
	}
}