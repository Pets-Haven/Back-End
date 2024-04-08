namespace PetsHeaven.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Double Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public Double TotalWeight { get; set; }
        public string AnimalType { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

    }
}
