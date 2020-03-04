
using System.Collections.Generic;
using System;
using UnityEngine;
public class RegionalSenseManager {

    class Notification {
        public DateTime time;
        public Sensor sensor;
        public Signal signal;

        public Notification(DateTime Time, Sensor Sensor, Signal Signal){
            time = Time;
            sensor = Sensor;
            signal = Signal;
        }
    }

    List<Sensor> sensors;//sensores de quien nos iterese
    Queue<Notification> notificationQueue;//aqui iran las notificaciones que debemos enviar a quien pueda recibirlo

    public RegionalSenseManager(List<Sensor> Sensors){
        sensors = Sensors;
        notificationQueue = new Queue<Notification>();
    }

    //Funcion que toma la sennal de alguien digamos un weezing emitiendo 
    //gases toxicos y lo envia a quien pueda recibirlo
    public void AddSignal(Signal signal){

        // Aqui guardaremos los sensores que puedan recibir la sennal
        List<Sensor> validSensors = new List<Sensor>();


        float distance;
        float intensity;
        DateTime time;
        Notification notification;
        foreach(var sensor in sensors){

            // Si el sensor no tiene el mismo tipo que la sennal que se encia
            if(!sensor.DetectsModality(signal.modality)){
                continue;
            }

            distance =  Vector3.Distance(signal.transform.position, sensor.transform.position);
            // Si de donde viene la sennal es muy lejos como para que el sensor lo sienta
            if(signal.modality.maximumRange < distance){
                continue;
            }

            intensity = signal.strength * (float)Math.Pow(signal.modality.attenuation, distance);

            // Si cuando la sennal llega es muy pequenna como para que el sensor la sienta
            if(intensity < sensor.threshold){
                continue;
            }

            if(!(signal.modality.ExtraChecks(signal, sensor))){
                continue;
            }

            // Calculamos la hora actual y le sumamos los segundos que le tome 
            // a la sennal viajar hasta el sensor
            time = DateTime.Now;
            time = time.AddSeconds(distance*signal.modality.inverseTransmissionSpeed);

            // Creamos y encolamos la notifcacion
            notification = new Notification(time, sensor, signal);
            notificationQueue.Enqueue(notification);


        }

        //Intentamos enviar las sennales una vez
        SendSignals();

    }

    public void SendSignals(){

        DateTime currentTime = DateTime.Now;

        Notification notification;
        while(notificationQueue.Count != 0){
            notification = notificationQueue.Peek();

            // Si la notificacion debe ser enviada antes del tiempo actual
            if(notification.time < currentTime){
                // Enviamos
                notification.sensor.Notify(notification.signal);
                notificationQueue.Dequeue();
            }else{ // Recordando que la cola esta ordenada por fecha entonces
                   // si estamos aqui es que nadie mas en la cola podria tener el tiempo que buscmoas

                // Salimos
                break;
            }
        }
    }

}