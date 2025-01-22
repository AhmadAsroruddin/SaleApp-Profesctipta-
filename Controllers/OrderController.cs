using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SaleApp.DTO;
using SaleApp.Models;
using SaleApp.Repositories.Interface;
using SaleApp.ViewModels;
using Newtonsoft.Json.Linq;


namespace SaleApp.Controllers;

[Route("[controller]")]
public class OrderController :Controller
{
    private readonly IUnitOfWork unitOfWork;

    public OrderController(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    [Route("Index")]
    public IActionResult Index()
    {
        return View();
    }
    [Route("Data")]
    public IActionResult Data([FromQuery] DateTime? orderDate)
    {
        var orders = unitOfWork.Order.GetAll(e => orderDate == null || e.ORDER_DATE == orderDate, includeProperties:["Customer"]);

        return Json(new { data = orders });
    }

    [Route("Create")]
    public IActionResult Create()
    {
        var customers = unitOfWork.Customer.GetAll();

        var model = new CreateOrderVM
        {
            Customers = customers
        };
       
        return View(model);
    }

    [HttpPost]
    [Route("SaveOrder")]
    public IActionResult SaveOrder(CreateOrderVM dto)
    {
        using (var transaction = unitOfWork.CreateTransaction())
        {
            try
            {
                var order = new Order()
                {
                    COM_CUSTOMER_ID = dto.Form.COM_CUSTOMER_ID,
                    ORDER_NO = dto.Form.ORDER_NO,
                    ORDER_DATE = dto.Form.ORDER_DATE,
                    ADDRESS = dto.Form.ADDRESS,
                };

                unitOfWork.Order.Add(order);
                unitOfWork.Save();

                Order newOrder = order;

                foreach (var item in dto.Form.Items)
                {
                    var tempItem = new Item()
                    {
                        ITEM_NAME = item.ITEM_NAME,
                        PRICE = float.Parse(item.PRICE.ToString()),
                        QUANTITY = item.QUANTITY,
                        SO_ORDER_ID = newOrder.SO_ORDER_ID
                    };

                    unitOfWork.Item.Add(tempItem);
                }

                unitOfWork.Save();

                transaction.Commit();
                
                TempData["SuccessMessage"] = "Order created successfully";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Rollback transaksi jika terjadi error
                transaction.Rollback();
                ModelState.AddModelError("", "Terjadi kesalahan saat menyimpan data: " + ex.Message);
                return View("Error");
            }
        }
    }

    [Route("Delete/{id}")]
    public IActionResult Delete(int id)
    {
        var transaction = unitOfWork.CreateTransaction();
        try
        {
            var order = unitOfWork.Order.Get(e => e.SO_ORDER_ID == id);

            if (order == null)
            {
                return NotFound(); 
            }
            unitOfWork.Order.Remove(order);
            unitOfWork.Save();

            var items = unitOfWork.Item.GetAll(e => e.SO_ORDER_ID == order.SO_ORDER_ID);
            Console.WriteLine(items);
            foreach (var item in items)
            {
                unitOfWork.Item.Remove(item);
            }
            unitOfWork.Save();
            transaction.Commit();
            return Json(new { success = true, message = "Order deleted successfully" });
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return Json(new { success = false, message = "An error occurred: " + ex.Message });
        }
    }


    [Route("Edit/{id}")]
    public IActionResult Edit(int id)
    {
       
        var customers = unitOfWork.Customer.GetAll();

        var order = unitOfWork.Order.Get(e => e.SO_ORDER_ID == id, includeProperties:["Customer"]);

        if (order == null)
        {
            return NotFound(); 
        }
        
        List<Item> items = unitOfWork.Item.GetAll(e => e.SO_ORDER_ID == order.SO_ORDER_ID).ToList();
        List<UpdateItemDTO> itemsDTO = [];
        
        foreach (var item in items)
        {
            var itemDTO = new UpdateItemDTO()
            {
                ITEM_NAME = item.ITEM_NAME,
                PRICE = item.PRICE,
                QUANTITY = item.QUANTITY,
                SO_ITEM_ID = item.SO_ITEM_ID
            };
            itemsDTO.Add(itemDTO);
        }

        var dto = new UpdateOrderDTO()
        {
            SO_ORDER_ID = order.SO_ORDER_ID,
            ORDER_NO = order.ORDER_NO,
            ADDRESS = order.ADDRESS,
            Items = itemsDTO,
            COM_CUSTOMER_ID = order.COM_CUSTOMER_ID,
            ORDER_DATE = order.ORDER_DATE
        };

        var viewModel = new UpdateOrderVM()
        {
            Customers = customers,
            Form = dto
        };

        return View(viewModel);
    }



    [HttpPost]
    [Route("UpdateOrder")]
    public IActionResult UpdateOrder(UpdateOrderVM dto)
    {
        using (var transaction = unitOfWork.CreateTransaction())
        {
            try
            {
                // Cari order yang akan diupdate berdasarkan ID
                var existingOrder = unitOfWork.Order.Get(e => e.SO_ORDER_ID == dto.Form.SO_ORDER_ID);
                if (existingOrder == null)
                {
                    ModelState.AddModelError("", "Order tidak ditemukan");
                    return View("Error");
                }

                // Update data order
                existingOrder.ORDER_NO = dto.Form.ORDER_NO;
                existingOrder.ORDER_DATE = dto.Form.ORDER_DATE;
                existingOrder.ADDRESS = dto.Form.ADDRESS;
                existingOrder.COM_CUSTOMER_ID = dto.Form.COM_CUSTOMER_ID;

                unitOfWork.Order.Update(existingOrder); // Update order di database
                unitOfWork.Save();

                // Update atau tambah item
                foreach (var item in dto.Form.Items)
                {
                    var existingItem = unitOfWork.Item.Get(e=>e.SO_ITEM_ID == item.SO_ITEM_ID); 

                    if (existingItem != null)
                    {
                        if(item.ITEM_NAME == null)
                        {
                            unitOfWork.Item.Remove(existingItem);
                        }
                        else
                        {
                            existingItem.ITEM_NAME = item.ITEM_NAME;
                            existingItem.PRICE = float.Parse(item.PRICE.ToString());
                            existingItem.QUANTITY = item.QUANTITY;
                            existingItem.SO_ORDER_ID = existingOrder.SO_ORDER_ID;

                            unitOfWork.Item.Update(existingItem);
                        }
                    }
                    else
                    {
                        var newItem = new Item()
                        {
                            ITEM_NAME = item.ITEM_NAME,
                            PRICE = float.Parse(item.PRICE.ToString()),
                            QUANTITY = item.QUANTITY,
                            SO_ORDER_ID = existingOrder.SO_ORDER_ID
                        };
                       

                        unitOfWork.Item.Add(newItem); 
                    }
                }

                unitOfWork.Save(); // Menyimpan semua perubahan
                transaction.Commit(); // Commit transaksi

                TempData["SuccessMessage"] = "Order updated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Rollback transaksi jika terjadi error
                transaction.Rollback();
                ModelState.AddModelError("", "Terjadi kesalahan saat menyimpan data: " + ex.Message);
                return View("Error");
            }
        }
    }

    [HttpPost]
    [Route("DeleteItem")]
    public IActionResult DeleteItem(int itemId)
    {
        var itemToDelete =unitOfWork.Item.Get(item => item.SO_ITEM_ID == itemId);
        if (itemToDelete != null)
        {
            unitOfWork.Item.Remove(itemToDelete);
        }
        unitOfWork.Save();
        return Ok();
    }

    [Route("ExportExcel")]
    public IActionResult ExportExcel(DateTime? orderDate)
    {
        var orders = unitOfWork.Order.GetAll(e => orderDate == null || e.ORDER_DATE == orderDate, includeProperties:["Customer"]);

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Sale");

        worksheet.Column(1).Width = 5;  
        worksheet.Column(2).Width = 15; 
        worksheet.Column(3).Width = 30;
        worksheet.Column(4).Width = 20;

        string[] headers = ["No", "Sales Order", "Order Date", "Customer"];
        for(int x =0; x< headers.Length; x++)
        {
            worksheet.Cells[1, x + 1].Value = headers[x];
            worksheet.Cells[1, x + 1].Style.Font.Bold = true;
            worksheet.Cells[1, x + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            worksheet.Cells[1, x + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;   
        }

        int i = 0;
        int row = 2;
        foreach (var order in orders)
        {
            row += i;
            worksheet.Cells[row, 1].Value = i + 1;
            worksheet.Cells[row, 2].Value = order.ORDER_NO;
            worksheet.Cells[row, 3].Value = order.ORDER_DATE;
            worksheet.Cells[row, 4].Value = order.Customer!.CUSTOMER_NAME;
            i++;
        }

        var fileBytes = package.GetAsByteArray();
        var fileName = "Sale-report.xlsx";


        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

}