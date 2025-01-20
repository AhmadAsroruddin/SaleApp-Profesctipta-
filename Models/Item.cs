using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SaleApp.Models;

[Table("SO_ITEM")]
public class Item
{
    [Key]
    public int SO_ITEM_ID { get; set; }   
    public int SO_ORDER_ID { get; set; } 
    [ValidateNever]
    [ForeignKey("SO_ORDER_ID")] 
    public required Order? Order { get; set; }
    public required string ITEM_NAME { get; set; } 
    public int QUANTITY { get; set; }    
    public decimal PRICE { get; set; }  
}