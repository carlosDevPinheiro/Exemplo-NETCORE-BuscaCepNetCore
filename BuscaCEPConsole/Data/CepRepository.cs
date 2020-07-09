using BuscaCEPConsole.Models;
using BuscaCEPConsole.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BuscaCEPConsole.Data
{
    public class CepRepository : ICepRepsository
    {
        private readonly ApplicationDbContext _context;
        public CepRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddCepAsync(CepModel cep)
        {
            var reslt = await _context.Ceps.AddAsync(cep);
            return reslt.State == EntityState.Added ? 1 : 0;

        }
        
        public async Task<List<CepModel>> GetAllCeps()
        {
            return await _context.Ceps.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<CepModel>> GetAll()
        {
            return (from lista in await _context.Ceps.ToListAsync()
                    orderby lista.Cep
                    where lista.Complemento != ""
                    select lista);
        }

        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }


    }
}