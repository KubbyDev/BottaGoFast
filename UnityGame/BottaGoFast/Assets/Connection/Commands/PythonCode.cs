public static partial class Command
{
    /// <summary> Envoie un bout de code Python a executer sur le programme python </summary>
    public static class PythonCode
    {
        public const string COMMAND_ID = "PYTHON_CODE";
        
        /// <summary> Envoie un bout de code Python a executer sur le programme python </summary>
        public static void Send(string code) => Connection.SendCommand(COMMAND_ID + " " + code);
    }
}