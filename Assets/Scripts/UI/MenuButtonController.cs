using UnityEngine;
using UnityEngine.UI;

namespace ARCalligraphy.UI
{
    /// <summary>
    /// Simple button controller for menu navigation
    /// Handles Start button click to hide current UI and show selection options
    /// </summary>
    public class MenuButtonController : MonoBehaviour
    {
        [Header("UI Panels")]
        [Tooltip("The main menu panel (title screen)")]
        [SerializeField] private GameObject mainMenuPanel;
        
        [Tooltip("The selection options panel (shown after Start is clicked)")]
        [SerializeField] private GameObject selectionPanel;

        [Header("Buttons")]
        [Tooltip("The Start button that triggers the transition")]
        [SerializeField] private Button startButton;
        
        [Tooltip("The Sample button that displays the kanji quad")]
        [SerializeField] private Button sampleButton;

        [Header("Sample Display")]
        [Tooltip("The quad GameObject that will display the sample kanji material")]
        [SerializeField] private GameObject sampleQuad;
        
        [Tooltip("The material to display on the quad when sample button is clicked")]
        [SerializeField] private Material sampleMaterial;

        private void Start()
        {
            // Setup button listeners
            if (startButton != null)
            {
                startButton.onClick.AddListener(OnStartButtonClicked);
            }
            else
            {
                Debug.LogWarning("MenuButtonController: Start button is not assigned!");
            }

            if (sampleButton != null)
            {
                sampleButton.onClick.AddListener(OnSampleButtonClicked);
            }
            else
            {
                Debug.LogWarning("MenuButtonController: Sample button is not assigned!");
            }

            // Initialize UI state - show main menu, hide selection
            InitializeUI();
            
            // Initialize sample quad - hide it by default
            if (sampleQuad != null)
            {
                sampleQuad.SetActive(false);
            }
        }

        /// <summary>
        /// Initialize the UI to show main menu by default
        /// </summary>
        private void InitializeUI()
        {
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(true);
            
            if (selectionPanel != null)
                selectionPanel.SetActive(false);
        }

        /// <summary>
        /// Called when the Start button is clicked
        /// Hides the main menu and shows the selection options
        /// </summary>
        private void OnStartButtonClicked()
        {
            Debug.Log("Start button clicked - transitioning to selection screen");
            
            // Hide the main menu
            if (mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(false);
            }

            // Show the selection options
            if (selectionPanel != null)
            {
                selectionPanel.SetActive(true);
            }
            else
            {
                Debug.LogWarning("MenuButtonController: Selection panel is not assigned!");
            }
        }

        /// <summary>
        /// Public method to go back to main menu (useful for other scripts)
        /// </summary>
        public void ShowMainMenu()
        {
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(true);
            
            if (selectionPanel != null)
                selectionPanel.SetActive(false);
        }

        /// <summary>
        /// Public method to show selection panel (useful for other scripts)
        /// </summary>
        public void ShowSelectionPanel()
        {
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(false);
            
            if (selectionPanel != null)
                selectionPanel.SetActive(true);
        }

        /// <summary>
        /// Called when the Sample button is clicked
        /// Displays the quad mesh with the sample material
        /// </summary>
        private void OnSampleButtonClicked()
        {
            Debug.Log("Sample button clicked - displaying kanji quad");
            
            if (sampleQuad != null)
            {
                // Show the quad
                sampleQuad.SetActive(true);
                
                // Position quad in front of the camera
                Camera mainCamera = Camera.main;
                if (mainCamera == null)
                {
                    // Try to find XR camera if using AR/VR
                    mainCamera = FindObjectOfType<Camera>();
                }
                
                if (mainCamera != null)
                {
                    // Position 2 meters in front of camera
                    Vector3 cameraForward = mainCamera.transform.forward;
                    Vector3 cameraPosition = mainCamera.transform.position;
                    sampleQuad.transform.position = cameraPosition + cameraForward * 2f;
                    
                    // Make quad face the camera
                    sampleQuad.transform.LookAt(mainCamera.transform);
                    sampleQuad.transform.Rotate(0, 180, 0); // Flip to face camera correctly
                    
                    Debug.Log($"Quad positioned at: {sampleQuad.transform.position}");
                }
                else
                {
                    Debug.LogWarning("MenuButtonController: No camera found! Quad will use its current position.");
                }
                
                // Apply the material if provided
                if (sampleMaterial != null)
                {
                    MeshRenderer meshRenderer = sampleQuad.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        meshRenderer.material = sampleMaterial;
                    }
                    else
                    {
                        Debug.LogWarning("MenuButtonController: Sample quad does not have a MeshRenderer component!");
                    }
                }
                else
                {
                    Debug.LogWarning("MenuButtonController: Sample material is not assigned!");
                }
            }
            else
            {
                Debug.LogWarning("MenuButtonController: Sample quad is not assigned!");
            }
        }

        /// <summary>
        /// Public method to hide the sample quad (useful for other scripts)
        /// </summary>
        public void HideSampleQuad()
        {
            if (sampleQuad != null)
            {
                sampleQuad.SetActive(false);
            }
        }
    }
}


