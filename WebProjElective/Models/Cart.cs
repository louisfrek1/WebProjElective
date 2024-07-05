namespace WebProjElective.Models
{
    public class Cart
    {
        public int OrderId { get; set; }
        public int ProdId { get; set; }
        public string ProdName { get; set; }
        public int ProdPrice { get; set; }
        public byte[] ProdImage { get; set; }
        public string ProdImageBase64 { get; set; } // This is for the base64 string received from the client
        public string ProdCategory { get; set; }
        public string Username { get; set; }
        public DateTime ProdDateTime { get; set; }
    }
}
