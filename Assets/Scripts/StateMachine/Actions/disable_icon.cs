using UnityEngine;
//Accion que permite ocultar un icono (un corazon por ejemplo)
//que sea un objeto hijo del objeto agent
public class DisableIcon : Action {

    GameObject agent;
    string name;//nombre del componente del icono

    public DisableIcon(GameObject Agent, string Name){
        agent = Agent;
        name = Name;
    }

    public override void DoAction(){
        //buscamos los hijos del agente
        GameObject[] childs = agent.GetComponentsInChildren<GameObject>();//buscamos los hijos
        for(int i  = 0; i < childs.Length; i++){//para cada hijo
            if(childs[i].name == name){//si tiene el nombre que buscamos

                SpriteRenderer sprite = childs[i].GetComponent<SpriteRenderer>();
                if(!(sprite is null)){//si tiene sprite
                    sprite.enabled = false;//lo ocultamos
                    
                }
                break;
                
            }
        }
    }

}