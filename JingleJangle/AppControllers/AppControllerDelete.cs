using Azure;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JingleJangle.AppControllers
{
    public class AppControllerDelete : AppController
    {
        private int id;
        public AppControllerDelete(string command)
            : base(command) 
        {
            
        }
        private int checkIsValidId(string idStr)
        {
            int tempId;

            if (!int.TryParse(idStr, out tempId))
            {
                throw new ArgumentException($"Invalid PersonID: {idStr}. " +
                    $"The provided ID is not a number");
            }

            return tempId;
        }
        public override void InitializeCommand()
        {
            try
            {
                ValidateCommandStructure();
                ValidateUserProvidedFields();
            }
            catch (ArgumentException ex)
            {
                isValid = false;
                Console.WriteLine(ex.Message);
            }
        }
        private void ValidateCommandStructure()
        {
            string[] parts = UserInput.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                isValid = false;
                throw new ArgumentException($"Invalid amount of arguments. " +
                    $"Valid DELETE syntax is DELETE [personID]");
            }

            //id = checkIsValidId(parts[1]);
 
        }
        private void ValidateUserProvidedFields()
        {
            string[] parts = UserInput.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts[1] == "*")
            {
                // Set id to negative one if it is a delete all command
                id = -1;
            }
            else
            {
                id = checkIsValidId(parts[1]);

                if (id < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(id),
                        id, $"{nameof(id)} must be >= 0");
                }
            }
        }
        public override void ExecuteCommand()
        {


            //DatabaseCommand c = new DatabaseCommand();

            if (id != -1)
            {

                DatabaseCommand.DeletePerson(id);
                Console.WriteLine($"Person with Employee Id {id} deleted from the database");
            }
            else
            {
                DatabaseCommand.DeleteAllRecords();
                //Console.WriteLine("Deleted all records");
            }
        }

    }
}
