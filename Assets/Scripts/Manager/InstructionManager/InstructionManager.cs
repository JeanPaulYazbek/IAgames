using UnityEngine.SceneManagement;
using UnityEngine;

public class InstructionManager : MonoBehaviour
{
    public void BackToMenu(){
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);

    }
}
