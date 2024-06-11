namespace WebProjElective.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public byte[] ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductAvailableItems { get; set; }
        public DateTime ProductDateUpload { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }

    }
}
