using System;
// Esta condicion ayuda a saber si cierta cantidad de tiempo ha pasado desde que 
// se entro a un estado con esta condicion en alguna de sus transiciones
// Es necesaria ademas una accion que actualize la fecha de esta condicion 
// cuando se entra al estado
public class TimeOut : Condition {

    public DateTime startTime;//tiempo en que se entro al estado que tiene la transicion con esta condicion
    public float secondsToWait;//cantidad de segundos a esperar

    public TimeOut(float SecondsToWait){
        secondsToWait = SecondsToWait;
    }

    public override bool Test(){

        DateTime rightNow = DateTime.Now;

        float difference = (float) (rightNow - startTime).TotalSeconds;

        return secondsToWait < difference;

    }

    // Funcion para marcar el tiempo de comienzo de esta condicion
    public void SetStartTime(){
        startTime = DateTime.Now;
    }
}