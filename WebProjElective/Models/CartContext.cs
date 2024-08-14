using MySql.Data.MySqlClient;

namespace WebProjElective.Models
{
    public class CartContext
    {
        private readonly MySqlConnection _mySqlConnection;
        private readonly string _connectionString;

        public CartContext(string connectionString)
        {
            _connectionString = connectionString;
            _mySqlConnection = new MySqlConnection(_connectionString);
        }

        public bool InsertonCart(Cart cart)
        {
            try
            {
                _mySqlConnection.Open();

                // Insert into the first table (oncarts)
                MySqlCommand command1 = new MySqlCommand(
                    "INSERT INTO oncarts (idproducts, name, price, prodimg, category, username, dateordered) " +
                    "VALUES (@idproducts, @name, @price, @prodimg, @category, @username, @dateordered)", _mySqlConnection);
                command1.Parameters.AddWithValue("@idproducts", cart.ProdId);
                command1.Parameters.AddWithValue("@name", cart.ProdName);
                command1.Parameters.AddWithValue("@price", cart.ProdPrice);
                command1.Parameters.AddWithValue("@prodimg", cart.ProdImage);
                command1.Parameters.AddWithValue("@category", cart.ProdCategory);
                command1.Parameters.AddWithValue("@username", cart.Username);
                command1.Parameters.AddWithValue("@dateordered", cart.ProdDateTime);

                // Insert into the second table (oncartsadmin)
                //MySqlCommand command2 = new MySqlCommand(
                //    "INSERT INTO oncartsadmin (idproducts, name, price, prodimg, category, username, dateordered) " +
                //    "VALUES (@idproducts, @name, @price, @prodimg, @category, @username, @dateordered)", _mySqlConnection);
                //command2.Parameters.AddWithValue("@idproducts", cart.ProdId);
                //command2.Parameters.AddWithValue("@name", cart.ProdName);
                //command2.Parameters.AddWithValue("@price", cart.ProdPrice);
                //command2.Parameters.AddWithValue("@prodimg", cart.ProdImage);
                //command2.Parameters.AddWithValue("@category", cart.ProdCategory);
                //command2.Parameters.AddWithValue("@username", cart.Username);
                //command2.Parameters.AddWithValue("@dateordered", cart.ProdDateTime);

                // Execute both commands
                int rowsAffected1 = command1.ExecuteNonQuery();
                //int rowsAffected2 = command2.ExecuteNonQuery();

                // Return true if rows were inserted into both tables
                return rowsAffected1 > 0/* && rowsAffected2 > 0*/;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                // Log the full exception for debugging
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                return false;
            }
            finally
            {
                _mySqlConnection.Close();
            }
        }


        public List<Cart> GetCartItemsByUsername(string username)
        {
            var cartItems = new List<Cart>();

            try
            {
                _mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand(
                    "SELECT idproducts, name, price, prodimg, category, username FROM oncarts WHERE username = @username", _mySqlConnection);
                command.Parameters.AddWithValue("@username", username);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var cartItem = new Cart
                        {
                            ProdId = reader.GetInt32("idproducts"),
                            ProdName = reader.GetString("name"),
                            ProdPrice = reader.GetInt32("price"),
                            ProdImage = (byte[])reader["prodimg"],
                            ProdCategory = reader.GetString("category"),
                            Username = reader.GetString("username")
                        };
                        cartItems.Add(cartItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                // Log the full exception for debugging
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
            }
            finally
            {
                _mySqlConnection.Close();
            }

            return cartItems;
        }


        public List<Cart> GetOrders()
        {
            List<Cart> carts = new List<Cart>();
            _mySqlConnection.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM oncarts", _mySqlConnection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    carts.Add(new Cart
                    {
                        OrderId = reader.GetInt32("idoncarts"),
                        ProdId = reader.GetInt32("idproducts"),
                        ProdName = reader.GetString("name"),
                        ProdPrice = reader.GetInt32("price"),
                        ProdImage = (byte[])reader["prodimg"],
                        ProdCategory = reader.GetString("category"),
                        Username = reader.GetString("username"),
                        ProdDateTime = reader.GetDateTime("dateordered")
                    });
                }
            }
            _mySqlConnection.Close();
            return carts;
        }

        public bool DeleteOrder(int id)
        {
            try
            {
                _mySqlConnection.Open();
                MySqlCommand sqlCommand = new MySqlCommand("DELETE FROM oncarts WHERE idoncarts = @ID", _mySqlConnection);
                sqlCommand.Parameters.AddWithValue("@ID", id);
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting student: {ex.Message}");
                return false;
            }
        }

        public List<Cart> GetCartsByUserId(int userId)
        {
            var cartItems = new List<Cart>();

            try
            {
                _mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand(
                    "SELECT idproducts, name, price, prodimg, category, username FROM oncarts WHERE userid = @userId", _mySqlConnection);
                command.Parameters.AddWithValue("@userId", userId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var cartItem = new Cart
                        {
                            ProdId = reader.GetInt32("idproducts"),
                            ProdName = reader.GetString("name"),
                            ProdPrice = reader.GetInt32("price"),
                            ProdImage = (byte[])reader["prodimg"],
                            ProdCategory = reader.GetString("category"),
                            Username = reader.GetString("username")
                        };
                        cartItems.Add(cartItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
            }
            finally
            {
                _mySqlConnection.Close();
            }

            return cartItems;
        }


        public void DeleteCartItemsByUsername(string username)
        {
            try
            {
                _mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand(
                    "DELETE FROM oncarts WHERE username = @username", _mySqlConnection);
                command.Parameters.AddWithValue("@username", username);

                command.ExecuteNonQuery();
                _mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Handle exceptions
            }
        }

        public void TransferCartItemsToOrderedProducts(List<Cart> cartItems, string username)
        {
            try
            {
                _mySqlConnection.Open();
                foreach (var item in cartItems)
                {
                    MySqlCommand command = new MySqlCommand(
                        @"INSERT INTO orderedproducts (idproducts, name, price, prodimg, category, username, dateordered)
                      VALUES (@prodId, @prodName, @prodPrice, @prodImage, @prodCategory, @username, @dateOrdered)", _mySqlConnection);

                    command.Parameters.AddWithValue("@prodId", item.ProdId);
                    command.Parameters.AddWithValue("@prodName", item.ProdName);
                    command.Parameters.AddWithValue("@prodPrice", item.ProdPrice);
                    command.Parameters.AddWithValue("@prodImage", item.ProdImage);
                    command.Parameters.AddWithValue("@prodCategory", item.ProdCategory);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@dateOrdered", DateTime.Now);

                    command.ExecuteNonQuery();
                }
                _mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Handle exceptions
            }
        }

    }
}
