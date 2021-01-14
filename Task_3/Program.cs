using System;

namespace Task_3
{
    class Program
    {
        private static string author = "Made by Mulish Vadym\n";
        private static string programDescription = "Task 3 Notes";
        
        static void Main(string[] args)
        {
            
            Console.WriteLine(programDescription);
            Console.WriteLine(author);
            
            NotesWorker worker = new NotesWorker();    
            worker.Start();

        }
    }
}
