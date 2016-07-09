using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoData;
using MVCGridView.Models;

namespace MVCGridView.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(int? pageNumber)
        {
            ProductModel model = new ProductModel();
            model.PageNumber = (pageNumber == null ? 1 : Convert.ToInt32(pageNumber));
            model.PageSize = 4;

            List<Product> products = Product.GetSampleProducts();

            if (products != null)
            {
                model.Products = products.OrderBy(x => x.Id)
                         .Skip(model.PageSize * (model.PageNumber - 1))
                         .Take(model.PageSize).ToList();

                model.TotalCount = products.Count();
                var page = (model.TotalCount / model.PageSize) - (model.TotalCount % model.PageSize == 0 ? 1 : 0);
                model.PagerCount = page + 1;
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult WebGrid()
        {
            ProductModel model = new ProductModel();
            model.PageSize = 4;

            List<Product> products = Product.GetSampleProducts();

            if (products != null)
            {
                model.TotalCount = products.Count();
                model.Products = products;
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult jqGrid()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AngularJS()
        {
            return View();
        }

        public ActionResult GetProducts(string sidx, string sord, int page, int rows)
        {
            var products = Product.GetSampleProducts();
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = products.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            
            var data = products.OrderBy(x => x.Id)
                         .Skip(pageSize * (page - 1))
                         .Take(pageSize).ToList();

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProductById(int id)
        {
            var products = Product.GetSampleProducts().Where(x => x.Id == id); ;

            if (products != null)
            {
                ProductModel model = new ProductModel();

                foreach (var item in products)
                {
                    model.Name = item.Name;
                    model.Price = item.Price;
                    model.Department = item.Department;
                }

                return PartialView("_GridEditPartial", model);
            }

            return View();
        }

        [HttpPost]
        public ActionResult UpdateProduct(ProductModel model)
        {
            //update database
            return Content("Record updated!!", "text/html");
        }
    }
}
