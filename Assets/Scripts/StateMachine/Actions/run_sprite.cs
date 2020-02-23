public class RunSprite : Action {
    pokemon_data pokemon;

    public RunSprite(pokemon_data Pokemon){
        pokemon = Pokemon;
    }

    //Accion que muestra un sprite corriendo del pokemon dado
    //solo si el pokemon tiene dicho sprite
    public override void DoAction(){
        pokemon.Run();
    }
}