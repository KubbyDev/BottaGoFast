using System;
using System.IO;

public static class Connection
{
    /// <summary>
    /// Chemin du flux d'entree
    /// </summary>
    public const string inPath = "G:/CS_orders.txt";
    /// <summary>
    /// Chemin du flux de sortie
    /// </summary>
    public const string outPath = "G:/PY_orders.txt";

    private static Action<string> orderEvent;

    public static void Update()
    {
        LookForOrder();
    }

    private static void LookForOrder()
    {
        bool success = false;
        while(!success)
            try
            {
                //Lis le fichier
                string fileContent;
                using (StreamReader sr = new StreamReader(inPath))
                    fileContent = sr.ReadToEnd(); 
                    
                //Vide le fichier
                File.WriteAllText(inPath, "");

                //Appelle la fonction d'evenement pour chaque ordre
                string[] orders = fileContent.Split('\n');
                foreach (string order in orders)
                    if (order != "")
                        orderEvent(order);

                success = true;
            }
            catch (IOException)
            {
                //Si le fichier est en cours d'utilisation on reessaye
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

                success = true;
            }
            catch (IOException)
            {
                //Si le fichier est en cours d'utilisation on reessaye
            }
    }
}