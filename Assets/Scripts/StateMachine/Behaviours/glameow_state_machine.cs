using UnityEngine;
using System.Collections.Generic;

public class glameow_state_machine : MonoBehaviour {

    //DATOS GLAMEOW
    public static_data glameow;
    Kinetics kinGlameow;
    SteeringOutput steeringGlameow;

    //DATOS DEL TARGET (meowth en este caso)
    public static_data target;
    Kinetics kineticsTarget;

    //DATOS DE LOS TRAINERS
    public static_data trainer;
    Kinetics kineticsTrainer;
    public static_data rival;
    Kinetics kineticsRival;


    //DATOS SEEK
    public float maxSeekAccel = 10f;

    //DATOS ARRIVE
    public float targetRadiusArrive = 1f;
    public float slowRadiusArrive = 30f;
    public float MaxAccelerationArrive = 20f;

    //DATOS MAQUINA DE ESTADOS
    StateMachine glameowMachine;
    public float radiusAlert = 20f;//radio para alertarse
    public float radiusRun = 15f;//radio para huir

    void Start(){

        //INICIALIZAMOS LA DATA DEL EXTERIOR
        kinGlameow = glameow.kineticsAgent;
        steeringGlameow = glameow.steeringAgent;
        kineticsTarget = target.kineticsAgent;
        kineticsTrainer = trainer.kineticsAgent;
        kineticsRival = rival.kineticsAgent;

        //COMENZAMOS A CONSTRUIR LA MAQUINA DE ESTADOS

        //1. ACCIONES:

        //este es seek pero en verdad como meowth le hace seek tambien pareciera que huye
        FollowTarget seekTarget = new FollowTarget(steeringGlameow, kinGlameow, kineticsTarget, maxSeekAccel);
        //el target es null porque todavia no sabemos cual trainer se  acercara
        ArriveToTarget arriveTrainer = new ArriveToTarget(steeringGlameow, kinGlameow, null, MaxAccelerationArrive,glameow.maxspeed, targetRadiusArrive, slowRadiusArrive);

        Kinetics[] targets = new Kinetics[2];
        targets[0]=kineticsTrainer;
        targets[1]=kineticsRival;

        SetArriveTarget setTrainer = new SetArriveTarget(targets, arriveTrainer);
        //seek closes
        StopMoving stop = new StopMoving(kinGlameow, steeringGlameow);
        ShowIcon showHeart = new ShowIcon(this.gameObject, "Heart");
        DisableIcon disableHeart = new DisableIcon(this.gameObject, "Heart");
        ShowIcon showSweat = new ShowIcon(this.gameObject, "Sweat");
        DisableIcon disableSweat = new DisableIcon(this.gameObject, "Sweat");
        ShowIcon showExclamation = new ShowIcon(this.gameObject, "Exclamation");
        DisableIcon disableExclamation = new DisableIcon(this.gameObject, "Exclamation");


        //2. ESTADOS:

        List<Action> entryActions;//aqui iremos guardanndo todas las acciondes de entrada
        List<Action> exitActions;//aqui iremos guardanndo todas las acciones de salida
        List<Action> actions;//aqui guardaremos todas las acciones intermedias

        //2.a estado para huir de anamorado (meowth)

        entryActions = new List<Action>() {showSweat};//al entrar al estado ponemos un corazon
        actions= new List<Action>() {seekTarget};//durante el estado perseguimos al enamorado
        exitActions= new List<Action>() {disableSweat};//al salir quitamos el corazon

        State stalked = new State(actions, entryActions, exitActions);


        //2.b estado para alertarse de entrenador cercano

        entryActions= new List<Action>() {showExclamation, stop};//al entrar al estado debemos mostrar un signo de exclamacion
        actions = new List<Action>();
        exitActions=new List<Action>() {disableExclamation};//al salir quitamos el signo

        
        State alert = new State(actions, entryActions, exitActions);


        //2.c estado para perseguir enamorado al entrenador 

        entryActions = new List<Action>{setTrainer, showHeart};
        actions = new List<Action>() {arriveTrainer};
        exitActions = new List<Action>{disableHeart};
        

        State stalkTrainer = new State(actions, entryActions, exitActions);



        //3. CONDICIONES:

        TooClose closeTrainer = new TooClose(kinGlameow, kineticsTrainer, radiusAlert);
        TooClose veryCloseTrainer = new TooClose(kinGlameow, kineticsTrainer, radiusRun); 
        TooClose closeRival = new TooClose(kinGlameow, kineticsRival, radiusAlert);
        TooClose veryCloseRival = new TooClose(kinGlameow, kineticsRival, radiusRun); 
       

        //Estas son las que de verdad necesitamos
        OrCondition anyTargetClose = new OrCondition(closeTrainer, closeRival);
        OrCondition anyTargetVeryClose = new OrCondition(veryCloseRival, veryCloseTrainer);
        NotCondition noOneClose = new NotCondition(anyTargetClose);
       

        List<Action> noActions = new List<Action>();
        //4. TRANSICIONES:
        Transition anyHumanClose = new Transition(anyTargetClose, noActions, alert);
        Transition noHumanClose =  new Transition(noOneClose, noActions, stalkTrainer);
        Transition anyHumanVeryClose = new Transition(anyTargetVeryClose, noActions, stalkTrainer);
      

    
        //4.1 AGREGAMOS TRANSICIONES A ESTADOS
        List<Transition> transitions = new List<Transition>() {anyHumanClose};
        stalked.transitions = transitions;

        transitions = new List<Transition>() {noHumanClose, anyHumanVeryClose};
        alert.transitions = transitions;

        transitions = new List<Transition>() ;
        stalkTrainer.transitions = transitions;

        //5 MAQUINA DE ESTADOS
        State[] states = new State[] {stalked, alert, stalkTrainer};
        glameowMachine = new StateMachine(states, stalked);

    }

    void Update(){

        glameowMachine.RunMachine();

    }

    
}