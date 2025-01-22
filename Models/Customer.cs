using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace SaleApp.Models;

public class Customer:BaseModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int COM_CUSTOMER_ID { get; set; }
    public required string CUSTOMER_NAME { get; set; }
}