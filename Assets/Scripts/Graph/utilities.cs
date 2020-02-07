using UnityEngine;
using System.Collections.Generic;


public class Utilities 
{

    public void DrawTriangle(Vector3 vertexA, Vector3 vertexB, Vector3 vertexC, float duration){

        Debug.DrawLine(vertexA, vertexB, Color.white, duration, true);
        Debug.DrawLine(vertexA, vertexC, Color.white, duration, true);
        Debug.DrawLine(vertexB, vertexC, Color.white, duration, true);

    }

    public void DrawPath(List<Vector3> path, float duration){

        if(path is null){
            Debug.Log("Oh no, no hay camino");
            return;
        }

        if(path.Count < 2){
            Debug.Log("No puedo dibujarte un camino de un solo punto");
            return;
        }

        for(int i = 1; i<path.Count; i++){
            Debug.DrawLine(path[i], path[i-1], Color.black, duration, true);
        }
    }

}
