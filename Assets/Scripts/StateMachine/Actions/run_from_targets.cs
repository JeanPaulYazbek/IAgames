//Accion que permite huir de alguien
using UnityEngine;
public class RunFromTargets : Action {


    SteeringOutput steeringAgent;
    Kinetics kineticsAgent;
    Kinetics[] kineticsTargets;
    Seek seek;

    public RunFromTargets(SteeringOutput SteeringAgent, Kinetics KineticsAgent,
     Kinetics[] KineticsTargets, float MaxAccel){

         steeringAgent = SteeringAgent;
         kineticsAgent = KineticsAgent;
         kineticsTargets = KineticsTargets;
         seek = new Seek(kineticsAgent, kineticsTargets[0], MaxAccel);
    }

    //en este caso la accion es actualizar la acelaracion del usuario
    public override void DoAction(){

        //HUIMOS DEL TARGET QUE ESTE MAS CERCA
        float min = float.PositiveInfinity;
        Vector3 agentPosition = kineticsAgent.transform.position;
        Kinetics currentKin;
        float currentDistance;
        for(int i = 0; i<kineticsTargets.Length; i++){
            currentKin = kineticsTargets[i];
            currentDistance = Vector3.Distance(agentPosition, currentKin.transform.position);
            if(currentDistance<min){
                min = currentDistance;
                seek.target = currentKin;

            }
        }

        //actualizamos aceleracion
        steeringAgent.UpdateSteering(seek.getSteering(0));
    }
}