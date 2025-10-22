using System;
using System.Threading.Tasks;
using PublicAPI.Internal;
using Runtime.PublicAPI.Core;
using Runtime.PublicAPI.Internal;
using UnityEngine;

namespace Voodoo
{
    /// <summary>
    /// High-level API for interacting with the Monetization SDK.
    /// Provides synchronous and asynchronous methods to initialize the SDK,
    /// preload ads, query readiness, display rewarded ads, and dispose resources.
    /// </summary>
    /// <remarks>
    /// - The synchronous Initialize method reads the application id from a Resources config ("monetizationConfig").
    /// - The asynchronous InitializeAsync requires an explicit application id parameter.
    /// - Always call Initialize/InitializeAsync before other operations.
    /// </remarks>
    public static class Monetization
    {
        private const string ConfigLoadPath = "monetizationConfig";
        
        private static IMonetizationSDK _sdk;
        private static ConfigData _config;
        
        /// <summary>
        /// Initializes the Monetization SDK using configuration loaded from Resources/monetizationConfig.
        /// </summary>
        /// <param name="userId">A unique identifier for the current user/session.</param>
        /// <param name="onCompleted">Callback invoked when initialization succeeds.</param>
        /// <param name="onFailed">Callback invoked when initialization fails (e.g., missing config or invalid app id).</param>
        public static void Initialize(string userId, Action onCompleted, Action onFailed)
        {
            Debug.Log("Monetization Init");
            var configInitResult = InitializeConfig();
            if (!configInitResult.Success)
            {
                onFailed?.Invoke();
                return;           
            }

            _sdk = SDKProvider.Create();
            _sdk.Initialize(_config.appId, userId, onCompleted, onFailed);
        }
        
        /// <summary>
        /// Asynchronously initializes the Monetization SDK using the provided application id and user id.
        /// </summary>
        /// <param name="appId">The monetization application identifier supplied by the provider.</param>
        /// <param name="userId">A unique identifier for the current user/session.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
        public static Task<Result> InitializeAsync(string userId)
        {
            Debug.Log("Monetization Init async");
            var configInitResult = InitializeConfig();
            if (!configInitResult.Success)
            {
                return Task.FromResult(configInitResult);           
            }
            
            _sdk = SDKProvider.Create();
            return _sdk.InitializeAsync(_config.appId, userId);
        }
        
        /// <summary>
        /// Indicates whether the Monetization SDK has been created and initialized.
        /// </summary>
        /// <returns>True if initialized; otherwise, false.</returns>
        public static bool IsInitialized()
        {
            return _sdk != null && _sdk.IsInitialized();
        }
        
        /// <summary>
        /// Preloads ad content so that it can be displayed without delay.
        /// </summary>
        /// <param name="onCompleted">Callback invoked when ads are successfully loaded.</param>
        /// <param name="onFailed">Callback invoked if loading fails or the SDK is not initialized.</param>
        public static void LoadAds(Action onCompleted, Action onFailed)
        {
            Debug.Log("LoadAds");
            if (_sdk == null)
            {
                Debug.LogError("SDK not initialized.");
                return;
            }
            
            _sdk.LoadAds(onCompleted, onFailed);
        }
        
        /// <summary>
        /// Asynchronously preloads ad content.
        /// </summary>
        /// <returns>A <see cref="Result"/> that indicates whether loading succeeded.</returns>
        public static Task<Result> LoadAdsAsync()
        {
            Debug.Log("LoadAds async");
            if (_sdk == null)
            {
                Debug.LogError("SDK is not initialized.");
                return Task.FromResult(Result.FailedResult("SDK is not initialized"));
            }
            
            return _sdk.LoadAdsAsync();
        }
        
        /// <summary>
        /// Returns whether ads are currently ready to be shown.
        /// </summary>
        /// <returns>True if ad content is ready; otherwise, false.</returns>
        public static bool IsAdsReady()
        {
            if (_sdk == null)
            {
                Debug.LogError("SDK not initialized.");
                return false;
            }
            
            return _sdk.IsAdsReady();
        }

        /// <summary>
        /// Displays a rewarded ad if the SDK is initialized and ad content is ready.
        /// </summary>
        /// <param name="onRewarded">Invoked when the user finishes the ad and a reward should be granted.</param>
        /// <param name="onFailed">Invoked if showing the ad fails.</param>
        /// <param name="onSkipped">Invoked if the user skips the ad and no reward should be granted.</param>
        public static void ShowRewardedAds(Action onRewarded, Action onFailed, Action onSkipped)
        {
            Debug.Log("ShowRewardedAds");
            if (_sdk == null)
            {
                Debug.LogError("SDK not initialized.");
                return;
            }

            if (!IsAdsReady())
            {
                Debug.LogError("Ads not ready.");
                return;           
            }

            _sdk.ShowRewardedAds(onRewarded, onFailed, onSkipped);
        }

        /// <summary>
        /// Disposes the Monetization SDK instance and releases its resources.
        /// </summary>
        /// <remarks>
        /// After calling this method, the SDK reference is cleared. You must call Initialize/InitializeAsync again before using any API.
        /// </remarks>
        public static void Dispose()
        {
            Debug.Log("ShowRewardedAds");
            if (_sdk == null)
            {
                Debug.LogError("SDK not initialized.");
                return;
            }
            
            _sdk.Dispose();
            _sdk = null;
        }

        private static Result InitializeConfig()
        {
            var configJson = Resources.Load<TextAsset>(ConfigLoadPath);
            if (configJson == null)
            {
                Debug.LogError("Config not found. Open Tools/Monetization configuration and enter valid application id");
                return Result.FailedResult("Config not found. Open Tools/Monetization configuration and enter valid application id");
            }
            _config = JsonUtility.FromJson<ConfigData>(configJson.text);

            if (_config == null)
            {
                Debug.LogError("Failed to load config from file");
                return Result.FailedResult("Failed to load config from file");
            }

            if (!string.IsNullOrEmpty(_config.appId)) 
                return Result.SuccessResult;
            
            Debug.LogError("Application id is empty. Open Tools/Monetization configuration and enter valid application id");
            return Result.FailedResult("Application id is empty. Open Tools/Monetization configuration and enter valid application id");
        }
    }
}