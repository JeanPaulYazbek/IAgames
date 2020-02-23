public class Evolve : Action {
    pokemon_data pokemon;
    string evolutionMethod;

    public Evolve(pokemon_data Pokemon, string EvolutionMethod){
        pokemon = Pokemon;
        evolutionMethod = EvolutionMethod;
    }

    //accion que hace evolucionar un pokemon
    public override void DoAction(){
        pokemon.Evolve(evolutionMethod);
    }

    //actualizar metodo de evolucion
    public void UpdateMethod(string newMethod){
        evolutionMethod = newMethod;
    }
}