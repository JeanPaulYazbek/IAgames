using UnityEngine;
using System.Collections.Generic;

//componente que permite predecir la trayectoria de todos los pokemons (uno por uno)
//si uno es atrapado el pursue pasara al siguiente target
//este componente es principalmente para la trainer enemiga
public class find_pokemons_Astar : MonoBehaviour
{



    //Los objetos necesarios enemigo y agente
    GameObject[] pokemons;//lista de todos los pokemons
    Kinetics[] pokemonKins;//lista de kinetics
    List<Kinetics> kinsList = new List<Kinetics>();
    int currentPokemon = 0;//indice del pokemon que planeamos perseguir ahora
    Vector3 pokemonPosition;
    int amountPokemon;//cantidad de pokemones 


    //Estructuras estaticas del agente
    public static_data agent;
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;
    Vector3 currentPosition;
    


    //valores por defecto de seek
    public float maxAccel =3f;
    public int seek_or_flee = 1;
    //Movimientos
    public Seek seek; 

    Vector3 currentTarget;


    //Datos grafo
    public static_graph graphComponent;
    Graph graph;
    PathFindAStar aStar;

    Vector3[] currentPath;//aqui guardaremos el camino que estemos siguiendo 
    int indexPath = 0;


    //Extras
    Utilities utilities = new Utilities();




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


        //Inicializamos movimiento (no es necesario pasar el pokemon porque lo haremos por posicion)
        seek = new Seek(kineticsAgent, kineticsAgent, maxAccel);

        //Inicializamos grafo y A*
        graph = graphComponent.graph;
        pokemonPosition = pokemonKins[0].transform.position;
        Node start = graph.FindNode(transform.position);//buscamos el triangulos con nuestra posicion actual
        Node goal = graph.FindNode(pokemonPosition);//buscamos el triangulo donde esta el primer pokemon
        if(goal is null){//si no hay un triangulo para el goal vamos a donde estamos parados
            goal = start;
        }
        Euclidean euclidean = new Euclidean(goal);
        aStar = new PathFindAStar(graph,start , goal , euclidean);
        currentPath = aStar.GetPath();
        currentTarget = currentPath[0];
        utilities.DrawPath(currentPath, 40f);

    }

    void Update (){

        //un pokemon es atrapado cuando su tamanno se vuelve 0
        float caught = pokemons[currentPokemon].transform.localScale.x;

        //si el pokemon actual fue atrapado 
        if (caught == 0f){
            currentPokemon++;//pasamos al siguiente
            currentPokemon = currentPokemon % amountPokemon;//si nos pasamos del largo del arreglo volvemos al primero
            
            pokemonPosition = pokemons[currentPokemon].transform.position;
            //actualizamos el a start
            aStar.goal = graph.FindNode(pokemonPosition);
            aStar.start = graph.FindNode(transform.position);
            //generamos nuevo camino
            currentPath = aStar.GetPath();
            utilities.DrawPath(currentPath, 40f);

            indexPath = 0;
        }


        currentPosition = kineticsAgent.transform.position;
        

        if(Vector3.Distance(currentPosition, currentPath[indexPath])<5f){//si alcanzamos cierto punto del path
            indexPath++;//nos movemos al siguiente
            int n = currentPath.Length;
            if(indexPath >= n){//si nos pasamos del largo del path 
                indexPath = n -1;//
            }
            currentTarget = currentPath[indexPath];
        }

        //Debemos ver si alcanzamos la posicion que buscabamos en cuyo caso 
        //debemos actualizar nuestro path con el siguiente pokemon a atrapar
        if(Vector3.Distance(currentPosition, pokemonPosition)<3f){
            
            if(pokemonPosition != pokemons[currentPokemon].transform.position){//si el pokemon se movio hay que actualizar el camino
                pokemonPosition = pokemons[currentPokemon].transform.position;
                //actualizamos el a start
                aStar.goal = graph.FindNode(pokemonPosition);
                aStar.start = graph.FindNode(transform.position);
                //generamos nuevo camino
                currentPath = aStar.GetPath();
                utilities.DrawPath(currentPath, 40f);

                indexPath = 0;
            }else{
                currentTarget = pokemonPosition;
            }
        }

       

        
        //Perseguimos al enemigo
        // con seek aceleracion
        steeringAgent.UpdateSteering(seek.getSteering2(currentTarget ,seek_or_flee));

        
        
        
    }

}
