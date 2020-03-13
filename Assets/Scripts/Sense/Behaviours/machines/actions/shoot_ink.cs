using UnityEngine;
public class ShootInk : Action {

    //CHARACTER
    Transform characterTransform;

    //INK
    GameObject inkBallPrefab;
    GameObject currentInkBall;
    float ballSpeed;
    GameObject inkScreen;

    //SHOOT
    ProyectileFunctions shootHandler =  new ProyectileFunctions();//aqui tenemos funciones utiles para proyectiles
    Vector3 gravity = new Vector3(0f, 0f, 9.8f);

    //SENSOR
    SightSensor sensor;

    //TRAINERS
    Kinetics[] trainers;

    //OBSTACULOS
    Transform[] obstacles;

    public ShootInk(Transform Character, GameObject InkPrefab, float BallSpeed, GameObject InkScreen, SightSensor Sensor,Kinetics[] Trainers, Transform[] Obstacles){
        characterTransform = Character;
        inkBallPrefab = InkPrefab;
        ballSpeed = BallSpeed;
        inkScreen = InkScreen;
        sensor = Sensor;
        trainers = Trainers;
        obstacles = Obstacles;
    }

    public override void DoAction(){

        //Si la ultima bola de tinta ya desaparecio
        if(!currentInkBall){
           //CREAMOS POKE BALL
            //creamos un pokeball en el mismo lugar que esta el character que la lanza.
            currentInkBall = GameObject.Instantiate(inkBallPrefab, characterTransform.position, Quaternion.identity);
            
            static_ink_shoot ballStatic = InitBall();//iniciamos la bola con una direccion por defecto

        
            //DISPARAREMOS HACIA DONDE ESTE LO QUE VIMOS
            Vector3 target = sensor.detectedSignal.transform.position;
            

            //ahora calculamos a que direccion lanzar.
            Vector3 shootDirection = shootHandler.CalculateFiringSolution(characterTransform.position, target, ballSpeed, gravity, new Vector3(-1,-1,-1), false);

            
            if(shootDirection.z != -1f){//si shoot direction encontro una trayectoria
                ballStatic.direction = shootDirection;//ponemos la direccionq ue no es por defecto
            } 
        }
        
  
    }

    //Funcion que asigna los datos que necesita una bola de tinta
    static_ink_shoot InitBall(){
        static_ink_shoot ballStatic = currentInkBall.GetComponent<static_ink_shoot>();
        ballStatic.trainers = trainers;
        ballStatic.obstacles = obstacles;
        ballStatic.speed = ballSpeed;
        ballStatic.direction = new Vector3(1f,1f,-2f);
        ballStatic.inkScreen = inkScreen;
        return ballStatic;

    }



}