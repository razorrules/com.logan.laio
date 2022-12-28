using System;
using System.Collections.Generic;
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

        //TODO: Come up with better name? 

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

    }
}
