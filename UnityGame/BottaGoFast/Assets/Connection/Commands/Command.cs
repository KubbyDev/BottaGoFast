using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Debug = UnityEngine.Debug;

/// <summary> Cette classe gere toutes les commandes </summary>
/// <summary> Pour creer une commande: </summary>
/// <summary> - Creer une classe ayant pour nom le nom de la commande</summary>
/// <summary> - Copier coller le template dans Commands/Template.txt</summary>
/// <summary> - Pour envoyer la commande, appeller SendCommand( [parametres de la commande] )</summary>
/// <summary> - Si c'est un ordre a recevoir: Ajouter une fonction Receive(string parameters): elle sera appellee automatiquement</summary>
/// <summary> - Pour envoyer une requette (Commande attendant une reponse), utiliser la classe Request</summary>
public static partial class Command
{
    /// <summary> Envoie la commande avec les parametres specifies </summary>
    private static void SendCommand(string parameters)
    {
        Connection.SendCommand(new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name + " " + parameters);
    }
    
    /// <summary> Fonctions utilisees par le systeme de commandes </summary>
    /// <summary> Vous ne devriez pas utiliser ces fonctions </summary>
    public class System
    {
        /// <summary> Initialise le systeme. A appeller le plus tot possible dans l'execution </summary>
        public static void Init()
        {
            //Dit au systeme quoi faire quand il recoit un ordre
            Connection.SetOrderEvent(Receive);

            //On regarde parmi les sous classes de Command
            receivableCommands = new List<ReceivableCommand>();
            foreach (Type command in typeof(Command).GetNestedTypes())
            {
                //Si on trouve une methode Receive qui prend un string en parametre
                MethodInfo receiveMethod = command.GetMethods()
                    .FirstOrDefault(method =>
                        method.Name == "Receive" &&
                        method.GetParameters().Any(param => param.ParameterType == typeof(string)));

                //Si c'est le cas on l'enregistre pour les futures receptions d'ordres
                if (receiveMethod != null)
                    receivableCommands.Add(new ReceivableCommand(
                        receiveMethod,
                        (string) command.GetFields().First(field => field.Name == "COMMAND_ID").GetValue(null)
                    ));
            }
        }

        /// <summary> Gere totalement la reception d'une commande </summary>
        /// <summary> Cette fonction est doit etre referencee dans Connection.orderEvent </summary>
        public static void Receive(string order)
        {
            string firstArg = order.Substring(0, order.IndexOf(' ') + 1);
            if (long.TryParse(firstArg, out long ID))
                Request.ReceiveResponse(ID, order.Substring(order.IndexOf(' ') + 1));
            else
                Execute(order);
        }

        //Liste des commandes qui peuvent etre recues. Quand on recoit une commande
        //on regarde dans cette liste si il y en a une qui correspond pour la traiter
        private static List<ReceivableCommand> receivableCommands;
        private class ReceivableCommand
        {
            public ReceivableCommand(MethodInfo receiveMethod, string commandId)
            {
                ReceiveMethod = receiveMethod;
                CommandID = commandId;
            }

            public readonly MethodInfo ReceiveMethod;
            public readonly string CommandID;
        }

        //Cherche la commande a laquelle l'ordre fait reference et l'execute
        private static void Execute(string order)
        {
            string[] args = SeparateFirstArg(order);
            foreach (ReceivableCommand command in receivableCommands)
                if (command.CommandID == args[0])
                {
                    command.ReceiveMethod.Invoke(null, new object[] {args[1]});
                    return;
                }

            Debug.LogError("Couldn't find command " + args[0]);
        }

        //Renvoie un tableau contenant 2 strings: le premier argument, le reste
        //Les arguments sont supposes separes par des espaces
        private static string[] SeparateFirstArg(string order)
        {
            int i = 0;
            while (i < order.Length && order[i] != ' ')
                i += 1;

            return new string[] {order.Substring(0, i), order.Substring(i + 1)};
        }
    }
}