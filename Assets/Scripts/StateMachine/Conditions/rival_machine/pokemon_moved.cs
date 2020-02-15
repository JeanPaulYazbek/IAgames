
using System.Collections.Generic;
using UnityEngine;

//Condicion para saber si el pokemon que estamos estudiando ahora se movio
public class PokemonMoved : Condition {
    Stack<Kinetics> pokemons;
    PathFindAStar aStar;

    public PokemonMoved(Stack<Kinetics> Pokemons, PathFindAStar AStar){
        pokemons = Pokemons;
        aStar = AStar;
    }

    public override bool Test(){
        //calculamos la distancia entre el pokemon del tope de la pila
        //y el goal de aStar(el cual deberia ser el triangulo en donde ese pokemon estba)
        float distance = Vector3.Distance(pokemons.Peek().transform.position, aStar.goal.center);

        if(distance > 10f){//si la distancia es muy grande asumiremos que se movio a otro triangulo
            return true;

        }
        return false;
    }
}