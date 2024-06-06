// UserContext.cs

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
                    @"INSERT INTO users (username, password, email)
                      VALUES(@uname, @pass, @mail)", _mySqlConnection);
                command.Parameters.AddWithValue("@uname", user.UserName);
                command.Parameters.AddWithValue("@pass", user.Password);
                command.Parameters.AddWithValue("@mail", user.Email);
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
    }
}
