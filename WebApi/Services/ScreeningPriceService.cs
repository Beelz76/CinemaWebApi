using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi.Contracts;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class ScreeningPriceService : IScreeningPriceService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public ScreeningPriceService(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public async Task<bool> CreateScreeningPriceAsync(int price)
        {
            var screeningPrice = new ScreeningPrice
            {
                ScreeningPriceUid = Guid.NewGuid(),
                Price = price,
            };

            await _cinemaDbContext.AddAsync(screeningPrice);
            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<IReadOnlyList<ScreeningPriceDto>> GetScreeningPricesAsync()
        {
            return await _cinemaDbContext.Set<ScreeningPrice>()
                .OrderBy(x => x.Price)
                .Select(screeningPrice => new ScreeningPriceDto
                {
                    ScreeningPriceUid = screeningPrice.ScreeningPriceUid,
                    Price = screeningPrice.Price
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateScreeningPriceAsync(Guid screeningPriceUid, int price)
        {
            var totalRows = await _cinemaDbContext.Set<ScreeningPrice>()
                .Where(x => x.ScreeningPriceUid == screeningPriceUid)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(p => p.Price, p => price));

            return totalRows > 0;
        }

        public async Task<bool> DeleteScreeningPriceAsync(Guid screeningPriceUid)
        {
            var totalRows = await _cinemaDbContext.Set<ScreeningPrice>()
                .Where(x => x.ScreeningPriceUid == screeningPriceUid)
                .ExecuteDeleteAsync();
            
            return totalRows > 0;
        }

        public async Task<bool> ScreeningPriceExistsAsync(int price)
        {
            return await _cinemaDbContext.Set<ScreeningPrice>().AnyAsync(x => x.Price == price);
        }
    }
}