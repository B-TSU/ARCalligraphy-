using UnityEngine;

namespace ARCalligraphy.Core
{
    /// <summary>
    /// Enumeration of available game modes
    /// </summary>
    public enum GameMode
    {
        SampleWords,    // Use pre-defined Japanese Shuji words
        CustomName       // User inputs name, AI generates kanji
    }

    /// <summary>
    /// Enumeration of calligraphy styles
    /// </summary>
    public enum CalligraphyStyle
    {
        Smooth,      // 滑らか (Smooth, flowing)
        Aggressive,  // 激しい (Aggressive, bold)
        Powerful,    // 力強い (Powerful, strong)
        Abstract,    // 抽象的 (Abstract, artistic)
        Artistic     // 芸術的 (Artistic, creative)
    }

    /// <summary>
    /// Core game mode and style management
    /// </summary>
    public class GameModeManager : MonoBehaviour
    {
        [Header("Current Settings")]
        [SerializeField] private GameMode currentMode = GameMode.SampleWords;
        [SerializeField] private CalligraphyStyle currentStyle = CalligraphyStyle.Smooth;
        [SerializeField] private string currentWord = "";
        [SerializeField] private string currentKanji = "";

        // Events
        public System.Action<GameMode> OnModeChanged;
        public System.Action<CalligraphyStyle> OnStyleChanged;
        public System.Action<string> OnWordChanged;

        // Properties
        public GameMode CurrentMode
        {
            get => currentMode;
            set
            {
                if (currentMode != value)
                {
                    currentMode = value;
                    OnModeChanged?.Invoke(currentMode);
                }
            }
        }

        public CalligraphyStyle CurrentStyle
        {
            get => currentStyle;
            set
            {
                if (currentStyle != value)
                {
                    currentStyle = value;
                    OnStyleChanged?.Invoke(currentStyle);
                }
            }
        }

        public string CurrentWord
        {
            get => currentWord;
            set
            {
                if (currentWord != value)
                {
                    currentWord = value;
                    OnWordChanged?.Invoke(currentWord);
                }
            }
        }

        public string CurrentKanji
        {
            get => currentKanji;
            set => currentKanji = value;
        }

        private void Awake()
        {
            // Initialize with default values
            if (string.IsNullOrEmpty(currentWord))
            {
                currentWord = "愛"; // Default: "Love" in kanji
                currentKanji = currentWord;
            }
        }

        /// <summary>
        /// Get style description in Japanese
        /// </summary>
        public string GetStyleDescription(CalligraphyStyle style)
        {
            return style switch
            {
                CalligraphyStyle.Smooth => "滑らか",
                CalligraphyStyle.Aggressive => "激しい",
                CalligraphyStyle.Powerful => "力強い",
                CalligraphyStyle.Abstract => "抽象的",
                CalligraphyStyle.Artistic => "芸術的",
                _ => "標準"
            };
        }

        /// <summary>
        /// Get style description in English
        /// </summary>
        public string GetStyleDescriptionEnglish(CalligraphyStyle style)
        {
            return style switch
            {
                CalligraphyStyle.Smooth => "Smooth",
                CalligraphyStyle.Aggressive => "Aggressive",
                CalligraphyStyle.Powerful => "Powerful",
                CalligraphyStyle.Abstract => "Abstract",
                CalligraphyStyle.Artistic => "Artistic",
                _ => "Standard"
            };
        }
    }
}

