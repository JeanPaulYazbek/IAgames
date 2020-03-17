using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
public class cover_quality_for_nodes : MonoBehaviour {

    
    // datos para la calidad 
    public float radius = 15f; // que tan lejos estan los puntos que compararemos
    public float angle = 18f; // angulo de cambio
    public int iterations = 100;
    public bool randomized = false;// se escogeran de manera aleatoria los puntos de lanzamiento si se arca true

    // datos externos utiles
    public static_graph graphComponent;
    Utilities utilities = new Utilities();
    obstacle_data[] obstaclesData;

    // datos de salida
    public List<CoverNode> coverNodes = new List<CoverNode>();

    // tipo de obstaculos que de verdad cubren visualmente
    string[] visualObstacles = new string[] {"Tree1", "Tree2"};


    void Start() {

        angle = angle/57;//convertimos a radianes

        //Buscamos los nodos del grafo
        Node[] nodes =  graphComponent.graph.nodes;

        //obstaculos
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        obstaclesData =  new obstacle_data[obstacles.Length];

        for(int k = 0; k < obstacles.Length; k++){
            obstaclesData[k] = obstacles[k].GetComponent<obstacle_data>();
        }

        // CREAMOS UN ARREGLO CON LOS NODOS CON VALOR DE CALIDAD PARA CUBRIRSE

        CoverNode[] coverNodesArray = new CoverNode[nodes.Length];
        
        float coverQuality;
        for(int i = 0; i<nodes.Length; i++){

            coverQuality = GetCoverQuality(nodes[i], iterations);
            coverNodesArray[i] = new CoverNode(nodes[i], coverQuality);
            
        }

        // ordenamos por calidad descendentemente
        coverNodesArray =  coverNodesArray.OrderBy(node => -node.coverQuality).ToArray();

        // tomamos solo los primeros 10
        coverNodesArray = coverNodesArray.Take(30).ToArray();

        // agregamos todos los nodos a la lista que usaran los componentes que lo necesiten
        // por eso usamos una lista para que no de problemas el tiempo de update, start 
        foreach(var node in coverNodesArray){
            coverNodes.Add(node);
        }

        //Debugging 
        // foreach(var node in coverNodes){
        //     Debug.Log(node.coverQuality);
        //     node.node.DrawTriangle(10f);
        // }


    }

    float GetCoverQuality(Node node, int iterations){
        
        //Si un nodo no es de tierra no sirve de escondite
        if(node.type != "Earth"){
            return -1;// le damos la puntuacion mas baja posible
        }

        float theta = 0f;
        int hits = 0;
        int valid = 0;

        Vector3 from;
        Vector3 to;
        float random = 1f;
        for(int i = 0; i < iterations; i++){
            
            //calculamos el nodo ficticio de lanzamiento
            from = node.center;

            //si queremos que no todos los puntos sean a la misma distancia del centro
            if(randomized){
                random = (float)(UnityEngine.Random.Range(-1f,1f)*radius*Math.Sin(angle));
            }
            from.x += (float)(radius*Math.Cos(theta)*random);
            from.y += (float)(radius*Math.Sin(theta)*random);
            from.z = 0f;

            //aumentamos la cantidad de puntos de lanzamiento estudiados
           
            valid++;

            
            to = node.center;
            to.z = 0f; 

            //vemos si entre el nodo y el punto de lanzamiento hay obstaculos
            bool obstacleCollide =  utilities.LineSegmentIntersectionObstacleType(from, to, obstaclesData, visualObstacles);

            //si los hay sumamos hits
            if(obstacleCollide){
                hits++;
            }

            theta += angle;
        }

        return (float)hits / (float)valid;

    
    }
} 
