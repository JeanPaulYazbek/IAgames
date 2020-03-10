using UnityEngine;
using System.Collections.Generic;

public class roselia_machine : MonoBehaviour {

    //DATOS ROSELIA
    public static_data roselia;
    public send_smell sendSmell;
    Kinetics kinRoselia;
    SteeringOutput steeringRoselia;

   
    //DATOS DE LOS TRAINERS
    public static_data trainer;
    Kinetics kineticsTrainer;
    public static_data rival;
    Kinetics kineticsRival;

   
    //DATOS MAQUINA DE ESTADOS
    StateMachine roseliaMachine;
    public float radiusChange = 5f;//radio para asustarse y cambiar gas venenoso

    
    void Start(){

        //INICIALIZAMOS LA DATA DEL EXTERIOR
        kinRoselia = roselia.kineticsAgent;
        steeringRoselia = roselia.steeringAgent;
        kineticsTrainer = trainer.kineticsAgent;
        kineticsRival = rival.kineticsAgent;

        Kinetics[] targets = new Kinetics[2];
        targets[0]=kineticsTrainer;
        targets[1]=kineticsRival;


        //COMENZAMOS A CONSTRUIR LA MAQUINA DE ESTADOS

        //1. ACCIONES:

        ShowIcon showSweet = new ShowIcon(this.gameObject, "SweetGas");
        DisableIcon disableSweet = new DisableIcon(this.gameObject, "SweetGas");
        ShowIcon showPoison = new ShowIcon(this.gameObject, "PoisonGas");
        DisableIcon disablePoison = new DisableIcon(this.gameObject, "PoisonGas");
        ChangeSmellSent changeSmellPoison = new ChangeSmellSent(sendSmell, "Poison");
        ChangeSmellSent changeSmellSweet = new ChangeSmellSent(sendSmell, "Sweet");
        SetTimer setPoisonTimer;


        //2. ESTADOS:

        List<Action> entryActions;//aqui iremos guardanndo todas las acciondes de entrada
        List<Action> exitActions;//aqui iremos guardanndo todas las acciones de salida
        List<Action> actions;//aqui guardaremos todas las acciones intermedias

        //2.a estado para seguir camino

        entryActions = new List<Action>(){showSweet, changeSmellSweet};
        actions= new List<Action>();
        exitActions= new List<Action>(){disableSweet};

        State sweetGasState = new State(actions, entryActions, exitActions);

        entryActions = new List<Action>() { showPoison, changeSmellPoison};
        actions= new List<Action>();
        exitActions= new List<Action>() { disablePoison};

        State poisonGasState = new State(actions, entryActions, exitActions);

        //3. CONDICIONES:

        // roselia solo usara veneno durante 30 segs
        TimeOut poisonTimer = new TimeOut(30f);
        setPoisonTimer = new SetTimer(poisonTimer);

        TooClose closeTrainer = new TooClose(kinRoselia, kineticsTrainer, radiusChange);
        TooClose closeRival = new TooClose(kinRoselia, kineticsRival, radiusChange);

        OrCondition anyTargetClose = new OrCondition(closeTrainer, closeRival);
        NotCondition noOneClose = new NotCondition(anyTargetClose);
       

        //4. TRANSICIONES:

        List<Action> noActions = new List<Action>();
        List<Action> transitionActions;
        
        transitionActions =  new List<Action>(){setPoisonTimer};
        Transition anyHumanClose = new Transition(anyTargetClose, transitionActions, poisonGasState);
        transitionActions =  new List<Action>();
        Transition poisonTimeOut =  new Transition(poisonTimer, transitionActions, sweetGasState);
    
    
        //4.1 AGREGAMOS TRANSICIONES A ESTADOS
        List<Transition> transitions = new List<Transition>() {anyHumanClose};
        sweetGasState.transitions = transitions;

        transitions = new List<Transition>() {poisonTimeOut};
        poisonGasState.transitions = transitions;

       
        //5 MAQUINA DE ESTADOS
        State[] states = new State[] {sweetGasState, poisonGasState};
        roseliaMachine = new StateMachine(states, sweetGasState);

    }

    void Update(){

        roseliaMachine.RunMachine();

    }

    
}