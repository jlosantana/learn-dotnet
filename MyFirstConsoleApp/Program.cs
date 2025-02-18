//Console.WriteLine("What's your name?");
//var name = Console.ReadLine();
//var currentDate = DateTime.Now;
//Console.WriteLine($"{Environment.NewLine}Hello, {name}, on {currentDate:d}");
//Console.Write($"{Environment.NewLine}Press any key to exit...");

using PasswordGenerator;

var pwd = new Password();
var password = pwd.Next();
Console.WriteLine(password.ToString());

