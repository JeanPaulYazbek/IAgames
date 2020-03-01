using UnityEngine;
using System.Collections.Generic;

public class rival_machine : MonoBehaviour {



    //DATOS POKEMONS
    GameObject[] pokemons;//lista de todos los pokemons    
    Stack<Kinetics> pokemonKins;//pila de pokemones

 
    //DATOS RIVAL
    public static_data agent;
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;
    


    //DATOS SEEK
    public float maxAccel =3f;//poner la misma maxspeed que aqui al personaje
    public int seek_or_flee = 1;
    //Movimientos
    public Seek seek; 


    //DATOS GRAFO
    public static_graph graphComponent;//componente que tiene guardado el grafo
    Graph graph;
    PathFindAStar aStar;
    string[] walkable = new string[]{"Earth"};


    //DATOS MAQUINA DE ESTADOS
    StateMachine rivalMachine;




    void Start(){

        //INICIALIZAMOS LA DATA DEL EXTERIOR
        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;
        
        //pokemones
        pokemons = GameObject.FindGameObjectsWithTag("Pokemon");
        pokemonKins = new Stack<Kinetics>();
        //creamos la pila de kinetics de los pokemones
        for(int i = 0; i < pokemons.Length; i++){
            pokemonKins.Push(pokemons[i].GetComponent<static_data>().kineticsAgent);
        }

        //obstaculos
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        obstacle_data[] obstaclesData =  new obstacle_data[obstacles.Length];

        for(int k = 0; k < obstacles.Length; k++){
            obstaclesData[k] = obstacles[k].GetComponent<obstacle_data>();
        }

        //Inicializamos grafo y A*
        graph = graphComponent.graph;
        aStar = new PathFindAStar(graph,null ,null,null, walkable);

        //Inicializamos seek
        seek = new Seek(kineticsAgent, kineticsAgent, maxAccel);

        
        

        //COMENZAMOS A CONSTRUIR LA MAQUINA DE ESTADOS

        //1. ACCIONES:

        UpdateAStar updateAstar = new UpdateAStar(pokemonKins, aStar, graph, kineticsAgent, walkable);
        FollowPathOfPoints followPath = new FollowPathOfPoints(steeringAgent, seek, null);
        RemovePokemon removePokemon = new RemovePokemon(pokemonKins);
        UpdateFollowPathWithAstar updateFollow =  new UpdateFollowPathWithAstar(followPath,aStar, obstaclesData);
        StopMoving stop = new StopMoving(kineticsAgent, steeringAgent);

        //2. ESTADOS:

        List<Action> entryActions;//aqui iremos guardanndo todas las acciondes de entrada
        List<Action> exitActions;//aqui iremos guardanndo todas las acciones de salida
        List<Action> actions;//aqui guardaremos todas las acciones intermedias

        //2.a estado para seguir path

        entryActions = new List<Action>() {updateAstar, updateFollow};//al entrar al estado debemos
                                                                //actualizar astar y el camino nuevo que da aStar darselo a follow
        actions= new List<Action>() {followPath};//durante la accion seguimos el camino
        exitActions= new List<Action>() ;//al salir no hacemos nada

        State followState = new State(actions, entryActions, exitActions);

        //2.b 

        entryActions =  new List<Action>() {stop};
        actions = new List<Action>();
        exitActions =  new List<Action>();

        State stopState = new State(actions, entryActions, exitActions);


        //3. CONDICIONES:

        AllCaught allCaught = new AllCaught(pokemonKins);//ayudara a saber si todos los pokemon fueron atrpados
        CurrentWasCaught currentPokemonCaught = new CurrentWasCaught(pokemonKins);//ayuda a saber si el pokemon actual en la pila fue atrapado
        PokemonMoved pokemonMoved = new PokemonMoved(pokemonKins, aStar);//ayudara a saber si el pokemon que estamos siguiendo se sale de su triangulo
        
        //4. TRANSICIONES:

        Transition caughtTarget = new Transition(currentPokemonCaught, new List<Action>(){removePokemon}, followState);
        Transition movedTarget = new Transition(pokemonMoved, new List<Action>(), followState);
        Transition caughtAllTargets = new Transition(allCaught, new List<Action>(), stopState);
   
        //4.1 AGREGAMOS TRANSICIONES A ESTADOS

        List<Transition> transitions = new List<Transition>() {caughtAllTargets, caughtTarget, movedTarget};
        followState.transitions = transitions;

        stopState.transitions =  new List<Transition>();

        //5 MAQUINA DE ESTADOS
        State[] states = new State[] {followState, stopState};
        rivalMachine = new StateMachine(states, followState);

    }

    void Update(){

        rivalMachine.RunMachine();

    }

    
}