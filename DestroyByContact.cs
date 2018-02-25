using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue; // how many score will be added when an asteroid's shot
    private GameController gameController; // GameController script instance

    void Start ()
    {
        GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
        if (gameControllerObject != null)
            gameController = gameControllerObject.GetComponent<GameController> ();
        else
            Debug.Log ("找不到tag为GameController的对象");

        if (gameController == null)
            Debug.Log ("找不到 GameController 脚本");
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag ("Boundary"))
            return;
        Instantiate (explosion, transform.position, transform.rotation);
        if (other.gameObject.CompareTag ("Player"))
	      {
            Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
						gameController.GameOver();
        }

        gameController.AddScore (scoreValue);
        Destroy (other.gameObject);
        Destroy (gameObject);
    }
}
