using UnityEngine;
using System.Collections;
//修改后的文件
public class Mover : MonoBehaviour
{
    public float speed = 20.0f;

    void Start ()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
}
