namespace CounterStrike.IO
{
    using System;
    using System.IO;
    using CounterStrike.IO.Contracts;

    public class Writer : IWriter
    {
        public void Write(string message)
        {
            using(StreamWriter sw = new StreamWriter("text.txt", true))
            {
                sw.WriteLine(message);
            }

            Console.Write(message);
        }

        public void WriteLine(string message)
        {
            using (StreamWriter sw = new StreamWriter("text.txt", true))
            {
                sw.WriteLine(message);
            }

            Console.WriteLine(message);
        }
    }
}
