using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShowInterstitialScript : MonoBehaviour
{
	
 	public static String INTERSTITIAL_INSTANCE_ID = "DefaultInterstitial";

	string appKey;

	// Use this for initialization
	void Start ()
	{
		Debug.Log ("unity-script: ShowInterstitialScript Start called");

#if UNITY_WEBGL
		Debug.Log("WebGL: Interstitial ads disabled - IronSource not supported in browser");
		return;
#elif UNITY_ANDROID
         appKey = "1d7206c35";//"1a2a91f85";
#elif UNITY_IPHONE
		 appKey = "1a2a8e5fd";
#else
         appKey = "unexpected_platform";
#endif
		Debug.Log("unity-script: MyAppStart Start called");

		//Dynamic config example
		IronSourceConfig.Instance.setClientSideCallbacks(true);

		string id = IronSource.Agent.getAdvertiserId();
		Debug.Log("unity-script: IronSource.Agent.getAdvertiserId : " + id);


		Debug.Log("unity-script: IronSource.Agent.validateIntegration");
		IronSource.Agent.validateIntegration();

		Debug.Log("unity-script: unity version" + IronSource.unityVersion());


		// Add Interstitial Events
		

		IronSourceInterstitialEvents.onAdReadyEvent += InterstitialAdReadyEvent;
		IronSourceInterstitialEvents.onAdLoadFailedEvent+= InterstitialAdLoadFailedEvent;
		IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
		IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialAdShowFailedEvent;
		IronSourceInterstitialEvents.onAdClickedEvent += InterstitialAdClickedEvent;
		IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialAdOpenedEvent;
		IronSourceInterstitialEvents.onAdClosedEvent += InterstitialAdClosedEvent;

		IronSource.Agent.init(appKey,IronSourceAdUnits.INTERSTITIAL);
		IronSource.Agent.loadInterstitial();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	/************* Interstitial API *************/ 
	
	string sceneName;

	public void ShowInterstitialButtonClicked (string sceneName)
	{
		this.sceneName = sceneName;
#if UNITY_WEBGL
		// WebGL: Skip ads and load scene directly
		Debug.Log("WebGL: Skipping interstitial ad, loading scene directly: " + sceneName);
		SceneManager.LoadSceneAsync(sceneName);
		return;
#endif
		IronSource.Agent.hideBanner();
		Debug.Log ("unity-script: ShowInterstitialButtonClicked");
		if (IronSource.Agent.isInterstitialReady ()) {
			IronSource.Agent.showInterstitial ();
		} else {
			Debug.Log ("unity-script: IronSource.Agent.isInterstitialReady - False");			
			SceneManager.LoadSceneAsync(sceneName);
		}

	
	}

	
	

	/************* Interstitial Delegates *************/ 
	void InterstitialAdReadyEvent (IronSourceAdInfo adInfo)
	{
		Debug.Log ("unity-script: I got InterstitialAdReadyEvent");

	//	AuthenticationManager.Instance.ShowAlert("Ad Ready ", "Yes" );

	}

	void InterstitialAdLoadFailedEvent (IronSourceError error)
	{
		Debug.Log ("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode () + ", description : " + error.getDescription ());
		SceneManager.LoadSceneAsync(sceneName);
	}

	void InterstitialAdShowSucceededEvent (IronSourceAdInfo adInfo)
	{
		Debug.Log ("unity-script: I got InterstitialAdShowSucceededEvent");
		
		//	AuthenticationManager.Instance.ShowAlert("Ad Show Succeded ", "Yes");
	}
	
	void InterstitialAdShowFailedEvent (IronSourceError error, IronSourceAdInfo adInfo)
	{
		Debug.Log ("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode () + ", description : " + error.getDescription ());
		//	AuthenticationManager.Instance.ShowAlert("Ad Failed Error : " + error.getCode(), "Ad Desc : " + error.getDescription());
		SceneManager.LoadSceneAsync(sceneName);
	}

	void InterstitialAdClickedEvent (IronSourceAdInfo adInfo)
	{
		Debug.Log ("unity-script: I got InterstitialAdClickedEvent");
	}
	
	void InterstitialAdOpenedEvent (IronSourceAdInfo adInfo)
	{
		Debug.Log ("unity-script: I got InterstitialAdOpenedEvent");
	}

	void InterstitialAdClosedEvent (IronSourceAdInfo adInfo)
	{
		Debug.Log ("unity-script: I got InterstitialAdClosedEvent");
        SceneManager.LoadSceneAsync(sceneName);
		//IronSource.Agent.loadInterstitial();
	}

	

}

