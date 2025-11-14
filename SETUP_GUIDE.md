# AR Calligraphy Tracing App - Setup Guide

## Overview
This guide will help you set up your AR calligraphy tracing app for Meta Quest or Pico Ultra devices using Unity 6.2.

## Prerequisites

### Hardware
- **Meta Quest 2/3/Pro** OR **Pico 4/Ultra**
- USB-C cable for device connection
- Development PC (Windows recommended)

### Software
- **Unity 6.2** (or Unity 6.0.2+)
- **Android SDK** (via Unity Hub)
- **Android NDK** (via Unity Hub)
- **OpenJDK** (via Unity Hub)
- **Meta Quest Developer Hub** (if using Quest) - Download from Meta Developer Portal
- **Pico SDK** (if using Pico) - Download from Pico Developer Portal

### Accounts & API Keys
- **OpenAI API Key** (or alternative: Anthropic Claude, Google Gemini)
  - Sign up at: https://platform.openai.com/
  - Get API key from: https://platform.openai.com/api-keys
- **GitHub account** (for version control)

## Step 1: Unity Project Setup

### 1.1 Install Required Unity Packages

The following packages are already installed via `Packages/manifest.json`:
- ✅ XR Management (4.5.3)
- ✅ OpenXR (1.15.1)
- ✅ Input System (1.14.2)
- ✅ Universal Render Pipeline (17.2.0)

### 1.2 Enable OpenXR Features

1. Open **Edit > Project Settings > XR Plug-in Management > OpenXR**
2. For **Android** platform:
   - Enable **Meta Quest Support** feature
   - Enable **Hand Interaction Profile** (for hand tracking)
   - Enable **Oculus Touch Controller Profile** (for controllers)
3. For **Pico** (if using):
   - Enable **Pico SDK** (install Pico SDK package separately)
   - Enable hand tracking features

### 1.3 Configure Build Settings

1. **File > Build Settings**
2. Select **Android** platform
3. Click **Switch Platform**
4. **Player Settings**:
   - **Minimum API Level**: Android 7.0 (API 24) or higher
   - **Target API Level**: Automatic (highest installed)
   - **Scripting Backend**: IL2CPP
   - **Target Architectures**: ARM64 (required for Quest/Pico)

## Step 2: Gen AI Integration Setup

### 2.1 Choose Your Gen AI Provider

**Option A: OpenAI (Recommended)**
- Pros: Best quality, reliable
- Cons: Requires API key, costs per request
- Setup: Use `OpenAIService.cs` script

**Option B: Anthropic Claude**
- Pros: Good quality, competitive pricing
- Cons: Requires API key
- Setup: Use `ClaudeService.cs` script

**Option C: Google Gemini**
- Pros: Free tier available
- Cons: May have rate limits
- Setup: Use `GeminiService.cs` script

### 2.2 API Key Configuration

