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
                    @"INSERT INTO users (username, password, email, fname, lname,
                              staddress, city, province, pnumber, gender, bdate, accttype)
                      VALUES(@uname, @pass, @mail, @fname, @lname, @staddress, @city, @province,
                             @pnumber, @gender, @bdate, @accttype)", _mySqlConnection);
                command.Parameters.AddWithValue("@uname", user.UserName);
                command.Parameters.AddWithValue("@pass", user.Password);
                command.Parameters.AddWithValue("@mail", user.Email);
                command.Parameters.AddWithValue("@fname", user.FName);
                command.Parameters.AddWithValue("@lname", user.LName);
                command.Parameters.AddWithValue("@staddress", user.StAddress);
                command.Parameters.AddWithValue("@city", user.City);
                command.Parameters.AddWithValue("@province", user.Province);
                command.Parameters.AddWithValue("@pnumber", user.PNumber);
                command.Parameters.AddWithValue("@gender", user.Gender);
                command.Parameters.AddWithValue("@bdate", user.BDate);
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

        public List<Users> GetUsersni()
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
                        UserName = reader.GetString("username")
                    });
                }
            }
            _mySqlConnection.Close();
            return users;
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
        public Users GetUsersByIds(int id)
        {
            Users users = null;
            _mySqlConnection.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM users WHERE idusers = @id", _mySqlConnection);
            command.Parameters.AddWithValue("@id", id);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    users = new Users
                    {
                        Id = reader.GetInt32("idusers"),
                        UserName = reader.GetString("username"),
                        Email = reader.GetString("email"),
                        AcctType = reader.GetString("accttype")
                    };
                }
            }
            _mySqlConnection.Close();
            return users;
        }

        public Users GetUsersById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid user ID", nameof(id));
            }

            Users user = null;
            try
            {
                _mySqlConnection.Open();
                var command = new MySqlCommand(
                    "SELECT username, email, accttype, fname, lname, staddress, city, province, pnumber FROM users WHERE idusers = @id",
                    _mySqlConnection);
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new Users
                        {
                            Id = reader.GetInt32("idusers"),
                            UserName = reader.GetString("username"),
                            Email = reader.GetString("email"),
                            AcctType = reader.GetString("accttype"),
                            StAddress = reader.GetString("staddress"),
                            City = reader.GetString("city"),
                            Province = reader.GetString("province"),
                            PNumber = reader.GetInt32("pnumber"),
                            FName = reader.GetString("fname"),
                            LName = reader.GetString("lname")
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user: {ex.Message}");
            }
            finally
            {
                _mySqlConnection.Close();
            }

            return user;
        }


        public bool UpdateUser(Users users)
        {
            try
            {
                _mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand(
                    @"UPDATE users SET username = @un, email = @email, accttype = @acc
                        WHERE idusers = @id", _mySqlConnection);
                command.Parameters.AddWithValue("@id", users.Id);
                command.Parameters.AddWithValue("@un", users.UserName);
                command.Parameters.AddWithValue("@email", users.Email);
                command.Parameters.AddWithValue("@acc", users.AcctType);

                int rowsAffected = command.ExecuteNonQuery();
                _mySqlConnection.Close();

                return rowsAffected > 0;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool DeleteUser(int id)
        {
            try
            {
                _mySqlConnection.Open();
                MySqlCommand sqlCommand = new MySqlCommand("DELETE FROM users WHERE idusers = @id", _mySqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", id);
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting student: {ex.Message}");
                return false;
            }
        }

        public Users GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Invalid username", nameof(username));
            }

            Users user = null;
            try
            {
                _mySqlConnection.Open();
                var command = new MySqlCommand(
                    "SELECT idusers, username, email, accttype, fname, lname, staddress, city, province, pnumber, bdate FROM users WHERE username = @username",
                    _mySqlConnection);
                command.Parameters.AddWithValue("@username", username);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new Users
                        {
                            Id = reader.GetInt32("idusers"),
                            UserName = reader.GetString("username"),
                            Email = reader.GetString("email"),
                            AcctType = reader.GetString("accttype"),
                            StAddress = reader.GetString("staddress"),
                            City = reader.GetString("city"),
                            Province = reader.GetString("province"),
                            PNumber = reader.GetInt32("pnumber"),
                            FName = reader.GetString("fname"),
                            LName = reader.GetString("lname"),
                            BDate = reader.GetDateTime("bdate")
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user: {ex.Message}");
            }
            finally
            {
                _mySqlConnection.Close();
            }

            return user;
        }





    }


}
