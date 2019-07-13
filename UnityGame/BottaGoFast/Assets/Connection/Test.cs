using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    private System.Diagnostics.Stopwatch sw;
    
    void Start()
    {
        StartCoroutine(LoadCoroutine());
    }

    IEnumerator LoadCoroutine()
    {
        yield return new WaitForSeconds(2);
        
        Connection.SetOrderEvent(SendResponse);

        Connection.SendCommand("Order 1");
        sw = System.Diagnostics.Stopwatch.StartNew();
    }

    void Update()
    {
        if (sw == null)
            return;

        if (sw.ElapsedMilliseconds < 1000)
        {
            Debug.Log(Time.deltaTime);
            Connection.Update();
        }
    }

    public static void SendResponse(string order)
    {
        Debug.Log(order);
        string response = order.Substring(0, 6) + (int.Parse(order.Substring(6)) + 1);
        Debug.Log("Response: " + response);
        Connection.SendCommand(response);
    }
}