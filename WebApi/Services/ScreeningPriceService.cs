using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
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

        public bool CreateScreeningPrice(int price)
        {
            var screeningPrice = new ScreeningPrice
            {
                ScreeningPriceUid = Guid.NewGuid(),
                Price = price,
            };

            _cinemaDbContext.Add(screeningPrice);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public List<Contracts.ScreeningPrice>? GetScreeningPrices()
        {
            var screeningPrices = _cinemaDbContext.Set<ScreeningPrice>().OrderBy(x => x.Price).ToList();

            if (screeningPrices.Count == 0 ) { return null; }

            return screeningPrices.Select(screeningPrice => new Contracts.ScreeningPrice
            {
                ScreeningPriceUid = screeningPrice.ScreeningPriceUid,
                Price = screeningPrice.Price
            }).ToList();
        }

        public bool UpdateScreeningPrice(Guid screeningPriceUid, int price)
        {
            var screeningPrice = _cinemaDbContext.Set<ScreeningPrice>().SingleOrDefault(x => x.ScreeningPriceUid == screeningPriceUid);

            if (screeningPrice == null) { return false; }

            screeningPrice.Price = price;

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public bool DeleteScreeningPrice(Guid screeningPriceUid)
        {
            var screeningPrice = _cinemaDbContext.Set<ScreeningPrice>().SingleOrDefault(x => x.ScreeningPriceUid == screeningPriceUid);

            if (screeningPrice == null) { return false; }

            _cinemaDbContext.Remove(screeningPrice);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public bool CheckScreeningPrice(int price)
        {
            var screeningPrice = _cinemaDbContext.Set<ScreeningPrice>().SingleOrDefault(x => x.Price == price);

            if (screeningPrice == null) { return false; }

            return true;
        }

        public bool IsScreeningPriceExists(Guid screeningPriceUid)
        {
            var screeningPrice = _cinemaDbContext.Set<ScreeningPrice>().SingleOrDefault(x => x.ScreeningPriceUid == screeningPriceUid);

            if (screeningPrice == null) { return false; }

            return true;
        }
    }
}
