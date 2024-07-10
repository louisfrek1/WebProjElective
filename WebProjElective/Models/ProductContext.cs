using Microsoft.EntityFrameworkCore.Diagnostics;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

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
            MySqlCommand command = new MySqlCommand("SELECT * FROM products", _mySqlConnection);
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
                        ProductCategory = reader.GetString("category"),
                        ProductAvailableItems = reader.GetInt32("availitems"),
                        ProductDateUpload = reader.GetDateTime("dateupload"),
                        ProductUserName = reader.GetString("uploaderun")
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
                    "INSERT INTO products (name, price, description, prodimg, category, availitems, dateupload, uploaderun) " +
                    "VALUES (@name, @price, @description, @prodimg, @category, @availitems, @dateupload, @uploaderun)", _mySqlConnection);
                command.Parameters.AddWithValue("@name", product.ProductName);
                command.Parameters.AddWithValue("@price", product.ProductPrice);
                command.Parameters.AddWithValue("@description", product.ProductDescription);
                command.Parameters.AddWithValue("@prodimg", product.ProductImage);
                command.Parameters.AddWithValue("@category", product.ProductCategory);
                command.Parameters.AddWithValue("@availitems", product.ProductAvailableItems);
                command.Parameters.AddWithValue("@dateupload", product.ProductDateUpload);
                command.Parameters.AddWithValue("@uploaderun", product.ProductUserName);

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

        public List<Product> GetProductsByCategory(string category)
        {
            List<Product> products = new List<Product>();
            _mySqlConnection.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM products WHERE category = @category", _mySqlConnection);
            command.Parameters.AddWithValue("@category", category);
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
                        ProductCategory = reader.GetString("category"),
                        ProductAvailableItems = reader.GetInt32("availitems"),
                        ProductDateUpload = reader.GetDateTime("dateupload"),
                        ProductUserName = reader.GetString("uploaderun")
                    });
                }
            }
            _mySqlConnection.Close();
            return products;
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            await _mySqlConnection.OpenAsync();
            MySqlCommand command = new MySqlCommand("SELECT * FROM products WHERE idproducts = @productId", _mySqlConnection);
            command.Parameters.AddWithValue("@productId", productId);

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new Product
                    {
                        ProductId = reader.GetInt32("idproducts"),
                        ProductName = reader.GetString("name"),
                        ProductPrice = reader.GetInt32("price"),
                        ProductDescription = reader.GetString("description"),
                        ProductImage = reader["prodimg"] as byte[],
                        ProductCategory = reader.GetString("category"),
                        ProductAvailableItems = reader.GetInt32("availitems"),
                        ProductDateUpload = reader.GetDateTime("dateupload"),
                        ProductUserName = reader.GetString("uploaderun")
                    };
                }
            }

            _mySqlConnection.Close();
            return null;
        }

        public bool UpdateProduct(Product product)
        {
            try
            {
                _mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand(
                    @"UPDATE products SET name=@name, price=@price, description=@description,
                availitems=@availitems, category=@category, dateupload=@dateupload
                WHERE idproducts=@id", _mySqlConnection);

                command.Parameters.AddWithValue("@id", product.ProductId);
                command.Parameters.AddWithValue("@name", product.ProductName);
                command.Parameters.AddWithValue("@price", product.ProductPrice);
                command.Parameters.AddWithValue("@description", product.ProductDescription);
                command.Parameters.AddWithValue("@availitems", product.ProductAvailableItems);
                command.Parameters.AddWithValue("@category", product.ProductCategory);
                command.Parameters.AddWithValue("@dateupload", product.ProductDateUpload);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"UpdateProduct: {rowsAffected} row(s) updated successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine("UpdateProduct: No rows affected. Check if the product exists.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("UpdateProduct Error: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                return false;
            }
            finally
            {
                _mySqlConnection.Close();
            }
        }

        public bool DeleteProduct(int id)
        {
            try
            {
                _mySqlConnection.Open();
                MySqlCommand sqlCommand = new MySqlCommand("DELETE FROM products WHERE idproducts = @id", _mySqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", id);
                int rowAffected = sqlCommand.ExecuteNonQuery();
                return rowAffected > 0;
            }catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Product: {ex.Message}");
                return false;
            }
        }

    }
}
