using AzureStorageLibrary;
using AzureStorageLibrary.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class TableStoragesController : Controller
    {
        private readonly INoSqlStorage<Product> _productNoSqlStorage;

        public TableStoragesController(INoSqlStorage<Product> productNoSqlStorage)
        {
            _productNoSqlStorage = productNoSqlStorage;
        }

        public IActionResult Index()
        {
            ViewBag.products = _productNoSqlStorage.AllAsync().ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            product.RowKey = Guid.NewGuid().ToString();
            product.PartitionKey = "Pencils";

            await _productNoSqlStorage.AddAsync(product);

            return RedirectToAction("Index");
        }
    }
}
