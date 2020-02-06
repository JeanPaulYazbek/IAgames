using UnityEngine;

// Esta clase representa un nodo del grafo, en este caso triangulos
public class Connection {

    Node nodeA;
    Node nodeB;

    public Connection(Node NodeA,Node NodeB){
        nodeA = NodeA;
        nodeB = NodeB;
    }

    public void DrawConnection(float duration){

        Debug.DrawLine(nodeA.center, nodeB.center, Color.black, duration, true);
        
       

    }

    

}