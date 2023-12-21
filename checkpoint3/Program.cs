using System;
using Bogus;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace FakerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Подключение к базе данных
            string connectionString = "YourConnectionString";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var faker = new Faker("en");
                var users = Enumerable.Range(1, 10).Select(_ =>
                    new
                    {
                        Name = faker.Name.FirstName(),
                        BirthDate = faker.Date.Between(DateTime.Now.AddYears(-70), DateTime.Now.AddYears(-14))
                    });

                foreach (var user in users)
                {
                    if ((DateTime.Now - user.BirthDate).TotalDays / 365 < 14)
                    {
                        Console.WriteLine("Регистрация запрещена для пользователей младше 14 лет");
                    }
                    SqlCommand command = new SqlCommand("INSERT INTO Users (Name, BirthDate) VALUES (@Name, @BirthDate)", connection);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@BirthDate", user.BirthDate);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}