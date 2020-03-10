using UnityEngine;
public class SetAngularAccel : Action {

    SteeringOutput steeringAgent;
    float newAccel;

    public SetAngularAccel(SteeringOutput SteeringAgent, float NewAccel){
        steeringAgent = SteeringAgent;
        newAccel = NewAccel;
    }
    public override void DoAction(){
        steeringAgent.angular = newAccel;
    }
}