using UnityEngine;

// Esta clase representa un nodo del grafo, en este caso triangulos
public class Connection {

    Node fromNode;
    Node toNode;
    public float cost;

    public Connection(Node FromNode,Node ToNode){
        fromNode = FromNode;
        toNode = ToNode;
        cost = Vector3.Distance(fromNode.center, toNode.center);
    }

    public void DrawConnection(float duration){

        Debug.DrawLine(fromNode.center, toNode.center, Color.black, duration, true);
        
    }

    public Node GetToNode(){
        return toNode;
    }

    public Node GetFromNode(){
        return fromNode;
    }

    

}