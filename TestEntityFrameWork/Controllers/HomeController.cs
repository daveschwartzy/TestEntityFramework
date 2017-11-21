using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using TestEntityFrameWork.Models;

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

            //Make Link in Layout First!

            //1. Creating an object for the ORM

            NorthwindEntities ORM = new NorthwindEntities();

            //2. Load the data from the DbSet into a data structure (list, array, etc.)

            List<Customer> CustomerList = ORM.Customers.ToList();

            //3. Filter the data (optional)

            ViewBag.CustomerList = CustomerList;

            return View("CustomersView");

            //Go to view and write Foreach loop!
        }

        public ActionResult ListCustomersByCountry(string Country)
        {
            NorthwindEntities ORM = new NorthwindEntities();

            List<Customer> OutputList = new List<Customer>();

            foreach(Customer CustomerRecord in ORM.Customers.ToList())
            {
                if(CustomerRecord.Country != null && CustomerRecord.Country.ToLower() == Country.ToLower())
                {
                    OutputList.Add(CustomerRecord);

                }
            }

            ViewBag.CustomerList = OutputList;

            return View("CustomersView");
        }
       
        public ActionResult ListCustomersByCustomerID(string CustomerID)
        {
            NorthwindEntities ORM = new NorthwindEntities();

            List<Customer> IdList = new List<Customer>();

            foreach (Customer CustomerRecord in ORM.Customers.ToList())
            {
                if (CustomerRecord.CustomerID.Contains(CustomerID.ToUpper()))
                {
                    IdList.Add(CustomerRecord);

                }
            }

            ViewBag.CustomerList = IdList;

            return View("CustomersView");
        }
    }
}