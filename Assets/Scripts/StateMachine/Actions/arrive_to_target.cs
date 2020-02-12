//Accion que permite perseguir a alguien
public class ArriveToTarget : Action {


    SteeringOutput steeringAgent;
    Kinetics kineticsAgent;
    Kinetics kineticsTarget;
    public Arrive arrive;

    public ArriveToTarget(SteeringOutput SteeringAgent, Kinetics KineticsAgent,
     Kinetics KineticsTarget, float MaxAccel,float MaxSpeed, float TargetRadius,
     float SlowRadius){

         steeringAgent = SteeringAgent;
         kineticsAgent = KineticsAgent;
         kineticsTarget = KineticsTarget;
         arrive = new Arrive(kineticsAgent, kineticsTarget, MaxAccel, MaxSpeed,TargetRadius, SlowRadius, 0.1f);
    }

    //en este caso la accion es actualizar la acelaracion del usuario
    public override void DoAction(){
        steeringAgent.UpdateSteering(arrive.getSteering());
    }
}