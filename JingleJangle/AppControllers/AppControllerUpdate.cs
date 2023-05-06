using DataAccessLayer;
using JingleJangle.Models;
using System.IO;

namespace JingleJangle.AppControllers
{
    public class AppControllerUpdate : AppController
    {
        public static readonly string[] AllowedTypes = { "All", "Prehire", "Employee", "Retiree" };
        //HashSet<string> validEmployeeFields = new HashSet<string> { "FirstName", "LastName", "JobTitle", "MonthlySalary", "EmploymentDate" };
        //HashSet<string> validPrehireFields = new HashSet<string> { "FirstName", "LastName", "OfferExtendedDate", "OfferAcceptanceDate", "EmploymentDate" };
        //HashSet<string> validRetireeFields = new HashSet<string> { "FirstName", "LastName", "RetirementProgram", "RetirementDate", "EmploymentDate" };

        public static readonly HashSet<string> validPersonFields = new() {"FirstName", "LastName", "EmploymentDate" };
        public static readonly HashSet<string> validEmployeeFields = new(validPersonFields.Concat(new[] { "JobTitle", "MonthlySalary" }));
        public static readonly HashSet<string> validPrehireFields = new(validPersonFields.Concat(new[] { "OfferExtendedDate", "OfferAcceptanceDate" }));
        public static readonly HashSet<string> validRetireeFields = new(validPersonFields.Concat(new[] { "RetirementProgram", "RetirementDate" }));

        private Dictionary<string, object> fieldValuePair;
        private Person person;
        private int id;
        private string type;


        HashSet<string> validFields;


