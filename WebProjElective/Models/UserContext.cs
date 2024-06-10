﻿// UserContext.cs

using MySql.Data.MySqlClient;
using WebProjElective.Models;

namespace WebProjElective.Models
{
    public class UserContext
    {
        private readonly MySqlConnection _mySqlConnection;

        public UserContext(string connectionString)
        {
            _mySqlConnection = new MySqlConnection(connectionString);
        }

        public bool InsertUsers(Users user)
        {
            try
            {
                _mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand(
                    @"INSERT INTO users (username, password, email, accttype)
                      VALUES(@uname, @pass, @mail, 'user')", _mySqlConnection);
                command.Parameters.AddWithValue("@uname", user.UserName);
                command.Parameters.AddWithValue("@pass", user.Password);
                command.Parameters.AddWithValue("@mail", user.Email);
                command.Parameters.AddWithValue("@accttype", user.AcctType);
                int rowsAffected = command.ExecuteNonQuery();
                _mySqlConnection.Close();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public Users GetUser(string email, string password)
        {
            try
            {
                _mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand(
                    @"SELECT username, email, accttype FROM users WHERE email = @mail AND password = @pass", _mySqlConnection);
                command.Parameters.AddWithValue("@mail", email);
                command.Parameters.AddWithValue("@pass", password);
                MySqlDataReader reader = command.ExecuteReader();

                Users user = null;
                if (reader.Read())
                {
                    user = new Users
                    {
                        UserName = reader["username"].ToString(),
                        Email = reader["email"].ToString(),
                        AcctType = reader["accttype"].ToString()
                    };
                }
                _mySqlConnection.Close();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<Users> GetUsers()
        {
            List<Users> users = new List<Users>();
            _mySqlConnection.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM users", _mySqlConnection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(new Users
                    {
                        Id = reader.GetInt32("idusers"),
                        UserName = reader.GetString("username"),
                        Email = reader.GetString("email"),
                        AcctType = reader.GetString("accttype")
                    }) ;
                }
            }
            _mySqlConnection.Close();
            return users;
        }

    }
}
