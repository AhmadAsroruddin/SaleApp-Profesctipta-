using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SaleApp.Models;

[Table("SO_ITEM")]
public class Item:BaseModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public long SO_ITEM_ID { get; set; }   
    public long SO_ORDER_ID { get; set; } 
    [ValidateNever]
    [ForeignKey("SO_ORDER_ID")] 
    public Order? Order { get; set; }
    public required string ITEM_NAME { get; set; } 
    public int QUANTITY { get; set; }    
    public float PRICE { get; set; }  
}