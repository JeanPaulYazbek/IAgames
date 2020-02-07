using UnityEngine;
using System.Collections.Generic;


// Esta clase representa un nodo del grafo, en este caso triangulos
public class Graph {

    public Node[] nodes;//un arreglo de nodos donde el indice donde esta un nodo es su id
    public int numberNodes;//cuentos nodos tendremos
    public List<Connection>[] connections;//un arreglo donde estaran los nodos vecinos de cada nodo

    public Graph(Node[] Nodes){


        nodes = Nodes;
        numberNodes = nodes.Length;
        connections = new List<Connection>[numberNodes];
        
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

}