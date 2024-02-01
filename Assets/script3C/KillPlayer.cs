using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class NewBehaviourScript : MonoBehaviour
{

    public int Respawn;
    private void Start()
    {
        
    }
    // si jamais je trigger le sol ( la lave ) la scene se relance depuis le debut 
    // lancer une coroutine pour quil y ai un delais entre la chute et le respawn afin d afficher un UI de mort 

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("hello");
            StartCoroutine(MaCoroutine());
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
        }
    }

    private IEnumerator MaCoroutine()
    {
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
