using UnityEngine;
public class EnableScript : Action {

    MonoBehaviour script;

    public EnableScript(MonoBehaviour Script){
        script = Script;
    }
    public override void DoAction(){
        script.enabled = true;
    }
}