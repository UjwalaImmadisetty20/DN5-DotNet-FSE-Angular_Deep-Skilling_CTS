using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MySqlConnector;

namespace Module3_CSharp_ADO
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            Console.WriteLine("Module3 - C# & ADO.NET examples. Pass an exercise number (1-30) or 'all'.");
            var choice = args.Length > 0 ? args[0] : "menu";
            // When running 'all' we set AutoRunAll so interactive exercises use defaults
            AutoRunAll = (choice == "all");
            if (choice == "menu") ShowMenu();

            if (choice == "all")
            {
                for (int i = 1; i <= 30; i++)
                {
                    Console.WriteLine($"\n--- Running exercise {i} ---\n");
                    await RunExercise(i);
                }
                return 0;
            }

            if (int.TryParse(choice, out int num) && num >= 1 && num <= 30)
            {
                await RunExercise(num);
                return 0;
            }

            Console.WriteLine("Invalid option. Use an exercise number 1-30, 'all', or run without args to see the menu.");
            return 1;
        }

        static bool AutoRunAll = false;

        static void ShowMenu()
        {
            for (int i = 1; i <= 30; i++) Console.WriteLine($"{i}. Exercise {i}");
            Console.WriteLine("Usage: dotnet run -- <n>   (where n is 1..30) or 'all'");
        }

        static async Task RunExercise(int n)
        {
            switch (n)
            {
                case 1: Exercise1_SetupEnv(); break;
                case 2: Exercise2_ValueVsReference(); break;
                case 3: Exercise3_PrimaryConstructor(); break;
                case 4: Exercise4_TypeInference(); break;
                case 5: Exercise5_GradeCalculation(); break;
                case 6: Exercise6_ArrayLoops(); break;
                case 7: Exercise7_MethodOverloading(); break;
                case 8: Exercise8_RefOutIn(); break;
                case 9: Exercise9_LocalFunctions(); break;
                case 10: Exercise10_Constructors(); break;
                case 11: Exercise11_AccessModifiers(); break;
                case 12: Exercise12_AutoProperties(); break;
                case 13: Exercise13_RecordsInit(); break;
                case 14: Exercise14_InheritanceOverride(); break;
                case 15: Exercise15_AbstractVsInterface(); break;
                case 16: Exercise16_NullSafe(); break;
                case 17: Exercise17_NullConditionalChain(); break;
                case 18: Exercise18_RequiredModifier(); break;
                case 19: Exercise19_ListsDictionaries(); break;
                case 20: Exercise20_LINQ(); break;
                case 21: Exercise21_PatternMatching(); break;
                case 22: Exercise22_Tuples(); break;
                case 23: await Exercise23_AsyncUpload(); break;
                case 24: Exercise24_JSONSerialization(); break;
                case 25: Exercise25_FileAndMemoryStream(); break;
                case 26: Exercise26_RaceConditions(); break;
                case 27: Exercise27_DeadlockSimulation(); break;
                case 28: Exercise28_TraceLogging(); break;
                case 29: Exercise29_SanitizeInput(); break;
                case 30: Exercise30_ADONET_CRUD(); break;
                default: Console.WriteLine("Not implemented."); break;
            }
        }

        // 1
        static void Exercise1_SetupEnv()
        {
            Console.WriteLine("Verify .NET SDK and editor installed.");
            Console.WriteLine($"Environment: {Environment.OSVersion}");
            Console.WriteLine("Create a new project: dotnet new console -n Module3_CSharp_ADO");
            Console.WriteLine("Hello World from Main():\nHello World!");
        }

        // 2
        static void Exercise2_ValueVsReference()
        {
            int a = 5;
            Console.WriteLine($"Before: a={a}");
            ModifyValue(a);
            Console.WriteLine($"After ModifyValue: a={a}");

            var person = new SimplePerson { Name = "Alice" };
            Console.WriteLine($"Before: person.Name={person.Name}");
            ModifyReference(person);
            Console.WriteLine($"After ModifyReference: person.Name={person.Name}");

            static void ModifyValue(int x) { x = 42; }
            static void ModifyReference(SimplePerson p) { p.Name = "Bob"; }

            Console.WriteLine("Value types copy; reference types allow mutation of the object referenced.");
        }

        class SimplePerson { public string Name { get; set; } }

        // 3
        static void Exercise3_PrimaryConstructor()
        {
            // C# 12 primary constructor example
            var p = new Person("Sam", "Miller", 30);
            p.PrintInfo();
        }
        public class Person(string FirstName, string LastName, int Age)
        {
            public string FirstName { get; } = FirstName;
            public string LastName { get; } = LastName;
            public int Age { get; } = Age;
            public void PrintInfo() => Console.WriteLine($"Person: {FirstName} {LastName}, Age {Age}");
        }

        // 4
        static void Exercise4_TypeInference()
        {
            var i = 10;
            var s = "hello";
            var sp = new SimplePerson { Name = "Inferred" };
            var list = new List<int> { 1, 2, 3 };
            Console.WriteLine($"Types: i={i.GetType()}, s={s.GetType()}, sp={sp.GetType()}, list={list.GetType()}");
            Console.WriteLine("Use var when the type is obvious; avoid when it hurts readability.");
        }

        // 5
        static void Exercise5_GradeCalculation()
        {
            int score;
            if (AutoRunAll)
            {
                score = 85; // default for automated runs
                Console.WriteLine($"Auto-run: using default score {score}");
            }
            else
            {
                Console.Write("Enter score (0-100): ");
                if (!int.TryParse(Console.ReadLine(), out score)) { Console.WriteLine("Invalid"); return; }
            }
            string grade;
            if (score >= 90) grade = "A";
            else if (score >= 80) grade = "B";
            else if (score >= 70) grade = "C";
            else if (score >= 60) grade = "D";
            else grade = "F";
            Console.WriteLine($"If/else grade: {grade}");

            // switch with pattern matching
            var switchGrade = score switch
            {
                >= 90 => "A",
                >= 80 => "B",
                >= 70 => "C",
                >= 60 => "D",
                _ => "F"
            };
            Console.WriteLine($"Switch pattern grade: {switchGrade}");
        }

        // 6
        static void Exercise6_ArrayLoops()
        {
            int[] arr = { 1, 2, 3, 4, 99, 5 };
            Console.WriteLine("for:");
            for (int i = 0; i < arr.Length; i++) { if (arr[i] == 99) break; Console.WriteLine(arr[i]); }
            Console.WriteLine("foreach (skip even):");
            foreach (var v in arr) { if (v % 2 == 0) continue; Console.WriteLine(v); }
            Console.WriteLine("while:");
            int idx = 0; while (idx < arr.Length) { Console.WriteLine(arr[idx]); idx++; }
            Console.WriteLine("do-while:");
            idx = 0; do { Console.WriteLine(arr[idx]); idx++; } while (idx < 1);
        }

        // 7
        static void Exercise7_MethodOverloading()
        {
            Console.WriteLine(CalculateTotal(2, 3));
            Console.WriteLine(CalculateTotal(1.5, 2.5, 3.0));
        }


        // 8
        static void Exercise8_RefOutIn()
        {
            int a = 1; Console.WriteLine($"Before ref: {a}"); RefExample(ref a); Console.WriteLine($"After ref: {a}");
            int b; OutExample(out b); Console.WriteLine($"After out: {b}");
            int c = 5; InExample(in c); Console.WriteLine($"After in (unchanged): {c}");

            static void RefExample(ref int x) { x = 10; }
            static void OutExample(out int x) { x = 20; }
            static void InExample(in int x) { Console.WriteLine($"In received {x}"); }
        }

        // 9
        static void Exercise9_LocalFunctions()
        {
            Console.WriteLine(CalculateFactorial(5));
            long CalculateFactorial(int n)
            {
                long InnerFact(int m) => m <= 1 ? 1 : m * InnerFact(m - 1);
                return InnerFact(n);
            }
        }

        // 10
        static void Exercise10_Constructors()
        {
            var c1 = new Car();
            var c2 = new Car("Toyota", "Camry", 2021);
            Console.WriteLine(c1);
            Console.WriteLine(c2);
        }
        class Car { public string Make { get; set; } public string Model { get; set; } public int Year { get; set; } public Car() { Make = "Unknown"; Model = "Unknown"; Year = 0; } public Car(string make, string model, int year) { Make = make; Model = model; Year = year; } public override string ToString() => $"{Year} {Make} {Model}"; }

        // 11
        static void Exercise11_AccessModifiers()
        {
            var d = new Derived();
            d.TestAccess();
            var non = new NonDerived();
            non.TryAccess();
        }
        class BaseAccess { public string Pub = "public"; private string Priv = "private"; protected string Prot = "protected"; internal string Intern = "internal"; }
        class Derived : BaseAccess { public void TestAccess() { Console.WriteLine(Pub); Console.WriteLine(Prot); Console.WriteLine(Intern); } }
        class NonDerived { public void TryAccess() { var b = new BaseAccess(); Console.WriteLine(b.Pub); Console.WriteLine(b.Intern); /* cannot access b.Priv or b.Prot */ } }

        // 12
        static void Exercise12_AutoProperties()
        {
            var prod = new Product { Name = "Widget", Price = 9.99m };
            Console.WriteLine($"Product: {prod.Name} {prod.Price}");
            try { prod.Price = -5; } catch (ArgumentOutOfRangeException ex) { Console.WriteLine("Validation caught: " + ex.Message); }
        }
        class Product { public string Name { get; set; } private decimal _price; public decimal Price { get => _price; set { if (value < 0) throw new ArgumentOutOfRangeException(nameof(Price)); _price = value; } } }

        // 13
        static void Exercise13_RecordsInit()
        {
            var emp = new Employee("E001", "Alice", 30);
            var emp2 = emp with { Name = "Alicia" };
            Console.WriteLine(emp);
            Console.WriteLine(emp2);
        }
        public record Employee(string Id, string Name, int Age);

        // 14
        static void Exercise14_InheritanceOverride()
        {
            Shape s1 = new Circle(); Shape s2 = new Rectangle(); s1.Draw(); s2.Draw();
        }
        class Shape { public virtual void Draw() => Console.WriteLine("Drawing shape"); }
        class Circle : Shape { public override void Draw() => Console.WriteLine("Drawing circle"); }
        class Rectangle : Shape { public override void Draw() => Console.WriteLine("Drawing rectangle"); }

        // 15
        static void Exercise15_AbstractVsInterface()
        {
            Vehicle v = new CarVehicle(); v.Drive(); ((IDrivable)v).Start();
        }
        abstract class Vehicle { public abstract void Drive(); }
        interface IDrivable { void Start(); }
        class CarVehicle : Vehicle, IDrivable { public override void Drive() => Console.WriteLine("Driving"); public void Start() => Console.WriteLine("Starting"); }

        // 16
        static void Exercise16_NullSafe()
        {
            PersonNullable p = null;
            Console.WriteLine(p?.Name ?? "No name");
            p = new PersonNullable { Name = null };
            Console.WriteLine(p?.Name ?? "No name");
        }
        class PersonNullable { public string? Name { get; set; } }

        // 17
        static void Exercise17_NullConditionalChain()
        {
            Contact? contact = null;
            Console.WriteLine(contact?.Name ?? "No contact");
            contact = new Contact { Name = null, PhoneNumber = "123" };
            Console.WriteLine(contact?.Name ?? "Name not set");
        }
        class Contact { public string? Name { get; set; } public string? PhoneNumber { get; set; } }

        // 18
        static void Exercise18_RequiredModifier()
        {
            // Requires C# 11/12
            var s = new Student { FirstName = "John", LastName = "Doe", Age = 20 };
            Console.WriteLine(s);
        }
        class Student { public required string FirstName { get; set; } public required string LastName { get; set; } public int Age { get; set; } public override string ToString() => $"{FirstName} {LastName} ({Age})"; }

        // 19
        static void Exercise19_ListsDictionaries()
        {
            var list = new List<string> { "apple", "banana" };
            var dict = new Dictionary<int, string> { [1] = "one", [2] = "two" };
            foreach (var item in list) Console.WriteLine(item);
            foreach (var kv in dict) Console.WriteLine($"{kv.Key} => {kv.Value}");
            list.Add("cherry"); dict.Remove(1);
        }

        // 20
        static void Exercise20_LINQ()
        {
            var orders = new List<Order> { new Order(1, "A", 100), new Order(2, "B", 50), new Order(3, "A", 200) };
            var q = orders.Where(o => o.TotalAmount >= 100).Select(o => new { o.OrderId, o.TotalAmount });
            foreach (var x in q) Console.WriteLine(x);
        }
        record Order(int OrderId, string CustomerName, decimal TotalAmount);

        // 21
        static void Exercise21_PatternMatching()
        {
            void Inspect(object? o)
            {
                switch (o)
                {
                    case int i: Console.WriteLine($"int {i}"); break;
                    case string s: Console.WriteLine($"string {s}"); break;
                    case Order ord when ord.TotalAmount > 100: Console.WriteLine($"Large order {ord.OrderId}"); break;
                    case null: Console.WriteLine("null"); break;
                    default: Console.WriteLine("other"); break;
                }
            }
            Inspect(5); Inspect("hi"); Inspect(new Order(5, "C", 150));
        }

        // 22
        static void Exercise22_Tuples()
        {
            (int, string) GetPair() => (42, "answer");
            var (num, text) = GetPair();
            Console.WriteLine($"{num} {text}");
        }

        // 23
        static async Task Exercise23_AsyncUpload()
        {
            try
            {
                var result = await SimulateUploadAsync("file.bin");
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Upload failed: " + ex.Message);
            }

            static async Task<string> SimulateUploadAsync(string file)
            {
                await Task.Delay(3000);
                return $"Uploaded {file} at {DateTime.Now}";
            }
        }

        // 24
        static void Exercise24_JSONSerialization()
        {
            var user = new User { Name = "Ann", Age = 28, Email = "ann@example.com" };
            var json = JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("user.json", json);
            Console.WriteLine(json);
            var read = JsonSerializer.Deserialize<User>(File.ReadAllText("user.json"));
            Console.WriteLine(read?.Name);
        }
        class User { public string Name { get; set; } public int Age { get; set; } public string Email { get; set; } }

        // 25
        static void Exercise25_FileAndMemoryStream()
        {
            File.WriteAllText("sample.txt", "Hello streams");
            using var fs = new FileStream("sample.txt", FileMode.Open, FileAccess.Read);
            using var ms = new MemoryStream();
            fs.CopyTo(ms);
            Console.WriteLine($"Bytes in memory: {ms.Length}");
        }

        // 26
        static void Exercise26_RaceConditions()
        {
            int counter = 0; object sync = new();
            var threads = new List<Thread>();
            for (int t = 0; t < 10; t++)
            {
                var th = new Thread(() => { for (int i = 0; i < 1000; i++) { lock (sync) { counter++; } } });
                threads.Add(th); th.Start();
            }
            foreach (var th in threads) th.Join();
            Console.WriteLine($"Counter (with lock) = {counter}");
        }

        // Helper overloads for Exercise 7
        static int CalculateTotal(int a, int b) => a + b;
        static double CalculateTotal(double a, double b, double c) => a + b + c;

        // 27
        static void Exercise27_DeadlockSimulation()
        {
            object a = new(); object b = new();
            var t1 = new Thread(() => { lock (a) { Thread.Sleep(100); lock (b) { Console.WriteLine("t1 done"); } } });
            var t2 = new Thread(() => { lock (b) { Thread.Sleep(100); lock (a) { Console.WriteLine("t2 done"); } } });
            t1.Start(); t2.Start();
            if (!t1.Join(1000) || !t2.Join(1000)) Console.WriteLine("Possible deadlock detected; using TryEnter recommended to avoid this.");
        }

        // 28
        static void Exercise28_TraceLogging()
        {
            System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener("log.txt"));
            System.Diagnostics.Trace.AutoFlush = true;
            System.Diagnostics.Trace.WriteLine("Trace entry at " + DateTime.Now);
            Console.WriteLine("Wrote trace to log.txt and console.");
        }

        // 29
        static void Exercise29_SanitizeInput()
        {
            string unsafeInput = "<script>alert('x')</script>Hi";
            string safe = System.Net.WebUtility.HtmlEncode(unsafeInput);
            Console.WriteLine(safe);
        }

        // 30 - ADO.NET CRUD example using MySQL
        static void Exercise30_ADONET_CRUD()
        {
            Console.WriteLine("ADO.NET CRUD demo using MySQL.");
            string connStr = "Server=localhost;Port=3307;Uid=root;Pwd=root;";
            using var conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                Console.WriteLine("Connected to MySQL.");

                using (var cmd = new MySqlCommand("CREATE DATABASE IF NOT EXISTS Module3_ADO", conn)) cmd.ExecuteNonQuery();
                using (var cmd = new MySqlCommand("USE Module3_ADO", conn)) cmd.ExecuteNonQuery();
                using (var cmd = new MySqlCommand(@"CREATE TABLE IF NOT EXISTS Employees (Id INT AUTO_INCREMENT PRIMARY KEY, Name VARCHAR(100), Age INT)", conn)) cmd.ExecuteNonQuery();

                using (var cmd = new MySqlCommand("INSERT INTO Employees (Name, Age) VALUES (@n, @a)", conn))
                {
                    cmd.Parameters.AddWithValue("@n", "John");
                    cmd.Parameters.AddWithValue("@a", 30);
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new MySqlCommand("SELECT Id, Name, Age FROM Employees", conn))
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read()) Console.WriteLine($"{r.GetInt32(0)} {r.GetString(1)} {r.GetInt32(2)}");
                }

                using (var cmd = new MySqlCommand("UPDATE Employees SET Age=@a WHERE Name=@n", conn))
                {
                    cmd.Parameters.AddWithValue("@a", 31);
                    cmd.Parameters.AddWithValue("@n", "John");
                    var rows = cmd.ExecuteNonQuery();
                    Console.WriteLine("Updated rows=" + rows);
                }

                using (var cmd = new MySqlCommand("DELETE FROM Employees WHERE Name=@n", conn))
                {
                    cmd.Parameters.AddWithValue("@n", "John");
                    var rows = cmd.ExecuteNonQuery();
                    Console.WriteLine("Deleted rows=" + rows);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ADO.NET error: " + ex.Message);
            }
        }
    }
}
