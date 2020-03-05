using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayAgainManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayAgain(){
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
