using System.Collections.Generic;

//Action que quita un pokemon de la pila 
public class RemovePokemon : Action {
    Stack<Kinetics> pokemons;

    public RemovePokemon(Stack<Kinetics> Pokemons){
        pokemons = Pokemons;
    }
    public override void DoAction(){
        pokemons.Pop();
    }
}