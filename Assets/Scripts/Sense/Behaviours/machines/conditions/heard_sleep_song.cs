using UnityEngine;
//condicion para saber si escuchamos cancion de cuna
public class HeardSleepSong : Condition {

    SoundSensor sensor;

    float minSongIntensity;

    public HeardSleepSong(SoundSensor Sensor){
        sensor = Sensor;
        //Solo nos dormiremos si la cancion es mas fuerte que el 
        //threshold mas un offset
        minSongIntensity = Sensor.threshold + 6f;
    }

    public override bool Test(){

        return sensor.notified && sensor.soundType == "Sleep" && sensor.soundIntensity > minSongIntensity;
    }
}