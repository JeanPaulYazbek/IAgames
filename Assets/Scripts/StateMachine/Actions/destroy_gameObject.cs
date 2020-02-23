using System.Collections.Generic;
using UnityEngine;
//Action que quita un pokemon de la pila 
public class DestroyGameObject : Action {
    Stack<GameObject> items;

    public DestroyGameObject(Stack<GameObject> Items){
        items = Items;
    }

    public override void DoAction(){
        Object.Destroy(items.Peek());
    }
}