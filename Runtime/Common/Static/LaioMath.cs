using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Laio
{
    /// <summary>
    /// Additional methods related to math that might be helpful.
    /// </summary>
    public static class LaioMath
    {

        /// <summary>
        /// Calculate a quadratic bezier curve from three points, using t for step
        /// </summary>
        /// <param name="Start">Begin point</param>
        /// <param name="End">End point</param>
        /// <param name="Control">Curve control point</param>
        /// <param name="t">time</param>
        /// <returns>Location on curve</returns>
        public static UnityEngine.Vector3 CalculateQuadraticBezierPoint(UnityEngine.Vector3 Start, UnityEngine.Vector3 End, UnityEngine.Vector3 Control, float t)
        {
            float u = (1 - t);
            float us = u * u;
            float ts = t * t;
            UnityEngine.Vector3 returnVal = us * Start + 2 * u * t * Control + ts * End;
            return returnVal;
        }

        //TODO: Come up with better names? 

        /// <summary>
        /// Returns blend between two floats
        /// </summary>
        /// <param name="current">Current value</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <returns>Blend</returns>
        public static float GetBlend(float current, float min, float max)
        {
            if (current < min)
                return 0.0f;
            if (current > max)
                return 1.0f;
            current -= min;
            max -= min;
            return current / max;
        }

        /// <summary>
        /// Returns blend between two floats
        /// </summary>
        /// <param name="current">Current value</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <returns>Blend</returns>
        public static float GetBlend(UnityEngine.Vector3 current, UnityEngine.Vector3 min, UnityEngine.Vector3 max)
        {
            float d1 = UnityEngine.Vector3.Distance(ClampPoint(current, min, max), min);
            float d2 = UnityEngine.Vector3.Distance(ClampPoint(current, min, max), max);
            return d2 / (d2 + d1);
        }

        public static UnityEngine.Vector3 ClampPoint(UnityEngine.Vector3 point, UnityEngine.Vector3 segmentStart, UnityEngine.Vector3 segmentEnd)
        {
            return ClampProjection(ProjectPoint(point, segmentStart, segmentEnd), segmentStart, segmentEnd);
        }

        public static UnityEngine.Vector3 ProjectPoint(UnityEngine.Vector3 point, UnityEngine.Vector3 segmentStart, UnityEngine.Vector3 segmentEnd)
        {
            return segmentStart + UnityEngine.Vector3.Project(point - segmentStart, segmentEnd - segmentStart);
        }

        private static UnityEngine.Vector3 ClampProjection(UnityEngine.Vector3 point, UnityEngine.Vector3 start, UnityEngine.Vector3 end)
        {
            var toStart = (point - start).sqrMagnitude;
            var toEnd = (point - end).sqrMagnitude;
            var segment = (start - end).sqrMagnitude;
            if (toStart > segment || toEnd > segment) return toStart > toEnd ? end : start;
            return point;
        }

        public static UnityEngine.Vector3 RotateAround(UnityEngine.Vector3 point, UnityEngine.Vector3 pivot, UnityEngine.Quaternion rotation)
        {
            return rotation * (point - pivot) + pivot;
        }

        public static float FromBlend(float alpha, float min, float max)
        {
            float difference = max - min;
            return min + (difference * UnityEngine.Mathf.Clamp01(alpha));
        }

        public static float FromBlend(double alpha, float min, float max)
        {
            float difference = max - min;
            return min + (difference * UnityEngine.Mathf.Clamp01((float)alpha));
        }

    }
}
