using AzureStorageLibrary;
using AzureStorageLibrary.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            ViewBag.isUpdate = false;
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

        public async Task<IActionResult> Update(string rowKey, string partitionKey)
        {
            var product = await _productNoSqlStorage.GetAsync(rowKey, partitionKey);
            ViewBag.products = _productNoSqlStorage.AllAsync().ToList();
            ViewBag.isUpdate = true;

            return View("Index", product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            product.ETag = "*";
            ViewBag.isUpdate = true;
            await _productNoSqlStorage.UpdateAsync(product);

            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> Delete(string rowKey, string partitionKey)
        {
            await _productNoSqlStorage.DeleteAsync(rowKey, partitionKey);
            return RedirectToAction("Index");
        } 
    }
}
