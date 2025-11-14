# Scene Setup Guide - Pre-Hackathon Preparation

## Overview
This guide covers the essential Unity scenes, prefabs, and setup you should prepare before the hackathon to save time during development.

## Essential Scenes to Create

### 1. Main Menu Scene
**Purpose:** Entry point, mode selection, settings

**Setup:**
- Create: `Assets/Scenes/MainMenu.unity`
- Add XR Origin (for VR camera setup)
- Add UI Canvas (World Space for VR)
- Add UI elements:
  - Title text
  - "Start" button
  - "Settings" button (optional)
- Add `AppManager` GameObject with `AppManager` script
- Add `UIManager` GameObject with `UIManager` script
- Add `GameModeManager` GameObject with `GameModeManager` script

### 2. Tracing Scene (Main Game Scene)
**Purpose:** Where users trace calligraphy

**Setup:**
- Create: `Assets/Scenes/TracingScene.unity`
- Add XR Origin (VR camera rig)
- Add Tracing Plane (where users trace)
- Add UI Canvas (for instructions, score display)
- Add all manager scripts
- Add hand tracking setup
- Add lighting (important for VR)

## Detailed Scene Setup

### Scene 1: MainMenu.unity

#### Step 1: XR Origin Setup
1. **Delete** default Main Camera
2. Add **XR Origin**:
   - Right-click in Hierarchy > **XR > XR Origin (VR)**
   - This provides:
     - Camera (head tracking)
     - Left/Right Controllers (if using controllers)
     - Hand tracking setup

#### Step 2: UI Setup
1. Create **Canvas**:
   - Right-click > **UI > Canvas**
   - Set **Render Mode** to **"World Space"** (for VR)
   - Position in front of camera (e.g., Z = 2, Scale = 0.001)
   - Add **Canvas Scaler** component
   - Add **Graphic Raycaster** component

2. Add **UI Elements**:
   - **Title Text**: "AR Calligraphy"
   - **Start Button**: Triggers mode selection
   - **Settings Button** (optional)

3. Attach **UIManager** script to Canvas or separate GameObject

#### Step 3: Manager Setup
Create empty GameObjects and add scripts:
- **AppManager** (with `AppManager.cs`)
- **GameModeManager** (with `GameModeManager.cs`)
- **UIManager** (with `UIManager.cs`)

#### Step 4: Scene Settings
- **Lighting**: Add Directional Light
- **Skybox**: Can use default or custom
- **Audio Listener**: XR Origin camera has this automatically

### Scene 2: TracingScene.unity

#### Step 1: XR Origin Setup
Same as MainMenu - add XR Origin (VR)

#### Step 2: Tracing Plane
1. Create **Plane** GameObject:
   - Right-click > **3D Object > Plane**
   - Rename to "TracingPlane"
   - Position: Y = 1.2 (eye level), Z = 1.5 (in front of user)
   - Scale: 1x1 or larger (adjust for visibility)
   - Material: Semi-transparent or with grid pattern

2. Add **TracingManager** script:
   - Attach `TracingManager.cs` to TracingPlane
   - Assign TracingPlane transform in inspector

#### Step 3: Reference Kanji Display
1. Create **Text** or **Image** showing target kanji:
   - Use TextMeshPro for kanji display
   - Position above or beside tracing plane
   - Shows what user should trace

2. Create **KanjiDisplay** GameObject:
   - Add TextMeshPro component
   - Set font to support Japanese characters
   - Connect to `GameModeManager` to show current kanji

#### Step 4: UI Canvas
1. Create **Canvas** (World Space):
   - Position: In front of user
   - Add UI elements:
     - Current word/kanji display
     - Style indicator
     - "Finish Tracing" button
     - Score display (hidden until end)

#### Step 5: Manager Setup
Add all manager scripts:
- **AppManager**
- **GameModeManager**
- **TracingManager** (on TracingPlane)
- **CharacterRecognizer**
- **ScoringSystem**
- **UIManager**
- **AIServiceBase** (or OpenAIService)

#### Step 6: Hand Tracking Setup
1. **XR Origin** already has hand tracking support
2. Verify in **XR Origin**:
   - Left Hand Controller
   - Right Hand Controller
   - These work for both controllers and hand tracking

#### Step 7: Lighting
- **Directional Light**: Main scene light
- **Ambient Light**: Settings > Rendering > Ambient
- **Reflection Probes**: Optional, for better visuals

## Prefab Setup

### Create These Prefabs Beforehand

#### 1. Stroke Prefab
**Purpose:** Visual representation of traced strokes

**Create:**
- `Assets/Prefabs/Tracing/StrokePrefab.prefab`
- Use **LineRenderer** component
- Material: Calligraphy brush texture
- Width: Adjustable based on style

**Properties:**
- Material (different for each style)
- Width curve (for brush effect)
- Color (black ink, or style-based)

#### 2. UI Panel Prefabs
**Purpose:** Reusable UI elements

**Create:**
- `Assets/Prefabs/UI/MainMenuPanel.prefab`
- `Assets/Prefabs/UI/ModeSelectionPanel.prefab`
- `Assets/Prefabs/UI/TracingPanel.prefab`
- `Assets/Prefabs/UI/ScorePanel.prefab`
- `Assets/Prefabs/UI/LoadingPanel.prefab`

Each panel should have:
- Background
- Text elements
- Buttons
- Proper layout

#### 3. Hand Indicator Prefab (Optional)
**Purpose:** Visual indicator of hand position

