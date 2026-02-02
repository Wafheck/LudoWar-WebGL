using UnityEngine;
using System;
using System.Collections;

public class ShowRewardedVideoScript : MonoBehaviour
{
	
	
	
	public static String REWARDED_INSTANCE_ID = "DefaultRewardedVideo";//DefaultRewardedVideo
	AuthenticationManager manager;
	string appKey;

	// Use this for initialization
	void Start ()
	{	
		Utils.Log ("Reward Ad", " ShowRewardedVideoScript Start called");
#if UNITY_WEBGL
		Debug.Log("WebGL: Rewarded video ads disabled - IronSource not supported in browser");
		manager = AuthenticationManager.Instance;
		return;
#elif UNITY_ANDROID
        appKey = "1d7206c35";//"1a2a91f85";
#elif UNITY_IPHONE
		appKey = "1a2a8e5fd";
#else
        appKey = "unexpected_platform";
#endif
		Utils.Log("Reward Ad", " MyAppStart Start called");

		//Dynamic config example
		IronSourceConfig.Instance.setClientSideCallbacks(true);

		string id = IronSource.Agent.getAdvertiserId();
		Utils.Log("Reward Ad", " IronSource.Agent.getAdvertiserId : " + id);

	//	AuthenticationManager.Instance.ShowAlert("Ad Ready for " + appKey, "AdvertiserId = " + id);

		Utils.Log("Reward Ad", " IronSource.Agent.validateIntegration");
		IronSource.Agent.validateIntegration();

		Utils.Log("Reward Ad", " unity version" + IronSource.unityVersion());

		manager  = AuthenticationManager.Instance;
		//Add Rewarded Video Events
		
		IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoAdOpenedEvent;
		IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoAdClosedEvent;
		IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoAvailabilityChangedEvent;
		IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoAdStartedEvent;
		IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoAdEndedEvent;
		IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoAdRewardedEvent;
		IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
		IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoAdClickedEvent;

		IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO);

		IronSource.Agent.loadRewardedVideo();
	}

    






    // Update is called once per frame
    void Update ()
	{
		
	}

	/************* RewardedVideo API *************/ 
	public void ShowRewardedVideoButtonClicked ()
	{
		Utils.Log ("Reward Ad", "ShowRewardedVideoButtonClicked");
#if UNITY_WEBGL
		// WebGL: Give free coins instead of showing ad
		Debug.Log("WebGL: Giving free coins - rewarded video not available in browser");
		manager.ShowAlert("Free Coins!", "You received 200 coins!", "CointHit");
		manager.AddBonus(200);
		manager.ShowCoinCollectionAnim();
		return;
#endif
		if (IronSource.Agent.isRewardedVideoAvailable ()) {
			IronSource.Agent.showRewardedVideo ();
		} else {
			manager.ShowAlert("Collect Rewards !!!", "Reward Video Not Available!", "FailHit");
		}

		// DemandOnly
		// ShowDemandOnlyRewardedVideo ();
	}

  
    /************* RewardedVideo Delegates *************/ 
	void RewardedVideoAvailabilityChangedEvent ()
	{
		
			manager.ShowAlert("Ad Rewards","Reward Video Not Available","FailHit");
		
	}

	void RewardedVideoAdOpenedEvent (IronSourceAdInfo obj)
	{
		Utils.Log ("Reward Ad", " I got RewardedVideoAdOpenedEvent");
	}

	Boolean rewarded = false;
	void RewardedVideoAdRewardedEvent (IronSourcePlacement ssp, IronSourceAdInfo arg2)
	{
		Utils.Log ("Reward Ad", " I got RewardedVideoAdRewardedEvent, amount = " + ssp.getRewardAmount () + " name = " + ssp.getRewardName ());

		//manager.ShowAlert("Ad Rewards :"+ssp.getRewardName(), "Reward = "+ssp.getRewardAmount());
		rewarded = true;
	}
	
	void RewardedVideoAdClosedEvent (IronSourceAdInfo obj)
	{
		manager.ShowAlert("Wow !!!","You collected the Reward", "CointHit");
		Utils.Log ("Reward Ad", " I got RewardedVideoAdClosedEvent");
		if(rewarded)
		manager.ShowCoinCollectionAnim();
		IronSource.Agent.loadRewardedVideo();
	}

	void RewardedVideoAdStartedEvent (IronSourceAdInfo obj)
	{
		rewarded = false;
		Utils.Log ("Reward Ad", " I got RewardedVideoAdStartedEvent");
	}

	void RewardedVideoAdEndedEvent (IronSourceAdInfo obj)
	{
		Utils.Log ("Reward Ad", "I got RewardedVideoAdEndedEvent");
	}
	
	void RewardedVideoAdShowFailedEvent (IronSourceError error, IronSourceAdInfo arg2)
	{
		Utils.Log ("Reward Ad", " I got RewardedVideoAdShowFailedEvent, code :  " + error.getCode () + ", description : " + error.getDescription ());
		manager.ShowAlert("Ad Rewards Err :" + error.getErrorCode(), "Desc = " + error.getDescription(), "FailHit");

	}

	void RewardedVideoAdClickedEvent (IronSourcePlacement ssp, IronSourceAdInfo arg2)
	{
		Utils.Log ("Reward Ad", " I got RewardedVideoAdClickedEvent, name = " + ssp.getRewardName ());
	}

	

}
