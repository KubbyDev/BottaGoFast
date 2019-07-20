using UnityEngine;

public class Main : MonoBehaviour
{
    void Start()
    {
        Command.System.Init();
    }

    void Update()
    {
        Connection.Update();
        
        if(Input.GetKeyDown(KeyCode.A))
            Command.NetworkAnswer.SendAsync(
                new float[]{Random.Range(0,10),Random.Range(0,10),Random.Range(0,10)}, 
                response => Debug.Log(new Vector3(response[0],response[1],response[2]))
            );
    }
}