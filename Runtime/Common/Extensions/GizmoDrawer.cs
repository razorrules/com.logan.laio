using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class GizmoDrawer
{

    /*
     * 
     * https://www.youtube.com/watch?v=-6H-uYh80vc&list=PLKeKudbESdcy6TlcfyYWrh3yChaQ-g0PB&ab_channel=Tarodev
     * 
     */

    public static void DrawTexture()
    {

    }

    //TODO: Cleanup
    public static void DrawPie(Transform transform, float FOV, float SightDistance, Color? color = null, int Thickness = 3)
    {
#if UNITY_EDITOR
        Matrix4x4 cache = Handles.matrix;
        Color cacheColor = Handles.color;
        if (color.HasValue)
            Handles.color = color.Value;
        else
            Handles.color = Color.blue;

        Vector3 p1 = -(Quaternion.AngleAxis(-FOV, Vector3.down) * -transform.forward) * SightDistance;
        Vector3 p2 = -(Quaternion.AngleAxis(FOV, Vector3.down) * -transform.forward) * SightDistance;
        Handles.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Handles.DrawLine(Vector3.zero, p1, 3);
        Handles.DrawLine(Vector3.zero, p2, 3);

        Handles.DrawWireArc(Vector3.zero, Vector3.up, transform.forward, FOV, SightDistance, 3);
        Handles.DrawWireArc(Vector3.zero, Vector3.up, transform.forward, -FOV, SightDistance, 3);
        Handles.matrix = cache;
        Handles.color = cacheColor;
#endif
    }

    //Get folder where script templates are, add editor to change it
    public static void DrawSoldAndWireSphere(Vector3 location, float radius, Color wire, Color solid)
    {
        Color _cacheColor = Gizmos.color;
        Gizmos.color = solid;
        Gizmos.DrawSphere(location, radius);
        Gizmos.color = wire;
        Gizmos.DrawWireSphere(location, radius);
        Gizmos.color = _cacheColor;
    }

    public static void DrawSoldAndWireCube(Vector3 location, Vector3 size, Color wire, Color solid)
    {
        Color _cacheColor = Gizmos.color;
        Gizmos.color = solid;
        Gizmos.DrawCube(location, size);
        Gizmos.color = wire;
        Gizmos.DrawWireCube(location, size);
        Gizmos.color = _cacheColor;
    }


    public static void DrawQuadraticBezier(Vector3 p1, Vector3 p2, Vector3 p3, Color color, int density = 30)
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < density; i++)
        {
            points.Add(Laio.LaioMath.CalculateQuadraticBezierPoint(p1, p2, p3, (float)i / density));
        }
        points.Add(Laio.LaioMath.CalculateQuadraticBezierPoint(p1, p2, p3, 1));
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
