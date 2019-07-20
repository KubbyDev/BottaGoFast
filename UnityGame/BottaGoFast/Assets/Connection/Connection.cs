using System;
using System.IO;
using UnityEngine;

public static class Connection
{
    /// <summary>
    /// Chemin du flux d'entree
    /// </summary>
    public const string inPath = "F:/CS_orders.txt";
    /// <summary>
    /// Chemin du flux de sortie
    /// </summary>
    public const string outPath = "F:/PY_orders.txt";
    /// <summary>
    /// True: Logs debug things
    /// </summary>
    public static bool DEBUG_MODE = true;

    private static Action<string> orderEvent;
    private static long lastUpdateMoment = DateTime.Now.Ticks;

    public static void Update()
    {
        if (DateTime.Now.Ticks - lastUpdateMoment > 1 * 10000)
        {
            lastUpdateMoment = DateTime.Now.Ticks;
            LookForOrder();
        }
    }

    private static void LookForOrder()
    {
        string fileContent;
        try
        {
            //Lis le fichier
            using (StreamReader sr = new StreamReader(inPath))
                fileContent = sr.ReadToEnd(); 
                
            //Vide le fichier si besoin
            if (fileContent != "")
                File.WriteAllText(inPath, "");
        }
        catch (IOException)
        {
            //Si le fichier est en cours d'utilisation on abandonne
            return;
        }
        
        //Appelle la fonction d'evenement pour chaque ordre
        string[] orders = fileContent.Split('\n');
        foreach (string order in orders)
            if (order != "")
            {    
                if(DEBUG_MODE) Debug.Log("Received: \"" + order + "\"");
                orderEvent(order);
            }
    }

    /// <summary>
    /// Definit la fonction a appeller quand un ordre est recu
    /// </summary>
    /// <param name="receiver"></param>
    public static void SetOrderEvent(Action<string> receiver) => orderEvent = receiver;

    /// <summary>
    /// Envoie une commande. Attention, ne doit pas contenir de \n
    /// </summary>
    public static void SendCommand(string command)
    {
        bool success = false;
        while(!success)
            try
            {
                using (StreamWriter sw = new StreamWriter(outPath, true))
                    sw.WriteLine(command);

                if(DEBUG_MODE) Debug.Log("Sent: \"" + command + "\"");
                
                success = true;
            }
            catch (IOException)
            {
                //Si le fichier est en cours d'utilisation on reessaye
            }
    }
}