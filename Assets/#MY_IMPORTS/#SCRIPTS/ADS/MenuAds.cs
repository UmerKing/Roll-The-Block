using UnityEngine;

public class MenuAds : MonoBehaviour
{
    void Start()
    {
        if (GoogleMobileAdsManager.Instance != null)
        {
            GoogleMobileAdsManager.Instance.ShowSmallBanner();
        }
    }
}
