using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace AllInOneMobile
{
	public class InAppStore : Singleton<InAppStore>, IStoreListener
	{
		static IStoreController storeController;
		static IExtensionProvider storeExtensionProvider;

		public Dictionary<AddedProduct, Action> ActionOnPurchaseSucces = new Dictionary<AddedProduct, Action>();

		void Start()
		{
			if (storeController == null)
			{
				InitializePurchasing();
			}
		}

		public void InitializePurchasing()
		{
			if (IsInitialized())
			{
				return;
			}

			var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

			foreach (InAppProduct product in AllInOneMobileSettings.Instance.products)
			{
				builder.AddProduct(product.id, product.productType);
			}

			UnityPurchasing.Initialize(this, builder);
		}

		bool IsInitialized()
		{
			return storeController != null && storeExtensionProvider != null;
		}

		public void BuyProduct(AddedProduct addedProduct)
		{
			string id = AllInOneMobileSettings.Instance.products.Find(x => x.name == addedProduct.ToString()).id;
			BuyProductID(id);
		}

		void BuyProductID(string productId)
		{
			if (IsInitialized())
			{
				Product product = storeController.products.WithID(productId);

				if (product != null && product.availableToPurchase)
				{
					Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
					storeController.InitiatePurchase(product);
				}
				else
				{
					Debug.Log(
						"BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				}
			}
			else
			{
				Debug.Log("BuyProductID FAIL. Not initialized.");
			}
		}

		/// <summary>
		/// RestorePurchases works only on Apple devices
		/// </summary>
		public void RestorePurchases()
		{
			if (!IsInitialized())
			{
				Debug.Log("RestorePurchases FAIL. Not initialized.");
				return;
			}

			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}

		public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
		{
			Debug.Log("OnInitialized: PASS");

			storeController = controller;
			storeExtensionProvider = extensions;
		}

		public void OnInitializeFailed(InitializationFailureReason error)
		{
			Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
		}


		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
		{
			int index = AllInOneMobileSettings.Instance.products.FindIndex(x =>
				x.id == args.purchasedProduct.definition.id);

			if (ActionOnPurchaseSucces.ContainsKey((AddedProduct) index))
				ActionOnPurchaseSucces[(AddedProduct) index].Invoke();

			Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");


			return PurchaseProcessingResult.Complete;
		}


		public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
		{
			Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
				product.definition.storeSpecificId, failureReason));
		}
	}
}