using System.ComponentModel.DataAnnotations;

namespace FuelManagementApi.Models
{
    public class Fuel
    {
        [Key]
        public int Id { get; set; }
        public string? VehicleName { get; set; }
        public DateTime? RefillDate { get; set; }
        public decimal? Amount { get; set; }
        public string? Image { get; set; }
    }
}
