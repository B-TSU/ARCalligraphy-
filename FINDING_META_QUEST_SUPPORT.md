# How to Find and Enable Meta Quest Support

## Where to Find Meta Quest Support

Meta Quest Support should appear in the **OpenXR Feature Groups** list. Here's exactly where to look:

## Step-by-Step Location Guide

### 1. Open Project Settings
- **Edit > Project Settings** (or press `Ctrl+Shift+,` on Windows)

### 2. Navigate to OpenXR
- In the left sidebar, click **XR Plug-in Management**
- Then click **OpenXR** (it's a sub-item under XR Plug-in Management)

### 3. Select Android Platform Tab
- At the top of the OpenXR settings window, you'll see tabs:
  - **Standalone** (for PC/Editor)
  - **Android** ← **Click this one!**
  - **WebGL** (if available)
  
**Important:** Meta Quest Support only appears in the **Android** tab, not Standalone!

### 4. Scroll to "OpenXR Feature Groups"
- Look for a section called **"OpenXR Feature Groups"**
- This is a list/table of features with checkboxes

### 5. Find "Meta Quest Support"
- Scroll through the list of features
- Look for **"Meta Quest Support"** (should be alphabetically listed)
- It should have:
  - A checkbox (currently unchecked if you don't see it enabled)
  - A gear icon ⚙️ (for settings)
  - Possibly a warning icon ⚠️

## If You Don't See It

### Check 1: Are you on the Android tab?
- Meta Quest Support **only appears** in the **Android** platform tab
- Switch from "Standalone" to "Android" at the top

### Check 2: Is OpenXR installed?
- Make sure **OpenXR** is enabled in **XR Plug-in Management > Plug-in Providers**
- If not, enable it first

### Check 3: Refresh Unity
- Sometimes Unity needs a refresh
- Try: **Assets > Refresh** (or press `Ctrl+R`)
- Or close and reopen the Project Settings window

### Check 4: Check Package Manager
- **Window > Package Manager**
- Filter: **In Project**
- Look for **"OpenXR"** package
- Make sure it's installed (version 1.15.1 or similar)

## Visual Guide

```
Unity Editor
└── Edit > Project Settings
    └── XR Plug-in Management
        └── OpenXR
            ├── [Standalone Tab] ← Don't use this
            ├── [Android Tab] ← USE THIS ONE!
            │   └── OpenXR Feature Groups
            │       ├── Application SpaceWarp
            │       ├── D-Pad Binding
            │       ├── Foveated Rendering
            │       ├── Hand Interaction Poses
            │       ├── Hand Interaction Profile
            │       ├── Meta Quest Support ← HERE!
            │       ├── Mock Runtime
            │       └── ... (other features)
            └── [WebGL Tab]
```

## What It Should Look Like

When you find it, you should see:

```
☐ Meta Quest Support  ⚙️  ⚠️
```

- ☐ = Checkbox (unchecked)
- ⚙️ = Gear icon (click to configure)
- ⚠️ = Warning icon (may or may not appear)

## Enable It

1. **Check the checkbox** next to "Meta Quest Support"
2. **Click the gear icon** ⚙️ to configure:
   - Select your target Quest devices (Quest 2, Quest 3, etc.)
   - Enable "Optimize Buffer Discards"
3. The warning should go away or become less critical

## Alternative: Search Feature

If you can't find it by scrolling:

1. In the OpenXR settings window, look for a **search box**
2. Type: `Meta Quest` or `metaquest`
3. It should filter to show only matching features

## Still Can't Find It?

If Meta Quest Support is completely missing:

1. **Check OpenXR Package Version:**
   - Window > Package Manager
   - Find "OpenXR" package
   - Make sure it's version 1.15.1 or newer
   - Older versions might not have Meta Quest Support

2. **Reimport OpenXR:**
   - Package Manager > OpenXR > Remove
   - Then re-add it via Package Manager

3. **Check Unity Version:**
   - Meta Quest Support requires Unity 2021.2 or newer
   - You're using Unity 6.0.2, so this should be fine

## Quick Checklist

- [ ] Opened **Edit > Project Settings**
- [ ] Clicked **XR Plug-in Management**
- [ ] Clicked **OpenXR** (sub-item)
- [ ] Selected **Android** tab (not Standalone)
- [ ] Scrolled to **OpenXR Feature Groups** section
- [ ] Found **Meta Quest Support** in the list
- [ ] Checked the checkbox to enable it

## Current Status in Your Project

Based on your project files, Meta Quest Support exists but is **currently disabled** (`m_enabled: 0`). You just need to find it in the Unity UI and enable it!

The feature is there - it's just a matter of navigating to the right place in Unity's interface.

