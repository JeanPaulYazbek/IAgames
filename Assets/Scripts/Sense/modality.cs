public abstract class Modality {

    // Nombre del sentido de la modalidad
    public string senseType;
    // Informacion util como el sub-tipo del sentido que se envia, por ejemplo un olor puede ser tipo Poison
    public string description;

    // Que tan lejos puede llegar a lo mas el olor por ejemplo
    public float maximumRange;
    // Que tanto baja su intensidad a medida que se aleja
    public float attenuation;
    // Cantidad de tiempo (si es tiempo no velocidad) que toma recorrer una unidad de distancia
    public float inverseTransmissionSpeed;
    // Funcion que hace revisiones extra de si una sennal con esta modalidad deberia poder ser recibida por el sensor
    public abstract bool ExtraChecks(Signal signal, Sensor sensor);



    public Modality(float MaximumRange, float Attenuation, float InverseTransmissionSpeed,string Description, string SenseType){
        maximumRange = MaximumRange;
        attenuation = Attenuation;
        inverseTransmissionSpeed = InverseTransmissionSpeed;
        description = Description;
        senseType = SenseType;
    }
}