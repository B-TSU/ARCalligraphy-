using System.Threading.Tasks;
using ARCalligraphy.Core;

namespace ARCalligraphy.AI
{
    /// <summary>
    /// Base class for AI services (OpenAI, Claude, Gemini, etc.)
    /// </summary>
    public abstract class AIServiceBase : UnityEngine.MonoBehaviour
    {
        [UnityEngine.Header("AI Configuration")]
        [UnityEngine.SerializeField] protected string apiKey = "";
        [UnityEngine.SerializeField] protected string apiEndpoint = "";
        [UnityEngine.SerializeField] protected bool useSecureStorage = true;

        /// <summary>
        /// Generate kanji from English name
        /// </summary>
        /// <param name="englishName">The English name to convert</param>
        /// <param name="style">The calligraphy style</param>
        /// <returns>Generated kanji characters</returns>
        public abstract Task<string> GenerateKanjiFromName(string englishName, CalligraphyStyle style);

        /// <summary>
        /// Generate kanji with style description
        /// </summary>
        protected string BuildPrompt(string englishName, CalligraphyStyle style)
        {
            string styleDescription = GetStyleDescription(style);
            
            return $"Convert the English name '{englishName}' into appropriate Japanese kanji characters. " +
                   $"The kanji should represent the meaning or sound of the name. " +
                   $"Style: {styleDescription}. " +
                   $"Return only the kanji characters, no explanation, no additional text. " +
                   $"If multiple kanji are appropriate, return 2-3 characters maximum.";
        }

        /// <summary>
        /// Get style description for prompt
        /// </summary>
        protected string GetStyleDescription(CalligraphyStyle style)
        {
            return style switch
            {
                CalligraphyStyle.Smooth => "smooth and flowing calligraphy style",
                CalligraphyStyle.Aggressive => "aggressive and bold calligraphy style",
                CalligraphyStyle.Powerful => "powerful and strong calligraphy style",
                CalligraphyStyle.Abstract => "abstract and artistic calligraphy style",
                CalligraphyStyle.Artistic => "artistic and creative calligraphy style",
                _ => "standard calligraphy style"
            };
        }

        /// <summary>
        /// Load API key from secure storage or config
        /// </summary>
        protected virtual void LoadAPIKey()
        {
            if (useSecureStorage)
            {
                // Try to load from secure storage (PlayerPrefs, etc.)
                apiKey = UnityEngine.PlayerPrefs.GetString("AI_API_KEY", "");
            }

            if (string.IsNullOrEmpty(apiKey))
            {
                // Try to load from Resources/config
                LoadFromConfig();
            }
        }

        /// <summary>
        /// Load configuration from Resources
        /// </summary>
        protected virtual void LoadFromConfig()
        {
            try
            {
                UnityEngine.TextAsset configFile = UnityEngine.Resources.Load<UnityEngine.TextAsset>("Config/api_config");
                if (configFile != null)
                {
                    var config = UnityEngine.JsonUtility.FromJson<APIConfig>(configFile.text);
                    apiKey = config.openai_api_key;
                    apiEndpoint = config.endpoint ?? apiEndpoint;
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogWarning($"Failed to load API config: {e.Message}");
            }
        }

        /// <summary>
        /// Make HTTP request to AI service
        /// </summary>
        protected async Task<string> MakeAPIRequest(string prompt)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                LoadAPIKey();
            }

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new System.Exception("API key not configured");
            }

            return await SendRequest(prompt);
        }

        /// <summary>
        /// Send the actual HTTP request (implemented by subclasses)
        /// </summary>
        protected abstract Task<string> SendRequest(string prompt);

        /// <summary>
        /// Clean and validate kanji response
        /// </summary>
        protected string CleanKanjiResponse(string response)
        {
            if (string.IsNullOrEmpty(response))
                return "";

            // Remove quotes, whitespace, and non-kanji characters
            string cleaned = response.Trim();
            cleaned = cleaned.Trim('"', '\'', '`');
            
            // Extract only kanji, hiragana, katakana characters
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (char c in cleaned)
            {
                // Unicode ranges for Japanese characters
                if ((c >= 0x4E00 && c <= 0x9FFF) ||  // Kanji
                    (c >= 0x3040 && c <= 0x309F) ||  // Hiragana
                    (c >= 0x30A0 && c <= 0x30FF))    // Katakana
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        [System.Serializable]
        private class APIConfig
        {
            public string openai_api_key;
            public string endpoint;
            public string provider;
        }
    }
}

