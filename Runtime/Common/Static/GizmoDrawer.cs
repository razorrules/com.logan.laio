using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Laio
{
    /// <summary>
    /// Adds a ton of additional gizmos drawing methods so you can visalize what you need.
    /// </summary>
    public static class GizmoDrawer
    {

        /// <summary>
        /// Draw a pie at a given transform. Great for visualizing FOV or sight perception.
        /// </summary>
        /// <param name="Transform">Where to draw the pie and what rotation</param>
        /// <param name="FOV">FOV of pie</param>
        /// <param name="Distance">Distance</param>
        /// <param name="Color">Color of the line</param>
        /// <param name="Thickness">Thickness of the line</param>
        public static void DrawPie(Transform Transform, float FOV, float Distance, Color? Color = null, float Thickness = 3)
        {
#if UNITY_EDITOR
            Matrix4x4 cache = Handles.matrix;
            Color cacheColor = Handles.color;
            if (Color.HasValue)
                Handles.color = Color.Value;
            else
                Handles.color = UnityEngine.Color.blue;

            Vector3 p1 = Quaternion.AngleAxis(-FOV, Vector3.up) * Vector3.forward * Distance;
            Vector3 p2 = Quaternion.AngleAxis(FOV, Vector3.up) * Vector3.forward * Distance;
            Handles.matrix = Matrix4x4.TRS(Transform.position, Transform.rotation, Vector3.one);
            Handles.DrawLine(Vector3.zero, p1, Thickness);
            Handles.DrawLine(Vector3.zero, p2, Thickness);

            Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.forward, FOV, Distance, Thickness);
            Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.forward, -FOV, Distance, Thickness);
            Handles.matrix = cache;
            Handles.color = cacheColor;
#endif
        }

        public static bool PointInBounds(this Bounds bounds, Vector3 point)
        {
            return point.x < bounds.max.x && point.x > bounds.min.x &&
                point.y < bounds.max.y && point.y > bounds.min.y &&
                point.z < bounds.max.z && point.z > bounds.min.z;
        }

        public static void DrawBoxCollider(BoxCollider bounds, string label, Color color)
        {
            if (bounds == null)
            {
                Debug.LogWarning("Bounds are not set for: " + label + ". Go to Hospital singleton and add box collider reference.");
                return;
            }
            Matrix4x4 _cacheMatrix = Gizmos.matrix;
            Gizmos.matrix = bounds.transform.localToWorldMatrix;
            Gizmos.color = color;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
#if UNITY_EDITOR
            Matrix4x4 _cachedHandleMatrix = UnityEditor.Handles.matrix;
            UnityEditor.Handles.matrix = bounds.transform.localToWorldMatrix;
            UnityEditor.Handles.Label(bounds.center, label);
            UnityEditor.Handles.matrix = _cachedHandleMatrix;
#endif
            Gizmos.matrix = _cacheMatrix;

        }

        /// <summary>
        /// Draw a solid and wire sphere.
        /// </summary>
        /// <param name="location">Location of spheres</param>
        /// <param name="radius">Radius of spheres</param>
        /// <param name="wire">Wireframe sphere color</param>
        /// <param name="solid">Solid sphere color</param>
        public static void DrawSolidAndWireSphere(Vector3 location, float radius, Color wire, Color solid)
        {
            Color _cacheColor = Gizmos.color;
            Gizmos.color = solid;
            Gizmos.DrawSphere(location, radius);
            Gizmos.color = wire;
            Gizmos.DrawWireSphere(location, radius);
            Gizmos.color = _cacheColor;
        }

        /// <summary>
        /// Draw a solid and wire cube
        /// </summary>
        /// <param name="location">Location of cubes</param>
        /// <param name="size">Side of cubes</param>
        /// <param name="wire">Color of wire cube</param>
        /// <param name="solid">Color of solid cube</param>
        public static void DrawSolidAndWireCube(Vector3 location, Vector3 size, Color wire, Color solid)
        {
            Color _cacheColor = Gizmos.color;
            Gizmos.color = solid;
            Gizmos.DrawCube(location, size);
            Gizmos.color = wire;
            Gizmos.DrawWireCube(location, size);
            Gizmos.color = _cacheColor;
        }

        /// <summary>
        /// Draw a quadratic bezier curve.
        /// </summary>
        /// <param name="Start">Start point of curve</param>
        /// <param name="End">End point of curve</param>
        /// <param name="Control">Control point of curve (Apex)</param>
        /// <param name="color">Color of line</param>
        /// <param name="density">Density to draw</param>
        public static void DrawQuadraticBezier(Vector3 Start, Vector3 End, Vector3 Control, Color color, int density = 30)
        {
            List<Vector3> points = new List<Vector3>();
            for (int i = 0; i < density; i++)
            {
                points.Add(Laio.LaioMath.CalculateQuadraticBezierPoint(Start, End, Control, (float)i / density));
            }
            points.Add(Laio.LaioMath.CalculateQuadraticBezierPoint(Start, End, Control, 1));
            DrawLine(points, color);
        }

        public static void DrawLine(IList<Vector3> points, Color color)
        {
            if (points == null)
                return;

            Color cached = Gizmos.color;
            Gizmos.color = color;
            for (int i = 0; i < points.Count - 1; i++)
            {
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
            Gizmos.color = cached;
        }

    }
}