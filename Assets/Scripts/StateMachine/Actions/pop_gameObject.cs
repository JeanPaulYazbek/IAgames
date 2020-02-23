using System.Collections.Generic;
using UnityEngine;
//Action que quita un pokemon de la pila 
public class PopGameObject : Action {
    Stack<GameObject> items;

    public PopGameObject(Stack<GameObject> Items){
        items = Items;
    }

    public override void DoAction(){
        
        GameObject item = items.Pop();
    }
}