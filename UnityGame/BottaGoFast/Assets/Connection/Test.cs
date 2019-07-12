using UnityEngine;

public class Test : MonoBehaviour
{
    private System.Diagnostics.Stopwatch sw;
    
    void Start()
    {
        Connection.SetOrderEvent(SendResponse);

        Connection.SendCommand("Order 1");
        sw = System.Diagnostics.Stopwatch.StartNew();
    }

    void Update()
    {
        if (sw.ElapsedMilliseconds < 1000)
            Connection.Update();
    }

    public static void SendResponse(string order)
    {
        Debug.Log(order);
        string response = order.Substring(0, 6) + (int.Parse(order.Substring(6)) + 1);
        Debug.Log("Response: " + response);
        Connection.SendCommand(response);
    }
}