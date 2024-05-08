using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesService
    {
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);
        Task<List<CountryResponse>> GetAllCountries();

        Task<CountryResponse?> GetCountryByCountryID(Guid? countryID);
    }
}