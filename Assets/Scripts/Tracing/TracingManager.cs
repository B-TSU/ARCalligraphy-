using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using ARCalligraphy.Core;

namespace ARCalligraphy.Tracing
{
    /// <summary>
    /// Manages the tracing system for calligraphy
    /// </summary>
    public class TracingManager : MonoBehaviour
    {
        [Header("Tracing Settings")]
        [SerializeField] private float strokeWidth = 0.01f;
        [SerializeField] private Material strokeMaterial;
        [SerializeField] private float minStrokeDistance = 0.001f;
        [SerializeField] private bool useHandTracking = true;
        [SerializeField] private bool useControllerTracking = true;

        [Header("References")]
        [SerializeField] private Transform tracingPlane;
        [SerializeField] private GameObject strokePrefab;

        // Current state
        private bool isTracing = false;
        private CalligraphyStyle currentStyle = CalligraphyStyle.Smooth;
        private List<Stroke> currentStrokes = new List<Stroke>();
        private Stroke currentStroke = null;
        private Vector3 lastPoint = Vector3.zero;

        // Hand/Controller tracking
        private XRNode handNode = XRNode.RightHand;
        private bool handTracked = false;

        public void SetStyle(CalligraphyStyle style)
        {
            currentStyle = style;
            // Update material properties based on style
            UpdateStyleMaterial(style);
        }

        public void StartTracing()
        {
            isTracing = true;
            currentStrokes.Clear();
            currentStroke = null;
        }

        public List<Stroke> EndTracing()
        {
            isTracing = false;
            
            // Finalize last stroke
            if (currentStroke != null && currentStroke.points.Count > 0)
            {
                currentStrokes.Add(currentStroke);
                currentStroke = null;
            }

            return new List<Stroke>(currentStrokes);
        }

        private void Update()
        {
            if (!isTracing) return;

            // Try to get hand/controller position
            Vector3 currentPosition = GetTrackingPosition();
            
            if (currentPosition != Vector3.zero)
            {
                // Check if we should start a new stroke
                if (currentStroke == null)
                {
                    StartNewStroke(currentPosition);
                }
                else
                {
                    // Add point to current stroke
                    float distance = Vector3.Distance(currentPosition, lastPoint);
                    if (distance >= minStrokeDistance)
                    {
                        AddPointToStroke(currentPosition);
                        lastPoint = currentPosition;
                    }
                }
            }
            else if (currentStroke != null)
            {
                // Hand/controller lost, finalize current stroke
                FinalizeStroke();
            }
        }

        private Vector3 GetTrackingPosition()
        {
            // Try hand tracking first
            if (useHandTracking)
            {
                Vector3 handPos = GetHandPosition();
                if (handPos != Vector3.zero)
                {
                    handTracked = true;
                    return handPos;
                }
            }

            // Fall back to controller
            if (useControllerTracking)
            {
                Vector3 controllerPos = GetControllerPosition();
                if (controllerPos != Vector3.zero)
                {
                    handTracked = false;
                    return controllerPos;
                }
            }

            return Vector3.zero;
        }

        private Vector3 GetHandPosition()
        {
            // Use XR Hand Tracking if available
            // This is a simplified version - you'll need to implement proper hand tracking
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HandTracking, devices);
            
            if (devices.Count > 0)
            {
                InputDevice device = devices[0];
                if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position))
                {
                    return position;
                }
            }

            return Vector3.zero;
        }

        private Vector3 GetControllerPosition()
        {
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(handNode, devices);
            
            if (devices.Count > 0)
            {
                InputDevice device = devices[0];
                if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position))
                {
                    return position;
                }
            }

            return Vector3.zero;
        }

        private void StartNewStroke(Vector3 position)
        {
            currentStroke = new Stroke
            {
                points = new List<Vector3> { position },
                style = currentStyle,
                startTime = Time.time
            };
            lastPoint = position;
        }

        private void AddPointToStroke(Vector3 point)
        {
            if (currentStroke != null)
            {
                currentStroke.points.Add(point);
            }
        }

        private void FinalizeStroke()
        {
            if (currentStroke != null && currentStroke.points.Count > 1)
            {
                currentStroke.endTime = Time.time;
                currentStrokes.Add(currentStroke);
                RenderStroke(currentStroke);
            }
            currentStroke = null;
        }

        private void RenderStroke(Stroke stroke)
        {
            // Create visual representation of stroke
            if (strokePrefab != null)
            {
                GameObject strokeObj = Instantiate(strokePrefab);
                // Configure stroke rendering based on style
                // This would typically use a LineRenderer or custom mesh
            }
        }

        private void UpdateStyleMaterial(CalligraphyStyle style)
        {
            // Update stroke material properties based on style
            // Smooth: thin, flowing
            // Aggressive: thick, bold
            // Powerful: medium, strong
            // Abstract: varied, artistic
            // Artistic: creative, expressive
        }
    }

    /// <summary>
    /// Represents a single stroke in calligraphy
    /// </summary>
    [System.Serializable]
    public class Stroke
    {
        public List<Vector3> points;
        public CalligraphyStyle style;
        public float startTime;
        public float endTime;
        public float duration => endTime - startTime;
    }
}

