using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

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
            _mySqlConnection.Open();
            MySqlCommand command = new MySqlCommand(
                        "SELECT * FROM products", _mySqlConnection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = reader.GetInt32("idproducts"),
                        ProductName = reader.GetString("name"),
                        ProductPrice = reader.GetInt32("price"),
                        ProductDescription = reader.GetString("description"),
                        ProductImage = reader["prodimg"] as byte[],
                        ProductAvailableItems = reader.GetInt32("availitems"),
                        ProductDateUpload = reader.GetDateTime("dateupload"),
                    });
                }
            }
            _mySqlConnection.Close();
            return products;
        }

        public bool InsertProduct(Product product)
        {
            try
            {
                _mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand(
                        "INSERT INTO products (name, price, description, prodimg, availitems, dateupload) " +
                        "VALUES (@name, @price, @description, @prodimg, @availitems, @dateupload)", _mySqlConnection);
                command.Parameters.AddWithValue("@name", product.ProductName);
                command.Parameters.AddWithValue("@price", product.ProductPrice);
                command.Parameters.AddWithValue("@description", product.ProductDescription);
                command.Parameters.AddWithValue("@prodimg", product.ProductImage);
                command.Parameters.AddWithValue("@availitems", product.ProductAvailableItems);
                command.Parameters.AddWithValue("@dateupload", product.ProductDateUpload);

                int rowsAffected = command.ExecuteNonQuery();
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
