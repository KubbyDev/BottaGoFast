using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public static partial class Command
{
    /// <summary> Permet d'envoyer une commande et de traiter la reponse </summary>
    /// <summary> Pour envoyer une requette et recevoir la reponse sous forme d'un appel de fonction: utiliser SendAsync </summary>
    /// <summary> Pour envoyer une requette bloquer l'execution jusqu'a reception de la reponse: utiliser SendBlocking </summary>
    private class Request
    {
        private static long NextID = 0; //L'ID de la prochaine requette a etre envoyee
        private static Queue<Request> WaitingForResponse = new Queue<Request>(); //La liste des requettes pour lesquelles on attend la reponse
        
        private long ID; //L'ID de la requette (utilise pour identifier la reponse)
        private Action<string> onResponse; //La fonction a appeller quand la reponse est recue
        
        private Request(long ID, Action<string> onResponse)
        {
            this.ID = ID;
            this.onResponse = onResponse;
        }
        
        /// <summary> Envoie la requette et appelle la fonction callback quand la reponse est recue </summary>
        public static void SendAsync(string parameters, Action<string> callback)
        {
            Connection.SendCommand(NextID + " " + new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name + " " + parameters);

            WaitingForResponse.Enqueue(new Request(NextID, callback));
            NextID++;
        }
        
        /// <summary> Envoie la requette et bloque l'execution tant que la reponse n'est pas recue </summary>
        public static string SendBlocking(string parameters)
        {
            //Construit et envoie la commande
            Connection.SendCommand(NextID + " " + new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name + " " + parameters);
            
            //Quand la reponse est recue on met a jour res, ce qui permet de sortir du while bloquant
            string res = null;
            WaitingForResponse.Enqueue(new Request(NextID, answer => res = answer));
            NextID++;
            while (res == null)
                Connection.Update();

            return res;
        }

        /// <summary> Utilise par le systeme de commandes pour recevoir les reponses </summary>
        public static void ReceiveResponse(long ID, string response)
        {
            if (WaitingForResponse.Peek().ID == ID)
                //Dans le cas ou tout se passe bien la requette est au debut de la queue
                WaitingForResponse.Dequeue().onResponse(response);
            else
            {
                //Si la requette n'est pas au debut de la queue, il faut la trouver
                List<Request> requests = WaitingForResponse.ToList();
                Request corresponding = requests.Find(req => req.ID == ID);
                requests.Remove(corresponding);
                corresponding.onResponse(response);
                WaitingForResponse.Clear();
                requests.ForEach(req => WaitingForResponse.Enqueue(req));
            }
        }
    }
}