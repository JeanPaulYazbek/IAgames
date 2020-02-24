using UnityEngine;
using System.Collections.Generic;

public class meowth_state_machine : MonoBehaviour {

    //DATOS MEOWTH
    public static_data meowth;
    Kinetics kinMeowth;
    SteeringOutput steeringMeowth;

    //DATOS DEL TARGET (glameow en este caso)
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
    StateMachine meowthMachine;
    public float radiusAlert = 20f;//radio para alertarse
    public float radiusRun = 15f;//radio para huir

    void Start(){

        //INICIALIZAMOS LA DATA DEL EXTERIOR
        kinMeowth = meowth.kineticsAgent;
        steeringMeowth = meowth.steeringAgent;
        kineticsTarget = target.kineticsAgent;
        kineticsTrainer = trainer.kineticsAgent;
        kineticsRival = rival.kineticsAgent;

        //COMENZAMOS A CONSTRUIR LA MAQUINA DE ESTADOS

        //1. ACCIONES:

        FollowTarget seekTarget = new FollowTarget(steeringMeowth, kinMeowth, kineticsTarget, maxSeekAccel);
        FollowTarget seekWorried = new FollowTarget(steeringMeowth, kinMeowth, kineticsTarget, 100f);

        Kinetics[] targets = new Kinetics[2];
        targets[0]=kineticsTrainer;
        targets[1]=kineticsRival;
        RunFromTargets runFromTargets = new RunFromTargets(steeringMeowth, kinMeowth, targets, maxSeekAccel*5);
        StopMoving stop = new StopMoving(kinMeowth, steeringMeowth);
        UpdateMaxSpeed moreMaxSpeed = new UpdateMaxSpeed(meowth, meowth.maxspeed*5); // esto ayudara a aumentar la maxspeed
        UpdateMaxSpeed speedBackToNormal = new UpdateMaxSpeed(meowth, meowth.maxspeed);// esto guarda la maxspeed original para volverla a poner asi
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

        //2.a estado para perseguir enamorado (glameow)

        entryActions = new List<Action>() {showHeart};//al entrar al estado ponemos un corazon
        actions= new List<Action>() {seekTarget};//durante el estado perseguimos al enamorado
        exitActions= new List<Action>() {disableHeart};//al salir quitamos el corazon

        State stalkTarget = new State(actions, entryActions, exitActions);


        //2.b estado para alertarse de entrenador cercano

        entryActions= new List<Action>() {showExclamation, stop};//al entrar al estado debemos mostrar un signo de exclamacion
        actions = new List<Action>();
        exitActions=new List<Action>() {disableExclamation};//al salir quitamos el signo

        
        State alert = new State(actions, entryActions, exitActions);

        //2,c estado para perseguir sin corazon
        entryActions = new List<Action>() ;//al entrar al estado ponemos un corazon
        actions= new List<Action>() {seekTarget};//durante el estado perseguimos al enamorado
        exitActions= new List<Action>() ;//al salir quitamos el corazon

        State stalk = new State(actions, entryActions, exitActions);


        //2.d estado para huir del entrenador

        entryActions = new List<Action>() {moreMaxSpeed};
        actions = new List<Action>() {runFromTargets};
        exitActions = new List<Action>() {speedBackToNormal};
        

        State runAway = new State(actions, entryActions, exitActions);

        //2.e estado para preocuparse por alguien y seguirlo preocupado


        entryActions = new List<Action>() {showSweat, disableHeart, disableExclamation};
        actions = new List<Action>() {seekWorried};
        exitActions = new List<Action>() {disableSweat};

        State worry = new State(actions, entryActions, exitActions);


        //3. CONDICIONES:

        TooClose closeTrainer = new TooClose(kinMeowth, kineticsTrainer, radiusAlert);
        TooClose veryCloseTrainer = new TooClose(kinMeowth, kineticsTrainer, radiusRun); 
        TooClose closeRival = new TooClose(kinMeowth, kineticsRival, radiusAlert);
        TooClose veryCloseRival = new TooClose(kinMeowth, kineticsRival, radiusRun); 
       

        //Estas son las que de verdad necesitamos
        OrCondition anyTargetClose = new OrCondition(closeTrainer, closeRival);
        OrCondition anyTargetVeryClose = new OrCondition(veryCloseRival, veryCloseTrainer);
        NotCondition noOneClose = new NotCondition(anyTargetClose);
        NotCondition noOneVeryClose = new NotCondition(anyTargetVeryClose);
        WasCaught targetCaught = new WasCaught(kineticsTarget);


        List<Action> noActions = new List<Action>();
        //4. TRANSICIONES:
        Transition anyHumanClose = new Transition(anyTargetClose, noActions, alert);
        Transition noHumanClose =  new Transition(noOneClose, noActions, stalk);
        Transition anyHumanVeryClose = new Transition(anyTargetVeryClose, noActions, runAway);
        Transition noHumanVeryClose = new Transition(noOneVeryClose, noActions, alert);
        Transition targetWasCaught = new Transition(targetCaught, noActions, worry);
    
        //4.1 AGREGAMOS TRANSICIONES A ESTADOS
        List<Transition> transitions = new List<Transition>() {anyHumanClose, targetWasCaught};
        stalkTarget.transitions = transitions;

        transitions = new List<Transition>() {noHumanClose, anyHumanVeryClose, targetWasCaught};
        alert.transitions = transitions;

        transitions =  new List<Transition>() {anyHumanClose, targetWasCaught};
        stalk.transitions = transitions;

        transitions = new List<Transition>() {noHumanVeryClose, targetWasCaught};
        runAway.transitions = transitions;

        worry.transitions = new List<Transition>();//es un sumidero

        //5 MAQUINA DE ESTADOS
        State[] states = new State[] {stalkTarget, alert, stalk, runAway, worry};
        meowthMachine = new StateMachine(states, stalkTarget);

    }

    void Update(){

        meowthMachine.RunMachine();

    }

    
}