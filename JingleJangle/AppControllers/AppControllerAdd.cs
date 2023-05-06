using Azure;
using DataAccessLayer;
using JingleJangle.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace JingleJangle.AppControllers
{
    //private string[] data;
    public class AppControllerAdd : AppController
    {
        public static readonly string[] AllowedTypes = { "All", "Prehire", "Employee", "Retiree" };
        HashSet<string> validEmployeeFields = new HashSet<string> { "FirstName", "LastName", "JobTitle", "MonthlySalary", "EmploymentDate" };
        HashSet<string> validPrehireFields = new HashSet<string> { "FirstName", "LastName", "OfferExtendedDate", "OfferAcceptanceDate", "EmploymentDate" };
        HashSet<string> validRetireeFields = new HashSet<string> { "FirstName", "LastName", "RetirementProgram", "RetirementDate", "EmploymentDate" };

        private Dictionary<string, object> fieldValuePair;
        private string type;

        HashSet<string> validFields;

        public AppControllerAdd(string command)
        : base(command)
        {
            fieldValuePair = new Dictionary<string, object>();
            //isValid = true;
        }
        public DateTime  CheckIsValidDate(object dateObj)
        {
            DateTime tempDate;
            string stringDate = (string) dateObj;

            if (!(DateTime.TryParse( stringDate, out tempDate)))
            {
                isValid = false;
                throw new ArgumentException($"Invalid date format: {dateObj}");
            }

            return tempDate;

        }
        public decimal checkIsValidSalary(object salaryObj)
        {
            decimal tempSalary;
            string stringSalary = (string)fieldValuePair["MonthlySalary"];

            if (!(decimal.TryParse(stringSalary, out tempSalary)))
            {
                isValid = false;
                throw new ArgumentException($"Invalid salary: {fieldValuePair["MonthlySalary"]}");
            }

            return tempSalary;
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
            // Removing empty parts to avoid sending wrong errors to the user
            string[] parts = UserInput.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //string type = parts[1];

            type = parts[1];

            // Check the feild value pairs and add them to the dictionary
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
                    //isValid = false;
                    throw new ArgumentException($"Invalid field-value pair: {parts[i]}. ");
                }
            }
        }
        private void AssignValidFields()
        {
            // Update valid fields for the type of add
            if (type == "Employee")
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
                isValid = false;
                // throw exception
                throw new ArgumentException($"Invalid Type: {type}. " +
                    $"Supported Types to are {string.Join(", ", AllowedTypes)}");
            }
        }
        private void ValidateUserProvidedFields()
        {
            // Check if user provided fields are valid
            foreach (string fieldName in fieldValuePair.Keys)
            {

                if (!validFields.Contains(fieldName))
                {
                    isValid = false;

                    throw new ArgumentException($"Invalid fieldname: {fieldName}. " +
                        $"Supported field names are {string.Join(", ", validFields)}");

                }
            }

            List<string> missingFields = new List<string>();

            foreach (string requiredField in validFields)
            {
                if (!fieldValuePair.ContainsKey(requiredField))
                {
                    missingFields.Add(requiredField);
                }
            }
            if (missingFields.Count > 0)
            {
                isValid = false;
                throw new ArgumentException($"Missing required field names. " +
                        $"Missing field names are {string.Join(", ", missingFields)}");
            }

            Console.WriteLine("Initialized");

            // Employee specific checks
            if (type == "Employee")
            {
                fieldValuePair["EmploymentDate"] = CheckIsValidDate(fieldValuePair["EmploymentDate"]);
                fieldValuePair["MonthlySalary"] = checkIsValidSalary(fieldValuePair["MonthlySalary"]);
            }
            else if (type == "Prehire")
            {
                // Check to see if dates are proper
                fieldValuePair["OfferExtendedDate"] = CheckIsValidDate(fieldValuePair["OfferExtendedDate"]);
                fieldValuePair["OfferAcceptanceDate"] = CheckIsValidDate(fieldValuePair["OfferAcceptanceDate"]);
                fieldValuePair["EmploymentDate"] = CheckIsValidDate(fieldValuePair["EmploymentDate"]);


                //Console.WriteLine("You're in here bruv");
            }
            else if (type == "Retiree")
            {
                fieldValuePair["RetirementDate"] = CheckIsValidDate(fieldValuePair["RetirementDate"]);
                fieldValuePair["EmploymentDate"] = CheckIsValidDate(fieldValuePair["EmploymentDate"]);
            }
        }
        public override void ExecuteCommand()
        {
            //DatabaseCommand c = new DatabaseCommand();
            if (isValid)
            {
                Console.WriteLine("Executed");
                if (type == "Employee")
                {
                    Employee();
                    //Console.WriteLine("Employeeing");

                }
                else if (type == "Prehire")
                {
                    Prehire();
                    //Console.WriteLine("Prehireing");
                }
                else if (type == "Retiree")
                {
                    Retiree();
                }


            }
            else
            {
                Console.WriteLine("Invalid Command. Not Executing");
            }

        }
        public void Employee()
        {
            Console.WriteLine();
            Console.WriteLine("Add Employee with the following parameters ::");
            foreach (KeyValuePair<string, object> entry in fieldValuePair)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value}");
            }
            Console.WriteLine("-------------");

            string firstName = (string)fieldValuePair["FirstName"];
            string lastName = (string)fieldValuePair["LastName"];
            string jobTitle = (string)fieldValuePair["JobTitle"];
            decimal monthlySalary = (decimal)fieldValuePair["MonthlySalary"];
            DateTime employmentDate = (DateTime)fieldValuePair["EmploymentDate"];

            DatabaseCommand.AddEmployee(firstName, lastName, jobTitle, monthlySalary, employmentDate);

        }
        public void Prehire()
        {
            Console.WriteLine();
            Console.WriteLine("Add Prehire with the following parameters ::");
            foreach (KeyValuePair<string, object> entry in fieldValuePair)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value}");
            }
            Console.WriteLine("-------------");

            string firstName = (string) fieldValuePair["FirstName"];
            string lastName = (string) fieldValuePair["LastName"];
            DateTime offerExtendedDate = (DateTime) fieldValuePair["OfferExtendedDate"];
            DateTime offerAcceptanceDate = (DateTime) fieldValuePair["OfferAcceptanceDate"];
            DateTime employmentDate = (DateTime) fieldValuePair["EmploymentDate"];

            DatabaseCommand.AddPrehire(firstName, lastName, offerExtendedDate, offerAcceptanceDate, employmentDate);
        }
        public void Retiree()
        {
            Console.WriteLine();
            Console.WriteLine("Adding a Retiree with the following parameters ::");
            foreach (KeyValuePair<string, object> entry in fieldValuePair)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value}");
            }
            Console.WriteLine("-------------");

            string firstName = (string)fieldValuePair["FirstName"];
            string lastName = (string)fieldValuePair["LastName"];
            string retirementProgram = (string)fieldValuePair["RetirementProgram"];
            DateTime retirementDate = (DateTime)fieldValuePair["RetirementDate"];
            DateTime employmentDate = (DateTime)fieldValuePair["EmploymentDate"];

            DatabaseCommand.AddRetiree(firstName, lastName, retirementProgram, retirementDate, employmentDate);
        }
    }
}
