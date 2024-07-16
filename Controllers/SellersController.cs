using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using SalesWeb.Data;
using SalesWeb.Models;
using SalesWeb.Services;
using SalesWeb.Models.ViewModels;
using SalesWeb.Services.Exceptions;

namespace SalesWeb.Controllers
{
    public class SellersController : Controller
    {

        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;


        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;

        }

        public IActionResult Index()
        {
            var list = _sellerService.FindAll();

            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAll();
            ViewBag.Departments = new SelectList(departments, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller s)
        {
           var departments = await _departmentService.FindAll();
           ViewBag.Departments = new SelectList(departments, "Id", "Name");

            if (!ModelState.IsValid)
            {
               
                ViewBag.Departments = new SelectList(departments, "Id", "Name");
                return View(s);
            }
           

            s.Department = _departmentService.FindById(s.DepartmentId);
            _sellerService.Insert(s);
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var s = _sellerService.FindById(id.Value);
            var d = _departmentService.FindById(s.DepartmentId) ;
            s.Department = d;

            if(s == null)
            {
                return NotFound();
            }

            return View(s);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            
            return RedirectToAction(nameof(Index));
            
        }

        public IActionResult Details(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var s = _sellerService.FindById(id.Value);
            var d = _departmentService.FindById(s.DepartmentId);
            s.Department = d;

            if (s == null)
            {
                return NotFound();
            }

            return View(s);
        }

        public async Task<IActionResult> Edit(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }

            var s =  _sellerService.FindById(id.Value);          

            if (s == null)
            {
                return NotFound();
            }

            List<Department> departments = await _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = s,Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit (int id,SellerFormViewModel s)
        {
            if (!ModelState.IsValid)
            {
                return View(s);
            }
            if (id!= s.Seller.Id)
            {
                return BadRequest();
            }
            try
            {
                _sellerService.Update(s.Seller);

                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundExcepetion)
            {
                return NotFound();
            }
            catch (DbConcurrencyException) 
            {
                return BadRequest();
            }

        }


    }
}
