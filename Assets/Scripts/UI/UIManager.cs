using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ARCalligraphy.Core;

namespace ARCalligraphy.UI
{
    /// <summary>
    /// Manages all UI elements and screens
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject modeSelectionPanel;
        [SerializeField] private GameObject tracingPanel;
        [SerializeField] private GameObject scorePanel;
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private GameObject errorPanel;

        [Header("Main Menu UI")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingsButton;

        [Header("Mode Selection UI")]
        [SerializeField] private Button sampleWordsButton;
        [SerializeField] private Button customNameButton;
        [SerializeField] private TMP_Dropdown styleDropdown;

        [Header("Tracing UI")]
        [SerializeField] private TextMeshProUGUI currentWordText;
        [SerializeField] private TextMeshProUGUI styleText;
        [SerializeField] private Button finishTracingButton;

        [Header("Score UI")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private TextMeshProUGUI recognizedCharacterText;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button mainMenuButton;

        [Header("Loading UI")]
        [SerializeField] private TextMeshProUGUI loadingText;

        [Header("Error UI")]
        [SerializeField] private TextMeshProUGUI errorText;
        [SerializeField] private Button errorCloseButton;

        private void Awake()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Hide all panels initially
            HideAllPanels();
            
            // Show main menu by default
            ShowMainMenu();

            // Setup button listeners
            if (startButton != null)
                startButton.onClick.AddListener(OnStartClicked);

            if (sampleWordsButton != null)
                sampleWordsButton.onClick.AddListener(OnSampleWordsClicked);

            if (customNameButton != null)
                customNameButton.onClick.AddListener(OnCustomNameClicked);

            if (finishTracingButton != null)
                finishTracingButton.onClick.AddListener(OnFinishTracingClicked);

            if (retryButton != null)
                retryButton.onClick.AddListener(OnRetryClicked);

            if (mainMenuButton != null)
                mainMenuButton.onClick.AddListener(ShowMainMenu);

            if (errorCloseButton != null)
                errorCloseButton.onClick.AddListener(HideError);

            // Setup style dropdown
            if (styleDropdown != null)
            {
                styleDropdown.ClearOptions();
                styleDropdown.AddOptions(new System.Collections.Generic.List<string>
                {
                    "Smooth", "Aggressive", "Powerful", "Abstract", "Artistic"
                });
                styleDropdown.onValueChanged.AddListener(OnStyleChanged);
            }
        }

        #region Panel Management

        public void ShowMainMenu()
        {
            HideAllPanels();
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(true);
        }

        public void ShowModeSelection()
        {
            HideAllPanels();
            if (modeSelectionPanel != null)
                modeSelectionPanel.SetActive(true);
        }

        public void ShowTracingUI()
        {
            HideAllPanels();
            if (tracingPanel != null)
                tracingPanel.SetActive(true);
        }

        public void ShowScoreResults(float score, string recognizedCharacter)
        {
            HideAllPanels();
            if (scorePanel != null)
            {
                scorePanel.SetActive(true);
                UpdateScoreDisplay(score, recognizedCharacter);
            }
        }

        public void ShowLoading(string message = "Loading...")
        {
            if (loadingPanel != null)
            {
                loadingPanel.SetActive(true);
                if (loadingText != null)
                    loadingText.text = message;
            }
        }

        public void HideLoading()
        {
            if (loadingPanel != null)
                loadingPanel.SetActive(false);
        }

        public void ShowError(string message)
        {
            if (errorPanel != null)
            {
                errorPanel.SetActive(true);
                if (errorText != null)
                    errorText.text = message;
            }
        }

        public void HideError()
        {
            if (errorPanel != null)
                errorPanel.SetActive(false);
        }

        public void ShowKanjiResult(string kanji)
        {
            // Show the generated kanji result
            // This could be a separate panel or update the tracing UI
            if (currentWordText != null)
                currentWordText.text = kanji;
        }

        private void HideAllPanels()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (modeSelectionPanel != null) modeSelectionPanel.SetActive(false);
            if (tracingPanel != null) tracingPanel.SetActive(false);
            if (scorePanel != null) scorePanel.SetActive(false);
            if (loadingPanel != null) loadingPanel.SetActive(false);
            if (errorPanel != null) errorPanel.SetActive(false);
        }

        #endregion

        #region UI Updates

        public void UpdateCurrentWord(string word)
        {
            if (currentWordText != null)
                currentWordText.text = word;
        }

        public void UpdateStyle(CalligraphyStyle style)
        {
            if (styleText != null)
            {
                var manager = FindObjectOfType<GameModeManager>();
                if (manager != null)
                    styleText.text = manager.GetStyleDescriptionEnglish(style);
            }
        }

        private void UpdateScoreDisplay(float score, string recognizedCharacter)
        {
            if (scoreText != null)
            {
                int percentage = Mathf.RoundToInt(score * 100f);
                scoreText.text = $"Score: {percentage}%";
            }

            if (recognizedCharacterText != null)
            {
                recognizedCharacterText.text = $"Recognized: {recognizedCharacter}";
            }

            if (feedbackText != null)
            {
                var scoringSystem = FindObjectOfType<Recognition.ScoringSystem>();
                if (scoringSystem != null)
                    feedbackText.text = scoringSystem.GetScoreFeedback(score);
            }
        }

        #endregion

        #region Button Handlers

        private void OnStartClicked()
        {
            ShowModeSelection();
        }

        private void OnSampleWordsClicked()
        {
            var appManager = Core.AppManager.Instance;
            if (appManager != null)
            {
                var gameModeManager = FindObjectOfType<GameModeManager>();
                if (gameModeManager != null)
                {
                    gameModeManager.CurrentMode = GameMode.SampleWords;
                }
            }
            ShowTracingUI();
        }

        private void OnCustomNameClicked()
        {
            // Show name input UI (not implemented in this basic version)
            // For now, just switch to tracing mode
            var appManager = Core.AppManager.Instance;
            if (appManager != null)
            {
                var gameModeManager = FindObjectOfType<GameModeManager>();
                if (gameModeManager != null)
                {
                    gameModeManager.CurrentMode = GameMode.CustomName;
                }
            }
            ShowTracingUI();
        }

        private void OnFinishTracingClicked()
        {
            var appManager = Core.AppManager.Instance;
            if (appManager != null)
            {
                appManager.EndTracing();
            }
        }

        private void OnRetryClicked()
        {
            ShowTracingUI();
        }

        private void OnStyleChanged(int index)
        {
            var gameModeManager = FindObjectOfType<GameModeManager>();
            if (gameModeManager != null)
            {
                gameModeManager.CurrentStyle = (CalligraphyStyle)index;
                UpdateStyle(gameModeManager.CurrentStyle);
            }
        }

        #endregion
    }
}

