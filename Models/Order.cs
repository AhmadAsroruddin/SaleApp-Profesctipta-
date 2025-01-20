using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SaleApp.Models;

[Table("SO_ORDER")]
public class Order :Controller
{
    [Key]
    public int SO_ORDER_ID { get; set; }   
    public int ORDER_NO { get; set; }
    public DateTime ORDER_DATE{ get; set; }  
    public int COM_CUSTOMER_ID{ get; set; }  
    [ValidateNever]
    [ForeignKey("COM_CUSTOMER_ID")] 
    public required Customer Customer { get; set; }

    public string? ADDRESS{ get; set; }
}