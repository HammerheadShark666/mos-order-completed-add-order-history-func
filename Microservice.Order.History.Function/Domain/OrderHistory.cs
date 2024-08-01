using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservice.Order.History.Function.Domain;

[Table("MSOS_OrderHistory")]
public class OrderHistory
{
    [Key]
    public Guid Id { get; set; }
     
    public Guid CustomerId { get; set; }

    [MaxLength(10)]
    public string OrderNumber {  get; set; }

    public DateOnly OrderPlaced { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [MaxLength(25)]
    public string OrderStatus { get; set; } 

    [Required]
    [Column(TypeName = "decimal(19, 2)")]
    [Range(0.01, 99999, ErrorMessage = "{0} must be between {1:c} and {2:c}")]
    public decimal Total { get; set; }

    [Required]
    [StringLength(50)]
    public string AddressSurname { get; set; }

    [Required]
    [StringLength(50)]
    public string AddressForename { get; set; }


    [MaxLength(50)]
    public string? AddressLine1 { get; set; }

    [MaxLength(50)]
    public string? AddressLine2 { get; set; }

    [MaxLength(50)]
    public string? AddressLine3 { get; set; }

    [MaxLength(50)]
    [Required]
    public string TownCity { get; set; }

    [MaxLength(50)]
    public string? County { get; set; }

    [MaxLength(10)]
    public string? Postcode { get; set; } 

    [Required]
    [MaxLength(50)]
    public string Country { get; set; } 
     
    [Required]
    public DateTime Created { get; set; } = DateTime.Now;
     
}
