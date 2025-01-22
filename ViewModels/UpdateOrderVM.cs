using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SaleApp.DTO;
using SaleApp.Models;

namespace SaleApp.ViewModels;

public class UpdateOrderVM
{
    [ValidateNever]
    public IEnumerable<Customer> Customers { get; set; }
    public UpdateOrderDTO Form { get; set; }
}