**Create:**
- `Assets/Prefabs/Hand/HandIndicator.prefab`
- Simple sphere or custom hand model
- Follows hand position during tracing

## Material Setup

### Create Calligraphy Materials

**Location:** `Assets/Materials/ShujiStyles/`

Create materials for each style:
1. **Smooth** - Thin, flowing lines
2. **Aggressive** - Thick, bold strokes
3. **Powerful** - Medium, strong lines
4. **Abstract** - Varied, artistic
5. **Artistic** - Creative, expressive

**Properties:**
- Shader: URP/Lit or Unlit
- Color: Black (ink color)
- Texture: Brush texture (optional)
- Emission: Optional glow effect

## Sample Data Setup

### Shuji Sample Words

**Location:** `Assets/Resources/ShujiSamples/`

Create a scriptable object or JSON file with sample kanji:
```json
{
  "samples": [
    {"kanji": "愛", "meaning": "Love"},
    {"kanji": "美", "meaning": "Beauty"},
    {"kanji": "和", "meaning": "Harmony"},
    {"kanji": "夢", "meaning": "Dream"},
    {"kanji": "力", "meaning": "Power"}
  ]
}
```

## Scene Hierarchy Template

### MainMenu.unity
```
MainMenu
├── XR Origin (VR)
│   ├── Camera Offset
│   │   └── Main Camera
│   ├── Left Controller
│   └── Right Controller
├── Canvas (World Space)
│   ├── Title
│   ├── StartButton
│   └── SettingsButton
├── AppManager (GameObject)
│   └── AppManager (Script)
├── GameModeManager (GameObject)
│   └── GameModeManager (Script)
└── UIManager (GameObject)
    └── UIManager (Script)
```

### TracingScene.unity
```
TracingScene
├── XR Origin (VR)
│   ├── Camera Offset
│   │   └── Main Camera
│   ├── Left Controller
│   └── Right Controller
├── TracingPlane
│   ├── MeshRenderer
│   ├── MeshFilter
│   └── TracingManager (Script)
├── KanjiDisplay (GameObject)
│   └── TextMeshPro (Script)
├── Canvas (World Space)
│   ├── CurrentWordText
│   ├── StyleText
│   ├── FinishButton
│   └── ScorePanel (initially hidden)
├── AppManager (GameObject)
│   └── AppManager (Script)
├── GameModeManager (GameObject)
│   └── GameModeManager (Script)
├── CharacterRecognizer (GameObject)
│   └── CharacterRecognizer (Script)
├── ScoringSystem (GameObject)
│   └── ScoringSystem (Script)
├── UIManager (GameObject)
│   └── UIManager (Script)
├── OpenAIService (GameObject)
│   └── OpenAIService (Script)
└── Directional Light
```

## Pre-Hackathon Checklist

### Scenes
- [ ] MainMenu.unity created and configured
- [ ] TracingScene.unity created and configured
- [ ] Both scenes have XR Origin setup
- [ ] Both scenes have proper lighting
- [ ] Scene transitions work (can switch between scenes)

### Prefabs
- [ ] StrokePrefab created with LineRenderer
- [ ] UI Panel prefabs created
- [ ] Hand indicator prefab (optional)

### Materials
- [ ] Calligraphy materials created for each style
- [ ] Materials assigned to stroke prefab variants

### Scripts Setup
- [ ] All manager scripts attached to GameObjects
- [ ] Script references connected in Inspector
- [ ] No missing script references

### UI Setup
- [ ] Canvas configured (World Space)
- [ ] TextMeshPro imported and configured
- [ ] Buttons wired up to UIManager
- [ ] UI panels can show/hide properly

### Testing
- [ ] Can enter VR mode in editor (Mock Runtime)
- [ ] Scenes load without errors
- [ ] UI is visible and positioned correctly
- [ ] Basic interactions work

## Quick Setup Script

Create a helper script to quickly set up a scene:

```csharp
// Assets/Scripts/Editor/SceneSetupHelper.cs
// Place in Editor folder
using UnityEngine;
using UnityEditor;

public class SceneSetupHelper : EditorWindow
{
    [MenuItem("Tools/Setup Tracing Scene")]
    static void SetupTracingScene()
    {
        // Auto-create XR Origin, managers, etc.
        // This can save time during hackathon
    }
}
```

## Tips for Hackathon

1. **Prepare Basic Scenes First**
   - Get MainMenu and TracingScene working with basic setup
   - You can refine during hackathon

2. **Use Prefabs**
   - Create prefabs for reusable elements
   - Makes iteration faster

3. **Test in Editor First**
   - Use Mock Runtime to test without device
   - Fix major issues before building to device

4. **Keep It Simple**
   - Don't over-engineer before hackathon
   - Focus on core functionality
   - Polish can come later

5. **Document Your Setup**
   - Note any custom configurations
   - Helps if you need to recreate

## Common Issues to Avoid

- ❌ Forgetting to delete default Main Camera (conflicts with XR Origin)
- ❌ Using Screen Space Canvas instead of World Space (won't work in VR)
- ❌ Not setting up XR Origin properly
- ❌ Missing script references
- ❌ Forgetting to assign prefabs in Inspector

## Next Steps

1. Create the two main scenes (MainMenu, TracingScene)
2. Set up XR Origin in both
3. Create basic UI structure
4. Attach manager scripts
5. Test scene loading
6. Build and test on device

You're ready for the hackathon! 🚀

