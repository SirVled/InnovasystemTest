namespace Manager 
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Environment.CurrentDirectory);
            Watcher watcher = new Watcher();
            watcher.StartWatching();

            Console.ReadLine();
        }
    }
}