        public AppControllerUpdate(string command)
        : base(command)
        {
            fieldValuePair = new Dictionary<string, object>();
        }
        public override void InitializeCommand()
        {

            try
            {

                ValidateCommandStructure();

                // If structure is good, id is populated so we can 
                // fetch the person the user is trying to update
                person = DatabaseCommand.GetRecordByEmployeeId(id);

                // Checks if the DatabaseCommand actually returned a result
                if (person != null)
                {
                    // Determine the type of the person
                    type = person.PersonType.TypeName;

                    // Valid fields are based on the type of the returned person so we have to update those
                    AssignValidFields();

                    // We have to see if the user provided fields match the valid fields
                    ValidateUserProvidedFields();

                    // Update the person object with the new fields
                    UpdatePersonObject();

                    // After all this the actual update command is ready to be executed
                }
                else
                {
                    Console.WriteLine($"No person with EmployeeId {id}");
                }
            }
            catch (ArgumentException ex)
            {
                isValid = false;
                Console.WriteLine(ex.Message);
            }
        }
        private int checkIsValidId(string idStr)
        {
            int tempId;

            if (!int.TryParse(idStr, out tempId))
            {
                throw new ArgumentException($"Invalid EmployeeID: {idStr}");
            }

            return tempId;
        }
        private DateTime checkIsValidDate(object dateObj)
        {
            DateTime tempDate;
            string stringDate = (string)dateObj;

            if (!DateTime.TryParse(stringDate, out tempDate))
            {
                throw new ArgumentException($"Invalid date format: {dateObj}");
            }

            return tempDate;

        }
        private decimal checkIsValidSalary(object salaryObj)
        {
            decimal tempSalary;
            string stringSalary = (string)fieldValuePair["MonthlySalary"];

            if (!decimal.TryParse(stringSalary, out tempSalary))
            {
                throw new ArgumentException($"Invalid salary: {fieldValuePair["MonthlySalary"]}");
            }

            return tempSalary;
        }
        private void ValidateCommandStructure()
        {
            // Validates the STRUCTURE of the command string itself

            // Get the ID from the first part after the actual command
            string[] parts = UserInput.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            id = checkIsValidId(parts[1]);

            // Check the field value pairs and add them to the dictionary
            for (int i = 2; i < parts.Length; i++)
            {
                string[] pair = parts[i].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (pair.Length == 2)
                {
                    string fieldName = pair[0].Trim();
                    string fieldValue = pair[1].Trim();
                    fieldValuePair.Add(fieldName, fieldValue);
                }
                else
                {
                    isValid = false;
                    throw new ArgumentException($"Invalid field-value pair: {parts[i]}. ");
                }
            }
        }
        private void ValidateUserProvidedFields()
        {
            // Check if user provided fields match the valid fields for whatever type they're trying to update
            foreach (string fieldName in fieldValuePair.Keys)
            {

                if (!validFields.Contains(fieldName))
                {
                    isValid = false;

                    throw new ArgumentException($"Invalid fieldname: {fieldName}. " +
                        $"Supported field names are {string.Join(", ", validFields)}");

                }
            }
        }
        private void AssignValidFields()
        {
            if (type == "Employee")
            {
                validFields = validEmployeeFields;
            }
            else if (type == "Prehire" || type == "PreHire")
            {
                // Have to set this because I missspelled in the Database.
                type = "Prehire";

                validFields = validPrehireFields;
            }
            else if (type == "Retiree")
            {
                validFields = validRetireeFields;
                Console.WriteLine("Retiree");
            }
            else
            {
                isValid = false;
                // throw exception
                throw new ArgumentException($"Unsupported Type: {type}. " +
                    $"Supported Types to are {string.Join(", ", AllowedTypes)}");
            }
        }
        private void UpdatePersonObject()
        {
            if (fieldValuePair.ContainsKey("FirstName"))
            {
                person.FirstName = (string)fieldValuePair["FirstName"];
            }
            if (fieldValuePair.ContainsKey("LastName"))
            {
                person.LastName = (string)fieldValuePair["LastName"];
            }
            if (fieldValuePair.ContainsKey("EmploymentDate"))
            {
                person.EmploymentDate = checkIsValidDate(fieldValuePair["EmploymentDate"]);
            }

            // Employee fields
            if (type == "Employee")
            {
                if (fieldValuePair.ContainsKey("MonthlySalary"))
                {
                    person.Employee.MonthlySalary = checkIsValidSalary(fieldValuePair["MonthlySalary"]);
                }
                if (fieldValuePair.ContainsKey("JobTitle"))
                {
                    person.Employee.JobTitle = (string)fieldValuePair["JobTitle"];
                }
                //Console.WriteLine($"New first name :: {person.FirstName}");
                //Console.WriteLine($"New last name :: {person.LastName}");
                //Console.WriteLine($"New employment date :: {person.EmploymentDate}");
                //Console.WriteLine($"New monthly salary :: {person.Employee.MonthlySalary}");
                //Console.WriteLine($"New job title :: {person.Employee.JobTitle}");
                //Console.WriteLine("-------------");
            }
            else if (type == "Prehire")
            {
                if (fieldValuePair.ContainsKey("OfferExtendedDate"))
                {
                    person.Prehire.OfferExtendedDate = checkIsValidDate(fieldValuePair["OfferExtendedDate"]);
                }
                if (fieldValuePair.ContainsKey("OfferAcceptanceDate"))
                {
                    person.Prehire.OfferAcceptanceDate = checkIsValidDate(fieldValuePair["OfferAcceptanceDate"]);
                }
                //Console.WriteLine("Prehire");
                //Console.WriteLine("Employee");
                //Console.WriteLine($"New first name :: {person.FirstName}");
                //Console.WriteLine($"New last name :: {person.LastName}");
                //Console.WriteLine($"New employment date :: {person.EmploymentDate}");
                //Console.WriteLine($"New offer extended date :: {person.Prehire.OfferExtendedDate}");
                //Console.WriteLine($"New offer acceptance date ::  {person.Prehire.OfferAcceptanceDate}");
                //Console.WriteLine("-------------");
            }
            else if (type == "Retiree")
            {
                if (fieldValuePair.ContainsKey("RetirementProgram"))
                {
                    person.Retiree.RetirementProgram = (string)fieldValuePair["RetirementProgram"];
                }
                if (fieldValuePair.ContainsKey("RetirementDate"))
                {
                    person.Retiree.RetirementDate = checkIsValidDate(fieldValuePair["RetirementDate"]);
                }
            }
            else
            {
                isValid = false;
                // throw exception
                throw new ArgumentException($"Invalid Type: {type}. " +
                    $"Supported Types to are {string.Join(", ", AllowedTypes)}");
            }

            //foreach (KeyValuePair<string, object> entry in fieldValuePair)
            //{
            //    Console.WriteLine($"{entry.Key}: {entry.Value}");
            //}
            Console.WriteLine("-------------");
        }
        public override void ExecuteCommand()
        {
            //DatabaseCommand c = new DatabaseCommand();
            //Console.WriteLine("Executing");

            if (type == "Employee")
            {

                DatabaseCommand.UpdateEmployee(person);
            }
            if (type == "Prehire")
            {
                DatabaseCommand.UpdatePrehire(person);
            }
            if (type == "Retiree")
            {
                DatabaseCommand.UpdateRetiree(person);
            }

        }
    }
}
