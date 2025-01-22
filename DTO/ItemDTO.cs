using System.ComponentModel.DataAnnotations;

namespace SaleApp.DTO;

public class ItemDTO
{
    [Required]
    public required string ITEM_NAME { get; set; }

    [Required]
    public int QUANTITY { get; set; }

    [Required]
    public double PRICE { get; set; }
}