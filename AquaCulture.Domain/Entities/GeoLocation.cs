namespace AquaCulture.Domain.Entities
{
    public class GeoLocation
    {
        public decimal Latitude { get; }
        public decimal Longitude { get; }

        public GeoLocation(decimal latitude, decimal longitude)
        {
            ValidatePrecision(latitude);
            ValidatePrecision(longitude);

            Latitude = Math.Round(latitude, 4);
            Longitude = Math.Round(longitude, 4);
        }

        private void ValidatePrecision(decimal value)
        {
            if (decimal.Round(value, 4) != value)
                throw new ArgumentException($"{nameof(value)} must have max 4 decimal places");
        }
    }
}
