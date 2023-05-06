using DataAccessLayer;
using JingleJangle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace JingleJangle.AppControllers
{
    public class AppControllerList : AppController
    {


        public static readonly string[] AllowedTypes = { "All", "Prehire", "Employee", "Retiree" };
        public static readonly HashSet<string> validPersonFields = new() { "EmployeeID", "FirstName", "LastName", "EmploymentDate" };
        public static readonly HashSet<string> validEmployeeFields = new(validPersonFields.Concat(new[] { "JobTitle", "MonthlySalary" }));
        public static readonly HashSet<string> validPrehireFields = new(validPersonFields.Concat(new[] { "OfferExtendedDate", "OfferAcceptanceDate" }));
        public static readonly HashSet<string> validRetireeFields = new(validPersonFields.Concat(new[] { "RetirementProgram", "RetirementDate" }));


        

        private string type;
        private int page;
        private int count;
        private string orderBy;
        private string direction;

        HashSet<string> validFields;

        public AppControllerList(string command)
            : base(command)
        {
        }
        public override string ToString()
        {
            return $"{base.ToString()}\n" +
        $"Type: {type}\n" +
        $"Page count: {page}\n" +
        $"Records per page: {count}\n" +
        $"OrderBy: {orderBy}\n" +
        $"Direction: {direction}";
        }
        public override void InitializeCommand()
        {
            try
            {

                ValidateCommandStructure();
                // Valid fields are based on the type supplied in the command
                AssignValidFields();
                // We have to see if the user provided fields match the valid fields
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
            //string[] parts = UserInput.Split(" ");
            string[] parts = UserInput.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 6)
            {
                //isValid = false;
                throw new ArgumentException($"Invalid amount of arguments. " +
                    $"Valid LIST syntax is LIST [type] [page] [count] [orderby] [direction]");
            }
            else
            {
                type = parts[1];
                // Note for error handling
                page = int.Parse(parts[2]);
                count = int.Parse(parts[3]);
                orderBy = parts[4];
                direction = parts[5];
            }
        }
        private void AssignValidFields()
        {
            
            if (type == "All")
            {
                validFields = validPersonFields;
            }
            else if (type == "Employee")
            {
                validFields = validEmployeeFields;
            }
            else if (type == "Prehire")
            {
                validFields = validPrehireFields;
            }
            else if (type == "Retiree")
            {
                validFields = validRetireeFields;
            }
            else
            {
                // throw exception
                throw new ArgumentException($"Invalid Type: {type}. " +
                    $"Supported PersonTypes to query for are {string.Join(", ", AllowedTypes)}");
            }
        }
        private void ValidateUserProvidedFields()
        {
            if (page < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(page),
                    page, $"{nameof(page)} must be >= 0");
            }

            // Validate count per page
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count),
                    count, $"{nameof(count)} must be >= 0");
            }

            // Validate orderBy field
            if (!validFields.Contains(orderBy))
            {
                throw new ArgumentException($"Invalid OrderBy column for type \"{type}\": {orderBy}. " +
                    $"Allowed columns are {string.Join(", ", validFields)}");
            }

            // Validate direction field
            if (direction != "Ascending" && direction != "Descending")
            {
                // throw exception
                throw new ArgumentException($"Invalid value Direction column: {direction}. " +
                    $"Allowed values are Ascending or Descending");
            }
            // Change the direction
            if (direction == "Ascending")
            {
                direction = "ASC";
            }
            else if (direction == "Descending")
            {
                direction = "DESC";
            }
        }
        public override void ExecuteCommand()
        {
            //DatabaseCommand command = new DatabaseCommand();

            if (isValid)
            {

                if (type == "All")
                {
                    DumpAllPeople();
                }
                else if (type == "Employee")
                {
                    DumpAllEmployees();
                }
                else if (type == "Prehire")
                {
                    DumpAllPrehires();
                }
                else if (type == "Retiree")
                {
                    DumpAllRetirees();
                }
            }
            else
            {
                Console.WriteLine("Command is invalid. Not executing");
            }
        }
        public void DumpAllPeople()
        {
            
            Console.WriteLine();
            Console.WriteLine($"All People ordered by {orderBy} field and {direction}");
            Console.WriteLine("----------");

            var people = DatabaseCommand.SelectPeople(orderBy, direction);

            int index = 0;
            int totalPages = (int)Math.Ceiling((double)people.Count / count);

            for (int i = 1; i <= page && i <= totalPages; i++)
            {
                Console.WriteLine($"Page {i}");

                for (int j = 1; j <= count && index < people.Count; j++, index++)
                {
                    Console.WriteLine($"EmployeeID: {people[index].EmployeeId}\n" +
                                      $"First Name: {people[index].FirstName}\n" +
                                      $"Last Name: {people[index].LastName}\n" +
                                      $"EmploymentDate: {people[index].EmploymentDate}\n");
                }

                if (i != totalPages) // No need to ask for next page if it's the last page
                {
                    Console.Write("Press enter for the next page: ");
                    Console.ReadLine();
                }
            }
        }
        public void DumpAllEmployees()
        {
            Console.WriteLine();
            Console.WriteLine("All Employees");
            Console.WriteLine("-------------");

            var employees = DatabaseCommand.SelectEmployees(orderBy, direction);

            int index = 0;
            int totalPages = (int)Math.Ceiling((double)employees.Count / count);

            for (int i = 1; i <= page && i <= totalPages; i++)
            {
                Console.WriteLine($"Page {i}");

                for (int j = 1; j <= count && index < employees.Count; j++, index++)
                {
                    // Person ID not a type, I should have named it this in the DB.....
                    Console.WriteLine($"PersonID: {employees[index].EmployeeId}\n" +
                                      $"First Name: {employees[index].FirstName}\n" +
                                      $"Last Name: {employees[index].LastName}\n" +
                                      $"Job Title: {employees[index].JobTitle}\n" +
                                      $"Monthly Salary: {employees[index].MonthlySalary}\n" +
                                      $"Employment Date: {employees[index].EmploymentDate}\n");
                }

                if (i != totalPages) // No need to ask for next page if it's the last page
                {
                    Console.Write("Press enter for the next page: ");
                    Console.ReadLine();
                }
            }
        }
        public void DumpAllPrehires()
        {
            Console.WriteLine();
            Console.WriteLine("All Prehires");
            Console.WriteLine("-------------");

            var prehires = DatabaseCommand.SelectPrehires(orderBy, direction);

            int index = 0;
            int totalPages = (int)Math.Ceiling((double)prehires.Count / count);

            for (int i = 1; i <= page && i <= totalPages; i++)
            {
                Console.WriteLine($"Page {i}");

                for (int j = 1; j <= count && index < prehires.Count; j++, index++)
                {
                    Console.WriteLine($"PersonID: {prehires[index].EmployeeId}\n" +
                                      $"First Name: {prehires[index].FirstName}\n" +
                                      $"Last Name: {prehires[index].LastName}\n" +
                                      $"Offer Extended Date: {prehires[index].OfferExtendedDate}\n" +
                                      $"Offer Acceptance Date: {prehires[index].OfferAcceptanceDate}\n");
                }

                if (i != totalPages) // No need to ask for next page if it's the last page
                {
                    Console.Write("Press enter for the next page: ");
                    Console.ReadLine();
                }
            }
        }
        public void DumpAllRetirees()
        {
            Console.WriteLine();
            Console.WriteLine("All Retirees");
            Console.WriteLine("-------------");

            var retirees = DatabaseCommand.SelectRetirees(orderBy, direction);

            // Pagiation
            int index = 0;
            int totalPages = (int)Math.Ceiling((double)retirees.Count / count);

            for (int i = 1; i <= page && i <= totalPages; i++)
            {
                Console.WriteLine($"Page {i}");

                for (int j = 1; j <= count && index < retirees.Count; j++, index++)
                {
                    Console.WriteLine($"PersonID: {retirees[index].EmployeeId}\n" +
                                      $"First Name: {retirees[index].FirstName}\n" +
                                      $"Last Name: {retirees[index].LastName}\n" +
                                      $"Retirement Program: {retirees[index].RetirementProgram}\n" +
                                      $"Retirement Date: {retirees[index].RetirementDate}\n");
                }

                if (i != totalPages) // No need to ask for next page if it's the last page
                {
                    Console.Write("Press enter for the next page: ");
                    Console.ReadLine();
                }
            }

        }

    }
}
