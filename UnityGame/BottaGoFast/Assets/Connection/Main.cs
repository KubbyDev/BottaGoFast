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
    }
}