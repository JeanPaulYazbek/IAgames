public class Evolve : Action {
    pokemon_data pokemon;
    string evolutionMethod;

    UpdateAStarGameObject updateAStar;
    UpdateAStarTarget updateAStar2;
    PathFindAStar aStar;


    public Evolve(pokemon_data Pokemon, string EvolutionMethod, UpdateAStarGameObject UpdateAStar, UpdateAStarTarget UpdateAStar2, PathFindAStar AStar){
        pokemon = Pokemon;
        evolutionMethod = EvolutionMethod;
        updateAStar = UpdateAStar;
        updateAStar2 = UpdateAStar2;
        aStar= AStar;
    }

    //accion que hace evolucionar un pokemon
    public override void DoAction(){

        //si evolucionamos por piedra agua es que seremos
        //un pokemon de agua entonces actualizamos los walkable
        //de cada metodo para incluir agua
        if(evolutionMethod == "Water Stone"){
            string[] oldWalkable = updateAStar.walkable;
            int n = oldWalkable.Length;
            string[] newWalkable = new string[n+1];

            //copiamos sobre lo que podiamos caminar anter
            for(int i = 0; i<n; i++){
                newWalkable[i] = oldWalkable[i];
            }

            //agregamos que podemos caminar sobre agua
            newWalkable[n] = "Water";

            updateAStar.walkable = newWalkable;
            updateAStar2.walkable = newWalkable;
            aStar.walkable = newWalkable;

        }

        //EVOLUCIONAMOS
        pokemon.Evolve(evolutionMethod);

    }

    //actualizar metodo de evolucion
    public void UpdateMethod(string newMethod){
        evolutionMethod = newMethod;
    }
}