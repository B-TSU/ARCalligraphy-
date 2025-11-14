using UnityEngine;
using ARCalligraphy.Core;
using ARCalligraphy.AI;
using ARCalligraphy.Tracing;
using ARCalligraphy.Recognition;
using ARCalligraphy.UI;

namespace ARCalligraphy.Core
{
    /// <summary>
    /// Main application manager that coordinates all systems
    /// </summary>
    public class AppManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private GameModeManager gameModeManager;
        [SerializeField] private TracingManager tracingManager;
        [SerializeField] private CharacterRecognizer characterRecognizer;
        [SerializeField] private ScoringSystem scoringSystem;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private AIServiceBase aiService;

        [Header("Settings")]
        [SerializeField] private bool enableHandTracking = true;
        [SerializeField] private bool enableControllerSupport = true;

        // Singleton instance
        public static AppManager Instance { get; private set; }

        // State
        private bool isTracing = false;
        private bool isProcessing = false;

        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // Initialize managers
            InitializeManagers();
        }

        private void Start()
        {
            // Subscribe to events
            if (gameModeManager != null)
            {
                gameModeManager.OnModeChanged += HandleModeChanged;
                gameModeManager.OnStyleChanged += HandleStyleChanged;
                gameModeManager.OnWordChanged += HandleWordChanged;
            }

            // Initialize UI
            if (uiManager != null)
            {
                uiManager.ShowMainMenu();
            }
        }

        private void InitializeManagers()
        {
            // Auto-find managers if not assigned
            if (gameModeManager == null)
                gameModeManager = FindObjectOfType<GameModeManager>();

            if (tracingManager == null)
                tracingManager = FindObjectOfType<TracingManager>();

            if (characterRecognizer == null)
                characterRecognizer = FindObjectOfType<CharacterRecognizer>();

            if (scoringSystem == null)
                scoringSystem = FindObjectOfType<ScoringSystem>();

            if (uiManager == null)
                uiManager = FindObjectOfType<UIManager>();

            if (aiService == null)
                aiService = FindObjectOfType<AIServiceBase>();
        }

        #region Event Handlers

        private void HandleModeChanged(GameMode mode)
        {
            Debug.Log($"Game mode changed to: {mode}");
            // Handle mode change logic
        }

        private void HandleStyleChanged(CalligraphyStyle style)
        {
            Debug.Log($"Style changed to: {style}");
            // Update tracing style
            if (tracingManager != null)
            {
                tracingManager.SetStyle(style);
            }
        }

        private void HandleWordChanged(string word)
        {
            Debug.Log($"Word changed to: {word}");
            // Update UI and tracing target
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start tracing session
        /// </summary>
        public void StartTracing()
        {
            if (isTracing || isProcessing) return;

            isTracing = true;
            tracingManager?.StartTracing();
            uiManager?.ShowTracingUI();
        }

        /// <summary>
        /// End tracing session and evaluate
        /// </summary>
        public async void EndTracing()
        {
            if (!isTracing) return;

            isTracing = false;
            isProcessing = true;

            // Get traced strokes
            var strokes = tracingManager?.EndTracing();
            if (strokes == null || strokes.Count == 0)
            {
                isProcessing = false;
                return;
            }

            // Recognize character
            string recognizedCharacter = characterRecognizer?.Recognize(strokes);
            
            // Score the tracing
            float score = 0f;
            if (scoringSystem != null && !string.IsNullOrEmpty(recognizedCharacter))
            {
                string targetKanji = gameModeManager?.CurrentKanji ?? "";
                score = scoringSystem.CalculateScore(strokes, targetKanji, recognizedCharacter);
            }

            // Show results
            uiManager?.ShowScoreResults(score, recognizedCharacter);

            isProcessing = false;
        }

        /// <summary>
        /// Generate kanji from English name using AI
        /// </summary>
        public async void GenerateKanjiFromName(string englishName)
        {
            if (isProcessing || aiService == null) return;

            isProcessing = true;
            uiManager?.ShowLoading("Generating kanji...");

            try
            {
                CalligraphyStyle style = gameModeManager?.CurrentStyle ?? CalligraphyStyle.Smooth;
                string kanji = await aiService.GenerateKanjiFromName(englishName, style);

                if (!string.IsNullOrEmpty(kanji))
                {
                    gameModeManager.CurrentKanji = kanji;
                    gameModeManager.CurrentWord = englishName;
                    uiManager?.ShowKanjiResult(kanji);
                }
                else
                {
                    uiManager?.ShowError("Failed to generate kanji. Please try again.");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error generating kanji: {e.Message}");
                uiManager?.ShowError($"Error: {e.Message}");
            }
            finally
            {
                isProcessing = false;
                uiManager?.HideLoading();
            }
        }

        /// <summary>
        /// Select a sample word
        /// </summary>
        public void SelectSampleWord(string kanji)
        {
            gameModeManager.CurrentKanji = kanji;
            gameModeManager.CurrentWord = kanji;
            gameModeManager.CurrentMode = GameMode.SampleWords;
        }

        #endregion

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (gameModeManager != null)
            {
                gameModeManager.OnModeChanged -= HandleModeChanged;
                gameModeManager.OnStyleChanged -= HandleStyleChanged;
                gameModeManager.OnWordChanged -= HandleWordChanged;
            }
        }
    }
}

