//Esta accion cambia el sprite de un pokemon al que viene por defecto
public class ShowDefaultSprite : Action {

    pokemon_data pokemonData;

    public ShowDefaultSprite(pokemon_data PokemonData){
        pokemonData = PokemonData;
    }
    public override void DoAction(){
        pokemonData.DefaultSprite();
    }
}