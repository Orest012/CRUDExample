using Entities;
using ServiceContracts;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //private field
        private readonly PersonDbContext _db;

        //constructor
        public CountriesService(PersonDbContext personDbContext, bool init = true)
        {
            _db = personDbContext;
        //    if (init)
        //    {
        //        _db.AddRange(new List<Country>() {
        //new Country() {  CountryID = Guid.Parse("000C76EB-62E9-4465-96D1-2C41FDB64C3B"), CountryName = "USA" },

        //new Country() { CountryID = Guid.Parse("32DA506B-3EBA-48A4-BD86-5F93A2E19E3F"), CountryName = "Canada" },

        //new Country() { CountryID = Guid.Parse("DF7C89CE-3341-4246-84AE-E01AB7BA476E"), CountryName = "UK" },

        //new Country() { CountryID = Guid.Parse("15889048-AF93-412C-B8F3-22103E943A6D"), CountryName = "India" },

        //new Country() { CountryID = Guid.Parse("80DF255C-EFE7-49E5-A7F9-C35D7C701CAB"), CountryName = "Australia" }
        //});
        //    }
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {


            //Validation: countryAddRequest parameter can't be null
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //Validation: CountryName can't be null
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            //Validation: CountryName can't be duplicate
            if (await _db.Countries.CountAsync(temp => temp.CountryName == countryAddRequest.CountryName) > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            //Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();

            //generate CountryID
            country.CountryID = Guid.NewGuid();

            //Add country object into _countries
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();

            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
         //   return _db.Select(country => country.ToCountryResponse()).ToList();
            return await _db.Countries
        .Select(country => country.ToCountryResponse()).ToListAsync();
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null) {
                return null;
            }

            Country? country_response_from_list = await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryID == countryID);
            if (country_response_from_list == null) { 
                return null;
            }
            
            return country_response_from_list.ToCountryResponse();
        }
    }
}

