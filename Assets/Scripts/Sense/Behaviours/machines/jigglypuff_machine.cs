using UnityEngine;
using System.Collections.Generic;
using System;

public class jigglypuff_machine : MonoBehaviour {

    //DATOS jigglypuff
    public static_data jigglypuff;
    public pokemon_data jigglypuffData;
    public send_sound soundComp;
    Kinetics kinJigglypuff;
    SteeringOutput steeringJigglypuff;


    //DATOS SEEK
    Seek seek;

    //DATOS GRAFO
    public static_graph graphComponent;//componente que tiene guardado el grafo
    Graph graph;
    PathFindAStar aStar;
    string[] walkable = new string[]{"Earth", "Tree1"};//esto es para saber sobre que tipos de nodos podemos caminar

   
    //DATOS MAQUINA DE ESTADOS
    StateMachine jigglypuffMachine;

    

    

    void Start(){

        //INICIALIZAMOS LA DATA DEL EXTERIOR
        kinJigglypuff = jigglypuff.kineticsAgent;
        steeringJigglypuff = jigglypuff.steeringAgent;
        
        graph = graphComponent.graph;
        aStar = new PathFindAStar(graph,null ,null,null, walkable);
        seek = new Seek(kinJigglypuff, kinJigglypuff, jigglypuff.maxspeed);


        //obstaculos
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        obstacle_data[] obstaclesData =  new obstacle_data[obstacles.Length];

        for(int k = 0; k < obstacles.Length; k++){
            obstaclesData[k] = obstacles[k].GetComponent<obstacle_data>();
        }

        //COMENZAMOS A CONSTRUIR LA MAQUINA DE ESTADOS

        //1. ACCIONES:

        UpdateAStarRandom updateAStar = new UpdateAStarRandom(aStar, graph, kinJigglypuff, null, walkable);
        FollowPathOfPoints followPath = new FollowPathOfPoints(steeringJigglypuff, seek, null, false);
        UpdateFollowPathWithAstar updateFollow =  new UpdateFollowPathWithAstar(followPath,aStar, obstaclesData);
        ShowFlySprite showFlySprite = new ShowFlySprite(jigglypuffData);
        ShowDefaultSprite showDefaultSprie = new ShowDefaultSprite(jigglypuffData);
        EnableSendSound enableSound = new EnableSendSound(soundComp);
        DisableSendSound disableSound = new DisableSendSound(soundComp);
        

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

        ArrivedToPosition arrived = new ArrivedToPosition(new Vector3(), transform, 10f);
        updateAStar.arrived = arrived;

        TargetPointAir nextIsAir = new TargetPointAir(transform);
        
        TargetPointEarth nextIsEarth  = new TargetPointEarth(transform);
        
        
        //4. TRANSICIONES:

        List<Action> noActions = new List<Action>();
        List<Action> transitionActions;


        transitionActions =  new List<Action>(){updateAStar, updateFollow};
        Transition pathEnd = new Transition(arrived,transitionActions , followPathState);

        transitionActions =  new List<Action>(){showFlySprite, disableSound};
        Transition toFly = new Transition(nextIsAir,transitionActions , followPathState);

        transitionActions =  new List<Action>(){showDefaultSprie, enableSound};
        Transition toGround = new Transition(nextIsEarth,transitionActions , followPathState);
      

    
        //4.1 AGREGAMOS TRANSICIONES A ESTADOS
        List<Transition> transitions = new List<Transition>() { pathEnd, toFly, toGround};
        followPathState.transitions = transitions;

       
        //5 MAQUINA DE ESTADOS
        State[] states = new State[] {followPathState};
        jigglypuffMachine = new StateMachine(states, followPathState);
        updateAStar.DoAction();//esto es para inicializar el primer camino
        updateFollow.DoAction();

    }

    void Update(){

        jigglypuffMachine.RunMachine();
        //Arreglamos el tamanno de pidgeotto de acuerdo a que tan alto va
        float targetSize = Math.Abs(transform.position.z)/2+1;
        transform.localScale = new Vector3(targetSize , targetSize , 1f);

        
        

    }

    
}