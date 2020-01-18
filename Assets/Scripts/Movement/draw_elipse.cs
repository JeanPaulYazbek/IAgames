using UnityEngine;
using System;
 
public class draw_elipse : MonoBehaviour
{
    public int Segments = 32;
    public Color Color = Color.blue;
    public float XRadius = 2;
    public float YRadius = 1;
 
    void Start()
    {
        DrawEllipse(transform.position, transform.forward, transform.up, (float)transform.localScale.x *1.1f /(float) Math.Sqrt(2) , (float)transform.localScale.y *1.1f/(float) Math.Sqrt(2), Segments, Color, 20f);
    }
 
    private static void DrawEllipse(Vector3 pos, Vector3 forward, Vector3 up, float radiusX, float radiusY, int segments, Color color, float duration = 0)
    {
        float angle = 0f;
        Quaternion rot = Quaternion.LookRotation(forward, up);
        Vector3 lastPoint = Vector3.zero;
        Vector3 thisPoint = Vector3.zero;
 
        for (int i = 0; i < segments + 1; i++)
        {
            thisPoint.x = Mathf.Sin(Mathf.Deg2Rad * angle) * radiusX;
            thisPoint.y = Mathf.Cos(Mathf.Deg2Rad * angle) * radiusY;
 
            if (i > 0)
            {
                Debug.DrawLine(rot * lastPoint + pos, rot * thisPoint + pos, color, duration);
            }
 
            lastPoint = thisPoint;
            angle += 360f / segments;
        }
    }
}