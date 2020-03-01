using UnityEngine;
using System.Collections.Generic;
using System;

public class pidgeotto_state_machine : MonoBehaviour {

    //DATOS PIDGEOTTO
    public static_data pidgeotto;
    Kinetics kinPidgeotto;
    SteeringOutput steeringPidgeotto;

   
    //DATOS DE LOS TRAINERS
    // public static_data trainer;
    // Kinetics kineticsTrainer;
    // public static_data rival;
    // Kinetics kineticsRival;


    //DATOS SEEK
    Seek seek;

    //DATOS GRAFO
    public static_graph graphComponent;//componente que tiene guardado el grafo
    Graph graph;
    PathFindAStar aStar;
    string[] walkable = new string[]{"Earth", "Tree1", "Water"};//esto es para saber sobre que tipos de nodos podemos caminar

   
    //DATOS MAQUINA DE ESTADOS
    StateMachine pidgeottoMachine;
    // public float radiusHide = 30f;//radio para asustarse y meterse bajo tierra
    // public float undergroundCoord = 5f;

    

    void Start(){

        //INICIALIZAMOS LA DATA DEL EXTERIOR
        kinPidgeotto = pidgeotto.kineticsAgent;
        steeringPidgeotto = pidgeotto.steeringAgent;
        // kineticsTrainer = trainer.kineticsAgent;
        // kineticsRival = rival.kineticsAgent;
        graph = graphComponent.graph;
        aStar = new PathFindAStar(graph,null ,null,null, walkable);
        seek = new Seek(kinPidgeotto, kinPidgeotto, pidgeotto.maxspeed);

        // Kinetics[] targets = new Kinetics[2];
        // targets[0]=kineticsTrainer;
        // targets[1]=kineticsRival;

        //obstaculos
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        obstacle_data[] obstaclesData =  new obstacle_data[obstacles.Length];

        for(int k = 0; k < obstacles.Length; k++){
            obstaclesData[k] = obstacles[k].GetComponent<obstacle_data>();
        }

        //COMENZAMOS A CONSTRUIR LA MAQUINA DE ESTADOS

        //1. ACCIONES:

        UpdateAStarRandom updateAStar = new UpdateAStarRandom(aStar, graph, kinPidgeotto, null, walkable);
        FollowPathOfPoints followPath = new FollowPathOfPoints(steeringPidgeotto, seek, null, false);
        UpdateFollowPathWithAstar updateFollow =  new UpdateFollowPathWithAstar(followPath,aStar, obstaclesData);
        

        //2. ESTADOS:

        List<Action> entryActions;//aqui iremos guardanndo todas las acciondes de entrada
        List<Action> exitActions;//aqui iremos guardanndo todas las acciones de salida
        List<Action> actions;//aqui guardaremos todas las acciones intermedias

        //2.a estado para seguir camino

        entryActions = new List<Action>() {};//al entrar al estado ponemos un corazon
        actions= new List<Action>() {followPath};//durante el estado perseguimos al enamorado
        exitActions= new List<Action>() {};//al salir quitamos el corazon

        State followPathState = new State(actions, entryActions, exitActions);

        //3. CONDICIONES:

        // TooClose closeTrainer = new TooClose(kinPidgeotto, kineticsTrainer, radiusHide);
        // TooClose closeRival = new TooClose(kinPidgeotto, kineticsRival, radiusHide);
        ArrivedToPosition arrived = new ArrivedToPosition(new Vector3(), transform, 10f);
        updateAStar.arrived = arrived;

        // OrCondition anyTargetClose = new OrCondition(closeTrainer, closeRival);
        // NotCondition noOneClose = new NotCondition(anyTargetClose);
        
        
        //4. TRANSICIONES:

        List<Action> noActions = new List<Action>();
        List<Action> transitionActions;

        // transitionActions =  new List<Action>(){showHole, underground};
        // Transition anyHumanClose = new Transition(closeAndNotUndeground, transitionActions, followPathState);
        // transitionActions =  new List<Action>(){disableHole, getOutOfGround};
        // Transition noHumanClose =  new Transition(notCloseAndUndeground, transitionActions, followPathState);
        transitionActions =  new List<Action>(){updateAStar, updateFollow};
        Transition pathEnd = new Transition(arrived,transitionActions , followPathState);
      

    
        //4.1 AGREGAMOS TRANSICIONES A ESTADOS
        List<Transition> transitions = new List<Transition>() { pathEnd};
        followPathState.transitions = transitions;

       
        //5 MAQUINA DE ESTADOS
        State[] states = new State[] {followPathState};
        pidgeottoMachine = new StateMachine(states, followPathState);
        updateAStar.DoAction();//esto es para inicializar el primer camino
        updateFollow.DoAction();

    }

    void Update(){

        pidgeottoMachine.RunMachine();
        //Arreglamos el tamanno de pidgeotto de acuerdo a que tan alto va
        float targetSize = Math.Abs(transform.position.z)/2+1;
        transform.localScale = new Vector3(targetSize , targetSize , 1f);

    }

    
}