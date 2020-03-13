using UnityEngine;
using System.Collections;
public class ink_screen_timer : MonoBehaviour {

    public float segs;//cuantoss segundos esperar antes de desaparecer

    GameObject inkScreen;


    void Start(){
        inkScreen = gameObject;
        StartCoroutine(WaitInk(segs));
    }

    //iterador para poner la tinta 30 segs en pantalla
    IEnumerator WaitInk(float segs)
    {
        
        yield return new WaitForSeconds(segs);
        inkScreen.SetActive(false);
    }

}