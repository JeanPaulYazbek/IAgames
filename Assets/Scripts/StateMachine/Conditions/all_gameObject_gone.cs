using System.Collections.Generic;
using UnityEngine;
public class AllGameObjectGone : Condition {

    Stack<GameObject> items;


    public AllGameObjectGone(Stack<GameObject> Items){
        items = Items;
    }
    public override bool Test(){
        return items.Count == 0 ;
    }
}