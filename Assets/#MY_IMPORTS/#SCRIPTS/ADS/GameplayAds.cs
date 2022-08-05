using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayAds : MonoBehaviour
{
    public void ShowAd()
    {
        if (GoogleMobileAdsManager.Instance != null)
        {
            GoogleMobileAdsManager.Instance.ShowSmallBanner();
        }
    }
    
    public void HideAd()
    {
        if (GoogleMobileAdsManager.Instance != null)
        {
            GoogleMobileAdsManager.Instance.HideSmallBanner();
        }
    }
}
