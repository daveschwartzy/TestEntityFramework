using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestEntityFrameWork.Models;
using System.Data.Entity;

namespace TestEntityFrameWork.Controllers
{
    public class NorthwindDAL
    {
        private NorthwindEntities ORM = new NorthwindEntities();

        public List<Customer> GetAllCustomers()
        {
            List<Customer> CustomerList = ORM.Customers.ToList();

            return CustomerList;
        }

        public List<string> GetCustomersByCountry()
        {

            List<string> CountryList = ORM.Customers.Select(x => x.Country).Distinct().ToList();

            return CountryList;
        }

        public List<Customer> GetCustomersByID(string CustomerID)
        {

            List<Customer> IdList = new List<Customer>();

            foreach (Customer CustomerRecord in ORM.Customers.ToList())
            {
                if (CustomerRecord.CustomerID.Contains(CustomerID.ToUpper()))
                {
                    IdList.Add(CustomerRecord);

                }
            }

            return IdList;
        }

        public List<Order> GetOrders(string CustomerID)
        {
            Customer Temp = ORM.Customers.Find(CustomerID);

            List<Order> OrderList = Temp.Orders.ToList();

            return OrderList;
        }

        public Customer SaveCustomer(Customer NewCustomerRecord)
        {
            ORM.Customers.Add(NewCustomerRecord);
            ORM.SaveChanges();
            return NewCustomerRecord;


        }

        public bool DeleteCustomer(string CustomerID)
        {
            DbContextTransaction DeleteCustomerTransaction = ORM.Database.BeginTransaction();

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
                return true;
            }

            catch (Exception ex)
            {   //Go to an error page!

                DeleteCustomerTransaction.Rollback();

            }
            return false;


        }

        public Customer SaveUpdatedCustomer(Customer RecordToBeUpdated)
        {
            Customer temp = ORM.Customers.Find(RecordToBeUpdated.CustomerID);


            //2. Update that record & save to database
            temp.CompanyName = RecordToBeUpdated.CompanyName;
            temp.City = RecordToBeUpdated.City;
            temp.Country = RecordToBeUpdated.Country;


            ORM.Entry(temp).State = System.Data.Entity.EntityState.Modified;

            ORM.SaveChanges();

            return temp;
        }

        public Customer UpdateCustomer(string CustomerID)
        {
            Customer output = ORM.Customers.Find(CustomerID);

            return output;
        }
    }
}