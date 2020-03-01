using System.Collections.Generic;
using UnityEngine;

//Action que actualiza el target de A* al ultimo pokemon de la pila
public class UpdateAStar : Action {
    Stack<Kinetics> pokemons;
    Kinetics kinAgent;
    PathFindAStar aStar;
    Graph graph;
    public string[] walkable;

    public UpdateAStar(Stack<Kinetics> Pokemons, PathFindAStar AStar, Graph Graph, Kinetics KinAgent, string[] Walkable){
        pokemons = Pokemons;
        aStar = AStar;
        graph = Graph;
        kinAgent = KinAgent;
        walkable = Walkable;
    }

    public override void DoAction(){

        if(pokemons.Count == 0){//si no quedan pokemones
            return;//no actualizamos
        }
        //ACTUALIZAMOS Astar
        Kinetics currentPokemon = pokemons.Peek();
        Vector3 targetPosition = currentPokemon.transform.position;
        Node targetNode = graph.FindNode(targetPosition, walkable);
        aStar.goal = targetNode;
        aStar.start = graph.FindNode(kinAgent.transform.position, walkable);
        aStar.heuristic = new Euclidean(targetNode);
    }
}