using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trainer_status_machine : MonoBehaviour
{

    //DATOS TRAINER
    public static_data trainerStaticData;

    //DATOS SENSORES
    public smell_sensor smellSensorScript;
    SmellSensor smellSensor;

    //DATOS VENENO
    public float poisonedSpeed = 2f;
    public float poisonTimer = 60f;


    //DATOS MAQUINA DE ESTADOS
    StateMachine trainerStatusMachine;

    // Start is called before the first frame update
    void Start()
    {
        //DATOS EXTERNOS
        smellSensor = smellSensorScript.sensor;

        //COMENZAMOS A CONSTRUIR LA MAQUINA DE ESTADOS

        //1. ACCIONES:
        ShowIcon showPoisoned = new ShowIcon(this.gameObject, "Poisoned");
        DisableIcon disablePoisoned = new DisableIcon(this.gameObject, "Poisoned");
        SetTimer setPoisonClock;
        ResetSensor resetSmellSensor= new ResetSensor(smellSensor);
        UpdateMaxSpeed setOriginalSpeed = new UpdateMaxSpeed(trainerStaticData, trainerStaticData.maxspeed);
        UpdateMaxSpeed setSlowSpeed = new UpdateMaxSpeed(trainerStaticData, poisonedSpeed);
        
        //2. ESTADOS:

        List<Action> entryActions;//aqui iremos guardanndo todas las acciondes de entrada
        List<Action> exitActions;//aqui iremos guardanndo todas las acciones de salida
        List<Action> actions;//aqui guardaremos todas las acciones intermedias

        //2.a estado donde estamos sanos

        entryActions = new List<Action>();//al entrar al estado ponemos un corazon
        actions= new List<Action>();//durante el estado perseguimos al enamorado
        exitActions= new List<Action>();//al salir quitamos el corazon

        State healthyState = new State(actions, entryActions, exitActions);

        //2.b estado donde estamos envenenados

        entryActions = new List<Action>() {showPoisoned, setSlowSpeed};//al entrar al estado ponemos un corazon
        actions= new List<Action>();//durante el estado perseguimos al enamorado
        exitActions= new List<Action>() {disablePoisoned, resetSmellSensor, setOriginalSpeed};//al salir quitamos el corazon

        State poisonedState = new State(actions, entryActions, exitActions);

        //3. CONDICIONES:
        SmelledPoison smelledPoison = new SmelledPoison(smellSensor);
        TimeOut poisonClock = new TimeOut(poisonTimer);
        setPoisonClock = new SetTimer(poisonClock);

        
        //4. TRANSICIONES:

        List<Action> noActions = new List<Action>();
        List<Action> transitionActions;


        transitionActions =  new List<Action>(){setPoisonClock};
        Transition gotPoisoned = new Transition(smelledPoison,transitionActions , poisonedState);

        Transition poisonTimeOut = new Transition(poisonClock,noActions , healthyState);
      
    
        //4.1 AGREGAMOS TRANSICIONES A ESTADOS
        List<Transition> transitions;

        transitions =  new List<Transition>() {gotPoisoned};;
        healthyState.transitions = transitions;

        poisonedState.transitions = new List<Transition>(){poisonTimeOut};

        //5 MAQUINA DE ESTADOS
        State[] states = new State[] {healthyState, poisonedState};
        trainerStatusMachine = new StateMachine(states, healthyState);
        


         
    }

    // Update is called once per frame
    void Update()
    {
        trainerStatusMachine.RunMachine();
        
    }
}
