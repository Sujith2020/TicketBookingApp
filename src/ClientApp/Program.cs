using System;
using static ClientApp.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using BookingServiceNS.Models;

namespace ClientApp
{
    class Program
    {
        static readonly string baseUrl = "http://localhost:8000";
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            Console.WriteLine("********** Welcome To BookMyShow !!!********");
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var login = ReadLoginDetails();
                var accessToken = await Authenticate(login);

                if (accessToken.auth_token == null)
                {
                    Console.WriteLine("\n Incorrect UserName or Password.");
                    System.Environment.Exit(0);
                }
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken.auth_token);
                Console.WriteLine("\n Login Successfull.");

                DisplayMenu();

                string key;
                while ((key = Console.ReadKey().KeyChar.ToString()) != "3")
                {
                    int.TryParse(key, out int keyValue);

                    switch (keyValue)
                    {
                        case 1:
                            await ShowMovies();
                            break;
                        case 2:
                            await ShowBookings();
                            break;
                        default:
                            break;
                    }

                    Console.Write("Enter the option (number): ");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("App interrupted.");
            }
            finally
            {
                Console.WriteLine();
                Console.WriteLine("App closed.");
            }

        }

        static Login ReadLoginDetails()
        {
            Console.WriteLine();
            Console.Write("Enter the user name: ");
            var username = Console.ReadLine();
            Console.Write("Enter the password: ");
            var password = ReadPassword();
            return new Login() { UserName = username, Password = password };
        }


        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        password = password.Substring(0, password.Length - 1);
                        int pos = Console.CursorLeft;
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }

            Console.WriteLine();
            return password;
        }


        static async Task<SecurityToken> Authenticate(Login login)
        {
            var response = await client.PostAsync("/user/authenticate", new StringContent(JsonConvert.SerializeObject(new { Username = login.UserName, Password = login.Password }),
                                Encoding.UTF8, "application/json"));
            var token = await response.Content.ReadAsStringAsync();
            if (token == null)
                return null;
            return JsonConvert.DeserializeObject<SecurityToken>(token);
        }


        static void DisplayMenu()
        {
            Console.WriteLine();
            Console.WriteLine("1. Get Movie Details");
            Console.WriteLine("2. Get Booking Details");
            Console.WriteLine("3. Close app (X)");
            Console.WriteLine();
            Console.Write("Enter the option (number): ");
        }


        static async Task ShowMovies()
        {


            var result = await ShowMoviesAsync();

            Console.WriteLine();
            Console.WriteLine("Movies are...");
            Console.WriteLine();

            if (result != null)
            {
                Console.WriteLine($"Movie Name: {result.Name}");
                Console.WriteLine($"Genre: {result.Genre}");
                Console.WriteLine($"Language: {result.Language}");

            }
            else
            {
                Console.WriteLine($"Status: Transaction failed");
            }

            Console.WriteLine();
        }

        static async Task ShowBookings()
        {
            var result = await ShowBookingsAsync();

            Console.WriteLine();
            Console.WriteLine("Project are...");
            Console.WriteLine();

            if (result != null)
            {
                Console.WriteLine($"BookingId: {result.BookingId}");
                Console.WriteLine($"ShowId: {result.ShowId}");
                Console.WriteLine($"BookingTime: {result.BookingTime}");
                Console.WriteLine($"seats: {result.seats}");

            }
            else
            {
                Console.WriteLine($"Status: Transaction failed");
            }

            Console.WriteLine();
        }


        static async Task<Movie> ShowMoviesAsync()
        {
            var response = await client.GetAsync($"/Movie/GetDetail");
            var transactionResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Movie>(transactionResult);
        }

        static async Task<Booking> ShowBookingsAsync()
        {
            var response = await client.GetAsync($"/Booking/BookingDetail");
            var transactionResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Booking>(transactionResult);
        }

    }
}
