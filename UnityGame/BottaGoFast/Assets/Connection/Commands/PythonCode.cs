public static partial class Command
{
    /// <summary> Envoie un bout de code Python a executer sur le programme python </summary>
    public static class PythonCode
    {
        /// <summary> Envoie un bout de code Python a executer sur le programme python </summary>
        public static void Send(string code) => SendCommand(code);
    }
}