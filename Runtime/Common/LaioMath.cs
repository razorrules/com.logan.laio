using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Laio
{
    public static class LaioMath
    {

        /// <summary>
        /// Calculate a quadratic bezier curve from three points, using t for step
        /// </summary>
        /// <param name="p1">Begin point</param>
        /// <param name="p2">End point</param>
        /// <param name="p3">Curve control point</param>
        /// <param name="t">time</param>
        /// <returns>Location on curve</returns>
        public static UnityEngine.Vector3 CalculateQuadraticBezierPoint(UnityEngine.Vector3 p1, UnityEngine.Vector3 p2, UnityEngine.Vector3 p3, float t)
        {
            float u = (1 - t);
            float us = u * u;
            float ts = t * t;
            UnityEngine.Vector3 returnVal = us * p1 + 2 * u * t * p3 + ts * p2;
            return returnVal;
        }

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
