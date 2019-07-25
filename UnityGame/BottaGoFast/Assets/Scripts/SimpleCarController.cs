using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;

    public float verticalInput;
    public float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        verticalInput = 0;
        horizontalInput = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Récupération des inputs verticaux et horizontaux
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        //changement position et rotation
        transform.position += verticalInput * transform.forward * speed * Time.deltaTime;
        transform.Rotate(Vector3.up * horizontalInput * rotationSpeed * Time.deltaTime);
        //print(transform.eulerAngles);
    }
}
