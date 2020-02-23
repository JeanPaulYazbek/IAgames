using UnityEngine;
using System.Collections.Generic;
public class updateEvolveMethod : Action {
    Evolve evolve;
    Stack<GameObject> stonesStack;

    public updateEvolveMethod(Evolve Evolve, Stack<GameObject> StonesStack){
        evolve = Evolve;
        stonesStack = StonesStack;
    }

    public override void DoAction(){
        evolve.UpdateMethod(stonesStack.Peek().name);
    }
}