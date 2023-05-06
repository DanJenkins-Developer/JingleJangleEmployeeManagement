using JingleJangle.AppControllers;

namespace AppControllerUnitTest
{
    [TestClass]
    public class UnitTest
    {
        // List
        [TestMethod]
        public void Test_ListEmployee_Working1()
        {
            string userInput = "LIST All 6 20 FirstName Descending";

            AppControllerList list = new(userInput);
            list.Initialize();
            Assert.AreEqual(list.IsValid, true);
        }
        [TestMethod]
        public void Test_ListAll_Working()
        {
            string userInput = "LIST Employee 6 20 LastName Ascending";
            AppControllerList list = new(userInput);
            list.Initialize();
            Assert.AreEqual(list.IsValid, true);
        }
        [TestMethod]
        public void Test_ListEmployee_Working3()
        {
            string userInput = "LIST Retiree 6 20 EmployeeID Ascending";
            AppControllerList list = new(userInput);
            list.Initialize();
            Assert.AreEqual(list.IsValid, true);
        }
        [TestMethod]
        public void Test_ListPrehire_Working4()
        {
            string userInput = "LIST Prehire 6 20 EmploymentDate Descending";
            AppControllerList list = new(userInput);
            list.Initialize();
            Assert.AreEqual(list.IsValid, true);
        }
        [TestMethod]
        public void Test_ListEmployee_NotWorking1()
        {
            string userInput = "Employee 1 1 OfferExtendedDate Descending";
            AppControllerList list = new(userInput);
            list.Initialize();
            Assert.AreEqual(list.IsValid, false);
        }
        // Add
        [TestMethod]
        public void Test_AddEmployee_Valid()
        {
            string userInput = "ADD Employee FirstName=Dan LastName=Jenkins JobTitle=IT MonthlySalary=70000 EmploymentDate=10/23/21";
            AppControllerAdd add = new(userInput);
            add.Initialize();
            Assert.AreEqual(add.IsValid, true);


        }
        [TestMethod]
        public void Test_AddPrehire_Valid ()
        {
            string userInput = "ADD Prehire FirstName=Nathan LastName=Snow OfferExtendedDate=01/21/23 OfferAcceptanceDate=01/31/23 EmploymentDate=02/14/23";
            AppControllerAdd add = new(userInput);
            add.Initialize();
            Assert.AreEqual(add.IsValid, true);
        }
        [TestMethod]
        public void Test_AddRetiree_Valid()
        {
            string userInput = "ADD Retiree FirstName=Austin LastName=Hale RetirementProgram=ORP RetirementDate=04/19/23 EmploymentDate=04/10/1989";
            AppControllerAdd add = new(userInput);
            add.Initialize();
            Assert.AreEqual(add.IsValid, true);
        }
        [TestMethod]
        public void Test_AddEmployee_Invalid()
        {
            // Missing first name
            string userInput = "ADD Employee LastName=Hale RetirementProgram=ORP RetirementDate=04/19/23 EmploymentDate=04/10/1989";
            AppControllerAdd add = new(userInput);
            add.Initialize();
            Assert.AreEqual(add.IsValid, false);
        }
        [TestMethod]
        public void Test_AddPrehire_Invalid()
        {
            // Missing offer extended date and Employement date
            string userInput = "ADD Prehire FirstName=Nathan LastName=Snow OfferAcceptanceDate=01/31/23";
            AppControllerAdd add = new(userInput);
            add.Initialize();
            Assert.AreEqual(add.IsValid, false);
        }
        [TestMethod]
        public void Test_AddRetiree_Invalid()
        {
            // Provided incorrect fields
            string userInput = "ADD Retiree FirstName=Dan LastName=Jenkins JobTitle=IT MonthlySalary=70000 EmploymentDate=10/23/21";
            AppControllerAdd add = new(userInput);
            add.Initialize();
            Assert.AreEqual(add.IsValid, false);
        }
        // Update
        [TestMethod]
        public void Test_Update_Valid()
        {
            string userInput = "UPDATE 23 FirstName=Alexis";
            AppControllerUpdate update = new(userInput);
            update.Initialize();
            Assert.AreEqual(update.IsValid, true);
            
        }
        public void Test_Update_Invalid()
        {
            // Person with ID 23 is a retiree so this shouldn't work
            string userInput = "UPDATE 23 OfferExtendedDate=04/30/2022";
            AppControllerUpdate update = new(userInput);
            update.Initialize();
            Assert.AreEqual(update.IsValid, false);
        }
        [TestMethod]
        public void Test_Delete_Valid1()
        {
            string userInput = "DELETE 23";
            AppControllerDelete delete = new(userInput);
            delete.Initialize();
            Assert.AreEqual(delete.IsValid, true);
        }
        [TestMethod]
        public void Test_Delete_Valid2() 
        {
            string userInput = "DELETE *";
            AppControllerDelete delete = new(userInput);
            delete.Initialize();
            Assert.AreEqual(delete.IsValid, true);
        }
        [TestMethod]
        public void Test_Delete_Invalid()
        {
            string userInput = "DELETE dfasdfas";
            AppControllerDelete delete = new(userInput);
            delete.Initialize();
            Assert.AreEqual(delete.IsValid, false);
        }
    }
}