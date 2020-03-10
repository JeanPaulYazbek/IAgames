using UnityEngine;
public class SetAngularSpeed : Action {

    Kinetics kinAgent;
    float newSpeed;

    public SetAngularSpeed(Kinetics KinAgent, float NewSpeed){
        kinAgent = KinAgent;
        newSpeed = NewSpeed;
    }
    public override void DoAction(){
        kinAgent.rotation = newSpeed;
    }
}