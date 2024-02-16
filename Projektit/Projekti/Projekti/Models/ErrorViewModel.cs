using System.ComponentModel.DataAnnotations;
namespace Projekti.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    //Käyttäjälle luokka
    public class User
    {
        public string email { get; set; }
        public string password { get; set; }
        public string usernameFirstName { get; set; }
        public string usernameLastName { get; set; }

        public string username { get; set; }

    }

    public class Tapahtuma
    {
        [Key]
        public string Nimi { get; set; }
        public string Kaupunki { get; set; }
        public string? Osoite { get; set; }
        public DateTime Paiva { get; set; }
        public TimeSpan Aika { get; set; }
        public string? Kuvaus { get; set; }
    }
}