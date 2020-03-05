using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class point_manager : MonoBehaviour
{


    public Text[] boards;//Tableros visuales
    int[] points;//Puntuaciones logicas

    int amountPokemon;//Cantidad de pokemones en el juego

    int caughtPokemon = 0;//Cantidad de pokemones atrapados

    // Start is called before the first frame update
    void Start()
    {
        int n = boards.Length; 
        points = new int[n];
        for(int i = 0; i< n; i++){
            points[i] = 0;
        }

        amountPokemon = GameObject.FindGameObjectsWithTag("Pokemon").Length;
        
    }

    public void UpdateScore(int index, int newScore){
        points[index] += newScore;//le aumentamos su puntuacion a quien atrapo el pokemon
        boards[index].text = points[index].ToString();//cambiamos el texto en el tablero
        caughtPokemon++;//Aumentamos la cantidad de pokemones atrapados

        //SI ATRAPARON A TODOS LOS POKEMONES
        if(caughtPokemon == amountPokemon){

            if(points[0] > points[1]){//si gano el jugador
                SceneManager.LoadScene("WinnerScreen");//cargamos pantalla de victoria
            }else{
                SceneManager.LoadScene("LoserScreen");//cargamos pantalla de perdedor

            }
        }

    }
}
