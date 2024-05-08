using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "PersonName can not be blank")]
        public string? PersonName { get; set; }
       
        [Required(ErrorMessage = "Email can not be blank")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "DateOfBirth can not be blank")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender can not be blank")]
        public GenderOptions? Gender { get; set; }

        [Required(ErrorMessage = "CountryID can not be blank")]
        public Guid? CountryID { get; set; }

        [Required(ErrorMessage = "Address can not be blank")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "ReceiveNewsLetters can not be blank")]
        public bool ReceiveNewsLetters { get; set; }

        /// <summary>
        /// Converts the current object of PersonAddRequest into a new object of Person type
        /// </summary>
        /// <returns></returns>
        public Person ToPerson()
        {
            return new Person() { PersonName = PersonName, Email = Email, DateOfBirth = DateOfBirth, Gender = Gender.ToString(), Address = Address, CountryID = CountryID, ReceiveNewsLetters = ReceiveNewsLetters };
        }
    }
}
