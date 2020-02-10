using UnityEngine;

// Esta condicion ayudara a saber si cierto pokemon fue atrapado
public class WasCaught : Condition{

    Kinetics pokemon;

    public WasCaught(Kinetics Pokemon){
        pokemon = Pokemon;
    }

    //Funcion que devuelve true si el pokemon fue atrapado
    public override bool Test(){

        return pokemon.transform.localScale.x == 0f;

    }
}