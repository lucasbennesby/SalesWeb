using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using SalesWeb.Data;
using SalesWeb.Models;
using SalesWeb.Services.Exceptions;
using ZstdSharp.Unsafe;

namespace SalesWeb.Services
{
    public class SellerService
    {

        private readonly SalesWebContext _context;

        public SellerService(SalesWebContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return  _context.Seller.Include(s => s.Department).ToList();

        }

        public void Insert(Seller s)
        {

            s.Department = _context.Department.Find(s.DepartmentId);

            _context.Add(s);
            _context.SaveChanges();
        }

        public Seller FindById(int id)
        {
            return _context.Seller.Find(id);

        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);
            _context.Remove(obj);
            _context.SaveChanges();

        }

        public void Update(Seller s)
        {
            if (!_context.Seller.Any(x => x.Id == s.Id))
            {
                throw new NotFoundExcepetion("id not found");
            }

            try
            {
                s.Department = _context.Department.Find(s.DepartmentId);
                _context.Update(s);              
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e) 
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
