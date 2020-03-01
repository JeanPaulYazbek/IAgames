using System.Collections.Generic;
using UnityEngine;

//Action que actualiza el target de A* al ultimo pokemon de la pila
public class UpdateAStarRandom : Action {

    Kinetics kinAgent;
    PathFindAStar aStar;
    Graph graph;
    public string[] walkable;

    public ArrivedToPosition arrived;//condicion que tenemos que actualizar tambien

    public UpdateAStarRandom(PathFindAStar AStar, Graph Graph, Kinetics KinAgent, ArrivedToPosition Arrived, string[] Walkable){
        aStar = AStar;
        graph = Graph;
        kinAgent = KinAgent;
        arrived = Arrived;
        walkable = Walkable;
    }

    public override void DoAction(){

        Node targetNode = graph.GetRandomNode(walkable);
        aStar.goal = targetNode;
        aStar.start = graph.FindNode(kinAgent.transform.position, walkable);
        aStar.heuristic = new Euclidean(targetNode);
        arrived.targetPos = targetNode.center;
    }
}