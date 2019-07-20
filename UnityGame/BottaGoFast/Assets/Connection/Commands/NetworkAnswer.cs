using System;
using System.Globalization;
using System.Linq;

public static partial class Command
{
    /// <summary> Envoie un tableau contenant les valeurs des inputs du reseau, et recoit une reponse contenant les valeurs des outputs du reseau </summary>
    public static class NetworkAnswer
    {
        /// <summary> Envoie la commande avec les inputs specifies, puis bloque l'execution jusqu'a reception de la reponse </summary>
        public static float[] SendBlocking(float[] inputs)
        {
            return Parse(Request.SendBlocking(Construct(inputs)));
        }

        /// <summary> Envoie la commande avec les inputs specifies mais ne bloque pas l'execution </summary>
        /// <summary> Quand la reponse est recue, la fonction callback est appellee </summary>
        public static void SendAsync(float[] inputs, Action<float[]> callback)
        {
            Request.SendAsync(Construct(inputs), resp => callback(Parse(resp)));
        }

        //Construit la commmande
        private static string Construct(float[] inputs)
        {
            string res = inputs.Length > 0 ? inputs[0].ToString(CultureInfo.InvariantCulture) : "";
            for (int i = 1; i < inputs.Length; i++)
                res += " " + inputs[i].ToString(CultureInfo.InvariantCulture);
            return res;
        }
        
        //Interprete la reponse de la commande
        private static float[] Parse(string order)
        {
            string[] parameters = order.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            float[] res = new float[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
                res[i] = float.Parse(parameters[i]);

            return res;
        }
    }
}