1. Create a file: `Assets/Resources/Config/api_config.json`
2. Add your API key (DO NOT commit this file - it's in .gitignore):
```json
{
  "openai_api_key": "sk-your-key-here",
  "provider": "openai"
}
```

**Security Note**: Never commit API keys to GitHub. Use environment variables or secure storage in production.

### 2.3 Gen AI Prompting Strategy

The app will use prompts like:
```
"Convert the English name '{userName}' into appropriate Japanese kanji characters. 
Return only the kanji characters, no explanation."
```

For style variations:
```
"Generate kanji for '{userName}' in a {style} calligraphy style: 
{smooth/aggressive/powerful/abstract/artistic}"
```

## Step 3: Hand Tracking Setup

### 3.1 Enable Hand Tracking

**For Meta Quest:**
1. Project Settings > XR Plug-in Management > OpenXR
2. Enable **Hand Interaction Profile**
3. Enable **Hand Tracking** in Quest settings

**For Pico:**
1. Install Pico SDK package
2. Enable hand tracking in Pico SDK settings

### 3.2 Hand Tracking Implementation

The app uses:
- **Unity XR Hands** package (if available)
- **OpenXR Hand Tracking** (via OpenXR features)
- Custom hand pose detection for tracing gestures

## Step 4: Character Recognition Setup

### 4.1 Recognition Approach

**Option A: Computer Vision (Recommended)**
- Use Unity's **Barracuda** package for on-device ML
- Train a simple CNN for kanji recognition
- Pros: Fast, works offline
- Cons: Requires model training

**Option B: Cloud API**
- Use Google Cloud Vision API or Azure Computer Vision
- Pros: High accuracy, no training needed
- Cons: Requires internet, API costs

**Option C: Template Matching**
- Compare traced strokes to reference templates
- Pros: Simple, fast
- Cons: Less accurate for variations

### 4.2 Scoring Algorithm

The scoring system evaluates:
1. **Stroke accuracy** (how close to reference)
2. **Stroke order** (correct sequence)
3. **Proportions** (character balance)
4. **Smoothness** (stroke flow)

## Step 5: Project Structure

```
Assets/
├── Scripts/
│   ├── Core/
│   │   ├── AppManager.cs
│   │   ├── GameMode.cs
│   │   └── CalligraphyStyle.cs
│   ├── AI/
│   │   ├── OpenAIService.cs
│   │   ├── AIServiceBase.cs
│   │   └── PromptBuilder.cs
│   ├── Tracing/
│   │   ├── HandTracker.cs
│   │   ├── StrokeRecorder.cs
│   │   └── TracingManager.cs
│   ├── Recognition/
│   │   ├── CharacterRecognizer.cs
│   │   ├── ScoringSystem.cs
│   │   └── TemplateMatcher.cs
│   └── UI/
│       ├── UIManager.cs
│       ├── ModeSelectionUI.cs
│       └── ScoreDisplayUI.cs
├── Prefabs/
│   ├── Hand/
│   ├── UI/
│   └── Tracing/
├── Materials/
│   └── ShujiStyles/
├── Resources/
│   ├── Config/
│   └── ShujiSamples/
└── Scenes/
    ├── MainMenu.unity
    └── TracingScene.unity
```

## Step 6: Pre-Hackathon Checklist

### Development Environment
- [ ] Unity 6.2 installed and working
- [ ] Android SDK/NDK installed via Unity Hub
- [ ] Device connected and recognized (Quest/Pico)
- [ ] Can build and deploy test app to device

### API Setup
- [ ] Gen AI API key obtained (OpenAI/Claude/Gemini)
- [ ] API key stored securely (not in git)
- [ ] Test API call successful

### Assets
- [ ] Shuji character set imported
- [ ] UI mockups/prefabs ready
- [ ] Materials for different styles prepared

### Testing
- [ ] Hand tracking works on device
- [ ] Basic tracing functionality works
- [ ] API integration tested
- [ ] Build process verified

## Step 7: Platform-Specific Setup

### Meta Quest Setup (NOT Oculus - that's the old name)

1. **Enable Developer Mode:**
   - Install **Meta Quest Developer Hub** (not Oculus)
   - Enable Developer Mode on Quest device
   - Register device in Developer Hub

2. **Build Configuration:**
   - Project Settings > Player > Android
   - Package Name: `com.yourcompany.arcalligraphy`
   - Minimum API: 24
   - Target API: Automatic

3. **Quest-Specific Features:**
   - Hand tracking (Quest 2/3/Pro)
   - Passthrough (for AR overlay)
   - Controller support

### Pico Setup

1. **Install Pico SDK:**
   - Download from Pico Developer Portal: https://developer.pico-interactive.com/
   - Import Pico SDK Unity package
   - **Important:** Pico requires separate SDK (not just OpenXR)
   - Configure in XR Management
   - See `PICO_SETUP.md` for detailed instructions

2. **Build Configuration:**
   - Similar to Quest setup
   - Enable Pico-specific features

## Step 8: Development Workflow

### Daily Development
1. Test in Unity Editor (using Mock Runtime)
2. Build to device for testing
3. Iterate on features
4. Commit to git regularly

### API Key Management
- Use `api_config.json` for development
- Use environment variables for production
- Never commit keys to git

### Version Control
- Commit code changes
- Don't commit: Library/, Temp/, Logs/, UserSettings/
- Don't commit API keys or sensitive data

## Troubleshooting

### Hand Tracking Not Working
- Check OpenXR settings
- Verify device supports hand tracking
- Enable hand tracking in device settings

### Build Fails
- Check Android SDK installation
- Verify minimum API level
- Check IL2CPP backend selected

### API Calls Failing
- Verify API key is correct
- Check internet connection
- Review API rate limits

### Performance Issues
- Reduce render quality
- Optimize shaders
- Use foveated rendering (Quest)

## Resources

- [Unity XR Documentation](https://docs.unity3d.com/Manual/XR.html)
- [OpenXR Unity Guide](https://docs.unity3d.com/Packages/com.unity.xr.openxr@latest)
- [Meta Quest Developer Docs](https://developer.oculus.com/) (Note: URL still says "oculus" but it's for Meta Quest)
- [Pico Developer Portal](https://developer.pico-interactive.com/)
- [OpenAI API Docs](https://platform.openai.com/docs)

## Next Steps

1. Review the project structure
2. Set up your API keys
3. Test hand tracking on device
4. Start implementing core features
5. Build and test iteratively

Good luck with your hackathon! 🚀

