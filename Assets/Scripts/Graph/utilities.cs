using UnityEngine;
using System.Collections.Generic;


public class Utilities 
{

    public void DrawTriangle(Vector3 vertexA, Vector3 vertexB, Vector3 vertexC, float duration){

        Debug.DrawLine(vertexA, vertexB, Color.white, duration, true);
        Debug.DrawLine(vertexA, vertexC, Color.white, duration, true);
        Debug.DrawLine(vertexB, vertexC, Color.white, duration, true);

    }

    //Funcion que toma una lista de puntos y un tiempo
    // y dibuja el camino formado por esos puntos durante ese tiempo
    public void DrawPath(Vector3[] path, float duration){

        if(path is null){
            Debug.Log("Oh no, no hay camino");
            return;
        }

        if(path.Length < 2){
            Debug.Log("No puedo dibujarte un camino de un solo punto");
            return;
        }

        for(int i = 1; i<path.Length; i++){
            Debug.DrawLine(path[i], path[i-1], Color.black, duration, true);
        }
    }

    //Funcion que toma un punto p y revisa si esta dentro del triangulo
    //formado por p0 p1 y p2
    public bool PointInTriangle(Vector3 p, Vector3 p0, Vector3 p1, Vector3 p2){
        var s = p0.y * p2.x - p0.x * p2.y + (p2.y - p0.y) * p.x + (p0.x - p2.x) * p.y;
        var t = p0.x * p1.y - p0.y * p1.x + (p0.y - p1.y) * p.x + (p1.x - p0.x) * p.y;

        if ((s < 0) != (t < 0))
            return false;

        var A = -p1.y * p2.x + p0.y * (p2.x - p1.x) + p0.x * (p1.y - p2.y) + p1.x * p2.y;

        return A < 0 ?
                (s <= 0 && s + t >= A) :
                (s >= 0 && s + t <= A);
    }

}
