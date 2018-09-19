using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;//https://developers.google.com/admob/unity/start

public class AdDriver : MonoBehaviour {

	public static AdDriver Instance{ set; get;}

	public int loadCount;
	public int interstitialInterval;//Play an interstitial after every 'x' amount of games
	//Android app ids
	public string appIdAndroid;
	public string bannerIdAndroid;
	public string interstitialIdAndroid;
    public string videoIdAndroid;
	//IOs app ids
	public string appIdIOs;
	public string bannerIdIOs;
	public string interstitialIdIOs;
    public string videoIdIOs;

	private BannerView bannerView;//From GoogleMobileAds.Api
    private InterstitialAd interstitial;
    private RewardBasedVideoAd rewardBasedVideo;
    private bool isRewarded;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GetComponent<AudioSource>().Play();
        }

        if (isRewarded) {
            GameObject.Find("GameDriver").GetComponent<GameDriver>().ContinueGame();
            isRewarded = false;
        }
    }

    void Awake () {
		Instance = this;
		DontDestroyOnLoad (gameObject);

		loadCount = 2;
        isRewarded = false;
		#if UNITY_EDITOR
			return;
        #elif UNITY_ANDROID
			        MobileAds.Initialize(appIdAndroid);
        #elif UNITY_IOS
			        MobileAds.Initialize(appIdIOs);
        #else
		        Debug.Log("Unsupported platform")
		        return;
        #endif

        //request a banner
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        //this.RequestBannerAd();

        //Request an interstitial ad
        this.RequestInterstitialAd();

        //Request a video ad
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        // Called when an ad request has successfully loaded.
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;


        this.RequestRewardBasedVideo();
    }

    public bool IsVideoLoaded() {
        #if UNITY_EDITOR
            return(false);
        #else
		    return (rewardBasedVideo.IsLoaded());
        #endif
    }

    private void RequestRewardBasedVideo() {
        #if UNITY_ANDROID
            string id = videoIdAndroid;
        #elif UNITY_IOS
			    string id = videoIdIOs;
        #else
			    Debug.Log("Unsupported platform");
			    return;
        #endif

        //Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        this.rewardBasedVideo.LoadAd(request, id);
    }

    private void RequestBannerAd(){
		//Create a BannerView
		#if UNITY_ANDROID
			string id = bannerIdAndroid;
		#elif UNITY_IOS
			string id = bannerIdIOs;
		#else
			Debug.Log("Unsupported platform");
			return;
		#endif

		bannerView = new BannerView (id, AdSize.Banner, AdPosition.Bottom);

		//Request an ad
		AdRequest req = new AdRequest.Builder().Build();

		//Load the banner with the request
		bannerView.LoadAd(req);
	}

	public void RequestInterstitialAd(){

		#if UNITY_ANDROID
		    string id = interstitialIdAndroid;
		#elif UNITY_IOS
		    string id = interstitialIdIOs;
		#else
		    Debug.Log("Unsupported platform");
		    return;
		#endif

		//Initialize an insterstitial ad
		interstitial = new InterstitialAd (id);

        // Called when an ad request has successfully loaded.
        interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        //Create an empty ad request
        AdRequest req = new AdRequest.Builder ().Build ();
		//Load the interstitial using the request, THIS IS A ONE TIME USE,
		//then a new InterstitialAd object must be created
		this.interstitial.LoadAd (req);
	}

	public void ShowInterstitial(){
		interstitial.Show ();
        return;
	}

    public void ShowBanner() {
        #if UNITY_EDITOR
            return;
        #else
            bannerView.Show();
        #endif
    }

    public void ShowVideoAd() {

        if (rewardBasedVideo.IsLoaded()) {
            rewardBasedVideo.Show();
        }

        return;
    }

    //Used when the game is starting to clean up the play area
    public void RemoveBanner() {
        #if UNITY_EDITOR
            return;
        #else
            this.bannerView.Destroy();//Destroy the current banner ad
            this.RequestBannerAd();//Request a new banner ad
        #endif
    }

    //-----------------------------------------------------------------------------------------
    //Insterstitial Ad Event Handlers----------------------------------------------------------
    //-----------------------------------------------------------------------------------------
#region
    public void HandleOnAdLoaded(object sender, EventArgs args){
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args){
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args){
        MonoBehaviour.print("HandleAdOpened event received");
    }

    private void HandleOnAdClosed(object sender, EventArgs args){
        interstitial.Destroy();
        RequestInterstitialAd();
        return;
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args){
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

#endregion


    //-----------------------------------------------------------------------------------------
    //Reward Video Event Handlers--------------------------------------------------------------
    //-----------------------------------------------------------------------------------------
#region
    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args){
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args){
        MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args){
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args){
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args){

        this.RequestRewardBasedVideo();
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args){
        isRewarded = true;
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args){
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }
#endregion
}
