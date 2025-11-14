using System.Collections.Generic;
using UnityEngine;
using ARCalligraphy.Tracing;

namespace ARCalligraphy.Recognition
{
    /// <summary>
    /// Scores calligraphy based on accuracy, stroke order, and style
    /// </summary>
    public class ScoringSystem : MonoBehaviour
    {
        [Header("Scoring Weights")]
        [SerializeField] private float accuracyWeight = 0.4f;
        [SerializeField] private float strokeOrderWeight = 0.3f;
        [SerializeField] private float proportionWeight = 0.2f;
        [SerializeField] private float smoothnessWeight = 0.1f;

        [Header("Thresholds")]
        [SerializeField] private float minAccuracyForPass = 0.6f;

        /// <summary>
        /// Calculate overall score for traced character
        /// </summary>
        public float CalculateScore(List<Stroke> tracedStrokes, string targetKanji, string recognizedKanji)
        {
            if (tracedStrokes == null || tracedStrokes.Count == 0)
                return 0f;

            // Check if recognized character matches target
            bool characterMatch = recognizedKanji == targetKanji;
            if (!characterMatch)
            {
                // Still give partial credit if close
                return CalculatePartialScore(tracedStrokes, targetKanji, recognizedKanji);
            }

            // Calculate individual scores
            float accuracyScore = CalculateAccuracyScore(tracedStrokes, targetKanji);
            float orderScore = CalculateStrokeOrderScore(tracedStrokes, targetKanji);
            float proportionScore = CalculateProportionScore(tracedStrokes, targetKanji);
            float smoothnessScore = CalculateSmoothnessScore(tracedStrokes);

            // Weighted combination
            float totalScore = 
                accuracyScore * accuracyWeight +
                orderScore * strokeOrderWeight +
                proportionScore * proportionWeight +
                smoothnessScore * smoothnessWeight;

            return Mathf.Clamp01(totalScore);
        }

        private float CalculatePartialScore(List<Stroke> tracedStrokes, string targetKanji, string recognizedKanji)
        {
            // Give partial credit even if character doesn't match
            // This encourages users to keep trying
            float baseScore = 0.3f; // Base score for attempting
            float smoothnessBonus = CalculateSmoothnessScore(tracedStrokes) * 0.2f;
            return Mathf.Clamp01(baseScore + smoothnessBonus);
        }

        private float CalculateAccuracyScore(List<Stroke> strokes, string targetKanji)
        {
            // Compare stroke shapes to reference
            // This would use the CharacterRecognizer's comparison methods
            // For now, simplified version
            return 0.8f; // Placeholder
        }

        private float CalculateStrokeOrderScore(List<Stroke> strokes, string targetKanji)
        {
            // Verify strokes are drawn in correct order
            // Japanese calligraphy has specific stroke orders
            // This would compare against reference stroke order
            return 0.7f; // Placeholder
        }

        private float CalculateProportionScore(List<Stroke> strokes, string targetKanji)
        {
            // Check if character proportions are correct
            // Compare bounding box, stroke lengths, etc.
            if (strokes.Count == 0) return 0f;

            // Calculate bounding box
            Bounds bounds = CalculateBounds(strokes);
            float aspectRatio = bounds.size.x / bounds.size.y;

            // Ideal aspect ratio for kanji is usually close to 1:1
            float idealRatio = 1f;
            float ratioScore = 1f - Mathf.Abs(aspectRatio - idealRatio);

            return Mathf.Clamp01(ratioScore);
        }

        private float CalculateSmoothnessScore(List<Stroke> strokes)
        {
            if (strokes.Count == 0) return 0f;

            float totalSmoothness = 0f;
            foreach (var stroke in strokes)
            {
                totalSmoothness += CalculateStrokeSmoothness(stroke);
            }

            return totalSmoothness / strokes.Count;
        }

        private float CalculateStrokeSmoothness(Stroke stroke)
        {
            if (stroke.points.Count < 3) return 1f; // Too few points to judge

            // Calculate average angle change
            float totalAngleChange = 0f;
            int angleCount = 0;

            for (int i = 1; i < stroke.points.Count - 1; i++)
            {
                Vector3 prevDir = (stroke.points[i] - stroke.points[i - 1]).normalized;
                Vector3 nextDir = (stroke.points[i + 1] - stroke.points[i]).normalized;

                float angle = Vector3.Angle(prevDir, nextDir);
                totalAngleChange += angle;
                angleCount++;
            }

            if (angleCount == 0) return 1f;

            float avgAngleChange = totalAngleChange / angleCount;
            // Lower angle change = smoother stroke
            // Convert to score (0-180 degrees, lower is better)
            float smoothness = 1f - (avgAngleChange / 180f);
            return Mathf.Clamp01(smoothness);
        }

        private Bounds CalculateBounds(List<Stroke> strokes)
        {
            if (strokes.Count == 0 || strokes[0].points.Count == 0)
                return new Bounds();

            Vector3 min = strokes[0].points[0];
            Vector3 max = strokes[0].points[0];

            foreach (var stroke in strokes)
            {
                foreach (var point in stroke.points)
                {
                    min = Vector3.Min(min, point);
                    max = Vector3.Max(max, point);
                }
            }

            Vector3 center = (min + max) / 2f;
            Vector3 size = max - min;
            return new Bounds(center, size);
        }

        /// <summary>
        /// Get score as percentage
        /// </summary>
        public int GetScorePercentage(float score)
        {
            return Mathf.RoundToInt(score * 100f);
        }

        /// <summary>
        /// Get score feedback message
        /// </summary>
        public string GetScoreFeedback(float score)
        {
            int percentage = GetScorePercentage(score);

            if (percentage >= 90)
                return "Excellent! Perfect calligraphy!";
            else if (percentage >= 75)
                return "Great job! Very well done!";
            else if (percentage >= 60)
                return "Good work! Keep practicing!";
            else if (percentage >= 40)
                return "Not bad! Try to be more precise.";
            else
                return "Keep practicing! Focus on stroke accuracy.";
        }
    }
}

