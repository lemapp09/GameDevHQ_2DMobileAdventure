using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

namespace LemApperson_2D_Mobile_Adventure.Managers
{
    public class AdsManager : MonoSingleton<AdsManager>, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        
        private string _gameId;
        [SerializeField] string _androidGameId = "5438777";
        [SerializeField] bool _testMode = true;
        [SerializeField] Button _showAdButton;
        [SerializeField] string _androidAdUnitId = "Rewarded_Android";
        string _adUnitId = null; // This will remain null for unsupported platforms
        private bool _rewardOnce;
        
        
        void OnEnable() {
            InitializeAds();
            Invoke( "LoadAd", 2f);
        } 
        
        public void InitializeAds() {
#if UNITY_ANDROID
            _gameId = _androidGameId;
            _adUnitId = _androidAdUnitId;
#elif UNITY_EDITOR
              _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
            if (!Advertisement.isInitialized && Advertisement.isSupported) {
                Advertisement.Initialize(_gameId, _testMode, this);
            }
            // Disable the button until the ad is ready to show:
            _showAdButton.interactable = false;
        }

                
        public void ShowAd()
        {
            // Disable the button:
            _showAdButton.interactable = false;
            _rewardOnce = false;
            // Then show the ad:
            Advertisement.Show(_adUnitId, this);
        }

        public void OnInitializationComplete() {
            Debug.Log("Unity Ads initialization complete.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }
        
        // Call this public method when you want to get an ad ready to show.
        public void LoadAd()
        {
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }

        // If the ad successfully loads, add a listener to the button and enable it:
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            Debug.Log("Ad Loaded: " + adUnitId);
 
            if (adUnitId.Equals(_adUnitId))
            {
                // Configure the button to call the ShowAd() method when clicked:
                _showAdButton.onClick.AddListener(ShowAd);
                // Enable the button for users to click:
                _showAdButton.interactable = true;
            }
        }

        public void OnUnityAdsFailedToLoad(string adUnityId, UnityAdsLoadError error, string message)
        {
            throw new System.NotImplementedException();
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }

        public void OnUnityAdsShowStart(string placementId)
        { }

        public void OnUnityAdsShowClick(string placementId)
        { }

        // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                if(!_rewardOnce)
                {
                    _rewardOnce = true;
                    GameManager.Instance.RewardGems(100);
                    Debug.Log("Unity Ads Rewarded Ad Completed");
                }
            } 
        }
        
        void OnDestroy()
        {
            // Clean up the button listeners:
            _showAdButton.onClick.RemoveAllListeners();
        }
    }
}