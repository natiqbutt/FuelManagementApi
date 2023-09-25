namespace FuelManagementApi.Dtos
{
    public class FuelModel
    {
        public int Id { get; set; }
        public string? VehicleName { get; set; }
        public DateTime? RefillDate { get; set; }
        public decimal? Amount { get; set; }
        public IFormFile? Image { get; set; }
    }
}
