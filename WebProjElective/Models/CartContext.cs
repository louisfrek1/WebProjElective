using MySql.Data.MySqlClient;

namespace WebProjElective.Models
{
    public class CartContext
    {
        private readonly MySqlConnection _mySqlConnection;

        public CartContext(string connectionString)
        {
            _mySqlConnection = new MySqlConnection(connectionString);
        }
        public bool InsertonCart(Cart cart)
        {

            try
            {
                _mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand(
                    "INSERT INTO oncarts (idproducts, name, price, prodimg, category, username) " +
                    "VALUES (@idproducts, @name, @price, @prodimg, @category, @username)", _mySqlConnection);
                command.Parameters.AddWithValue("@idproducts", cart.ProdId);
                command.Parameters.AddWithValue("@name", cart.ProdName);
                command.Parameters.AddWithValue("@price", cart.ProdPrice);
                command.Parameters.AddWithValue("@prodimg", cart.ProdImage);
                command.Parameters.AddWithValue("@category", cart.ProdCategory);
                command.Parameters.AddWithValue("@username", cart.Username);

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
    }
}
