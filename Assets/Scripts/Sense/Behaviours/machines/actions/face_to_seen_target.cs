// Accion que hace que el personaje que la use 
// gire hacia lo que ve
public class FaceToSeenTarget : Action {
    Face face;
    SightSensor sensor;//sensor de vision del personaje
    Kinetics character;//el personaje
    SteeringOutput steering;//steering del personaje

    public FaceToSeenTarget(SightSensor Sensor,  Kinetics Character, SteeringOutput Steering){
        face = new Face(Character, Character);
        sensor = Sensor;
        character = Character;
        steering = Steering;
    }

    public override void DoAction(){

        steering.UpdateSteering(face.getSteeringF2(sensor.detectedSignal.transform.position));
    }
}