using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab3Charp
{
    class Date
    {
        protected int day;
        protected int month;
        protected int year;

        public Date(int day, int month, int year)
        {
            Day = day;
            Month = month;
            Year = year;
        }

        public int Day
        {
            get => day;
            set
            {
                if (value >= 1 && value <= 31)
                    day = value;
                else
                    day = -1;
            }
        }

        public int Month
        {
            get => month;
            set
            {
                if (value >= 1 && value <= 12)
                    month = value;
                else
                    month = -1;
            }
        }

        public int Year
        {
            get => year;
            set
            {
                if (value >= 1)
                    year = value;
                else
                    year = -1;
            }
        }

        public int Century => (year - 1) / 100 + 1;

        public bool IsValid()
        {
            if (day == -1 || month == -1 || year == -1)
                return false;

            int[] daysInMonth = { 31, IsLeapYear() ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            return day <= daysInMonth[month - 1];
        }

        private bool IsLeapYear()
        {
            return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
        }

        public string PrintFull()
        {
            string[] months = { "січня", "лютого", "березня", "квітня", "травня", "червня", "липня", "серпня", "вересня", "жовтня", "листопада", "грудня" };
            return $"{day} {months[month - 1]} {year} року";
        }

        public string PrintShort()
        {
            return $"{day:D2}.{month:D2}.{year}";
        }

        public int DaysBetween(Date other)
        {
            DateTime d1 = new DateTime(this.year, this.month, this.day);
            DateTime d2 = new DateTime(other.year, other.month, other.day);
            return Math.Abs((d1 - d2).Days);
        }
    }

   
    class Document
    {
        public string Title { get; set; }
        public int Number { get; set; }

        public Document(string title, int number)
        {
            Title = title;
            Number = number >= 0 ? number : 0;
        }

        public virtual void Print()
        {
            Console.WriteLine($"Документ: {Title}, №{Number}");
        }
    }

    class Receipt : Document
    {
        public double Amount { get; set; }

        public Receipt(string title, int number, double amount) : base(title, number)
        {
            Amount = amount >= 0 ? amount : 0;
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Квитанція на суму: {Amount} грн");
        }
    }

    class Waybill : Document
    {
        public string Product { get; set; }

        public Waybill(string title, int number, string product) : base(title, number)
        {
            Product = product ?? "Невідомо";
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Накладна на товар: {Product}");
        }
    }

    class Invoice : Document
    {
        public string Client { get; set; }

        public Invoice(string title, int number, string client) : base(title, number)
        {
            Client = client ?? "Невідомо";
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Рахунок для: {Client}");
        }
    }

   
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("=== МЕНЮ ===");
                Console.WriteLine("1. Завдання 1: Обробка дат");
                Console.WriteLine("2. Завдання 2: Документи");
                Console.WriteLine("0. Вихід");
                Console.Write("Ваш вибір: ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    RunDateTask();
                }
                else if (choice == "2")
                {
                    RunDocumentTask();
                }
                else if (choice == "0")
                {
                    Console.WriteLine("Завершення програми...");
                    break;
                }
                else
                {
                    Console.WriteLine("Невірний вибір. Спробуйте ще.");
                }

                Console.WriteLine("\nНатисніть Enter для продовження...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        static void RunDateTask()
        {
            List<Date> dates = new List<Date>
            {
                new Date(5, 1, 2022),
                new Date(28, 2, 2020), // високосний
                new Date(15, 8, 2018),
                new Date(31, 4, 2023), // невалідна дата
                new Date(1, 12, 2024)
            };

            Console.WriteLine("\nКоректні дати:");
            var validDates = dates.Where(d => d.IsValid()).ToList();
            foreach (var d in validDates)
            {
                Console.WriteLine(d.PrintFull());
            }

            Console.WriteLine("\nСортування за зростанням:");
            validDates.Sort((a, b) =>
            {
                DateTime da = new DateTime(a.Year, a.Month, a.Day);
                DateTime db = new DateTime(b.Year, b.Month, b.Day);
                return da.CompareTo(db);
            });
            foreach (var d in validDates)
            {
                Console.WriteLine(d.PrintShort());
            }

            Console.WriteLine("\nНайбільша різниця між двома датами:");
            int maxDiff = 0;
            Date d1 = null, d2 = null;
            for (int i = 0; i < validDates.Count; i++)
            {
                for (int j = i + 1; j < validDates.Count; j++)
                {
                    int diff = validDates[i].DaysBetween(validDates[j]);
                    if (diff > maxDiff)
                    {
                        maxDiff = diff;
                        d1 = validDates[i];
                        d2 = validDates[j];
                    }
                }
            }

            if (d1 != null && d2 != null)
            {
                Console.WriteLine($"{d1.PrintShort()} <-> {d2.PrintShort()} = {maxDiff} днів");
            }
        }

        static void RunDocumentTask()
        {
            Console.WriteLine("\nДокументи:");
            List<Document> docs = new List<Document>
            {
                new Receipt("Квитанція про оплату", 101, 2500.75),
                new Invoice("Рахунок-фактура", 202, "ТОВ \"УкрТехно\""),
                new Waybill("Видаткова накладна", 303, "Принтер Canon LBP")
            };

            foreach (var doc in docs)
            {
                doc.Print();
                Console.WriteLine();
            }
        }
    }
}
