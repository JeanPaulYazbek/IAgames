﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trainer_status_machine : MonoBehaviour
{

    //DATOS TRAINER
    public static_data trainerStaticData;
    public List<MonoBehaviour> scriptsToDisable;//scripts para deshabilitar cuando estemos dormidos
    public Kinetics kinTrainer;
    public SteeringOutput steeringTrainer;

    //DATOS SENSORES
    public smell_sensor smellSensorScript;
    SmellSensor smellSensor;

    public sound_sensor soundSensorScript;
    SoundSensor soundSensor;

    //DATOS VENENO
    public float poisonedSpeed = 2f;
    public float poisonTimer = 60f;
    public float sleepTimer = 10f;


    //DATOS MAQUINA DE ESTADOS
    StateMachine trainerStatusMachine;

    // Start is called before the first frame update
    void Start()
    {
        //DATOS EXTERNOS
        smellSensor = smellSensorScript.sensor;
        soundSensor = soundSensorScript.sensor;
        kinTrainer = trainerStaticData.kineticsAgent;
        steeringTrainer = trainerStaticData.steeringAgent;

        //COMENZAMOS A CONSTRUIR LA MAQUINA DE ESTADOS

        //1. ACCIONES:
        ShowIcon showPoisoned = new ShowIcon(this.gameObject, "Poisoned");
        DisableIcon disablePoisoned = new DisableIcon(this.gameObject, "Poisoned");
        ShowIcon showSleep = new ShowIcon(this.gameObject, "Sleeping");
        DisableIcon disableSleep = new DisableIcon(this.gameObject, "Sleeping");
        SetTimer setPoisonClock;
        SetTimer setSleepClock;
        ResetSensor resetSmellSensor= new ResetSensor(smellSensor);
        ResetSensor resetSoundSensor = new ResetSensor(soundSensor);

        List<ResetSensor> resets = new List<ResetSensor>(){resetSmellSensor, resetSoundSensor};
        ResetSensorList resetAllSensor = new ResetSensorList(resets);

        UpdateMaxSpeed setOriginalSpeed = new UpdateMaxSpeed(trainerStaticData, trainerStaticData.maxspeed);
        UpdateMaxSpeed setSlowSpeed = new UpdateMaxSpeed(trainerStaticData, poisonedSpeed);
        UpdateMaxSpeed setZeroSpeed = new UpdateMaxSpeed(trainerStaticData, 0f);
        SetAngularSpeed setAngularToZero = new SetAngularSpeed(kinTrainer, 0f);
        SetAngularAccel setAngularAccelToZero = new SetAngularAccel(steeringTrainer, 0f);


        //accion para deshabilitar todos scripts de interes mientras dormimos
        List<DisableScript> disables = new List<DisableScript>();
        foreach(var script in scriptsToDisable){
            disables.Add(new DisableScript(script));
        }
        DisableScriptList disableScripts = new DisableScriptList(disables);

        //accion para habilitar todos los script de interes si despertamos
        List<EnableScript> enables = new List<EnableScript>();
        foreach(var script in scriptsToDisable){
            enables.Add(new EnableScript(script));
        }
        EnableScriptList enableScripts = new EnableScriptList(enables);

        
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
        exitActions= new List<Action>() {disablePoisoned, resetAllSensor, setOriginalSpeed};//al salir quitamos el corazon

        State poisonedState = new State(actions, entryActions, exitActions);

        //2.c estado donde estamos dormidos

        entryActions = new List<Action>() {showSleep, setZeroSpeed, disableScripts, setAngularAccelToZero, setAngularToZero};//al entrar al estado ponemos un corazon
        actions= new List<Action>();//durante el estado perseguimos al enamorado
        //al salir debemos:
        // quitar el icono de dormir, resetear el sensor de sonido, reset el sensor de aroma porque puede haber un sweet 
        // volver a habilitar lo que se deba
        exitActions= new List<Action>() {disableSleep, resetAllSensor, setOriginalSpeed, enableScripts};//al salir quitamos el corazon

        State sleepState = new State(actions, entryActions, exitActions);

        //3. CONDICIONES:
        SmelledSomething smelledPoison = new SmelledSomething(smellSensor, "Poison");
        SmelledSomething smelledSweet = new SmelledSomething(smellSensor, "Sweet");
        HeardSleepSong heardSong = new HeardSleepSong(soundSensor);
        TimeOut poisonClock = new TimeOut(poisonTimer);
        setPoisonClock = new SetTimer(poisonClock);
        TimeOut sleepClock = new TimeOut(sleepTimer);
        setSleepClock = new SetTimer(sleepClock);

        

        
        //4. TRANSICIONES:

        List<Action> noActions = new List<Action>();
        List<Action> transitionActions;


        transitionActions =  new List<Action>(){setPoisonClock};
        Transition gotPoisoned = new Transition(smelledPoison,transitionActions , poisonedState);
        Transition poisonTimeOut = new Transition(poisonClock,noActions , healthyState);
        Transition sweetScent = new Transition(smelledSweet, noActions, healthyState);

        transitionActions = new List<Action>(){setSleepClock};
        Transition gotSlept = new Transition(heardSong, transitionActions, sleepState);
        Transition sleepTimeOut = new Transition(sleepClock,noActions , healthyState);


      
    
        //4.1 AGREGAMOS TRANSICIONES A ESTADOS
        List<Transition> transitions;

        transitions =  new List<Transition>() {gotPoisoned, gotSlept};;
        healthyState.transitions = transitions;

        poisonedState.transitions = new List<Transition>(){poisonTimeOut, sweetScent};

        sleepState.transitions = new List<Transition>(){sleepTimeOut, sweetScent};

        //5 MAQUINA DE ESTADOS
        State[] states = new State[] {healthyState, poisonedState, sleepState};
        trainerStatusMachine = new StateMachine(states, healthyState);
        


         
    }

    // Update is called once per frame
    void Update()
    {
        trainerStatusMachine.RunMachine();
        
    }
}
