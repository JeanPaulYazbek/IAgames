using System.Collections.Generic;

public class AllCaught : Condition {

    Stack<Kinetics> pokemons;


    public AllCaught(Stack<Kinetics> Pokemons){
        pokemons = Pokemons;
    }
    public override bool Test(){
        return pokemons.Count == 0 ;
    }
}