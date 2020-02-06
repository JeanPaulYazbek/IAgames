using UnityEngine;

public class Utilities 
{

    public void DrawTriangle(Vector3 vertexA, Vector3 vertexB, Vector3 vertexC, float duration){

        Debug.DrawLine(vertexA, vertexB, Color.white, duration, true);
        Debug.DrawLine(vertexA, vertexC, Color.white, duration, true);
        Debug.DrawLine(vertexB, vertexC, Color.white, duration, true);

    }

}
