using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook1.Models;
using BulkyBook1.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BulkyBookWeb1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller

    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {

            return View();
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            ProductVM productVm = new ProductVM // Use parentheses instead of curly braces
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i =>
                    new SelectListItem()
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i =>
                    new SelectListItem()
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    })
            };

            if (id == null || id == 0)
            {
                return View(productVm);
            }
            else
            {
                productVm.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
                return View(productVm);

                // Handle the case when id is not null or 0
            }

        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;

                }
                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {

            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return Json(new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successfull" });
        }
        #endregion

    }


}