using JingleJangle.Data;
using JingleJangle.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using DataAccessLayer.Models.List;
using DataAccessLayer.Models.Update;

namespace DataAccessLayer
{

    // •	Implement a DatabaseCommand class that coordinates between the user interface, command handlers, and the data tier
    // •	Utilize EntityFramework to issue all database commands
    // Data Source=GRATEFULDEAD;Initial Catalog=jingleJangleEmployees2023;Integrated Security=True

    // Data command methods that the command class will use to execute actions against the database.
    // These methods will issue EntityFramework commands and assemble the results

    public class DatabaseCommand
    {
        //private readonly JingleJangleEmployees2023Context context = new JingleJangleEmployees2023Context();

        //private string command;
        public DatabaseCommand()
        {
            //this.context = context;
        }
        // list
        public static IList<PersonResult> SelectPeople(string sortField = null, string sortDirection = null)
        {

            using (var dbContext = new JingleJangleEmployees2023Context())
            {
                if (sortField == null)
                {
                    sortField = "EmployeeID";
                }
                if (sortDirection == null)
                {
                    sortDirection = "ASC";
                }
                string sortCondition = sortField + " " + sortDirection;
                var people = dbContext.People.AsQueryable();
                var personTypes = dbContext.PersonTypes.AsQueryable();

                var result = people
                    .Join(
                        personTypes,
                        person => person.PersonTypeId,
                        personType => personType.PersonTypeId,
                        (person, personType) => new PersonResult
                        {
                            EmployeeId = person.EmployeeId,
                            TypeName = personType.TypeName,
                            FirstName = person.FirstName,
                            LastName = person.LastName,
                            EmploymentDate = person.EmploymentDate
                        }
                    ).OrderBy(sortCondition).ToList();




                //people = people.OrderBy(sortCondition);
                //return people.ToList();
                return result;
            }
        }
        public static IList<EmployeeResult> SelectEmployees(string sortField = null, string sortDirection = null)
        {
            using (var dbContext = new JingleJangleEmployees2023Context())
            {
                if (sortField == null)
                {
                    sortField = "EmployeeID";
                }
                if (sortDirection == null)
                {
                    sortDirection = "ASC";
                }
                string sortCondition = sortField + " " + sortDirection;

                var employees = dbContext.Employees.AsQueryable();
                var people = dbContext.People.AsQueryable();

                var result = employees
                    .Join(
                        people,
                        employee => employee.EmployeeId,
                        person => person.EmployeeId,
                        (employee, person) => new EmployeeResult
                        {
                            EmployeeId = employee.EmployeeId,
                            JobTitle = employee.JobTitle,
                            MonthlySalary = employee.MonthlySalary,
                            FirstName = person.FirstName,
                            LastName = person.LastName,
                            EmploymentDate = person.EmploymentDate
                        }
                    ).OrderBy(sortCondition).ToList();

                return result;
            }

        }
        public static IList<PrehireResult> SelectPrehires(string sortField = null, string sortDirection = null)
        {
            using (var dbContext = new JingleJangleEmployees2023Context())
            {
                if (sortField == null)
                {
                    sortField = "EmployeeID";
                }
                if (sortDirection == null)
                {
                    sortDirection = "ASC";
                }
                string sortCondition = sortField + " " + sortDirection;

                var prehires = dbContext.Prehires.AsQueryable();
                var people = dbContext.People.AsQueryable();

                var result = prehires
                    .Join(
                        people,
                        prehire => prehire.EmployeeId,
                        person => person.EmployeeId,
                        (prehire, person) => new PrehireResult
                        {
                            EmployeeId = prehire.EmployeeId,
                            OfferExtendedDate = prehire.OfferExtendedDate,
                            OfferAcceptanceDate = prehire.OfferAcceptanceDate,
                            FirstName = person.FirstName,
                            LastName = person.LastName
                        }
                    ).OrderBy(sortCondition).ToList();

                return result;
            }
        }
        public static IList<RetireeResult> SelectRetirees(string sortField = null, string sortDirection = null)
        {
            using (var dbContext = new JingleJangleEmployees2023Context())
            {
                if (sortField == null)
                {
                    sortField = "EmployeeID";
                }
                if (sortDirection == null)
                {
                    sortDirection = "ASC";
                }
                string sortCondition = sortField + " " + sortDirection;

                var retirees = dbContext.Retirees.AsQueryable();
                var people = dbContext.People.AsQueryable();

                var result = retirees
                    .Join(
                        people,
                        retiree => retiree.EmployeeId,
                        person => person.EmployeeId,
                        (retiree, person) => new RetireeResult
                        {
                            EmployeeId = retiree.EmployeeId,
                            RetirementProgram = retiree.RetirementProgram,
                            RetirementDate = retiree.RetirementDate,
                            FirstName = person.FirstName,
                            LastName = person.LastName
                        }
                    ).OrderBy(sortCondition).ToList();

                return result;
            }
        }
        // Add
        public static void AddEmployee(string firstName, string lastName, string jobTitle, decimal monthlySalary, DateTime employmentDate)
        {
            //Console.WriteLine("INSIDE BBY");
            //foreach (KeyValuePair<string, object> entry in employeeKeyValuePairs)
            //{
            //    Console.WriteLine($"{entry.Key}: {entry.Value}");
            //}

            //string firstName = (string)employeeKeyValuePairs["FirstName"];
            //string lastName = (string)employeeKeyValuePairs["LastName"];
            //DateTime employmentDate = (DateTime)employeeKeyValuePairs["EmploymentDate"];
            //string jobTitle = (string)employeeKeyValuePairs["JobTitle"];
            //decimal monthlySalary = (decimal)employeeKeyValuePairs["MonthlySalary"];

            using (var dbContext = new JingleJangleEmployees2023Context())
            {
                var newPerson = new Person
                {
                    FirstName = firstName,
                    LastName = lastName,
                    EmploymentDate = employmentDate,
                    PersonTypeId = 2,


                };

                dbContext.People.Add(newPerson);
                dbContext.SaveChanges();

                var newEmployee = new Employee
                {
                    EmployeeId = newPerson.EmployeeId,
                    JobTitle = jobTitle,
                    MonthlySalary = monthlySalary
                };

                dbContext.Employees.Add(newEmployee);
                dbContext.SaveChanges();
            }
        }
        public static void AddPrehire(string firstName, string lastName, DateTime offerExtendedDate, DateTime offerAcceptanceDate, DateTime employmentDate)
        {
            using (var dbContext = new JingleJangleEmployees2023Context())
            {

                var newPerson = new Person
                {
                    FirstName = firstName,
                    LastName = lastName,
                    EmploymentDate = employmentDate,
                    PersonTypeId = 1,
                };

                dbContext.People.Add(newPerson);
                dbContext.SaveChanges();

                var newPrehire = new Prehire
                {
                    EmployeeId = newPerson.EmployeeId,
                    OfferExtendedDate = offerExtendedDate,
                    OfferAcceptanceDate = offerAcceptanceDate,
                };

                dbContext.Prehires.Add(newPrehire);
                dbContext.SaveChanges();
            }
        }
        public static void AddRetiree(string firstName, string lastName, string retirementProgram, DateTime retirementDate, DateTime employmentDate)
        {

            using (var dbContext = new JingleJangleEmployees2023Context())
            {

                var newPerson = new Person
                {
                    FirstName = firstName,
                    LastName = lastName,
                    EmploymentDate = employmentDate,
                    PersonTypeId = 3,
                };

                dbContext.People.Add(newPerson);
                dbContext.SaveChanges();

                var newRetiree = new Retiree
                {
                    EmployeeId = newPerson.EmployeeId,
                    RetirementProgram = retirementProgram,
                    RetirementDate = retirementDate,
                };

                dbContext.Retirees.Add(newRetiree); 
                dbContext.SaveChanges();
            }

           
        }
        //Update
        public static Person GetRecordByEmployeeId(int employeeId)
        {
            using (var dbContext = new JingleJangleEmployees2023Context())
            {
                var person = dbContext.People
                    .Include(p => p.Employee)
                    .Include(p => p.Prehire)
                    .Include(p => p.Retiree)
                    .Include(p => p.PersonType)
                    .SingleOrDefault(p => p.EmployeeId == employeeId);

              return person;
            }
        }
        public static void UpdateEmployee(Person updatedPerson)
        {
            using (var dbContext = new JingleJangleEmployees2023Context())
            {
                dbContext.People.Update(updatedPerson);
                dbContext.Employees.Update(updatedPerson.Employee);
                dbContext.SaveChanges();    

            }
        }
        public static void UpdatePrehire(Person updatedPerson)
        {
            using (var dbContext = new JingleJangleEmployees2023Context())
            {
                dbContext.People.Update(updatedPerson);
                dbContext.Prehires.Update(updatedPerson.Prehire);
                dbContext.SaveChanges();

            }
        }
        public static void UpdateRetiree(Person updatedPerson)
        {
            using (var dbContext = new JingleJangleEmployees2023Context())
            {
                dbContext.People.Update(updatedPerson);
                dbContext.Retirees.Update(updatedPerson.Retiree);
                dbContext.SaveChanges();

            }
        }
        // Delete
        public static void DeletePerson(int id)
        {
            
            using (var dbContext = new JingleJangleEmployees2023Context())
            {
                var person = dbContext.People
                    .Include(p => p.Employee)
                    .Include(p => p.Prehire)
                    .Include(p => p.Retiree)
                    .SingleOrDefault(p => p.EmployeeId == id);

                if (person != null)
                {
                    if (person.Employee != null)
                    {
                        Console.WriteLine($"The person is in the Employee table.");
                        dbContext.Employees.Remove(person.Employee);
                    }
                    else if (person.Prehire != null)
                    {
                        Console.WriteLine($"The person is in the Prehire table.");

                        dbContext.Prehires.Remove(person.Prehire);
                    }
                    else if (person.Retiree != null)
                    {
                        Console.WriteLine($"The person is in the Retiree table.");
                        dbContext.Retirees.Remove(person.Retiree);
                    }
                    else
                    {
                        Console.WriteLine($"The person is not in any of the specified tables.");
                    }

                    dbContext.People.Remove(person);
                    dbContext.SaveChanges();
                }
                else
                {
                    Console.WriteLine($"No person with EmployeeId {id} was found.");
                    //return null;
                }
            }
        }
        public static void DeleteAllRecords()
        {
            using (var dbContext = new JingleJangleEmployees2023Context())
            {
                Console.WriteLine("Deleting all records");

                // Remove all records from the Employee table
                dbContext.Employees.RemoveRange(dbContext.Employees);

                // Remove all records from the Prehire table
                dbContext.Prehires.RemoveRange(dbContext.Prehires);

                // Remove all records from the Retiree table
                dbContext.Retirees.RemoveRange(dbContext.Retirees);

                // Remove all records from the People table
                dbContext.People.RemoveRange(dbContext.People);

                // Save changes to the database
                dbContext.SaveChanges();

                Console.WriteLine("All records have been deleted.");
            }
        }
    }


    // Classes required to represent the database to and integrate with EntityFramework so the data command methods can function
    // Hint: You’ll need classes to describe each table in the database
}
