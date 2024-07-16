using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWeb.Data;
using SalesWeb.Models;
using SalesWeb.Models.Enums;
using SalesWeb.Services.Exceptions;
using System.Xml;


namespace SalesWeb.Services
{
    public class SalesRecordService
    {

        private readonly SalesWebContext _context;

        public SalesRecordService(SalesWebContext context)
        {
            _context = context;
        }

        public List<SalesRecord> FindAll()
        {
            return _context.SalesRecord
                .Include(sr => sr.Seller)
                    .ThenInclude(d => d.Department)
                    .ToList();
        }

        public async Task<SalesRecord> FindById(int id)
        {

            return await _context.SalesRecord.FindAsync(id);
        }

        public void Update(SalesRecord sr)
        {
            if (!_context.SalesRecord.Any(x => x.Id == sr.Id))
            {
                throw new NotFoundExcepetion("id not found");
            }

            try
            {
                sr.Seller = _context.Seller.Find(sr.SellerId);
                _context.Update(sr);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
        public void Insert(SalesRecord sr)
        {


            _context.Add(sr);
            _context.SaveChanges();
        }
        public void Remove(int id)
        {
            var obj = _context.SalesRecord.Find(id);
            _context.Remove(obj);
            _context.SaveChanges();

        }



        public async Task<List<SalesRecord>> FindByDate(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();

        }

        public async Task<List<SalesRecord>> FindBySeller(int? id)
        {
            var result = from obj in _context.SalesRecord select obj;

            if (id.HasValue)
            {
                result = result.Where(x => x.Seller.Id == id);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();

        }

        public async Task<List<SalesRecord>> FindByStatus(SaleStatus? status)
        {
            var result = from obj in _context.SalesRecord select obj;

            if(status.HasValue)
            {
                result = result.Where(x=> x.Status == status);  
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<SalesRecord>> FindByDepartment(int? id)
        {
            var result = from obj in _context.SalesRecord select obj;

            if (id != null)
            {
                result = result.Where(x=>x.Seller.Department.Id == id);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }
    }

}
