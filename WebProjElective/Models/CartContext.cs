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
                MySqlCommand command = new MySqlCommand(
                    "INSERT INTO oncarts (idproducts, name, price, prodimg, category, username, dateordered) " +
                    "VALUES (@idproducts, @name, @price, @prodimg, @category, @username, @dateordered)", _mySqlConnection);
                command.Parameters.AddWithValue("@idproducts", cart.ProdId);
                command.Parameters.AddWithValue("@name", cart.ProdName);
                command.Parameters.AddWithValue("@price", cart.ProdPrice);
                command.Parameters.AddWithValue("@prodimg", cart.ProdImage);
                command.Parameters.AddWithValue("@category", cart.ProdCategory);
                command.Parameters.AddWithValue("@username", cart.Username);
                command.Parameters.AddWithValue("@dateordered", cart.ProdDateTime);


                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
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
    }
}
