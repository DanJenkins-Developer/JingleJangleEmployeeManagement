// Daniel Jenkins
// CSCI 428
// Assignment 4 



using JingleJangle.AppControllers;
using JingleJangle.Data;

namespace jingleJangle
{
    class Program
    {
        static void Main(string[] args)
        {

            using JingleJangleEmployees2023Context context = new JingleJangleEmployees2023Context();

             //•	At startup, the program will simply greet the user and await a command. 

            Console.WriteLine("Greeting user.");
            Console.WriteLine("Type \"help\" for a list of available commands or \"quit\" to exit the program");
            Console.WriteLine();

            bool running = true;

            while (running)
            {

                string command;
                string[] commandArr;

                Console.Write("Enter a command :: ");
                command = Console.ReadLine();
                commandArr = command.Split(" ");

                try
                {
                    // The different supported commands
                    switch (commandArr[0].ToLower())
                    {
                        case "list":
                            
                            AppControllerList list = new(command);
                            list.Initialize();
                            list.Execute();
                            break;
                            
                        case "add":

                            AppControllerAdd add = new(command);
                            add.Initialize();
                            add.Execute();
                            
                            break;
                        case "update":

                            AppControllerUpdate update = new(command);
                            update.Initialize();
                            update.Execute();
                            break;

                        case "delete":

                            AppControllerDelete delete = new(command);
                            delete.Initialize();
                            delete.Execute();
                            break;

                        case "help":

                            Console.WriteLine("LIST syntax :: LIST [type] [page] [count] [orderby] [direction] - Supported types are Employee, Prehire, Retiree, and All.");
                            Console.WriteLine("ADD syntax :: ADD [type] [FIELD=VALUE] [FIELD=VALUE] ... ");
                            Console.WriteLine("UPDATE syntax :: UPDATE [id] [FIELD=VALUE] [FIELD=VALUE] ...  ");
                            Console.WriteLine("DELETE syntax :: DELETE [id] or DELETE * to delete all records");
                            Console.WriteLine("Type \"clear\" to clear the console window");
                            Console.WriteLine("");
                            break;

                        case "clear":

                            Console.Clear();
                            break;

                        case "quit":

                            running = false;
                            break;

                        default:

                            Console.WriteLine("Invalid Command");
                            break;

                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }

            }
        }
    }
}
