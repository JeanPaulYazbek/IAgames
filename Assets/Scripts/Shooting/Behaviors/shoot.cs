using UnityEngine;
using System.Collections.Generic;
using System;

//este compoenente permite a un personaje disparar presionando x y o z
public class shoot : MonoBehaviour
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


    //obstaculos que evadir
    List<Transform> obstacles_list = new List<Transform>();
    Transform[] obstacles;
    
    // Start is called before the first frame update
    void Start()
    {
        //TRAINER
        agentKinetics = agentStatic.kineticsAgent;
       

        //POKEMONS
        pokemons = GameObject.FindGameObjectsWithTag("Pokemon");
        for (int i = 0; i<pokemons.Length; i++){
            
            pokemons_list.Add(pokemons[i].GetComponent<static_data>().kineticsAgent);
           
        }
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
 
        //NORMAL SHOOT
        if (Input.GetKey("x") && !pokeBall) {//si presionamos x y ya termino la ultima pokeBall lanzada
            
            //creamos un pokeball en el mismo lugar que esta el character que la lanza.
            pokeBall = Instantiate(pokeBallPrefab, transform.position, Quaternion.identity);
            //le pasamos la lista de pokemones y obstaculos
            static_shoot ballStatic = pokeBall.GetComponent<static_shoot>();
            //le pasamos datos necesarios
            ballStatic.pokemons = pokemons_kins;
            ballStatic.obstacles = obstacles;
            ballStatic.speed = ballSpeed;
            //le damos la direccion de la velocidad del trainer
            agentVelocity = agentKinetics.velocity;
            agentVelocity.Normalize();//esta sera la direccion del lanzamiento
            agentVelocity.z = -1f;
            ballStatic.direction = agentVelocity;
        
      
        }

        //CHEAT SHOOT 
        //z si disparo rapido
        //s si disparo lento
        if ((Input.GetKey("z") || Input.GetKey("s")) && !pokeBall) {//si presionamos z o s y ya termino la ultima pokeBall lanzada
        

            //CREAMOS POKE BALL
            //creamos un pokeball en el mismo lugar que esta el character que la lanza.
            pokeBall = Instantiate(pokeBallPrefab, transform.position, Quaternion.identity);
            //le pasamos la lista de pokemones y obstaculos
            static_shoot ballStatic = pokeBall.GetComponent<static_shoot>();
            ballStatic.pokemons = pokemons_kins;
            ballStatic.obstacles = obstacles;
            ballStatic.speed = ballSpeed;

            //--BUSCAMOS EL POKEMON MAS CERCANO
            Vector3 trainerPos = agentKinetics.transform.position;//posicion actual del trainer
            Vector3 pokemonPos = searchPokemon(trainerPos);//valor por defecto en caso de que todos los pokemon esten atrapados
            
            //usaremos esta direccion para el disparo en caso de que CalculateFiringSolution falle en encontrar
            //una direccion de disparo
            agentVelocity = agentKinetics.velocity;
            agentVelocity.Normalize();//esta sera la direccion del lanzamiento
            agentVelocity.z = -1f;

            bool slow = true;
            if(Input.GetKey("z")){//si queremos rapido
                slow = false;
            }

            //ahora calculamos a que direccion lanzar. PD: el *2 es porque necesitamos mas alcance
            Vector3 shootDirection = shootHandler.CalculateFiringSolution(trainerPos, pokemonPos, ballSpeed*2, gravity, agentVelocity, slow);

            //si casualmente la direccion tiene -1 es porque yo cablee el disparo
            //si cablee el disparo debemos usar la ballSpeed sola, sino usamos *2
            if(shootDirection.z != -1f){
                ballStatic.speed *= 2;
            }
  
            ballStatic.direction = shootDirection;

        }

    }

    //funcion que busca el pokemon mas cercano
    Vector3 searchPokemon(Vector3 trainerPos){
        
        float closest = float.PositiveInfinity;
        float distance;
        Transform pokemonTrans;
        Vector3 pokemonPos = Vector3.zero;//valor por defecto en caso de que todos los pokemon esten atrapados
        for (int i =0; i<pokemons_kins.Length; i++) {
            
            pokemonTrans = pokemons_kins[i].transform;
            distance = Vector3.Distance(trainerPos, pokemonTrans.position);
            if((closest > distance) && (pokemonTrans.localScale.x > 0f)){//si esta mas cerca y no el pokemon no ha sido atrapado
                closest = distance;
                pokemonPos = pokemonTrans.position;
                
            }
        }  

        return pokemonPos;
    }
}
