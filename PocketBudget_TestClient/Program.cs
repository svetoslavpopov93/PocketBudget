using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PocketBudget.Models;
using System.Data.Entity;
using System.Globalization;
using PocketBudget.Data.Migrations;

namespace PocketBudget_TestClient
{
    class Program
    {
        const string currency = "lv";

        static void Main()
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<BillContext, Configuration>());

            while (true)
            {
                try
                {
                    MainMenu();

                    ConsoleKey userChoise = Console.ReadKey().Key;
                    Console.Clear();

                    if (userChoise == ConsoleKey.D1)
                    {
                        DisplayCurrentBillInfo();

                        if (ContinueChoise() == ConsoleKey.Spacebar)
                        {
                            continue;
                        }
                        else
                        {
                            Console.WriteLine();
                            break;
                        }
                    }

                    if (userChoise == ConsoleKey.D2)
                    {
                        DislayAllBIlls();

                        if (ContinueChoise() == ConsoleKey.Spacebar)
                        {
                            continue;
                        }
                        else
                        {
                            Console.WriteLine();
                            break;
                        }
                    }

                    if (userChoise == ConsoleKey.D3)
                    {
                        AddNewBill();

                        if (ContinueChoise() == ConsoleKey.Spacebar)
                        {
                            continue;
                        }
                        else
                        {
                            Console.WriteLine();
                            break;
                        }
                    }

                    if (userChoise == ConsoleKey.D4)
                    {
                        EditBIll();

                        if (ContinueChoise() == ConsoleKey.Spacebar)
                        {
                            continue;
                        }
                        else
                        {
                            Console.WriteLine();
                            break;
                        }
                    }

                    if (userChoise == ConsoleKey.D5)
                    {
                        Console.WriteLine("Enter the name of the wanted bill:");
                        string currentName = Console.ReadLine();

                        DeleteBill(currentName);

                        Console.WriteLine("Done!");
                        Console.WriteLine("To continue press SPACEBAR.");
                        ConsoleKey continueChoice = Console.ReadKey().Key;

                        if (ContinueChoise() == ConsoleKey.Spacebar)
                        {
                            continue;
                        }
                        else
                        {
                            Console.WriteLine();
                            break;
                        }
                    }

                    if (userChoise == ConsoleKey.Escape)
                    {
                        Console.WriteLine("Thank you! Good bye :)");
                        break;
                    }
                }

                catch (FormatException)
                {
                    Console.WriteLine("Incorrect input! Only numbers are alowed!");
                }
            }
        }

        private static void DisplayCurrentBillInfo()
        {
            double totalFamilyEarnings;
            Console.Clear();
            Console.WriteLine("Please enter the total family earnings for this month:");
            totalFamilyEarnings = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            Console.Clear();
            Console.WriteLine("Please enter the total family earnings for this month: {0}{1}", totalFamilyEarnings, currency);
            Console.WriteLine();

            using (var db = new BillContext())
            {
                double totalMoneyAmount = 0;

                Console.Write("Total money needed for paying all bills: ");

                foreach (var bill in db.Bills)
                {
                    if (bill.HasFixedFee == 1)
                    {
                        totalMoneyAmount += bill.TotalAmount;
                    }
                }

                Console.WriteLine(totalMoneyAmount + currency);

                totalMoneyAmount = 0;
                Console.Write("Total money needed for paying all bills this month: ");

                foreach (var bill in db.Bills)
                {
                    totalMoneyAmount += bill.Fee;
                }

                Console.WriteLine(totalMoneyAmount + currency + "\r\n");

                if (totalMoneyAmount > totalFamilyEarnings)
                {
                    double difference = totalMoneyAmount - totalFamilyEarnings;
                    Console.WriteLine("Insufficient funds. Need {0}{1} more for all bills.", difference, currency);
                    Console.WriteLine();
                }
            }

            CountNextBillForPayment();
            Console.WriteLine();
        }

        private static void DislayAllBIlls()
        {
            Console.Clear();
            Console.Write("Current bills in focus: ");

            using (var db = new BillContext())
            {
                Console.WriteLine(db.Bills.Count() + "\r\n");

                foreach (var bill in db.Bills)
                {
                    if (bill.HasFixedFee == 1)
                    {
                        Console.WriteLine
                            ("[{0}] Name:{1}, Monthly fee: {2}{3} per month, Total amount: {4}{3}. First day to pay {5}, last day to pay {6}\r\n",
                            bill.BillId, bill.Name, bill.Fee, currency, bill.TotalAmount, bill.FirstDayToPay, bill.LastDayToPay);
                    }
                    else
                    {
                        Console.WriteLine
                            ("[{0}] Name:{1}, Monthly fee: {2}{3} per month. First day to pay {4}, last day to pay {5}\r\n",
                            bill.BillId, bill.Name, bill.Fee, currency, bill.FirstDayToPay, bill.LastDayToPay);
                    }
                }

                Console.WriteLine("--------------------------------------------------------------------------------");
            }
        }

        private static void AddNewBill()
        {
            Console.Clear();
            Console.WriteLine("To create new bill you need to enter the information needed. \r\nPress any key to continue!");
            ConsoleKey clickToContinue = Console.ReadKey().Key;
            Console.Clear();

            Console.WriteLine("Enter name:");
            string newBillName = Console.ReadLine();
            Console.Clear();

            Console.WriteLine("Enter fee:");
            double newBillFee = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            Console.Clear();

            Console.WriteLine("New bill has fee: (1 or 0)");
            int newBillHasFixedFee = int.Parse(Console.ReadLine());
            Console.Clear();

            double newBillTotalAmount;

            if (newBillHasFixedFee == 1)
            {
                Console.WriteLine("Total amount needed to be paid:");
                newBillTotalAmount = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                Console.Clear();
            }
            else
            {
                newBillTotalAmount = 0;
            }

            Console.WriteLine("First day for payment:");
            int newBillFirstDayToPay = int.Parse(Console.ReadLine());
            Console.Clear();

            Console.WriteLine("Last day for payment:");
            int newBillLastDayToPay = int.Parse(Console.ReadLine());
            Console.Clear();

            using (var db = new BillContext())
            {
                var bill = new Bill()
                {
                    Name = newBillName,
                    Fee = newBillFee,
                    HasFixedFee = newBillHasFixedFee,
                    TotalAmount = newBillTotalAmount,
                    FirstDayToPay = newBillFirstDayToPay,
                    LastDayToPay = newBillLastDayToPay
                };

                db.Bills.Add(bill);
                db.SaveChanges();
            }

            Console.WriteLine("Done! The new bill is added to the database.");
        }

        private static void EditBIll()
        {
            Console.WriteLine("Please select what do you want to change in the current bil:");
            Console.WriteLine("( 1 ) Name");
            Console.WriteLine("( 2 ) Fee");
            Console.WriteLine("( 3 ) First day for payment");
            Console.WriteLine("( 4 ) Last day for payment");
            Console.WriteLine("( 5 ) Has fixed fee");
            Console.WriteLine("( 6 ) Total amount");

            ConsoleKey currentUserChoise = Console.ReadKey().Key;

            if (currentUserChoise == ConsoleKey.D1)
            {
                using (var db = new BillContext())
                {
                    Console.WriteLine("Please select the name of the wanted bill: ");
                    string currentName = Console.ReadLine();
                    string newName = Console.ReadLine();
                    Bill original = (from bill 
                                           in db.Bills 
                                       where bill.Name == currentName 
                                       select bill)
                                       .First();

                    original.Name = newName;
                    original.Fee = original.Fee;
                    original.FirstDayToPay = original.FirstDayToPay;
                    original.LastDayToPay = original.LastDayToPay;
                    original.HasFixedFee = original.HasFixedFee;
                    original.TotalAmount = original.TotalAmount;
                    db.SaveChanges();

                    Console.WriteLine("Done!");
                }
            }
        }

        private static void CountNextBillForPayment()
        {
            using (var db = new BillContext())
            {
                int minDateValue = int.MaxValue;
                var minDate = new Bill();

                foreach (var bill in db.Bills)
                {
                    if (bill.FirstDayToPay < minDateValue)
                    {
                        minDateValue = bill.FirstDayToPay;
                        minDate = bill;
                    }
                }

                Console.WriteLine("Next bill for payment: \r\nName:{0}, Fee:{1} {2}, First day:{3}", minDate.Name, minDate.Fee, currency, minDate.FirstDayToPay);
            }
        }

        public static void DeleteBill(string deleteQueuedName)
        {
            using (var db = new BillContext())
            {
                var result = from r
                                 in db.Bills
                             where r.Name == deleteQueuedName
                             select r;
                //check if there is a record with this name

                if (result.Count() > 0)
                {
                    Bill product = result.First();
                    db.Bills.Remove(product);
                    db.SaveChanges();
                }
            }
        }

        public static ConsoleKey ContinueChoise()
        {
            Console.WriteLine("To continue press SPACEBAR.");
            ConsoleKey continueChoice = Console.ReadKey().Key;
            Console.Clear();

            return continueChoice;
        }

        public static void MainMenu()
        {
            Console.Beep();
            Console.Clear();
            Console.Title = "Pocket Budget";
            Console.WriteLine("Please enter your choise.");
            Console.WriteLine("( 1 ) Display current bill information");
            Console.WriteLine("( 2 ) Display all bills in the database");
            Console.WriteLine("( 3 ) Add new bill to the database");
            Console.WriteLine("( 4 ) Edit bill in the database");
            Console.WriteLine("( 5 ) Delete bill from the database");
            Console.WriteLine("(Esc) Exit");

        }
    }
}
