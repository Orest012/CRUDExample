using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        //private fields
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        //constructor
        public PersonsController(IPersonsService personsService, ICountriesService countriesService)
        {
            _personsService = personsService;
            _countriesService = countriesService;
        }

        [Route("[action]")]
        [Route("/")]
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            //Search
            ViewBag.SearchFields = new Dictionary<string, string>()
      {
        { nameof(PersonResponse.PersonName), "Person Name" },
        { nameof(PersonResponse.Email), "Email" },
        { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
        { nameof(PersonResponse.Gender), "Gender" },
        { nameof(PersonResponse.CountryID), "Country" },
        { nameof(PersonResponse.Address), "Address" }
      };
            List<PersonResponse> persons = await _personsService.GetFilteredPersons(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sort
            List<PersonResponse> sortedPersons = await _personsService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();
            ViewBag.SortList = new Dictionary<string, string>()
            {
                { "DESC", "DESC" },
                { "ASC", "ASC" }
            };

            return View(sortedPersons); //Views/Persons/Index.cshtml
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp => new SelectListItem()
            {
                Text = temp.CountryName,
                Value = temp.CountryID.ToString()
            });

            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = await _countriesService.GetAllCountries();
                ViewBag.Countries = countries;

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }

            //call the service method
            PersonResponse personResponse = await _personsService.AddPerson(personAddRequest);

            //navigate to Index() action method (it makes another get request to "persons/index"
            return RedirectToAction("Index", "Persons");
        }

        [Route("Test")]
        public IActionResult Test()
        {

            return View();
        }


        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            PersonResponse personResponse = await _personsService.GetPersonByPersonID(id);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest updatedPerson = personResponse.ToPersonUpdateRequest();
            return View(updatedPerson);
        }


        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = await _personsService.GetPersonByPersonID(personUpdateRequest.PersonID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            else if (ModelState.IsValid)
            {
                PersonResponse updatedPerson = await _personsService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");
            }

            else
            {
                PersonResponse updatedPerson = await _personsService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");
                //List<CountryResponse> countries = _countriesService.GetAllCountries();
                //ViewBag.Countries = countries.Select(temp =>
                //new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

                //ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                //return View();
            }
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            PersonResponse personResponse = await _personsService.GetPersonByPersonID(id);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            return View(personUpdateRequest);
        }

        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = await _personsService.GetPersonByPersonID(personUpdateRequest.PersonID);
            if (personResponse == null)
                return RedirectToAction("Index");

            _personsService.DeletePerson(personUpdateRequest.PersonID);
            return RedirectToAction("Index");
        }

        [Route("PersonsPDF")]
        public async Task<IActionResult> PersonsPDF()
        {
            List<PersonResponse> persons = await
            _personsService.GetAllPersons();

            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Right = 20,
                    Bottom = 20,
                    Left = 20,
                },

                PageOrientation = new Rotativa.AspNetCore.Options.Orientation()
            };
        }

        [Route("PersonsCSV")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream memoryStream = await _personsService.GetPersonsCSV();
            return File(memoryStream, "application/octet-stream", "persons.csv");
        }

        [Route("PersonsExcel")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personsService.GetPersonsExcel();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
    }
}