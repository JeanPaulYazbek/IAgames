using UnityEngine;
using System.Collections.Generic;

//componente que permite predecir la trayectoria de todos los pokemons (uno por uno)
//si uno es atrapado el pursue pasara al siguiente target
//este componente es principalmente para la trainer enemiga
public class dyn_pursue_all : MonoBehaviour
{



    //Los objetos necesarios enemigo y agente
    GameObject[] pokemons;//lista de todos los pokemons
    Kinetics[] pokemonKins;//lista de kinetics
    List<Kinetics> kinsList = new List<Kinetics>();
    int currentPokemon = 0;//indice del pokemon que planeamos perseguir ahora
    int amountPokemon;//cantidad de pokemones 


    //Estructuras estaticas del agente
    public static_data agent;
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;
    


    //valores por defecto de los seek
    public float maxPrediction = 10f;

    //Movimientos
    public Pursue pursue; 

    public int seek_or_flee = 1;


    void Start (){

        //POKEMONES
        pokemons = GameObject.FindGameObjectsWithTag("Pokemon");

        for(int i = 0; i<pokemons.Length; i++){//le sacamos todos los kinetics a los pokes
            kinsList.Add(pokemons[i].GetComponent<static_data>().kineticsAgent);
        }

        pokemonKins = kinsList.ToArray();
        amountPokemon = pokemons.Length;

        //TRAINER
        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;


        //Inicializamos movimientos con el primer pokemon
        pursue = new Pursue(kineticsAgent, pokemonKins[currentPokemon], maxPrediction);

    }

    void Update (){

        //si el pokemon actual fue atrapado 
        if (pokemons[currentPokemon].transform.localScale.x == 0f){
            currentPokemon++;//pasamos al siguiente
            currentPokemon = currentPokemon % amountPokemon;//si nos pasamos del largo del arreglo volvemos al primero
            //actualizamos el target
            pursue.pTarget = pokemonKins[currentPokemon];
        }

       

        
        //Perseguimos al enemigo
        // con seek aceleracion
        steeringAgent.UpdateSteering(pursue.getSteering(seek_or_flee));

        
        
        
    }

}
