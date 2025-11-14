# OpenXR Feature Setup Guide

## Current Status
You have **Hand Interaction Poses** and **Meta Quest Support** enabled, but they show warnings. Here's how to fix this and enable all necessary features.

## Required Features for AR Calligraphy App

### For Android Platform (Meta Quest/Pico):

1. **Meta Quest Support** ✅ (Enabled - but has warning)
   - This is CRITICAL for Quest devices
   - Warning usually means: Need to configure Quest-specific settings

2. **Hand Interaction Profile** ⚠️ (Should be enabled)
   - Different from "Hand Interaction Poses"
   - This enables hand tracking input
   - Required for tracing with hands

3. **Hand Interaction Poses** ✅ (Enabled - but has warning)
   - Provides hand pose data
   - Warning might indicate missing dependencies

4. **Controller Profiles** (Optional but recommended)
   - For controller-based tracing as fallback
   - **Meta Quest Touch Pro Controller Profile** - For Quest Pro controllers
   - **Meta Quest Touch Plus Controller Profile** - For Quest 3/3S controllers
   - **Oculus Touch Controller Profile** - For Quest 2 and older (legacy)
   - Enable the ones matching your target device(s)

## How to Fix the Warnings

### Step 1: Enable Hand Interaction Profile

1. In Unity, go to **Edit > Project Settings > XR Plug-in Management > OpenXR**
2. Make sure you're viewing the **Android** platform tab (not Standalone)
3. Scroll down to find **"Hand Interaction Profile"**
4. **Enable** the checkbox
5. This is different from "Hand Interaction Poses" - you need BOTH

### Step 2: Fix Meta Quest Support Warning

The yellow warning on Meta Quest Support usually means:

1. **Check Quest Device Targets:**
   - Click the gear icon ⚙️ next to "Meta Quest Support"
   - Make sure your target Quest device is checked:
     - ✅ Quest
     - ✅ Quest 2
     - ✅ Quest 3
     - ✅ Quest Pro (if applicable)

2. **Verify Android Build Settings:**
   - **File > Build Settings**
   - Ensure **Android** is selected
   - **Player Settings > Android:**
     - Minimum API Level: 24 or higher
     - Target API Level: Automatic
     - Scripting Backend: IL2CPP
     - Target Architectures: ARM64

### Step 3: Fix Hand Interaction Poses Warning

The orange warning might indicate:

1. **Missing OpenXR Hand Tracking Extension:**
   - The feature requires the device to support hand tracking
   - This is normal - the warning will disappear when you build and run on Quest
   - Quest 2/3/Pro support hand tracking natively

2. **Check Feature Dependencies:**
   - "Hand Interaction Poses" requires "Hand Interaction Profile" to be enabled
   - Make sure both are enabled

## Recommended Feature Configuration

### For Meta Quest (Android Platform):

**Enable these features:**
- ✅ Meta Quest Support (with gear icon configured)
- ✅ Hand Interaction Profile
- ✅ Hand Interaction Poses
- ✅ Controller Profile(s) (optional, for fallback):
  - **Meta Quest Touch Pro** - If targeting Quest Pro
  - **Meta Quest Touch Plus** - If targeting Quest 3/3S
  - **Oculus Touch Controller** - If targeting Quest 2 or older
  - You can enable multiple if supporting multiple devices

**Optional but useful:**
- ⚪ Foveated Rendering (improves performance)
- ⚪ Application SpaceWarp (improves performance)
- ⚪ XR Performance Settings (optimization)

### For Pico Ultra/4 (Android Platform):

**Important:** Pico requires a separate SDK package. OpenXR alone is not enough.

1. **Install Pico SDK:**
   - Download from: https://developer.pico-interactive.com/
   - Import the Pico SDK Unity package
   - Or install via Package Manager if available

2. **Enable Pico in XR Management:**
   - **Edit > Project Settings > XR Plug-in Management**
   - Under "Provider", enable **Pico SDK** (not just OpenXR)
   - Pico has its own XR provider

3. **Enable Hand Tracking:**
   - Pico SDK has its own hand tracking system
   - Enable in Pico SDK settings (not OpenXR)
   - Similar to Meta Quest hand tracking

4. **Note:** You can support BOTH Meta Quest and Pico:
   - Enable Meta Quest Support (OpenXR) for Quest devices
   - Enable Pico SDK for Pico devices
   - Unity will use the appropriate provider based on device

## Step-by-Step Setup

### 1. Open OpenXR Settings
- **Edit > Project Settings > XR Plug-in Management > OpenXR**

