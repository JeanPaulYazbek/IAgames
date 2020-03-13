using UnityEngine;
using System.Collections.Generic;

public class octillery_machine : MonoBehaviour {

    //DATOS octillery
    public static_data octillery;
    Kinetics kinOctillery;
    float defaulAngularSpeed;
    SteeringOutput steeringOctillery;

    //DATOS SENSORES
    public sight_sensor sightSensorScript;
    SightSensor sightSensor;
   
    //DATOS DE LOS TRAINERS
    public static_data trainer;
    Kinetics kineticsTrainer;
    public static_data rival;
    Kinetics kineticsRival;

    //DATOS PARA PODER DISPARAR TINTA

    public GameObject inkBallPrefab;
    public float inkSpeed;
    public GameObject inkScreen;
    Transform[] obstacles;

   
    //DATOS MAQUINA DE ESTADOS
    StateMachine octilleryMachine;
    

    
    void Start(){

        //INICIALIZAMOS LA DATA DEL EXTERIOR
        kinOctillery = octillery.kineticsAgent;
        steeringOctillery = octillery.steeringAgent;
        defaulAngularSpeed = kinOctillery.rotation; 
        kineticsTrainer = trainer.kineticsAgent;
        kineticsRival = rival.kineticsAgent;
        sightSensor = sightSensorScript.sensor;

        Kinetics[] targets = new Kinetics[2];
        targets[0]=kineticsTrainer;
        targets[1]=kineticsRival;

        //obstacles
        List<Transform> obstacles_list = new List<Transform>();
        GameObject[] obstaclesGo = GameObject.FindGameObjectsWithTag("Obstacle");
        for (int i = 0; i<obstaclesGo.Length; i++){
            obstacles_list.Add(obstaclesGo[i].GetComponent<Transform>());           
        }

        obstacles = obstacles_list.ToArray();


        //COMENZAMOS A CONSTRUIR LA MAQUINA DE ESTADOS

        //1. ACCIONES:

        ShowIcon showExclamation = new ShowIcon(this.gameObject, "Exclamation");
        DisableIcon disableExclamation = new DisableIcon(this.gameObject, "Exclamation");
        SetAngularSpeed setAngularToZero = new SetAngularSpeed(kinOctillery, 0f);
        SetAngularSpeed setAngularToDefault = new SetAngularSpeed(kinOctillery, defaulAngularSpeed);
        SetAngularAccel setAngularAccelToZero = new SetAngularAccel(steeringOctillery, 0f);
        ResetSensor resetSightSensor= new ResetSensor(sightSensor);
        FaceToSeenTarget faceTarget =  new FaceToSeenTarget(sightSensor, kinOctillery, steeringOctillery);
        ShootInk shootInkToTarget = new ShootInk(transform, inkBallPrefab, inkSpeed, inkScreen, sightSensor, targets, obstacles);


        //2. ESTADOS:

        List<Action> entryActions;//aqui iremos guardanndo todas las acciondes de entrada
        List<Action> exitActions;//aqui iremos guardanndo todas las acciones de salida
        List<Action> actions;//aqui guardaremos todas las acciones intermedias

        //2.a estado para seguir camino

        entryActions = new List<Action>(){setAngularToDefault, setAngularAccelToZero};
        actions= new List<Action>();
        exitActions= new List<Action>();

        State guardState = new State(actions, entryActions, exitActions);

        entryActions = new List<Action>() { showExclamation, setAngularToZero};
        actions= new List<Action>() {faceTarget, shootInkToTarget};
        exitActions= new List<Action>() { disableExclamation};

        State shootState = new State(actions, entryActions, exitActions);

        //3. CONDICIONES:

        // octillery solo disparara si ve un humano
        SawSomething sawHuman = new SawSomething(sightSensor, "Human");

        // octillery solo entrara en guardia de nuevo si no hay humanos en su cono de vision
        NoInSightCone noHumanInCone = new NoInSightCone(sightSensor);
       

        //4. TRANSICIONES:

        List<Action> noActions = new List<Action>();
        List<Action> transitionsActions;


        transitionsActions = new List<Action>(){resetSightSensor};   
        Transition noHumanInConeSight = new Transition(noHumanInCone, transitionsActions, guardState);

        Transition humanSeen =  new Transition(sawHuman, noActions, shootState);
    
    
        //4.1 AGREGAMOS TRANSICIONES A ESTADOS
        List<Transition> transitions = new List<Transition>() {humanSeen};
        guardState.transitions = transitions;

        transitions = new List<Transition>() {noHumanInConeSight};
        shootState.transitions = transitions;

       
        //5 MAQUINA DE ESTADOS
        State[] states = new State[] {guardState, shootState};
        octilleryMachine = new StateMachine(states, guardState);

    }

    void Update(){

        octilleryMachine.RunMachine();

    }

    
}