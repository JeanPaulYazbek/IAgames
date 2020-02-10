//Accion que permite perseguir a alguien
public class FollowTarget : Action {


    SteeringOutput steeringAgent;
    Kinetics kineticsAgent;
    Kinetics kineticsTarget;
    Seek seek;

    public FollowTarget(SteeringOutput SteeringAgent, Kinetics KineticsAgent,
     Kinetics KineticsTarget, float MaxAccel){

         steeringAgent = SteeringAgent;
         kineticsAgent = KineticsAgent;
         kineticsTarget = KineticsTarget;
         seek = new Seek(kineticsAgent, kineticsTarget, MaxAccel);
    }

    //en este caso la accion es actualizar la acelaracion del usuario
    public override void DoAction(){
        steeringAgent.UpdateSteering(seek.getSteering(1));
    }
}