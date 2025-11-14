using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using ARCalligraphy.Core;

namespace ARCalligraphy.AI
{
    /// <summary>
    /// OpenAI API service implementation
    /// </summary>
    public class OpenAIService : AIServiceBase
    {
        [Header("OpenAI Settings")]
        [SerializeField] private string model = "gpt-4o-mini"; // or "gpt-3.5-turbo" for cheaper option
        [SerializeField] private float temperature = 0.7f;
        [SerializeField] private int maxTokens = 50;

        private const string OPENAI_API_URL = "https://api.openai.com/v1/chat/completions";

        protected void Awake()
        {
            if (string.IsNullOrEmpty(apiEndpoint))
            {
                apiEndpoint = OPENAI_API_URL;
            }
        }

        public override async Task<string> GenerateKanjiFromName(string englishName, CalligraphyStyle style)
        {
            string prompt = BuildPrompt(englishName, style);
            return await MakeAPIRequest(prompt);
        }

        protected override async Task<string> SendRequest(string prompt)
        {
            // Create request payload
            var requestData = new OpenAIRequest
            {
                model = model,
                messages = new[]
                {
                    new OpenAIMessage
                    {
                        role = "user",
                        content = prompt
                    }
                },
                temperature = temperature,
                max_tokens = maxTokens
            };

            string jsonData = JsonUtility.ToJson(requestData);

            using (UnityWebRequest request = new UnityWebRequest(apiEndpoint, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

                var operation = request.SendWebRequest();

                // Wait for request to complete
                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var response = JsonUtility.FromJson<OpenAIResponse>(request.downloadHandler.text);
                    if (response.choices != null && response.choices.Length > 0)
                    {
                        string kanji = response.choices[0].message.content;
                        return CleanKanjiResponse(kanji);
                    }
                }
                else
                {
                    Debug.LogError($"OpenAI API Error: {request.error}");
                    throw new Exception($"API Request failed: {request.error}");
                }
            }

            return "";
        }

        #region Data Classes

        [Serializable]
        private class OpenAIRequest
        {
            public string model;
            public OpenAIMessage[] messages;
            public float temperature;
            public int max_tokens;
        }

        [Serializable]
        private class OpenAIMessage
        {
            public string role;
            public string content;
        }

        [Serializable]
        private class OpenAIResponse
        {
            public OpenAIChoice[] choices;
        }

        [Serializable]
        private class OpenAIChoice
        {
            public OpenAIMessage message;
        }

        #endregion
    }
}

