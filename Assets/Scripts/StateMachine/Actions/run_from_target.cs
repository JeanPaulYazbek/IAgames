//Accion que permite huir de alguien
public class RunFromTarget : Action {


    SteeringOutput steeringAgent;
    Kinetics kineticsAgent;
    Kinetics kineticsTarget;
    Seek seek;

    public RunFromTarget(SteeringOutput SteeringAgent, Kinetics KineticsAgent,
     Kinetics KineticsTarget, float MaxAccel){

         steeringAgent = SteeringAgent;
         kineticsAgent = KineticsAgent;
         kineticsTarget = KineticsTarget;
         seek = new Seek(kineticsAgent, kineticsTarget, MaxAccel);
    }

    //en este caso la accion es actualizar la acelaracion del usuario
    public override void DoAction(){
        steeringAgent.UpdateSteering(seek.getSteering(0));
    }
}