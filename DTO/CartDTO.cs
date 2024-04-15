namespace PetsHeaven.DTO
{
    public class CartDTO
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public Double productPrice { get; set; }
        public string productImage { get; set; }
        public int productQuantity { get; set; }
        public int cartQunatity { get; set; }
    }
}
