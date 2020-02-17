using UnityEngine;
public class AmIUnderground : Condition{

    Transform agent;

    public AmIUnderground(Transform Agent){
        agent = Agent;
    }

    public override bool Test(){

        return agent.position.z > 0.01f;

    }
}