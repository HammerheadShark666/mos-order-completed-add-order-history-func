using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservice.Order.History.Function.Domain;

[Table("MSOS_OrderHistory_OrderItem")]
public class OrderItem
{
    [Key]
    [ForeignKey("OrderId")]
    public Guid OrderId { get; set; }

    [Key]
    public Guid ProductId { get; set; }

    public OrderHistory Order { get; set; } = default!;

    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    public string ProductType { get; set; } = string.Empty;

    [Required]
    [Range(1, 999, ErrorMessage = "{0} must be between {1:c} and {2:c}")]
    public int Quantity { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    [Range(0.00, 9999, ErrorMessage = "{0} must be between {1:c} and {2:c}")]
    public decimal? UnitPrice { get; set; }


}