using UnityEngine;
using System.Collections.Generic;

public class eevee_state_machine2 : MonoBehaviour {



    //DATOS PIEDRAS EVOLUTIVAS
    GameObject[] stones;//lista de todas las piedras  

 
    //DATOS EEVEE
    public static_data agent;
    Kinetics kineticsAgent;
    SteeringOutput steeringAgent;
    public pokemon_data pokemonData;
    public sight_sensor sightComponent;
    public sight_sensor sightComponentPokemon;

    public sound_sensor soundComponent;
    SightSensor sightSensor;
    SightSensor sightSensorPokemon;//tenemos un sensor separado para los pokemones

    SoundSensor soundSensor;

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


    //DATOS LOOKWHEREYOUGOING

    LookWhereYouAreGoing look;


    //DATOS GRAFO
    public static_graph graphComponent;//componente que tiene guardado el grafo
    Graph graph;
    PathFindAStar aStar;
    string[] walkable = new string[]{"Earth"};//representa sobre que podemos caminar

    //DATOS PUNTOS ESTRATEGICOS
    public cover_quality_for_nodes pointsComponent;
    List<CoverNode> strategicPoints;


    //DATOS MAQUINA DE ESTADOS
    StateMachine eeveeMachine;
    // public float radiusAlert = 40f;//radio para alertarse
    // public float radiusRun = 35f;//radio para huir


