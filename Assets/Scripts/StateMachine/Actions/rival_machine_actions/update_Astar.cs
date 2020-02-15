using System.Collections.Generic;
using UnityEngine;

//Action que actualiza el target de A* al ultimo pokemon de la pila
public class UpdateAStar : Action {
    Stack<Kinetics> pokemons;
    Kinetics kinAgent;
    PathFindAStar aStar;
    Graph graph;

    public UpdateAStar(Stack<Kinetics> Pokemons, PathFindAStar AStar, Graph Graph, Kinetics KinAgent){
        pokemons = Pokemons;
        aStar = AStar;
        graph = Graph;
        kinAgent = KinAgent;
    }

    public override void DoAction(){

        if(pokemons.Count == 0){//si no quedan pokemones
            return;//no actualizamos
        }
        //ACTUALIZAMOS Astar
        Kinetics currentPokemon = pokemons.Peek();
        Vector3 targetPosition = currentPokemon.transform.position;
        Node targetNode = graph.FindNode(targetPosition);
        aStar.goal = targetNode;
        aStar.start = graph.FindNode(kinAgent.transform.position);
        aStar.heuristic = new Euclidean(targetNode);
    }
}