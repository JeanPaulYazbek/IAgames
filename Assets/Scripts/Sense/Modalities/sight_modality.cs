using UnityEngine;
public class SightModality : Modality {

    public SightModality(float MaximumRange, float Attenuation, float InverseTransmissionSpeed, string Description) : 
    base(MaximumRange, Attenuation, InverseTransmissionSpeed, Description, "Sight"){}

    Utilities utilities = new Utilities();

    public override bool ExtraChecks(Signal signal, Sensor sensor){
        Vector3 signalPos = signal.transform.position;
        Vector3 sensorPos = sensor.transform.position;
        float sensorOrien = sensor.transform.eulerAngles.z;
        SightSensor sigthSensor = (SightSensor) sensor;
        float width = sigthSensor.width;
        float large = sensor.threshold;
        return CheckSightCone(sensorPos, large, width, sensorOrien, signalPos);
    }



    //Esta funcion toma dos point1 y large y orientation y width,  genera un triangulo
    //cuyo primer punto es point1 y los otros dos puntos estan en direccion de la orientacion
    //separados por dos veces width y a una distancia mas o menos large del point1
    //La funcion devolvera true si point2 esta dentro de ese triangulo
    public bool CheckSightCone(Vector3 point1, float large, float width, float orientation, Vector3 point2){

        //conseguimos la direccion de la orientacion
        Vector3 middle = utilities.OrientationToVector(orientation);
        //middle.Normalize();
        //ahora tenemos el punto del medio de los dos que buscamos
        middle = middle*large;

        //rotamos el vector del medio obteniendo los que forman el triangulo
        Vector3 point1A = utilities.RotateVector(middle, width);
        Vector3 point1B = utilities.RotateVector(middle, -width);

        //los hacemos relativos al punto1 ya que antes eran relativos al centro
        point1A += point1;
        point1B += point1;

        utilities.DrawTriangle(point1, point1A, point1B, 2f);
        bool answer = utilities.PointInTriangle(point2, point1, point1A, point1B);
        return answer;

    }

}