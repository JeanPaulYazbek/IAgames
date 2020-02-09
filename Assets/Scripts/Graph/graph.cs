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
    public Node FindNode(Vector3 point){

        Node currentNode; 
        for(int i = 0; i< numberNodes; i++){
            currentNode = nodes[i];
            if(utilities.PointInTriangle(point, currentNode.vertexA,
             currentNode.vertexB, currentNode.vertexC )){
                 return currentNode;
             }
        }
        return ClosestNode(point);
    }

    //Funcion que busca el triangulo mas cercano al punto dado
    //solo retorna null si el grafo no tiene nodos
    public Node ClosestNode(Vector3 point){
        Node currentNode;
        float currentDistance;
        Node closestNode = null ;
        float min = float.PositiveInfinity;

        for(int i = 0; i< numberNodes; i++){
            currentNode = nodes[i];

            currentDistance = Vector3.Distance(point, currentNode.center);
            if(min > currentDistance){
                closestNode = currentNode;
                min = currentDistance;
            }
        }

        return closestNode;
    }

}