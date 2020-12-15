using BookingServiceNS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingServiceNS.Services
{
    public interface IMultiplexService
    {
        Task<IEnumerable<Multiplex>> GetMultiplexes();
        Task<Multiplex> GetMultiplex(int MulId);
        Task<int> PostMultiplex(Multiplex multiplex);
        Task<int> PutMultiplex(int id, Multiplex multiplex);
        Task<int> DeleteMultiplex(Multiplex multiplex);
        bool MultiplexExists(int id);
    }
    public class MultiplexService : IMultiplexService
    {
        private readonly MovieDbContext _movieContext;

        public MultiplexService(MovieDbContext movieContext)
        {
            this._movieContext = movieContext;
        }

        public async Task<IEnumerable<Multiplex>> GetMultiplexes()
        {
            var multiplexes = await _movieContext.Multiplexes.ToListAsync();
            return multiplexes;
        }

        public async Task<Multiplex> GetMultiplex(int MulId)
        {
            var multiplex = await _movieContext.Multiplexes.FindAsync(MulId);
            return multiplex;
        }

        public async Task<int> PostMultiplex(Multiplex multiplex)
        {
            _movieContext.Multiplexes.Add(multiplex);
            return await _movieContext.SaveChangesAsync();
        }

        public async Task<int> PutMultiplex(int id, Multiplex multiplex)
        {
            _movieContext.Entry(multiplex).State = EntityState.Modified;
            return await _movieContext.SaveChangesAsync();
        }

        public async Task<int> DeleteMultiplex(Multiplex multiplex)
        {
            _movieContext.Multiplexes.Remove(multiplex);
            return await _movieContext.SaveChangesAsync();
        }

        public bool MultiplexExists(int id)
        {
            return _movieContext.Multiplexes.Any(e => e.MulId == id);
        }
    }
}
