using System.ComponentModel.DataAnnotations;

namespace SaleApp.DTO;

public class UpdateOrderDTO
{
    public long? SO_ORDER_ID{ get; set; }
    [Required(ErrorMessage ="Order Number cannot be empty")]
    public required string ORDER_NO { get; set; }

    [Required(ErrorMessage ="Order Date cannot be empty")]
    public DateTime ORDER_DATE { get; set; }

    [Required(ErrorMessage ="Customer cannot be empty")]
    public int COM_CUSTOMER_ID { get; set; }

    [Required]
    public string? ADDRESS { get; set; }

    // Item-related properties
    [Required(ErrorMessage ="Items cannot be empty")]
    public List<UpdateItemDTO> Items { get; set; }
}   