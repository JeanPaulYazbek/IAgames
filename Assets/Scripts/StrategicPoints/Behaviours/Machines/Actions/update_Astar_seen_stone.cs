using System.Collections.Generic;
using UnityEngine;

//Action que actualiza el target de A* al ultimo pokemon de la pila
public class UpdateAStarSeenStone : Action {
    
    SightSensor sensor;//en este sensor estara la posicion de la piedra que vimos
    Transform character;
    PathFindAStar aStar;
    Graph graph;
    public string[] walkable;

    public UpdateAStarSeenStone(SightSensor Sensor, PathFindAStar AStar, Graph Graph, Transform Character, string[] Walkable){
        sensor = Sensor;
        aStar = AStar;
        graph = Graph;
        character = Character;
        walkable = Walkable;
    }

    public override void DoAction(){

        //ACTUALIZAMOS ASTAR
        Vector3 targetPosition = sensor.detectedSignal.transform.position;
        Node targetNode = graph.FindNode(targetPosition, walkable);
        aStar.goal = targetNode;
        aStar.start = graph.FindNode(character.position, walkable);

        aStar.heuristic = new Euclidean(targetNode);
    }
}