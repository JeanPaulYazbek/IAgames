public class LookWhereGoing : Action {

    LookWhereYouAreGoing look;
    SteeringOutput steeringAgent;

    public LookWhereGoing(LookWhereYouAreGoing Look, SteeringOutput Steering ){
        look = Look;
        steeringAgent = Steering;
    }

    public override void DoAction(){
        steeringAgent.angular = look.getSteering().angular;
    }
}