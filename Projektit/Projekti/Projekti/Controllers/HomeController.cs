using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Projekti.Models;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Data.SqlClient;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace Projekti.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Tapahtumienluonti()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TapahtumaLuonti(string nimi, string kaupunki, string osoite, DateTime paiva, TimeSpan aika, string kuvaus)
        {
            string connectionString = "Data Source=tcp:91.156.129.113,1433;Initial Catalog=project;Persist Security Info=True;User ID=sa;Password=Salasana1";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO [dbo].[Tapahtuma]
                        ([nimi], [kaupunki], [osoite], [paiva], [aika], [kuvaus])
                        VALUES
                        (@nimi, @kaupunki, @osoite, @paiva, @aika, @kuvaus)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@nimi", nimi);
                    cmd.Parameters.AddWithValue("@kaupunki", kaupunki);
                    cmd.Parameters.AddWithValue("@osoite", osoite);
                    cmd.Parameters.AddWithValue("@paiva", paiva);
                    cmd.Parameters.AddWithValue("@aika", aika);
                    cmd.Parameters.AddWithValue("@kuvaus", kuvaus);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return RedirectToAction("Index");
        }

		public IActionResult TapahtumienMuokkaus(string nimi)
		{
			string connectionString = "Data Source=tcp:91.156.129.113,1433;Initial Catalog=project;Persist Security Info=True;User ID=sa;Password=Salasana1; Connection Timeout=30";

			Tapahtuma tapahtuma = new Tapahtuma();

			using (SqlConnection con = new SqlConnection(connectionString))
			{
				con.Open();

				string query = "SELECT * FROM [dbo].[Tapahtuma] WHERE nimi = @nimi";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Parameters.AddWithValue("@nimi", nimi);
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							tapahtuma.Nimi = reader["nimi"].ToString();
							tapahtuma.Kaupunki = reader["kaupunki"].ToString();
							tapahtuma.Osoite = reader["osoite"].ToString();
							tapahtuma.Paiva = Convert.ToDateTime(reader["paiva"]);
							tapahtuma.Aika = TimeSpan.Parse(reader["aika"].ToString());
							tapahtuma.Kuvaus = reader["kuvaus"].ToString();
						}
					}
				}
				con.Close();
			}

			return View(tapahtuma);
		}

		[HttpPost]
		public IActionResult TapahtumaMuokkaus(Tapahtuma tapahtuma)
		{
			string connectionString = "Data Source=tcp:91.156.129.113,1433;Initial Catalog=project;Persist Security Info=True;User ID=sa;Password=Salasana1; Connection Timeout=30";

			using (SqlConnection con = new SqlConnection(connectionString))
			{
				con.Open();

				tapahtuma.Kaupunki = Request.Form["kaupunki"].ToString();
				tapahtuma.Osoite = Request.Form["osoite"].ToString();
				tapahtuma.Paiva = DateTime.Parse(Request.Form["paiva"]);
				tapahtuma.Aika = TimeSpan.Parse(Request.Form["aika"]);
				tapahtuma.Kuvaus = Request.Form["kuvaus"].ToString();

				string query = "UPDATE [dbo].[Tapahtuma] SET kaupunki = @kaupunki, osoite = @osoite, paiva = @paiva, aika = @aika, kuvaus = @kuvaus WHERE nimi = @nimi";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Parameters.AddWithValue("@nimi", tapahtuma.Nimi);
					cmd.Parameters.AddWithValue("@kaupunki", tapahtuma.Kaupunki);
					cmd.Parameters.AddWithValue("@osoite", tapahtuma.Osoite);
					cmd.Parameters.AddWithValue("@paiva", tapahtuma.Paiva);
					cmd.Parameters.AddWithValue("@aika", tapahtuma.Aika);
					cmd.Parameters.AddWithValue("@kuvaus", tapahtuma.Kuvaus);
					cmd.ExecuteNonQuery();
				}

				con.Close();
			}

			return RedirectToAction("Index");
		}

		public IActionResult PoistaTapahtuma(string nimi)
		{
			string connectionString = "Data Source=tcp:91.156.129.113,1433;Initial Catalog=project;Persist Security Info=True;User ID=sa;Password=Salasana1; Connection Timeout=30";

			using (SqlConnection con = new SqlConnection(connectionString))
			{
				con.Open();

				string query = "DELETE FROM [dbo].[Tapahtuma] WHERE nimi = @nimi";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Parameters.AddWithValue("@nimi", nimi);
					cmd.ExecuteNonQuery();
				}

				con.Close();
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
        public IActionResult Info(string nimi)
        {
            string connectionString = "Data Source=tcp:91.156.129.113,1433;Initial Catalog=project;Persist Security Info=True;User ID=sa;Password=Salasana1; Connection Timeout=30";
            Tapahtuma tapahtuma = new Tapahtuma();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Retrieve the Tapahtuma object with the updated values from the database
                string query = "SELECT * FROM [dbo].[Tapahtuma] WHERE nimi = @nimi";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@nimi", nimi);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tapahtuma.Nimi = reader["nimi"].ToString();
                            tapahtuma.Kaupunki = reader["kaupunki"].ToString();
                            tapahtuma.Osoite = reader["osoite"].ToString();
                            tapahtuma.Paiva = Convert.ToDateTime(reader["paiva"]);
                            tapahtuma.Aika = TimeSpan.Parse(reader["aika"].ToString());
                            tapahtuma.Kuvaus = reader["kuvaus"].ToString();
                        }
                    }
                }
                con.Close();
            }

            return View(tapahtuma);
        }


        public IActionResult Index()
        {
            List<Tapahtuma> tapahtumat = new List<Tapahtuma>();
            string connectionString = "Data Source=tcp:91.156.129.113,1433;Initial Catalog=project;Persist Security Info=True;User ID=sa;Password=Salasana1;Connection Timeout=30";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM [dbo].[Tapahtuma] ORDER BY nimi";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tapahtuma tapahtuma = new Tapahtuma();
                            tapahtuma.Nimi = reader["nimi"].ToString();
                            tapahtuma.Kaupunki = reader["kaupunki"].ToString();
                            tapahtuma.Osoite = reader["osoite"].ToString();
                            tapahtuma.Paiva = Convert.ToDateTime(reader["paiva"]);
                            tapahtuma.Aika = TimeSpan.Parse(reader["aika"].ToString());
                            tapahtuma.Kuvaus = reader["kuvaus"].ToString();
                            tapahtumat.Add(tapahtuma);
                        }
                    }
                    con.Close();
                }
            }
            return View(tapahtumat);
        }

        [HttpPost]
        public IActionResult KayttajanLuonti(string username, string etunimi, string sukunimi, string salasana)
        {
            string connectionString = "Data Source=tcp:91.156.129.113,1433;Initial Catalog=project;Persist Security Info=True;User ID=sa;Password=Salasana1";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO [dbo].[Rekisteroidy]
                        ([username], [etunimi], [sukunimi], [salasana])
                        VALUES
                        (@username, @etunimi, @sukunimi, @salasana)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@etunimi", etunimi);
                    cmd.Parameters.AddWithValue("@sukunimi", sukunimi);
                    cmd.Parameters.AddWithValue("@salasana", salasana);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Rekisteröidy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Kirjaudu(string username, string salasana)
        {
            if (username == null || salasana == null)
            {
                //"Syötä käyttäjänimi ja salasana"
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    string connectionString = "Data Source=tcp:91.156.129.113,1433;Initial Catalog=project;Persist Security Info=True;User ID=sa;Password=Salasana1";
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        string query = @"SELECT COUNT(*) FROM [dbo].[Rekisteroidy] WHERE [username] = @username AND [salasana] = @salasana";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@salasana", salasana);
                            con.Open();
                            int count = (int)cmd.ExecuteScalar();
                            con.Close();
                            if (count > 0)
                            {
                                // Authentication cookie
                                var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Name, username)
                                };
                                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                var authProperties = new AuthenticationProperties
                                {
                                    AllowRefresh = true,
                                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),
                                    IsPersistent = true
                                };
                                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties).Wait();
                                //Onnistunut kirjautuminen
                                return RedirectToAction("Index"); //<-Testasin vaan tolla et se toimii
                            }
                            else
                            {
                                //"Virheellinen käyttäjänimi tai salasana."
                                return RedirectToAction("Index");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //"Virhe kirjautumisessa" + ex (antaa virhe koodin jos tän jotenki tulostaa)
                    return RedirectToAction("Index");
                }
            }
        }

        //Cookies tarkistus
        public bool Login(User user)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.username) }; 
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); 
            var authProperties = new AuthenticationProperties { AllowRefresh = true, ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30), IsPersistent = true }; 
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties); 
            return true;
        }
        [Authorize] // Vain kirjautunut näkee authroize methodit


        //Uloskirajutuminen
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}