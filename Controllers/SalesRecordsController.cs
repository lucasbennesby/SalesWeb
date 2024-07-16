using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.UA;
using SalesWeb.Models;
using SalesWeb.Models.Enums;
using SalesWeb.Models.ViewModels;
using SalesWeb.Services;
using System.Security.Cryptography;
using ZstdSharp.Unsafe;

namespace SalesWeb.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;
        private readonly SalesRecordService _salesRecordService;

        public SalesRecordsController(SellerService sellerService, DepartmentService departmentService,SalesRecordService salesRecordService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
            _salesRecordService = salesRecordService;

        }

        public async Task<IActionResult> Index(DateTime? minDate, DateTime? maxDate, int? sId, SaleStatus? status,int? dId)
        {            
            ViewBag.Sellers = _sellerService.FindAll();
            ViewBag.Status = Enum.GetValues(typeof(SaleStatus)).Cast<SaleStatus>().Select(s => new { Id = s, Name = s.ToString() }).ToList();
            ViewBag.Departments = await _departmentService.FindAll(); 
            //filtrar por data
            if (minDate != null && maxDate != null)
            {
                var list = await _salesRecordService.FindByDate(minDate, maxDate);
                ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
                ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

                return View(list);
            }
            //filtrar por seller
            if(sId != null)
            {
                var list = await _salesRecordService.FindBySeller(sId);
                return View(list);
            }
            //filtrar por status
            if(status.HasValue)
            {
                var list = await _salesRecordService.FindByStatus(status.Value);
                return View(list);
            }
            //filtrar por departamento
            if (dId != null)
            {
                var list = await _salesRecordService.FindByDepartment(dId);

                if(list.Count != 0)
                {
                    return View(list);
                }
                    ViewBag.Message = "No records found for the selected department.";
                    return View(list);
            }
           

            var records =  _salesRecordService.FindAll();

            double sum = records.Sum(r => r.Amount);
            ViewBag.Sum = sum;

            return View(records);

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }

           var sr = await _salesRecordService.FindById(id.Value);
            var d = _departmentService.FindById(sr.Id);
            var s = _sellerService.FindById(sr.Id);
         
            return View(sr);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            List<Seller> sellers =  _sellerService.FindAll();
            var sr = await _salesRecordService.FindById(id.Value);

            SalesRecordFormViewModel viewModel = new SalesRecordFormViewModel { SalesRecord = sr,Sellers = sellers };

            return View(viewModel);

        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit (int id, SalesRecordFormViewModel viewModel)
        {
            _salesRecordService.Update(viewModel.SalesRecord);
             return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {   
            var sellers = _sellerService.FindAll();
            ViewBag.Sellers = new SelectList(sellers,"Id","Name");
           
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(SalesRecord salesrecord)
        {
            //definir ID => feito automaticamente pelo entityframework se no banco estiver como AI e PK
            //definir Seller => pegando do asp-for
            salesrecord.Seller = _sellerService.FindById(salesrecord.Seller.Id);
                
            _salesRecordService.Insert(salesrecord);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var sr = await _salesRecordService.FindById(id.Value);
            return View(sr);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(int id)
        {

            _salesRecordService.Remove(id);
            return RedirectToAction(nameof(Index));

        }

       
    } 
}
