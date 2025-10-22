# VoodooSDK

A lightweight monetization SDK for Unity projects. It provides a simple, high‑level API to initialize the SDK, preload ads, check readiness, and show rewarded ads across platforms. Includes an Editor tooling window for configuring your Application Id.

## Features
- Simple synchronous and asynchronous initialization flows
- Preload and readiness checks to avoid show-time delays
- Rewarded ads with rewarded/failed/skipped callbacks
- Editor tooling: Tools/Monetization configuration
- Clear Result type for async operations

## Requirements
- Unity: tested with 6000.2.7f2 (Apple Silicon/Intel compatible). Other modern LTS versions should work.
- Scripting backend: .NET/IL2CPP
- Platforms: Editor (mock), Android (example implementation). iOS/others can be added by implementing the platform interface.

## Installation
You can use any of these approaches:

Add from a Git URL:
- Window > Package Manager > + > Add package from Git URL…
- Enter your repository URL (for example: https://github.com/your-org/voodoo-sdk.git)

Note: This repository already contains a package.json under Assets/, so Unity will treat it as an embedded UPM package when added from disk.

## Configuration
Before using the API, set your Application Id in the configuration window:
- Menu: Tools > Monetization configuration
- An Editor window opens. Enter your Application Id.
- This writes a JSON file at Assets/Resources/monetizationConfig.json

At runtime, the SDK loads this TextAsset via Resources.Load("monetizationConfig"). If the file is missing or empty, initialization will fail with an explanatory error.

## Quick Start
Synchronous API (callbacks):

```
using Voodoo;
using UnityEngine;

public class MonetizationExample : MonoBehaviour
{
    void Start()
    {
        Monetization.Initialize(
            userId: SystemInfo.deviceUniqueIdentifier,
            onCompleted: () => Debug.Log("Monetization initialized"),
            onFailed:    () => Debug.LogError("Monetization init failed"));
    }

    public void Preload()
    {
        Monetization.LoadAds(
            onCompleted: () => Debug.Log("Ads loaded"),
            onFailed:    () => Debug.LogError("Ads load failed"));
    }

    public void ShowRewarded()
    {
        if (!Monetization.IsAdsReady())
        {
            Debug.Log("Ads not ready yet");
            return;
        }

        Monetization.ShowRewardedAds(
            onRewarded: () => Debug.Log("Reward the user"),
            onFailed:   () => Debug.LogError("Show failed"),
            onSkipped:  () => Debug.Log("User skipped, no reward"));
    }
}
```

Asynchronous API (Tasks):

```
using System.Threading.Tasks;
using UnityEngine;
using Voodoo;

public class MonetizationAsyncExample : MonoBehaviour
{
    async void Start()
    {
        var init = await Monetization.InitializeAsync(SystemInfo.deviceUniqueIdentifier);
        if (!init.Success)
        {
            Debug.LogError($"Init failed: {init.Message}");
            return;
        }

        var load = await Monetization.LoadAdsAsync();
        if (!load.Success)
        {
            Debug.LogError($"Load failed: {load.Message}");
            return;
        }

        if (Monetization.IsAdsReady())
        {
            Monetization.ShowRewardedAds(
                onRewarded: () => Debug.Log("Reward!"),
                onFailed:   () => Debug.LogError("Show failed"),
                onSkipped:  () => Debug.Log("Skipped"));
        }
    }
}
```

## Public API Overview
Namespace: Voodoo

- Initialize(string userId, Action onCompleted, Action onFailed)
  Initializes using Resources/monetizationConfig. Calls the provided callbacks.

- Task<Result> InitializeAsync(string userId)
  Asynchronously initializes using Resources/monetizationConfig. Returns a Result with Success and Message.

- bool IsInitialized()
  Returns true if the SDK instance exists and is initialized.

- void LoadAds(Action onCompleted, Action onFailed)
  Preloads ads. Requires prior initialization.

- Task<Result> LoadAdsAsync()
  Async preload. Returns Result.

- bool IsAdsReady()
  Indicates whether ad content is ready to be shown.

- void ShowRewardedAds(Action onRewarded, Action onFailed, Action onSkipped)
  Shows a rewarded ad if ready. Emits reward/failed/skipped callbacks.

- void Dispose()
  Disposes SDK resources. Call Initialize again to reuse after disposal.

Result type (Runtime.PublicAPI.Core.Result):
- bool Success
- string Message
- static Result SuccessResult
- static Result FailedResult(string message)

## Editor Tooling
- Window/Menu: Tools > Monetization configuration
- Stores config at Assets/Resources/monetizationConfig.json
- The runtime loader expects this file to exist under a Resources folder as a TextAsset named monetizationConfig

## Platform Notes
- Editor: Uses a mock provider so you can exercise the flow in Edit Mode and unit tests
- Android: Example implementation present under Runtime/PublicAPI/Internal/Platform/Android
- Extensibility: Add new platforms by implementing IMonetizationSDK and wiring it in SDKProvider

## Troubleshooting
- Init failed: "Config not found" or "Application id is empty"
  Ensure Tools > Monetization configuration has your Application Id and the generated file exists at Assets/Resources/monetizationConfig.json
- SDK not initialized
  Call Initialize/InitializeAsync before any other API
- Ads not ready
  Call LoadAds/LoadAdsAsync and wait until it completes successfully before showing

## Testing
There are EditMode tests under Assets/Tests/EditMode which exercise editor flows. You can run them via the Unity Test Runner.

