public static partial class Command
{
    public static void Receive(string order)
    {
        string firstArg = order.Substring(0, order.IndexOf(' ') + 1);
        if (long.TryParse(firstArg, out long ID))
            Request.ReceiveResponse(ID, order.Substring(order.IndexOf(' ') + 1));
        else
            Execute(order);
    }

    public static void Execute(string order)
    {
        
    }
}