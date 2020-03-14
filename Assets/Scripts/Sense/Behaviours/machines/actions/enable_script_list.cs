using System.Collections.Generic;
using UnityEngine;
public class EnableScriptList : Action {

    List<EnableScript> enables;

    public EnableScriptList(List<EnableScript> Enables){
        enables = Enables;
    }
    public override void DoAction(){
        foreach(var enable in enables){
            enable.DoAction();
        }
    }
}