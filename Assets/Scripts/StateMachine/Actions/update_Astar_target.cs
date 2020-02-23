using System.Collections.Generic;
using UnityEngine;

//Action que actualiza el target de A* al nodo dado newTarget
public class UpdateAStarTarget : Action {

    Kinetics kinAgent;
    PathFindAStar aStar;
    Graph graph;

    Node newTarget;

    public UpdateAStarTarget(PathFindAStar AStar, Graph Graph, Kinetics KinAgent, Node Target){
        aStar = AStar;
        graph = Graph;
        kinAgent = KinAgent;
        newTarget = Target;
    }

    public override void DoAction(){

        aStar.goal = newTarget;
        aStar.start = graph.FindNode(kinAgent.transform.position);
        aStar.heuristic = new Euclidean(newTarget);
    }
}