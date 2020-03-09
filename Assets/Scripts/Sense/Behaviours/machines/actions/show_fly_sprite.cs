//Esta accion cambia el sprite de un pokemon a uno de vuelo
public class ShowFlySprite : Action {

    pokemon_data pokemonData;

    public ShowFlySprite(pokemon_data PokemonData){
        pokemonData = PokemonData;
    }
    public override void DoAction(){
        pokemonData.Fly();
    }
}