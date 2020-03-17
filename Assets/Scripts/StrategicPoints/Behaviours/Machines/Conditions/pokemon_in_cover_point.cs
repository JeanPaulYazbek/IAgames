using UnityEngine;
// Esta condicion ayudara a un pokemon a saber si hay 
// ve otro pokemon en el escondite al que va, esto le permitira
// saber cuando buscar otro escondite
public class PokemonInCoverPoint : Condition {

    PathFindAStar aStar;//astar sera util porque ahi esta guardado el nodo al que vamos, el goal, el escondite
    SightSensor sensor;//aqui estara lo que vemos en caso de que veamos al pokemon

    Transform character;//esto sera util para saber que quien vimos no somos nosotros mismos

    Utilities utilities = new Utilities();

    public PokemonInCoverPoint(PathFindAStar AStar, SightSensor Sensor, Transform Character){
        aStar = AStar;
        sensor = Sensor;
        character = Character;
    }   

    public override bool Test(){

        //si vemos un pokemon
        if(sensor.notified && sensor.sightType == "Pokemon" && (character != sensor.detectedSignal.transform)){

          
            //posicion del pokemon que vimos
            Vector3 pokemonPos = sensor.detectedSignal.transform.position;
            //nodo donde nos planeamos esconder
            Node coverNode = aStar.goal;

            //retornamos true si el pokemon que vimos esta muy cerca del nodo
            return  Vector3.Distance(pokemonPos, coverNode.center) < 15f;

            

        }


        //si no vimos un pokemon
        return false;
        
    }


}