# Pico Ultra/4 Setup Guide

## Overview
This guide covers setting up your AR Calligraphy app for **Pico Ultra** or **Pico 4** devices. Pico uses its own SDK (separate from OpenXR/Meta Quest).

## Important Notes

⚠️ **Pico requires a separate SDK** - OpenXR alone is not enough for Pico devices.

✅ **You can support BOTH Meta Quest and Pico** in the same project:
- Meta Quest uses OpenXR (already set up)
- Pico uses Pico SDK
- Unity will automatically use the correct provider based on device

## Step 1: Download Pico SDK

1. Go to **Pico Developer Portal**: https://developer.pico-interactive.com/
2. Sign up/login (free developer account)
3. Download **Pico SDK for Unity**
4. Extract the package

## Step 2: Install Pico SDK in Unity

### Option A: Import Package
1. In Unity: **Assets > Import Package > Custom Package...**
2. Select the Pico SDK `.unitypackage` file
3. Import all files

### Option B: Package Manager (if available)
1. **Window > Package Manager**
2. Click **+** button > **Add package from disk**
3. Select Pico SDK `package.json`

## Step 3: Configure XR Management

1. **Edit > Project Settings > XR Plug-in Management**
2. Under **"Providers"** section:
   - ✅ Enable **Pico SDK** (not just OpenXR)
   - ✅ Keep **OpenXR** enabled (for Meta Quest)
3. Both can be enabled simultaneously

## Step 4: Configure Pico SDK Settings

1. **Edit > Project Settings > XR Plug-in Management > Pico SDK**
2. Enable required features:
   - ✅ **Hand Tracking** (for tracing)
   - ✅ **Controller Support** (optional fallback)
   - ✅ **Passthrough** (for AR overlay, if available)

## Step 5: Hand Tracking Setup

Pico has its own hand tracking system (different from OpenXR):

1. In Pico SDK settings, enable **Hand Tracking**
2. Configure hand tracking parameters:
   - Hand tracking mode
   - Update frequency
   - Confidence threshold

3. **In your code**, you'll need to use Pico SDK APIs:
   ```csharp
   // Pico SDK hand tracking (different from OpenXR)
   using Pico.Platform;
   using Pico.Platform.Models;
   ```

## Step 6: Build Settings for Pico

1. **File > Build Settings**
2. Select **Android** platform
3. **Player Settings > Android:**
   - Package Name: `com.yourcompany.arcalligraphy`
   - Minimum API Level: 24 or higher
   - Target API Level: Automatic
   - Scripting Backend: IL2CPP
   - Target Architectures: ARM64

## Step 7: Update Your Code

Your `TracingManager.cs` will need to support both platforms:

```csharp
private Vector3 GetHandPosition()
{
    // Try Pico SDK first (if available)
    #if PICO_SDK
    if (PicoPlatform.IsInitialized())
    {
        // Use Pico hand tracking API
        // Implementation depends on Pico SDK version
    }
    #endif
    
    // Fall back to OpenXR (Meta Quest)
    List<InputDevice> devices = new List<InputDevice>();
    InputDevices.GetDevicesWithCharacteristics(
        InputDeviceCharacteristics.HandTracking, devices);
    // ... rest of OpenXR code
}
```

## Pico vs Meta Quest Differences

| Feature | Meta Quest (OpenXR) | Pico (Pico SDK) |
|---------|-------------------|-----------------|
| Hand Tracking | OpenXR Hand Tracking | Pico Hand Tracking API |
| Controller | Oculus Touch Profile | Pico Controller API |
| Passthrough | OpenXR Passthrough | Pico Passthrough API |
| Setup | OpenXR features | Pico SDK package |

## Testing on Pico

1. **Enable Developer Mode:**
   - Pico Settings > About > Tap version number 7 times
   - Enable USB debugging

2. **Connect Device:**
   - Connect Pico to PC via USB
   - Allow USB debugging on device

3. **Build and Deploy:**
   - Build APK in Unity
   - Install via ADB or Pico Developer Hub

## Troubleshooting

### Pico SDK not showing in XR Management?
- Make sure package is properly imported
- Check that Pico SDK files are in `Assets/Plugins/Pico/`
- Restart Unity

### Hand tracking not working?
- Verify Pico SDK hand tracking is enabled
- Check device supports hand tracking (Pico 4/Ultra do)
- Enable hand tracking in Pico device settings

### Can't build for both Quest and Pico?
- You CAN support both! Enable both providers
- Unity will use the correct one at runtime
- Test on each device separately

## Quick Checklist

- [ ] Pico SDK downloaded from developer portal
- [ ] Pico SDK imported into Unity project
- [ ] Pico SDK enabled in XR Management
- [ ] Hand tracking enabled in Pico SDK settings
- [ ] Android build settings configured
- [ ] Code updated to support Pico APIs
- [ ] Tested on Pico device

## Resources

- **Pico Developer Portal**: https://developer.pico-interactive.com/
- **Pico SDK Documentation**: Check portal for latest docs
- **Pico Unity SDK GitHub**: (if available)

## Important Reminder

Since you're targeting **both Meta Quest and Pico**, make sure:
1. ✅ OpenXR + Meta Quest Support enabled (for Quest)
2. ✅ Pico SDK enabled (for Pico)
3. ✅ Your code handles both platforms
4. ✅ Test on both device types before hackathon

The good news: Your app can work on both platforms! Just need to set up both SDKs.

