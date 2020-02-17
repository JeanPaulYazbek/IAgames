using System.Collections.Generic;
using UnityEngine;

//Action que actualiza el target de A* al ultimo pokemon de la pila
public class UpdateAStarRandom : Action {

    Kinetics kinAgent;
    PathFindAStar aStar;
    Graph graph;

    public ArrivedToPosition arrived;//condicion que tenemos que actualizar tambien

    public UpdateAStarRandom(PathFindAStar AStar, Graph Graph, Kinetics KinAgent, ArrivedToPosition Arrived){
        aStar = AStar;
        graph = Graph;
        kinAgent = KinAgent;
        arrived = Arrived;
    }

    public override void DoAction(){

        Node targetNode = graph.GetRandomNode();
        aStar.goal = targetNode;
        aStar.start = graph.FindNode(kinAgent.transform.position);
        aStar.heuristic = new Euclidean(targetNode);
        arrived.targetPos = targetNode.center;
    }
}