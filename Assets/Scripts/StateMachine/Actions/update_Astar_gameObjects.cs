using System.Collections.Generic;
using UnityEngine;

//Action que actualiza el target de A* al ultimo item de la pila
public class UpdateAStarGameObject : Action {
    Stack<GameObject> items;
    Kinetics kinAgent;
    PathFindAStar aStar;
    Graph graph;
    public string[] walkable;

    public UpdateAStarGameObject(Stack<GameObject> Items, PathFindAStar AStar, Graph Graph, Kinetics KinAgent, string[] Walkable){
        items = Items;
        aStar = AStar;
        graph = Graph;
        kinAgent = KinAgent;
        walkable =  Walkable;
    }

    public override void DoAction(){

        if(items.Count == 0){//si no quedan items
            aStar.start = graph.FindNode(kinAgent.transform.position, walkable);
            return;//no actualizamos
        }
        //ACTUALIZAMOS Astar
        Vector3 targetPosition = items.Peek().transform.position;
        Node targetNode = graph.FindNode(targetPosition, walkable);
        aStar.goal = targetNode;
        aStar.start = graph.FindNode(kinAgent.transform.position, walkable);
        aStar.heuristic = new Euclidean(targetNode);
    }


}