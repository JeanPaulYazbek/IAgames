using UnityEngine;
using System.Collections.Generic;
using System;

//este componente permite al rival disparar diractamente a los 
//pokemons que vea 
public class rival_hard_shoot : MonoBehaviour
{

    //RIVAL
    public static_data agentStatic;
    Kinetics agentKinetics;


    //POKEBALL
    public GameObject pokeBallPrefab;//aqui va el modelo de una pokeball
    public float ballSpeed = 8f;//magnitud de velocidad de lanzamiento
    GameObject pokeBall;//aqui guardaremos la instancia de una pokeball

    //SHOOT
    ProyectileFunctions shootHandler =  new ProyectileFunctions();//aqui tenemos funciones utiles para proyectiles
    Vector3 gravity = new Vector3(0f, 0f, 9.8f);

    //POKEMONES 
    GameObject[] pokemons;
    List<Kinetics> pokemons_list = new List<Kinetics>();
    Kinetics[] pokemons_kins;
    pokemon_data[] pokemons_data;

    //obstaculos que evadir
    List<Transform> obstacles_list = new List<Transform>();
    Transform[] obstacles;

    //SENSOR DE VISION
    public sight_sensor sensorComp;
    SightSensor sightSensor;

    //MANEJADOR DE PUNTOS
    public point_manager manager;
    


    void Start(){
        //rival
        agentKinetics = agentStatic.kineticsAgent;
       

        //POKEMONS
        pokemons = GameObject.FindGameObjectsWithTag("Pokemon");
        List<pokemon_data> pokemonDataList = new List<pokemon_data>();

        for (int i = 0; i<pokemons.Length; i++){
            
            pokemons_list.Add(pokemons[i].GetComponent<static_data>().kineticsAgent);
            pokemonDataList.Add(pokemons[i].GetComponent<pokemon_data>());

        }
        pokemons_data = pokemonDataList.ToArray();
        pokemons_kins =  pokemons_list.ToArray();

        //OBSTACLES
        GameObject[] obstaclesGo = GameObject.FindGameObjectsWithTag("Obstacle");
        for (int i = 0; i<obstaclesGo.Length; i++){
            
            obstacles_list.Add(obstaclesGo[i].GetComponent<Transform>());
           
        }

        obstacles = obstacles_list.ToArray();

        //SENSOR
        sightSensor = sensorComp.sensor;
    }

    void Update(){

        //Si la ultima pokeball que lanzamos ya desaparecio
        // y si vemos algo
        // y si ese algo es un pokemon
        // le lanzamos un poke ball
        if(!pokeBall && sightSensor.notified && sightSensor.sightType == "Pokemon"){

            //CREAMOS POKE BALL
            //creamos un pokeball en el mismo lugar que esta el character que la lanza.
            pokeBall = Instantiate(pokeBallPrefab, transform.position, Quaternion.identity);
                     
            static_shoot ballStatic = InitBall();//iniciamos la bola con una direccion por defecto

            //--BUSCAMOS EL POKEMON MAS CERCANO
            Vector3 rivalPos = agentKinetics.transform.position;//posicion actual del rival
            Vector3 pokemonPos = sightSensor.detectedSignal.transform.position;//la posicion del pokemon que vimos

            //ahora calculamos a que direccion lanzar. 
            Vector3 shootDirection = shootHandler.CalculateFiringSolution(rivalPos, pokemonPos, ballSpeed, gravity, ballStatic.direction, false);

            ballStatic.direction = shootDirection;
           
            sightSensor.ResetSensor();
        }

    }

    //Funcion que asigna los datos que necesita una poke bola
    static_shoot InitBall(){
        static_shoot ballStatic = pokeBall.GetComponent<static_shoot>();
        ballStatic.pokemons = pokemons_kins;
        ballStatic.pokemonsObjs = pokemons;
        ballStatic.pokemonsData = pokemons_data;
        ballStatic.obstacles = obstacles;
        ballStatic.speed = ballSpeed;
        ballStatic.direction = new Vector3(agentKinetics.velocity.x,agentKinetics.velocity.z,-1f); // DIRECCION POR DEFECTO
        ballStatic.pointManager = manager;
        ballStatic.ultraBall = true;
        return ballStatic;

    }

}