using UnityEngine;
using System.Collections.Generic;
using System;

//este componente permite a un personaje disparar automatico
public class auto_shoot : MonoBehaviour
{

    //TRAINER
    public static_data agentStatic;
    Kinetics agentKinetics;
    Vector3 agentVelocity;


    //POKEBALL
    public GameObject pokeBallPrefab;//aqui va el modelo de una pokeball
    public float ballSpeed = 8f;//magnitud de velocidad de lanzamiento
    GameObject pokeBall;//aqui guardaremos la instancia de una pokeball


    //SHOOT
    ProyectileFunctions shootHandler =  new ProyectileFunctions();//aqui tenemos funciones utiles para proyectiles
    Vector3 gravity = new Vector3(0f, 0f, 9.8f);

    
    //Aqui guardaremos los pokemones que pueden ser atrapados
    GameObject[] pokemons;
    List<Kinetics> pokemons_list = new List<Kinetics>();
    Kinetics[] pokemons_kins;
    pokemon_data[] pokemons_data;


    //obstaculos que evadir
    List<Transform> obstacles_list = new List<Transform>();
    Transform[] obstacles;

    //MANEJADOR DE PUNTOS
    public point_manager manager;
    
    // Start is called before the first frame update
    void Start()
    {
        //TRAINER
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

    }

    // Update is called once per frame
    void Update()
    {
 
       
            
        if(!pokeBall){//si ya desaparecio la ultima poke ball que lanzamos lanzamos otra

            //creamos un pokeball en el mismo lugar que esta el character que la lanza.
            pokeBall = Instantiate(pokeBallPrefab, transform.position, Quaternion.identity);
            //le pasamos la lista de pokemones y obstaculos
            static_shoot ballStatic = pokeBall.GetComponent<static_shoot>();
            //le pasamos datos necesarios
            ballStatic.pokemons = pokemons_kins;
            ballStatic.pokemonsObjs = pokemons;
            ballStatic.obstacles = obstacles;
            ballStatic.speed = ballSpeed;
            ballStatic.ultraBall = true;//estamos usando una ultra ball
            ballStatic.pokemonsData = pokemons_data;
            ballStatic.pointManager = manager;

            //le damos la direccion de la velocidad del trainer
            agentVelocity = agentKinetics.velocity;
            agentVelocity.Normalize();//esta sera la direccion del lanzamiento
            agentVelocity.z = -1f;
            ballStatic.direction = agentVelocity;
        }
        

        

    }


}
