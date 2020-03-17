using UnityEngine;
//Este metodo ayuda a evolucionar a eevee
public class Evolve2 : Action {
    pokemon_data pokemon;//data del pokemon a evolucionar
    TouchedGameObjects touchedStone;//esta condicion tendra guardada que roca tocamos
    UpdateAstarBestCoverPoint updateAStar;//debemos actualizar esto si evolucionamos a vaporeon para que pueda nadae
    PathFindAStar aStar;//para lo mismo que updateAstar


    public Evolve2(pokemon_data Pokemon, TouchedGameObjects TouchedStone, UpdateAstarBestCoverPoint UpdateAStar, PathFindAStar AStar){
        pokemon = Pokemon;
        touchedStone = TouchedStone;
        updateAStar = UpdateAStar;
        aStar= AStar;
    }

    //accion que hace evolucionar un pokemon
    public override void DoAction(){

        //nuestro metodo de evolucion sera el nombre del objeto que tocamos
        string evolutionMethod = touchedStone.touchedObj;


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
            aStar.walkable = newWalkable;

        }

        //EVOLUCIONAMOS
        pokemon.Evolve(evolutionMethod);

    }

}