using UnityEngine;

public class meowth_state_machine : MonoBehaviour {

    //DATOS MEOWTH
    public static_data meowth;
    Kinetics kinMeowth;
    SteeringOutput steeringMeowth;

    //DATOS DEL TARGET (glameow en este caso)
    public static_data target;
    Kinetics kineticsTarget;

    //DATOS SEEK
    public float maxSeekAccel = 10f;

    //DATOS ARRIVE
    public float targetRadiusArrive = 1f;
    public float slowRadiusArrive = 30f;
    public float MaxAccelerationArrive = 20f;


    void Start(){

        //INICIALIZAMOS LA DATA DEL EXTERIOR
        kinMeowth = meowth.kineticsAgent;
        steeringMeowth = meowth.steeringAgent;
        kineticsTarget = target.kineticsAgent;

        //COMENZAMOS A CONSTRUIR LA MAQUINA DE ESTADOS

        //1. ACCIONES:

        FollowTarget seekTarget = new FollowTarget(steeringMeowth, kinMeowth, kineticsTarget, maxSeekAccel);
        RunFromTarget runFromTarget = new RunFromTarget(steeringMeowth, kinMeowth, kineticsTarget, maxSeekAccel);
        StopMoving stop = new StopMoving(kinMeowth, steeringMeowth);
        DoNothing nothing = new DoNothing();
        ShowIcon showHeart = new ShowIcon(this.gameObject, "Heart");
        DisableIcon disableHeart = new DisableIcon(this.gameObject, "Heart");
        ShowIcon showSweat = new ShowIcon(this.gameObject, "Sweat");
        DisableIcon disableSweat = new DisableIcon(this.gameObject, "Sweat");
        ShowIcon showExclamation = new ShowIcon(this.gameObject, "Exclamation");
        DisableIcon disableExclamation = new DisableIcon(this.gameObject, "Exclamation");


        //2. ESTADOS:

        //State stalkTarget = new State()



    }

    void Update(){

    }
}