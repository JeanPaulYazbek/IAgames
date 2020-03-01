using UnityEngine;

// Esta clase representa un nodo del grafo, en este caso triangulos
public class Node {

    public int id;//Este entero sera util para generar arreglos 
    public string type;//Este string representa si el nodo es tierra agua etc..

    //Vertices de del triangulo
    public Vector3 vertexA;
    public Vector3 vertexB;
    public Vector3 vertexC;

    //centro del triangulo
    public Vector3 center;

    //Debemos guardar el punto medio de cada lado
    // esto facilitara algunos calculos
    public Vector3 centerAB;
    public Vector3 centerAC;
    public Vector3 centerBC;


    //Variables utiles para algoritmos
    public Node prev = null;


    public Node(int Id, Vector3 VertexA, Vector3 VertexB, Vector3 VertexC, string Type){

        id = Id;
        type = Type;

        vertexA = VertexA;
        vertexB = VertexB;
        vertexC = VertexC;

        //calculamos el centro
        center =  (vertexA + vertexB + vertexC) / 3f; 

        //calculamos los centros de los lados 
        centerAB = (vertexA + vertexB) / 2;
        centerAC = (vertexA + vertexC) / 2;
        centerBC = (vertexB + vertexC) / 2;

    }

    public void DrawTriangle(float duration){

        Debug.DrawLine(vertexA, vertexB, Color.white, duration, true);
        Debug.DrawLine(vertexA, vertexC, Color.white, duration, true);
        Debug.DrawLine(vertexB, vertexC, Color.white, duration, true);

    }

    

}