#if UNITY_ANDROID
using System;
using System.Threading.Tasks;
using Runtime.PublicAPI.Core;
using Runtime.PublicAPI.Internal;

namespace Runtime.Internal.Platform.Android
{
    sealed class AndroidMonetizationSDK : IMonetizationSDK
    {
        public void Initialize(string appId, string userId, Action onCompleted, Action onFailed)
        {
            // Android implementation not yet available.
        }

        public Task<Result> InitializeAsync(string appId, string userId)
        {
            // Android implementation not yet available.
            return null;
        }

        public bool IsInitialized()
        {
            // Android implementation not yet available.
            return false;
        }

        public void LoadAds(Action onCompleted, Action onFailed)
        {
            // Android implementation not yet available.
        }

        public Task<Result> LoadAdsAsync()
        {
            // Android implementation not yet available.
            return null;
        }

        public bool IsAdsReady()
        {
            // Android implementation not yet available.
            return false;
        }

        public void ShowRewardedAds(Action onRewarded, Action onFailed, Action onSkipped)
        {
            // Android implementation not yet available.
        }

        public void Dispose()
        {
            // Android implementation not yet available.
        }
    }
}
#endif