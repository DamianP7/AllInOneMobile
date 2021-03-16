using System;
using System.Collections;
using System.Collections.Generic;
using AllInOneMobile;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class TestShop : MonoBehaviour
{
    public InAppStore store;

    public Text subState, premiumState, pointsText;
    int points = 0;

    void Start()
    {
        store.ActionOnPurchaseSuccess.Add(AddedProduct.Przedmiot, BoughtConsumable);
        store.ActionOnPurchaseSuccess.Add(AddedProduct.Premium, BoughtNonConsumable);
        store.ActionOnPurchaseSuccess.Add(AddedProduct.Subskrypcja, BoughtSubsciption);
    }

    public void BuyConsumable()
    {
        store.BuyProduct(AddedProduct.Przedmiot);
    }
    
    public void BuyNonConsumable()
    {
        store.BuyProduct(AddedProduct.Premium);
    }
    
    public void BuySubscription()
    {
        store.BuyProduct(AddedProduct.Subskrypcja);
    }

    void BoughtConsumable()
    {
        points += 100;
        pointsText.text = points.ToString();
    }

    void BoughtNonConsumable()
    {
        premiumState.text = "Bought";
    }

    void BoughtSubsciption()
    {
        subState.text = "Active";
    }
    
    public void Services()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ServicesTest");
    }
    
    public void Ads()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("AdsTest");
    }
}
