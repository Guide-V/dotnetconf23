namespace VendingMachineService.Models
{
    public class DrinkOrder
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool HasStraw { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set;}
    }
}