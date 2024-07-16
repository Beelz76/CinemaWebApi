using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi.Interface;

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

        public async Task<List<Contracts.ScreeningPrice>> GetScreeningPricesAsync()
        {
            return await _cinemaDbContext.Set<ScreeningPrice>()
                .OrderBy(x => x.Price)
                .Select(screeningPrice => new Contracts.ScreeningPrice
                {
                    ScreeningPriceUid = screeningPrice.ScreeningPriceUid,
                    Price = screeningPrice.Price
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateScreeningPriceAsync(Guid screeningPriceUid, int price)
        {
            var screeningPrice = await _cinemaDbContext.Set<ScreeningPrice>().FirstOrDefaultAsync(x => x.ScreeningPriceUid == screeningPriceUid);

            if (screeningPrice == null) { return false; }

            screeningPrice.Price = price;

            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteScreeningPriceAsync(Guid screeningPriceUid)
        {
            var screeningPrice = await _cinemaDbContext.Set<ScreeningPrice>().FirstOrDefaultAsync(x => x.ScreeningPriceUid == screeningPriceUid);

            if (screeningPrice == null) { return false; }

            _cinemaDbContext.Remove(screeningPrice);
            return await _cinemaDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ScreeningPriceExistsAsync(int price)
        {
            return await _cinemaDbContext.Set<ScreeningPrice>().AnyAsync(x => x.Price == price);;
        }
    }
}