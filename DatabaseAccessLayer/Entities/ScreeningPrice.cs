namespace DatabaseAccessLayer.Entities
{
    public class ScreeningPrice
    {
        public int ScreeningPriceId { get; init; }

        public required Guid ScreeningPriceUid { get; init; }

        public required int Price { get; set; }

        public ICollection<Screening> Screenings { get; set; }
    }
}
