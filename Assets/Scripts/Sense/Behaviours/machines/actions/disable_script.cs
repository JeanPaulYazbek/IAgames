using UnityEngine;
public class DisableScript : Action {

    MonoBehaviour script;

    public DisableScript(MonoBehaviour Script){
        script = Script;
    }
    public override void DoAction(){
        script.enabled = false;
    }
}