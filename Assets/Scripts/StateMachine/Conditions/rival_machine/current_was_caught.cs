using System.Collections.Generic;

//Condicion para saber si el ultimo pokemon de la pila ha sido atrapdo
public class CurrentWasCaught : Condition {
    Stack<Kinetics> pokemons;

    public CurrentWasCaught(Stack<Kinetics> Pokemons){
        pokemons = Pokemons;
    }

    public override bool Test(){
        return pokemons.Peek().transform.localScale.x == 0f;
    }
}