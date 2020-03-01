using UnityEngine;
using System.Collections.Generic;

public class eevee_state_machine : MonoBehaviour {



    //DATOS PIEDRAS EVOLUTIVAS
    GameObject[] stones;//lista de todas las piedras  

 
    //DATOS EEVEE
    public static_data agent;
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;

    public pokemon_data pokemonData;

    //DATOS DE LOS TRAINERS
    public static_data trainer;
    Kinetics kineticsTrainer;
    public static_data rival;
    Kinetics kineticsRival;

    

    //DATOS SEEK
    public float maxAccel =3f;//poner la misma maxspeed que aqui al personaje
    public int seek_or_flee = 1;
    //Movimientos
    public Seek seek; 


    //DATOS GRAFO
    public static_graph graphComponent;//componente que tiene guardado el grafo
    Graph graph;
    PathFindAStar aStar;
    string[] walkable = new string[]{"Earth"};//representa sobre que podemos caminar


    //DATOS MAQUINA DE ESTADOS
    StateMachine eeveeMachine;
    public float radiusAlert = 40f;//radio para alertarse
    public float radiusRun = 35f;//radio para huir
    public Transform[] allEevees;//necesitamos saber donde estan todos los eevee


    void Start(){

        //INICIALIZAMOS LA DATA DEL EXTERIOR
        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;
        kineticsTrainer = trainer.kineticsAgent;
        kineticsRival = rival.kineticsAgent;

        Vector3 center = Vector3.zero;//ncesitamos el centro de masa de los eevee

        for(int k = 0; k < allEevees.Length; k++){
            center += allEevees[k].transform.position;
        }

        center  = center / allEevees.Length;
        
        //pokemones
        stones = GameObject.FindGameObjectsWithTag("Stone");

        Stack<GameObject> stonesStack = new Stack<GameObject>(stones);
    

        //Inicializamos grafo y A*
        graph = graphComponent.graph;
        aStar = new PathFindAStar(graph,null ,null,null,walkable);

        //Inicializamos seek
        seek = new Seek(kineticsAgent, kineticsAgent, maxAccel);

        //obstaculos
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        obstacle_data[] obstaclesData =  new obstacle_data[obstacles.Length];

        for(int k = 0; k < obstacles.Length; k++){
            obstaclesData[k] = obstacles[k].GetComponent<obstacle_data>();
        }

        
        

        //COMENZAMOS A CONSTRUIR LA MAQUINA DE ESTADOS

        //1. ACCIONES:

        UpdateAStarGameObject updateAStar = new UpdateAStarGameObject(stonesStack, aStar, graph, kineticsAgent, walkable);
        FollowPathOfPoints followPath = new FollowPathOfPoints(steeringAgent, seek, null);
        PopGameObject popStone = new PopGameObject(stonesStack);
        DestroyGameObject destroyStone = new DestroyGameObject(stonesStack);
        UpdateFollowPathWithAstar updateFollow = new UpdateFollowPathWithAstar(followPath, aStar, obstaclesData);
        UpdateAStarTarget updateAStar2 = new UpdateAStarTarget(aStar, graph, kineticsAgent, graph.GetNode(550), walkable);
        StopMoving stop = new StopMoving(kineticsAgent, steeringAgent);
        RunSprite showRunSprite = new RunSprite(pokemonData);
        Evolve evolve = new Evolve(pokemonData, "Sun", updateAStar, updateAStar2, aStar);
        updateEvolveMethod updateEvolve = new updateEvolveMethod(evolve, stonesStack);
        ShowIcon showExclamation = new ShowIcon(this.gameObject, "Exclamation");
        DisableIcon disableExclamation = new DisableIcon(this.gameObject, "Exclamation");
        ShowIcon showSweat = new ShowIcon(this.gameObject, "Sweat");
        DisableIcon disableSweat = new DisableIcon(this.gameObject, "Sweat");




        //2. ESTADOS:

        List<Action> entryActions;//aqui iremos guardanndo todas las acciondes de entrada
        List<Action> exitActions;//aqui iremos guardanndo todas las acciones de salida
        List<Action> actions;//aqui guardaremos todas las acciones intermedias

        //2.a estado para esperar sin hacer nada
        entryActions = new List<Action>() {stop};//al entrar al estado debemos parar
        actions= new List<Action>();
        exitActions= new List<Action>() ;
        
        State wait = new State(actions, entryActions, exitActions);


        //2.b estado para sorprenderse
        entryActions = new List<Action>() {showExclamation};//al entrar al estado debemos sorprendernos
        actions= new List<Action>();
        exitActions= new List<Action>() {disableExclamation};//al salir dejamos de sorprendernos
        
        State alert = new State(actions, entryActions, exitActions);

        //2.c estado para perseguir piedra
        entryActions = new List<Action>() {updateAStar, updateFollow, showSweat};//al entrar al estado debemos actualizar el a* y luego el camino
        actions= new List<Action>() {followPath};//durante la accion seguimos el camino
        exitActions= new List<Action>();//al salir no hacemos nada
        
        State followStone = new State(actions, entryActions, exitActions);

        //2.d estado para perseguir punto de encuentro
        entryActions = new List<Action>() {updateAStar2, updateFollow};//al entrar al estado debemos actualizar el a* y luego el camino
        actions= new List<Action>() {followPath};//durante la accion seguimos el camino
        exitActions= new List<Action>();//al salir no hacemos nada
        
        State followReunionPoint = new State(actions, entryActions, exitActions);

        //3. CONDICIONES:

        TooCloseToPoint closeTrainer = new TooCloseToPoint(center, kineticsTrainer, radiusAlert);
        TooCloseToPoint veryCloseTrainer = new TooCloseToPoint(center, kineticsTrainer, radiusRun); 
        TooCloseToPoint closeRival = new TooCloseToPoint(center, kineticsRival, radiusAlert);
        TooCloseToPoint veryCloseRival = new TooCloseToPoint(center, kineticsRival, radiusRun); 
    
        //Estas son las que de verdad necesitamos
        OrCondition anyTargetClose = new OrCondition(closeTrainer, closeRival);
        OrCondition anyTargetVeryClose = new OrCondition(veryCloseRival, veryCloseTrainer);
        NotCondition noOneClose = new NotCondition(anyTargetClose);
        NotCondition noOneVeryClose = new NotCondition(anyTargetVeryClose);

        GameObjectGone stoneGone = new GameObjectGone(stonesStack);//si una piedra es obtenida por alguien mas
        AllGameObjectGone allStoneGone = new AllGameObjectGone(stonesStack);//si se acaban las piedras
        ArrivedToStone arrivedToStone = new ArrivedToStone(stonesStack, transform, 2f);//si alcanzamos una piedra 


        
        //4. TRANSICIONES:

        Transition closeHuman = new Transition(anyTargetClose, new List<Action>(), alert);
        Transition noHumanClose = new Transition(noOneClose, new List<Action>(), wait);
        Transition veryCloseHuman = new Transition(anyTargetVeryClose, new List<Action>(){showRunSprite}, followStone);
        Transition stoneLost = new Transition(stoneGone, new List<Action>{popStone}, followStone);
        Transition allStonesLost = new Transition(allStoneGone, new List<Action>{evolve, disableSweat}, followReunionPoint);
        Transition reachStone = new Transition(arrivedToStone, new List<Action>{updateEvolve,evolve,destroyStone,popStone, disableSweat}, followReunionPoint);


   
        //4.1 AGREGAMOS TRANSICIONES A ESTADOS

        List<Transition> transitions;
        transitions = new List<Transition>() {closeHuman};
        wait.transitions = transitions;

        transitions = new List<Transition>() {veryCloseHuman, noHumanClose};
        alert.transitions = transitions;

        transitions = new List<Transition>() {allStonesLost, stoneLost, reachStone};
        followStone.transitions =  transitions;

        followReunionPoint.transitions = new List<Transition>();



        //5 MAQUINA DE ESTADOS
        State[] states = new State[] {wait, alert, followStone, followReunionPoint};
        eeveeMachine = new StateMachine(states, wait);

    }

    void Update(){

        eeveeMachine.RunMachine();

    }

    
}