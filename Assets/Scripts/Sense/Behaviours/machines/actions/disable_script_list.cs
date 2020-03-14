using System.Collections.Generic;
using UnityEngine;
public class DisableScriptList : Action {

    List<DisableScript> disables;

    public DisableScriptList(List<DisableScript> Disables){
        disables = Disables;
    }
    public override void DoAction(){
        foreach(var disable in disables){
            disable.DoAction();
        }
    }
}