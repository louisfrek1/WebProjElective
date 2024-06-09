using MySql.Data.MySqlClient;

namespace WebProjElective.Models
{
    public class ProductContext
    {
        private readonly MySqlConnection _mySqlConnection;

        public ProductContext(string connectionString)
        {
            _mySqlConnection = new MySqlConnection(connectionString);
        }

        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            try
            {
                _mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand(
                    "SELECT name, name, price, description, prodimg, availitems FROM products", _mySqlConnection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product
                    {
                        ProductName = reader["name"].ToString(),
                        ProductPrice = Convert.ToInt32(reader["price"]),
                        ProductDescription = reader["description"].ToString(),
                        ProductImage = (byte[])reader["prodimg"],
                        ProductAvailableItems = Convert.ToInt32(reader["availitems"])
                    };
                    products.Add(product);
                }
                _mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return products;
        }

    }
}
