using UnityEngine;
using System.Collections.Generic;

public class dugtrio_state_machine : MonoBehaviour {

    //DATOS DUGTRIO
    public static_data dugtrio;
    Kinetics kinDugtrio;
    SteeringOutput steeringDugtrio;

   
    //DATOS DE LOS TRAINERS
    public static_data trainer;
    Kinetics kineticsTrainer;
    public static_data rival;
    Kinetics kineticsRival;


    //DATOS SEEK
    Seek seek;

    //DATOS GRAFO
    public static_graph graphComponent;//componente que tiene guardado el grafo
    Graph graph;
    PathFindAStar aStar;
    string[] walkable = new string[]{"Earth"};//esto es para saber sobre que podemos caminar

   
    //DATOS MAQUINA DE ESTADOS
    StateMachine dugtrioMachine;
    public float radiusHide = 30f;//radio para asustarse y meterse bajo tierra
    public float undergroundCoord = 5f;

    

    void Start(){

        //INICIALIZAMOS LA DATA DEL EXTERIOR
        kinDugtrio = dugtrio.kineticsAgent;
        steeringDugtrio = dugtrio.steeringAgent;
        kineticsTrainer = trainer.kineticsAgent;
        kineticsRival = rival.kineticsAgent;
        graph = graphComponent.graph;
        aStar = new PathFindAStar(graph,null ,null,null, walkable);
        seek = new Seek(kinDugtrio, kinDugtrio, dugtrio.maxspeed);

        Kinetics[] targets = new Kinetics[2];
        targets[0]=kineticsTrainer;
        targets[1]=kineticsRival;

        //obstaculos
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        obstacle_data[] obstaclesData =  new obstacle_data[obstacles.Length];

        for(int k = 0; k < obstacles.Length; k++){
            obstaclesData[k] = obstacles[k].GetComponent<obstacle_data>();
        }

        //COMENZAMOS A CONSTRUIR LA MAQUINA DE ESTADOS

        //1. ACCIONES:

        UpdateAStarRandom updateAStar = new UpdateAStarRandom(aStar, graph, kinDugtrio, null, walkable);
        ShowIcon showHole = new ShowIcon(this.gameObject, "Digging");
        DisableIcon disableHole  = new DisableIcon(this.gameObject, "Digging");
        FollowPathOfPoints followPath = new FollowPathOfPoints(steeringDugtrio, seek, null, true);
        UpdateFollowPathWithAstar updateFollow =  new UpdateFollowPathWithAstar(followPath,aStar, obstaclesData);
        MoveOnZ underground = new MoveOnZ(transform, undergroundCoord);
        MoveOnZ getOutOfGround = new MoveOnZ(transform, 0f);

        //2. ESTADOS:

        List<Action> entryActions;//aqui iremos guardanndo todas las acciondes de entrada
        List<Action> exitActions;//aqui iremos guardanndo todas las acciones de salida
        List<Action> actions;//aqui guardaremos todas las acciones intermedias

        //2.a estado para seguir camino

        entryActions = new List<Action>() {};
        actions= new List<Action>() {followPath};//durante el estado seguimos el camino
        exitActions= new List<Action>() {};
        
        State followPathState = new State(actions, entryActions, exitActions);

        //3. CONDICIONES:

        TooClose closeTrainer = new TooClose(kinDugtrio, kineticsTrainer, radiusHide);
        TooClose closeRival = new TooClose(kinDugtrio, kineticsRival, radiusHide);
        ArrivedToPosition arrived = new ArrivedToPosition(new Vector3(), transform, 10f);
        AmIUnderground amIunderground = new AmIUnderground(transform);
        updateAStar.arrived = arrived;

        OrCondition anyTargetClose = new OrCondition(closeTrainer, closeRival);
        NotCondition noOneClose = new NotCondition(anyTargetClose);
        NotCondition notUnderground = new NotCondition(amIunderground);
        AndCondition closeAndNotUndeground = new AndCondition(anyTargetClose, notUnderground);
        AndCondition notCloseAndUndeground = new AndCondition(noOneClose, amIunderground);
       

        List<Action> noActions = new List<Action>();
        List<Action> transitionActions;
        //4. TRANSICIONES:
        transitionActions =  new List<Action>(){showHole, underground};
        Transition anyHumanClose = new Transition(closeAndNotUndeground, transitionActions, followPathState);
        transitionActions =  new List<Action>(){disableHole, getOutOfGround};
        Transition noHumanClose =  new Transition(notCloseAndUndeground, transitionActions, followPathState);
        transitionActions =  new List<Action>(){updateAStar, updateFollow};
        Transition pathEnd = new Transition(arrived,transitionActions , followPathState);
      

    
        //4.1 AGREGAMOS TRANSICIONES A ESTADOS
        List<Transition> transitions = new List<Transition>() {anyHumanClose, noHumanClose, pathEnd};
        followPathState.transitions = transitions;

       
        //5 MAQUINA DE ESTADOS
        State[] states = new State[] {followPathState};
        dugtrioMachine = new StateMachine(states, followPathState);
        updateAStar.DoAction();//esto es para inicializar el primer camino
        updateFollow.DoAction();

    }

    void Update(){

        dugtrioMachine.RunMachine();

    }

    
}