    void Start(){

        

        //INICIALIZAMOS LA DATA DEL EXTERIOR
        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;
        kineticsTrainer = trainer.kineticsAgent;
        kineticsRival = rival.kineticsAgent;
        sightSensor = sightComponent.sensor;
        sightSensorPokemon = sightComponentPokemon.sensor;
        soundSensor = soundComponent.sensor;


      
        //Piedras
        stones = GameObject.FindGameObjectsWithTag("Stone");
        List<GameObject> stonesList = new List<GameObject>(stones);
    
        
        //Inicializamos grafo y A*
        graph = graphComponent.graph;
        aStar = new PathFindAStar(graph,null ,null,null,walkable);

        //Inicializamos seek
        seek = new Seek(kineticsAgent, kineticsAgent, maxAccel);

        //Inicializamos lookwehereyougoing
        look = new LookWhereYouAreGoing(kineticsAgent);

        //Obstaculos
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        obstacle_data[] obstaclesData =  new obstacle_data[obstacles.Length];

        for(int k = 0; k < obstacles.Length; k++){
            obstaclesData[k] = obstacles[k].GetComponent<obstacle_data>();
        }

        //Puntos estrategicos
        strategicPoints = pointsComponent.coverNodes;

        
        

        //COMENZAMOS A CONSTRUIR LA MAQUINA DE ESTADOS

        //1. ACCIONES:

        //acciones relacionadas a astar
        FollowPathOfPoints followPath = new FollowPathOfPoints(steeringAgent, seek, null, false);
        UpdateFollowPathWithAstar updateFollow = new UpdateFollowPathWithAstar(followPath, aStar, obstaclesData);
        UpdateAstarBestCoverPoint updateAstarCover = new UpdateAstarBestCoverPoint(strategicPoints, transform, new Transform[] {kineticsRival.transform, kineticsTrainer.transform}, obstaclesData, graph, aStar, walkable);
        UpdateAStarSeenStone updateAstarStone = new UpdateAStarSeenStone(sightSensor, aStar, graph, transform, walkable);
        //acciones de manejo de giros
        SetAngularSpeed setDefaultRotation = new SetAngularSpeed(kineticsAgent, 10f);
        SetAngularSpeed setZeroRotation = new SetAngularSpeed(kineticsAgent, 0f);
        StopMoving stop = new StopMoving(kineticsAgent, steeringAgent);
        LookWhereGoing lookAction = new LookWhereGoing(look, steeringAgent);
        //acciones de manejo de srpites
        ShowDefaultSprite defaultSprite = new ShowDefaultSprite(pokemonData);
        RunSprite showRunSprite = new RunSprite(pokemonData);
        Evolve2 evolve;
        ShowIcon showExclamation = new ShowIcon(this.gameObject, "Exclamation");
        DisableIcon disableExclamation = new DisableIcon(this.gameObject, "Exclamation");
        ShowIcon showSweat = new ShowIcon(this.gameObject, "Sweat");
        DisableIcon disableSweat = new DisableIcon(this.gameObject, "Sweat");
        //acciones de asistencia
        ResetSensor resetSight = new ResetSensor(sightSensor);
        ResetSensor resetSightPokemon = new ResetSensor(sightSensorPokemon);
        ResetSensor resetSound = new ResetSensor(soundSensor);
        ResetSensorList resetSensors = new ResetSensorList(new List<ResetSensor>() { resetSight, resetSound, resetSightPokemon});
        //acciones de tiempo
        SetTimer setAlertTime;
        //acciones que modifican la maquina de estados misma
        RemoveStateTransition removeTouchStone;
        RemoveStateTransition removeSawStone;
        RemoveStateTransition removeSawStone2;
        RemoveAction removeDefaultSpriteAction;
        RemoveAction removeRunSpriteAction;
        RemoveAction removeRunSpriteAction2;





        //2. ESTADOS:

        List<Action> entryActions;//aqui iremos guardanndo todas las acciondes de entrada
        List<Action> exitActions;//aqui iremos guardanndo todas las acciones de salida
        List<Action> actions;//aqui guardaremos todas las acciones intermedias

        //2.a estado para esperar sin hacer nada
        //durante este estado eevee estara quieto hasta que algun humano lo haga reaccionar
        entryActions = new List<Action>() {stop, defaultSprite, setZeroRotation, resetSensors};//al entrar al estado debemos parar y sentarnos
        removeDefaultSpriteAction = new RemoveAction(defaultSprite, entryActions);
        actions= new List<Action>(){};//hacer guardia girando
        exitActions= new List<Action>(){} ;
        
        State wait = new State(actions, entryActions, exitActions);


        //2.b estado para sorprenderse
        //durante este estado eevee dara vueltas en alterta angustiado porque escucho algo, este estado durara solo cierto tiempo
        entryActions = new List<Action>() {showExclamation, setDefaultRotation};//al entrar al estado debemos sorprendernos
        actions= new List<Action>(){};
        exitActions= new List<Action>() {disableExclamation, setZeroRotation};//al salir dejamos de sorprendernos
        
        State alert = new State(actions, entryActions, exitActions);

        //2.c estado para perseguir piedra
        //durante este estado eevee se concentrara unicamente en buscar las piedra que vio
        entryActions = new List<Action>() {updateAstarStone, updateFollow, showExclamation, showRunSprite};//al entrar al estado debemos actualizar el a* y luego el camino
        removeRunSpriteAction2 = new RemoveAction(showRunSprite, entryActions);
        actions= new List<Action>() {followPath, lookAction};//durante la accion seguimos el camino
        exitActions= new List<Action>() { disableExclamation};//al salir no hacemos nada
        
        State followStone = new State(actions, entryActions, exitActions);

        //2.d estado para perseguir punto de encuentro
        //durante este estado eevee buscara donde esconderse, puede verse interrumpido si accidentalmente toca una piedra o se comprometio su escondite
        entryActions = new List<Action>() {updateAstarCover, updateFollow, showSweat, showRunSprite, resetSensors};//al entrar al estado debemos actualizar el a* y luego el camino
        removeRunSpriteAction = new RemoveAction(showRunSprite, entryActions);
        actions= new List<Action>() {followPath, lookAction};//durante la accion seguimos el camino
        exitActions= new List<Action>() { disableSweat};//al salir no hacemos nada
        
        State followCoverPoint = new State(actions, entryActions, exitActions);

        //2.extra dummy states
        //estos estados son de relleno para facilitar la activacion de ciertas acciones en le orden adecuado
        entryActions = new List<Action>();//al entrar al estado debemos parar y sentarnos
        actions= new List<Action>();//hacer guardia girando
        exitActions= new List<Action>();//al salir no hacemos nada

        State evolveState1 = new State(actions, entryActions, exitActions);
        State evolveState2 = new State(actions, entryActions, exitActions);



        //3. CONDICIONES:

        SawSomething sawStone = new SawSomething(sightSensor, "Stone");//si vemos una piedra evolutiva
        SawSomething sawHuman = new SawSomething(sightSensor, "Human");//si vemos una persona
        HeardSomething heardHumanClose = new HeardSomething(soundSensor, "Human", 0f);//si escuchamos a un humano cerca
        HeardSomething heardHumanVeryClose = new HeardSomething(soundSensor, "Human", 5f);//si escuchamos a un  humanos muy cerca
        TouchedGameObjects touchedStone = new TouchedGameObjects(stonesList, transform, "Sun");//si tocamos una piedra evolutiva
            evolve = new Evolve2(pokemonData, touchedStone, updateAstarCover, aStar);
        FollowArrived arrived = new FollowArrived(followPath, transform);//si llegamos al objetivo de follow
        PokemonInCoverPoint otherInMyCover = new PokemonInCoverPoint(aStar, sightSensorPokemon, transform);//si vemos que un pokemon se metio en nuestro escondite

        TimeOut alertTimeOut = new TimeOut(5f);
            setAlertTime = new SetTimer(alertTimeOut);
        TrueCondition alwaysTrue = new TrueCondition();

        
        //4. TRANSICIONES:

        List<Action> transitionsActions;
        List<Action> noActions = new List<Action>();

        transitionsActions = new List<Action>(){setAlertTime};
        Transition heardCloseHuman = new Transition(heardHumanClose, transitionsActions, alert);
        Transition seemsSafe = new Transition(alertTimeOut, noActions, wait);
        Transition heardVeryCloseHuman = new Transition(heardHumanVeryClose, noActions, followCoverPoint);

        transitionsActions = new List<Action>(){};
        Transition sawAhuman = new Transition(sawHuman, transitionsActions, followCoverPoint);
        Transition sawAstone = new Transition(sawStone, transitionsActions, followStone); 
        Transition pokemonInMyCover = new Transition(otherInMyCover, transitionsActions, followCoverPoint);

        //transiciones dummy
        transitionsActions = new List<Action>(){evolve};
        Transition evolving1 = new Transition(alwaysTrue, transitionsActions, followCoverPoint);
        Transition evolving2 = new Transition(alwaysTrue, transitionsActions, wait);


        transitionsActions = new List<Action>(){evolve, removeDefaultSpriteAction, removeRunSpriteAction, removeRunSpriteAction2};
        Transition touchStone1 = new Transition(touchedStone, transitionsActions, evolveState1);
        Transition touchStone2 = new Transition(touchedStone, transitionsActions, evolveState2);

        //si evolucionamos debemos quitar las transiciones relacionadas a las stones
        removeSawStone = new RemoveStateTransition(sawAstone,followCoverPoint);
        removeSawStone2 = new RemoveStateTransition(sawAstone, alert);
        removeTouchStone = new RemoveStateTransition(touchStone1, followCoverPoint);
        transitionsActions.Add(removeSawStone);
        transitionsActions.Add(removeSawStone2);
        transitionsActions.Add(removeTouchStone);

        Transition arrivedFollowEnd = new Transition(arrived, noActions, wait);
   
        //4.1 AGREGAMOS TRANSICIONES A ESTADOS

        List<Transition> transitions;
        transitions = new List<Transition>() {sawAhuman, heardCloseHuman};
        wait.transitions = transitions;

        transitions = new List<Transition>() {sawAhuman,sawAstone, heardVeryCloseHuman, seemsSafe};
        alert.transitions = transitions;

        transitions = new List<Transition>() {evolving1};
        evolveState1.transitions = transitions;

        transitions = new List<Transition>() {evolving2};
        evolveState2.transitions = transitions;


        transitions = new List<Transition>() {touchStone1, sawAstone, pokemonInMyCover ,arrivedFollowEnd, sawAhuman};
        followCoverPoint.transitions =  transitions;

        transitions = new List<Transition>() {touchStone2, arrivedFollowEnd, pokemonInMyCover};
        followStone.transitions = transitions;



        //5 MAQUINA DE ESTADOS
        State[] states = new State[] {wait, alert, followCoverPoint, followStone, evolveState1, evolveState2};
        eeveeMachine = new StateMachine(states, wait);

    }

    void Update(){

        eeveeMachine.RunMachine();


    }

    
}