### 2. Select Android Platform
- **IMPORTANT:** Make sure you're in the **Android** tab (not Standalone)
- Meta Quest Support **only appears** in the Android tab!
- Look at the top of the window for platform tabs

### 3. Enable Required Features
- ✅ **Meta Quest Support** - Click gear, verify Quest devices selected
- ✅ **Hand Interaction Profile** - Enable checkbox
- ✅ **Hand Interaction Poses** - Already enabled (warning is OK for now)
- ✅ **Controller Profile(s)** - Enable based on your target device:
  - **Meta Quest Touch Pro** - For Quest Pro
  - **Meta Quest Touch Plus** - For Quest 3/3S (newer)
  - **Oculus Touch Controller** - For Quest 2 (standard/legacy)
  - Enable all that apply to your target devices

### 4. Configure Meta Quest Support
- Click the **gear icon** ⚙️ next to Meta Quest Support
- In the settings window:
  - Check your target Quest device(s)
  - **Optimize Buffer Discards**: Enable (improves performance)
  - **Symmetric Projection**: Usually leave unchecked
  - **Foveated Rendering API**: Can enable for Quest Pro

### 5. Verify Build Settings
- **File > Build Settings > Android**
- Switch platform if needed
- **Player Settings:**
  - Package Name: `com.yourcompany.arcalligraphy`
  - Minimum API: 24
  - Target API: Automatic
  - Scripting Backend: IL2CPP
  - Target Architectures: ARM64

## Understanding the Warnings

### Yellow Warning (⚠️) on Meta Quest Support
- **Usually safe to ignore** if:
  - You've configured the gear settings
  - Build settings are correct
  - Will resolve when building to device

### Orange Warning (⚠️) on Hand Interaction Poses
- **Usually safe to ignore** if:
  - Hand Interaction Profile is also enabled
  - You're targeting Quest 2/3/Pro (which support hand tracking)
  - Warning appears because editor doesn't have hand tracking hardware

## Testing

### In Editor:
- Warnings are normal - editor can't fully test XR features
- Use **Mock Runtime** for basic testing (enable if needed)

### On Device:
- Warnings should disappear when running on Quest
- Hand tracking will work if:
  - Quest device supports it (Quest 2/3/Pro)
  - Hand tracking is enabled in Quest settings
  - App has proper permissions

## Troubleshooting

### "Hand Interaction Profile" not showing?
- Make sure you're viewing **Android** platform tab
- Some features are platform-specific
- Try refreshing Unity (close/reopen project)

### Warnings persist after setup?
- This is normal in the editor
- Build and test on actual device
- Warnings often resolve at runtime

### Hand tracking not working on device?
1. Enable hand tracking in Quest settings:
   - Quest Settings > Hands and Controllers > Hand Tracking
2. Check app permissions
3. Verify both "Hand Interaction Profile" and "Hand Interaction Poses" are enabled

## Quick Checklist

Before hackathon:
- [ ] Meta Quest Support enabled (Android platform)
- [ ] Hand Interaction Profile enabled (Android platform)
- [ ] Hand Interaction Poses enabled (Android platform)
- [ ] Appropriate controller profile(s) enabled:
  - [ ] Meta Quest Touch Pro (Quest Pro)
  - [ ] Meta Quest Touch Plus (Quest 3/3S)
  - [ ] Oculus Touch Controller (Quest 2/older)
- [ ] Meta Quest Support gear settings configured
- [ ] Android build settings correct (IL2CPP, ARM64)
- [ ] Can build APK successfully
- [ ] Tested on Quest device (warnings should be gone)

**For Pico:**
- [ ] Pico SDK package installed
- [ ] Pico SDK enabled in XR Management
- [ ] Pico hand tracking configured
- [ ] Tested on Pico device

## Controller Profile Selection

See `CONTROLLER_PROFILES.md` for detailed information about:
- Oculus Touch Controller Profile (Quest 2)
- Meta Quest Touch Plus Controller Profile (Quest 3/3S)
- Meta Quest Touch Pro Controller Profile (Quest Pro)

**Quick recommendation:** Enable all three profiles for maximum device compatibility.

## Next Steps

1. Enable "Hand Interaction Profile" if not already
2. Configure Meta Quest Support gear settings
3. Enable appropriate controller profile(s) - see `CONTROLLER_PROFILES.md`
4. Verify Android build configuration
5. Build and test on Quest device
6. Warnings should resolve on device

The warnings you see are common and usually resolve when you build and run on the actual Quest device. The important thing is that the features are **enabled** (checked), which they are!

