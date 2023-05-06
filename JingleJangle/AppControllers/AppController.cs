using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccessLayer;
using static System.Net.Mime.MediaTypeNames;

namespace JingleJangle.AppControllers
{
    // •	Implement a AppController class that manages the user input and display of the results
    // •	Implement an abstract AppCommand base class and subclasses for each of the LIST, ADD, UPDATE, and DELETE commands.
    public abstract class AppController : IAppCommand
    {
        //public static readonly string[] AllowedCommands = { "LIST", "ADD", "UPDATE", "DELETE" };
        //public string Command { get; set; }
        private string userInput;
        protected bool isValid;
        protected List<string> ErrorMessages { get; } = new List<string>();

        public AppController(string userInput)
        {
            UserInput = userInput;
            isValid = true;
        }
        public string UserInput
        {
            get
            {
                return userInput;
            }
            set
            {
                userInput = value;
            }
        }
        public bool IsValid
        {
            get
            {
                return isValid;
            }
        }
        public override string ToString()
        {
            return $"User input :: {userInput}";
        }
        public abstract void ExecuteCommand();
        public abstract void InitializeCommand();
        public void Execute() => ExecuteCommand();
        public void Initialize() => InitializeCommand();
    }

}
