using UnityEngine;
using System.Collections.Generic;


// Esta clase representa un nodo del grafo, en este caso triangulos
public class Graph {

    public Node[] nodes;//un arreglo de nodos donde el indice donde esta un nodo es su id
    public int numberNodes;//cuantos nodos tendremos
    public List<Connection>[] connections;//un arreglo donde estaran los nodos vecinos de cada nodo
    Utilities utilities;

    public Graph(Node[] Nodes){


        nodes = Nodes;
        numberNodes = nodes.Length;
        connections = new List<Connection>[numberNodes];
        utilities = new Utilities();
        
        //inicializamos todas las listas
        for(int i = 0; i< numberNodes; i++){
            connections[i] = new List<Connection>();
        }

    }

    //funcion que devuelve un nodo por su id 
    public Node GetNode(int id){
        return nodes[id];
    }


    //funcion que devuelve la lista de nodos vecinos de un nodo
    public List<Connection> GetConnections(int id){
        return connections[id];
    }

    //funcion que toma el id de dos nodos y agrega el primero a la 
    //lista de conecciones del segundo
    public void AddConnection(int id1, int id2){
        connections[id1].Add(new Connection(nodes[id1], nodes[id2]));
    }


    //Funcion que toma un punto y busca si hay un triangulo
    //en el grafo con ese punto adentro, si lo hay lo retorna
    //si no retorna null
    public Node FindNode(Vector3 point,string[] walkable){

        Node currentNode;
        for(int i = 0; i< numberNodes; i++){
            
            currentNode = nodes[i];

            // si el nodo es caminable y ademas es justo el nodo que buscamos
            if(utilities.PointInTriangle(point, currentNode.vertexA, currentNode.vertexB, currentNode.vertexC )
            && NodeIsOfType(walkable, currentNode)){
                 return currentNode;
             }
        }
        //No encontramos el nodo ideal asi que devolvemos el mas cercano
        return ClosestNode(point, walkable);
    }

    //Funcion que busca el triangulo mas cercano al punto dado
    //solo retorna null si el grafo no tiene nodos
    public Node ClosestNode(Vector3 point,string[] walkable){
        Node currentNode;
        float currentDistance;
        Node closestNode = null ;
        float min = float.PositiveInfinity;

        for(int i = 0; i< numberNodes; i++){
            currentNode = nodes[i];

            currentDistance = Vector3.Distance(point, currentNode.center);
            //si el nodo es mas cercano y ademas se puede caminar sobre el
            if(min > currentDistance && NodeIsOfType(walkable, currentNode)){
                closestNode = currentNode;
                min = currentDistance;
            }
        }

        return closestNode;
    }

    //Funcion que regresa un nodo al azar del grafo
    public Node GetRandomNode(string[] walkable){
        int randomNode = Random.Range(0, nodes.Length);
        Node target = nodes[randomNode];
        bool is_walkable = NodeIsOfType(walkable, target);
        
        if(is_walkable){
            return target;
        }

        return GetRandomNode(walkable);
    }

    public void DrawGraph(float duration){
        for(int i = 0; i < nodes.Length; i++){
            nodes[i].DrawTriangle(duration);
            foreach(var con in connections[i]){
                con.DrawConnection(duration);
            }
        }
    }
    public bool NodeIsOfType(string[] walkable, Node node){

        for(int i = 0; i<walkable.Length; i++){
            if(node.type == walkable[i]){
                return true;
            }
        }

        return false;

    }
    

}