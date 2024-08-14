using MySql.Data.MySqlClient;
using WebProjElective.Models;

namespace WebProjElective.Models
{
    public class PlacedContext
    {
        private readonly MySqlConnection _mySqlConnection;

        public PlacedContext(string connectionString)
        {
            _mySqlConnection = new MySqlConnection(connectionString);
        }

        public bool InsertPlacedOrder(PlacedOrder po)
        {
            try
            {
                _mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand(
                    @"INSERT INTO placedorder (totalprice, username, dateplaced, shipping, paymentmethod)
                      VALUES(@tprice, @uname, @dplaced, @shippingg, @pmethod)", _mySqlConnection);
                command.Parameters.AddWithValue("@tprice", po.TotalPrice);
                command.Parameters.AddWithValue("@uname", po.UN);
                command.Parameters.AddWithValue("@dplaced", po.DatePlaced);
                command.Parameters.AddWithValue("@shippingg", po.Shipping);
                command.Parameters.AddWithValue("@pmethod", po.PayMethod);

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

        public List<PlacedOrder> GetOrders()
        {
            List<PlacedOrder> po = new List<PlacedOrder>();
            _mySqlConnection.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM placedorder", _mySqlConnection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    po.Add(new PlacedOrder
                    {
                        PlacedId = reader.GetInt32("idplacedorder"),
                        TotalPrice = reader.GetInt32("totalprice"),
                        UN = reader.GetString("username"),
                        DatePlaced = reader.GetDateTime("dateplaced"),
                        Shipping = reader.GetString("shipping"),
                        PayMethod = reader.GetString("paymentmethod")
                    });
                }
            }
            _mySqlConnection.Close();
            return po;
        }

    }
}
