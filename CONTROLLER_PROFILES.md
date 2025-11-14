# Meta Quest Controller Profiles Guide

## Overview
Meta Quest has different controller types, and Unity OpenXR provides separate profiles for each. Here's which one to use for your device.

## Controller Types

### 1. **Oculus Touch Controller Profile** (Legacy/Standard)
- **For:** Quest 2, Quest 1, and older devices
- **Features:** Standard controllers with thumbsticks, buttons, triggers
- **When to enable:** If targeting Quest 2 or want backward compatibility
- **OpenXR Extension:** Standard OpenXR controller input

### 2. **Meta Quest Touch Plus Controller Profile** (Newer)
- **For:** Quest 3, Quest 3S
- **Features:** Enhanced controllers with improved tracking
- **When to enable:** If targeting Quest 3 or Quest 3S
- **OpenXR Extension:** `XR_META_touch_controller_plus`

### 3. **Meta Quest Touch Pro Controller Profile** (Premium)
- **For:** Quest Pro
- **Features:** Advanced controllers with built-in tracking cameras
- **When to enable:** If targeting Quest Pro specifically
- **OpenXR Extension:** `XR_FB_touch_controller_pro`

## Which One(s) to Enable?

### For Your AR Calligraphy App:

**Option 1: Support All Devices (Recommended)**
Enable all three profiles:
- ✅ Oculus Touch Controller Profile (Quest 2)
- ✅ Meta Quest Touch Plus Controller Profile (Quest 3/3S)
- ✅ Meta Quest Touch Pro Controller Profile (Quest Pro)

Unity will automatically use the correct profile based on the connected device.

**Option 2: Target Specific Device**
Enable only the profile(s) for your target device(s):
- Quest 2 only → Enable "Oculus Touch Controller Profile"
- Quest 3 only → Enable "Meta Quest Touch Plus Controller Profile"
- Quest Pro only → Enable "Meta Quest Touch Pro Controller Profile"
- Multiple devices → Enable all relevant profiles

## Device Compatibility

| Device | Recommended Profile | Also Works With |
|--------|-------------------|-----------------|
| Quest 1 | Oculus Touch Controller | - |
| Quest 2 | Oculus Touch Controller | Touch Plus (partial) |
| Quest 3 | Meta Quest Touch Plus | Oculus Touch (fallback) |
| Quest 3S | Meta Quest Touch Plus | Oculus Touch (fallback) |
| Quest Pro | Meta Quest Touch Pro | Touch Plus (partial) |

## For Your Hackathon

Since you mentioned **Meta Quest** (not specifying which model), I recommend:

**Enable all three profiles** to support:
- Quest 2 (most common)
- Quest 3/3S (newer models)
- Quest Pro (if available)

This gives you maximum compatibility without knowing which specific device will be available.

## Setup Steps

1. **Edit > Project Settings > XR Plug-in Management > OpenXR**
2. Make sure you're on the **Android** platform tab
3. Scroll to find controller profiles
4. Enable the checkbox(es) for:
   - ✅ Oculus Touch Controller Profile
   - ✅ Meta Quest Touch Plus Controller Profile
   - ✅ Meta Quest Touch Pro Controller Profile

## Important Notes

- **Hand tracking is primary** - Controllers are just a fallback
- You can enable multiple profiles - Unity handles device detection
- Enabling all profiles doesn't hurt performance
- If unsure, enable all three for maximum compatibility

## In Your Code

Your `TracingManager.cs` should handle both hand tracking and controllers:

```csharp
// Hand tracking (primary)
if (useHandTracking && handTracked)
{
    return GetHandPosition();
}

// Controller fallback
if (useControllerTracking)
{
    return GetControllerPosition();
}
```

The controller profile you enable determines which controller features are available, but your code can work with any of them.

## Quick Decision Guide

**Don't know which device?** → Enable all three profiles ✅

**Know your device?**
- Quest 2 → Enable "Oculus Touch Controller Profile"
- Quest 3 → Enable "Meta Quest Touch Plus Controller Profile"  
- Quest Pro → Enable "Meta Quest Touch Pro Controller Profile"

**Supporting multiple?** → Enable all relevant profiles ✅

