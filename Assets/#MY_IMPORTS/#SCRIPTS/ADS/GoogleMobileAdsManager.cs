using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;

public class GoogleMobileAdsManager : MonoBehaviour/*, IUnityAdsListener*/
{
	public static GoogleMobileAdsManager Instance;

	[Space(10)] public string smallBannerId;
	[Space(10)] public string interstitialId;

	[HideInInspector] public BannerView smallBannerView;
	[HideInInspector] public InterstitialAd interstitial;

	public RewardedAd rewardedAd;
	public string unityAdId;
	public string unityVideoPlacement;
	public bool isTestMode;

	private int adSwitch;


	public void Awake()
	{
		MakeSingleton();
		InitializeAds();
		
		if (PlayerPrefs.GetInt("removeAds") != 1)
		{
			RequestBanner();
			RequestInterstitial();
		}
		//LoadRewardedAd();
	}

	public void InitializeAds()
	{
		MobileAds.Initialize((initStatus) =>
		{
			Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
			foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
			{
				string className = keyValuePair.Key;
				AdapterStatus status = keyValuePair.Value;
				switch (status.InitializationState)
				{
					case AdapterState.NotReady:
						// The adapter initialization did not complete.
						break;
					case AdapterState.Ready:
						// The adapter was successfully initialized.
						break;
				}
			}
		});
			
		//Advertisement.AddListener(this);
		Advertisement.Initialize(unityAdId, isTestMode);
		
		
	}

	private void MakeSingleton()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	//========================================  Banner AD  =============================================================//

	public void RequestBanner()
	{
		if (PlayerPrefs.GetInt("removeAds") != 1)
		{
			smallBannerView = new BannerView(isTestMode ? "ca-app-pub-3940256099942544/6300978111" : smallBannerId, AdSize.Banner, AdPosition.BottomLeft);
			AdRequest requestMenu = new AdRequest.Builder().Build();
			smallBannerView.LoadAd(requestMenu);
			smallBannerView.Hide();
		}
	}

	public void ShowSmallBanner()
	{
		if (PlayerPrefs.GetInt("removeAds") != 1)
			smallBannerView.Show();
	}

	public void HideSmallBanner()
	{
		if (PlayerPrefs.GetInt("removeAds") != 1)
			smallBannerView.Hide();
	}


	//========================================  Interstitial AD  =======================================================//
	public void RequestInterstitial()
	{
		if (PlayerPrefs.GetInt("removeAds") != 1)
		{
			interstitial = new InterstitialAd(isTestMode ? "ca-app-pub-3940256099942544/1033173712" : interstitialId);
			AdRequest request = new AdRequest.Builder().Build();
			interstitial.LoadAd(request);
			
			Advertisement.Load(unityVideoPlacement);
		}
	}

	public void ShowInterstitial()
	{
		if (PlayerPrefs.GetInt("removeAds") != 1)
		{
			if (interstitial.IsLoaded())
			{
				interstitial.Show();
				RequestInterstitial();
			}
			else if (Advertisement.IsReady(unityVideoPlacement))
			{
				Advertisement.Show(unityVideoPlacement);
				Advertisement.Load(unityVideoPlacement);
			}
		}
	}

	/*public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) 
	{
		if (showResult == ShowResult.Finished) 
		{
			GiveUserReward();
			Advertisement.Load(placementId);
		}
	}
	public void OnUnityAdsReady(string placementId)
	{ }
	public void OnUnityAdsDidError(string message)
	{ }
	public void OnUnityAdsDidStart(string placementId)
	{ }*/
	
	/*public void OnDestroy() 
	{
		Advertisement.RemoveListener(this);
	}*/

	/*public void LoadRewardedAd()
	{
		rewardedAd = new RewardedAd(rewardAdId);

		AdRequest request = new AdRequest.Builder().Build();
		rewardedAd.LoadAd(request);

		rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
		
		Advertisement.Load(unityRewardedPlacement);
	}*/

	
	/*public void ShowRewardedAd(string locationParam)
	{
		if (rewardedAd.IsLoaded())
		{
			rewardedAd.Show();
		}
		else if(Advertisement.IsReady(unityRewardedPlacement))
		{
			Advertisement.Show(unityRewardedPlacement);
		}
		else
		{
			LoadRewardedAd();
		}
		
	}*/

	/*public void HandleUserEarnedReward(object sender, Reward args)
	{
		GiveUserReward();
		LoadRewardedAd();
	}*/

	/*public void GiveUserReward()
	{
		
	}*/

	
}