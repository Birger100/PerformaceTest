using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class EntityTest
    {
        public void SpeedTest()
        {
          
            AWContext db = new AWContext();
            var take = 20000;
            var persons = db.Person.Take(take).ToList();
            var sw = new Stopwatch();
            sw.Start();
            Console.WriteLine("------- start of run 1 ---------------");
            foreach (var person in persons)
            {
                var email = db.EmailAddress.First(x => x.BusinessEntityID == person.BusinessEntityID);
                email = DoSomethingToMail(email);
            }
            db.SaveChanges();
            sw.Stop();
          
            Console.WriteLine(DateTime.Now.ToLongTimeString() +" Time 1 : " + sw.ElapsedMilliseconds);
            Console.WriteLine("------- end of run 1 ---------------");
            Console.WriteLine("");
            CleanUpEmails();
            Console.WriteLine("");
            sw = new Stopwatch();
            db = new AWContext();
            Console.WriteLine("------- start of run 2 ---------------");
            // db.Configuration.AutoDetectChangesEnabled = false;
            var persons2 = db.Person.Take(take).ToList();
            sw.Start();
            var emailList = db.EmailAddress.ToList();
            foreach (var person in persons2)
            {
               
                var email = emailList.First(x => x.BusinessEntityID == person.BusinessEntityID);
                email = DoSomethingToMail(email);
            }
            db.SaveChanges();
            sw.Stop();
            Console.WriteLine(DateTime.Now.ToLongTimeString() + " Time 2 : " + sw.ElapsedMilliseconds);
            Console.WriteLine("------- end of run 2 ---------------");
            Console.WriteLine("");
            CleanUpEmails();
            Console.ReadLine();



        }

        private EmailAddress DoSomethingToMail(EmailAddress email)
        {
            if(email.BusinessEntityID % 3 == 0)
            {
                email.EmailAddress1 += "test";
            }else
            {
                email.EmailAddress1 += "test";

            }
            return email;
        } 

        private void CleanUpEmails()
        {
            AWContext db = new AWContext();
            var sw = new Stopwatch();
            Console.WriteLine(DateTime.Now.ToLongTimeString() + " Starting Cleanup");
            sw.Start();
            foreach (var item in db.EmailAddress)
            {
                item.EmailAddress1 = item.EmailAddress1.Replace("test", ""); ;
            }
            db.SaveChanges();
            sw.Stop();
            Console.WriteLine(DateTime.Now.ToLongTimeString() + " Cleanup Done : " +sw.ElapsedMilliseconds);
        }
    }
}
