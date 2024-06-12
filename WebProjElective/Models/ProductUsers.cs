namespace WebProjElective.Models
{
    public class ProductUsers
    {
        public IEnumerable<Users> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
