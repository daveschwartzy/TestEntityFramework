using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using TestEntityFrameWork.Models;
using System.Data.Entity;

namespace TestEntityFrameWork.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            /*SqlConnection c = new SqlConnection("");
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = c;
            cmd.CommandText = "select * from customers";

            c.Open();
            cmd.ExecuteReader();
            */
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        public ActionResult ListCustomers()
        {

            NorthwindDAL DAL = new NorthwindDAL();
            ////Make Link in Layout First!

            ////1. Creating an object for the ORM

            //NorthwindEntities ORM = new NorthwindEntities();

            ////2. Load the data from the DbSet into a data structure (list, array, etc.)

            //List<Customer> CustomerList = ORM.Customers.ToList();

            //3. Filter the data (optional)

            ViewBag.CustomerList = DAL.GetAllCustomers();

            ViewBag.CountryList = DAL.GetCustomersByCountry();

            return View("CustomersView");

            //Go to view and write Foreach loop!
        }

        public ActionResult ListCustomersByCountry(string Country)
        {
            NorthwindEntities ORM = new NorthwindEntities();

            List<Customer> OutputList = new List<Customer>();

            ViewBag.CountryList = ORM.Customers.Select(x => x.Country).Distinct().ToList();

            //foreach syntax
            //foreach(Customer CustomerRecord in ORM.Customers.ToList())
            //{
            //    if(CustomerRecord.Country != null && CustomerRecord.Country.ToLower() == Country.ToLower())
            //    {
            //        OutputList.Add(CustomerRecord);

            //    }
            //}

            //LINQ Query syntax
            //OutputList = (from CustomerRecord in ORM.Customers
            //where CustomerRecord.Country == Country
            //select CustomerRecord).ToList();



            //LINQ Method syntax
            //OutputList = ORM.Customers.Where(x => x.Country == Country).ToList();

            //Natural SQL 
            OutputList = ORM.Customers.SqlQuery("select * from customers where country = @param1", new SqlParameter("@param1",Country)).ToList();


            ViewBag.CustomerList = OutputList;
            return View("CustomersView");
        }
       
        public ActionResult ListCustomersByCustomerID(string CustomerID)
        {
            NorthwindDAL DAL = new NorthwindDAL();

            //List<Customer> IdList = new List<Customer>();

            //foreach (Customer CustomerRecord in ORM.Customers.ToList())
            //{
            //    if (CustomerRecord.CustomerID.Contains(CustomerID.ToUpper()))
            //    {
            //        IdList.Add(CustomerRecord);

            //    }
            //}

            ViewBag.CustomerList = DAL.GetCustomersByID(CustomerID);

            return View("CustomersView");
        }

        //this action deletes a single row (has no transactions)
        /*public ActionResult DeleteCustomer(string CustomerID)
        {   //ToDo: Add exception handling for db exceptions
            //1. Initalize the database
            NorthwindEntities ORM = new NorthwindEntities();

            //2.Find the record - Use find (which looks for a record based on the primary key)
            Customer RecordToBeDeleted = ORM.Customers.Find(CustomerID);

            //3. Delete the record using the ORM
            if (RecordToBeDeleted != null)
            {
                ORM.Customers.Remove(RecordToBeDeleted);
                ORM.SaveChanges();
            }

            //4. Reload the list
            //this will actually reload the list
            return RedirectToAction("ListCustomers");
            
        }
        */

        public ActionResult DeleteCustomer(string CustomerID)
        {
            NorthwindDAL DAL = new NorthwindDAL();
            //create database
            //NorthwindEntities ORM = new NorthwindEntities();

            //create transaction - make sure you have "using System.Data.Entity;" up top
            
            if (DAL.DeleteCustomer(CustomerID) == true)
            {

                return RedirectToAction("ListCustomers");
            }

            else
            {
                return View("ErrorMessage");
            }
            /*
            try
            {
                //1. Find the customer

                Customer Temp = ORM.Customers.Find(CustomerID);

                //2. Find all orders for that customer
                //3. Delete those orders

                //ORM.ArchivedOrders.AddRange(Temp.Orders); 

                Temp.Orders.Clear();

                //4. Delete that customer

                ORM.Customers.Remove(Temp);

                //5. Save changes to the db

                ORM.SaveChanges();

                //6. Commit the transaction - will not delete, will time out if it does not have this commit

                DeleteCustomerTransaction.Commit();
            }

            catch (Exception ex)
            {   //Go to an error page!

                DeleteCustomerTransaction.Rollback();

            }*/

            
        }

        public ActionResult NewCustomerForm()
        {
            return View();
        }

        public ActionResult SaveCustomer(Customer NewCustomerRecord)
        {
            //1. Validation (if model state is valid or not)

            if(ModelState.IsValid)
            {

                //2. Initalize DB
                NorthwindDAL DAL = new NorthwindDAL();
                DAL.SaveCustomer(NewCustomerRecord);
                //3. Add the new record to the ORM
                //ORM.Customers.Add(NewCustomerRecord);
                //ORM.SaveChanges();

                //4. Show the list of all customers
                return RedirectToAction("ListCustomers");

            }
            else
            {
                //if validation fails
                //go back to the form and show the list of errors

                return View("NewCustomerForm");

            }
        }

        public ActionResult UpdateCustomer(string CustomerID)
        {
            NorthwindDAL DAL = new NorthwindDAL();
            //1. Find the customer by using the CustomerID
            Customer output = DAL.UpdateCustomer(CustomerID);

            //2. Load the record into a ViewBag
            if (output != null)
            {
                ViewBag.RecordToBeUpdated = output;
                //3. Go to the view that has the update form
                return View("UpdateCustomerForm");

            }
            else
            {//ToDo: Create an error message
                return RedirectToAction("ListCustomers");

            }
        }

        public ActionResult SaveUpdatedCustomer(Customer RecordToBeUpdated)
        {
            //1. Find the original record
            NorthwindDAL DAL = new NorthwindDAL();
            DAL.SaveUpdatedCustomer(RecordToBeUpdated);

            //3. Load all the customer records
            return RedirectToAction("ListCustomers");
        }

        public ActionResult GetOrders(string CustomerID)
        {
            //1. Create ORM

            NorthwindDAL DAL = new NorthwindDAL();

            //2. Find that customer

            //Customer Temp = ORM.Customers.Find(CustomerID);

            //3. Pull orders made by customer

            //List<Order> ListOfOrders = Temp.Orders.ToList();

            //4. Send data to the view using the ViewBag

            ViewBag.ListOfOrders = DAL.GetOrders(CustomerID);

            return View();
        }
    }
}