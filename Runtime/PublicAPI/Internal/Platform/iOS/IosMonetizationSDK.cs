#if UNITY_IOS
using System;
using System.Threading.Tasks;
using Runtime.PublicAPI.Core;
using Runtime.PublicAPI.Internal;

namespace Runtime.Internal.Platform.iOS
{
    sealed class IosMonetizationSDK : IMonetizationSDK
    {
        public void Initialize(string appId, string userId, Action onCompleted, Action onFailed)
        {
            // iOS implementation not yet available.
        }

        public Task<Result> InitializeAsync(string appId, string userId)
        {
            // iOS implementation not yet available.
            return null;
        }

        public bool IsInitialized()
        {
            // iOS implementation not yet available.
            return false;
        }

        public void LoadAds(Action onCompleted, Action onFailed)
        {
            // iOS implementation not yet available.
        }

        public Task<Result> LoadAdsAsync()
        {
            // iOS implementation not yet available.
            return null;
        }

        public bool IsAdsReady()
        {
            // iOS implementation not yet available.
            return false;
        }

        public void ShowRewardedAds(Action onRewarded, Action onFailed, Action onSkipped)
        {
            // iOS implementation not yet available.
        }

        public void Dispose()
        {
            // iOS implementation not yet available.
        }
    }
}
#endif