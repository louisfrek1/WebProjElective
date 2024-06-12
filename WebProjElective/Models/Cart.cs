namespace WebProjElective.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int ProdId { get; set; }
        public string ProdName { get; set; }
        public int ProdPrice { get; set; }
        public byte[] ProdImage { get; set; }
        public string ProdCategory { get; set; }
        public string Username { get; set; }
    }
}
