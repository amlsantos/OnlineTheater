using Logic.Entities;

namespace Logic.Services;

public class CustomerService
{
    private readonly MovieService _movieService;

        public CustomerService(MovieService movieService)
        {
            _movieService = movieService;
        }

        public void PurchaseMovie(Customer customer, Movie movie)
        {
            var expirationDate = _movieService.GetExpirationDate(movie.LicensingModel);
            var price = CalculatePrice(customer.Status, movie.LicensingModel);

            customer.AddPurchaseMovie(movie, expirationDate, price);
        }

        private Dollars CalculatePrice(CustomerStatus status, LicensingModel licensingModel)
        {
            Dollars price;
            switch (licensingModel)
            {
                case LicensingModel.TwoDays:
                    price = Dollars.Of(4);
                    break;

                case LicensingModel.LifeLong:
                    price = Dollars.Of(8);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (status.IsAdvanced) 
                price = price * 0.75m;

            return price;
        }
}