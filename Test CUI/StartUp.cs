using DomainLayer;
using DomainLayer.Controllers;
using DomainLayer.Interfaces;
using DomainLayer.Objects;
using Microsoft.IdentityModel.Protocols;
using PersistenceLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_CUI {
    public class StartUp {

        public static void Setup(DomainController _controller) {
            _controller.AddUser("Contact", "persoon", "contactPersoon@gmail.com", "04 4589 3245");
            User contactPersoon = _controller.GetUserByEmail("contactPersoon@gmail.com");
            _controller.AddWebshop("Webshop 1", "04 7878 5703", "webshop1@gmail.com", "BE44 8503 5830 6620", contactPersoon);

            _controller.addBank("KBC", "04 9690 4893", "KBC@info.be", contactPersoon);
            User a = _controller.GetActiveUser();
            Bank b = _controller.GetBankByName("KBC");
            _controller.MakeAccountNumber(b.Id, a.Id, "BE44 6904 4906 4893");
            _controller.MakeAccountNumber(b.Id, a.Id, "BE93 2943 2809 0905");
        }

        private static IPaymentHubRepo _paymentHubRepo;
        public static void Main(string[] args) {
            string conn = ConfigurationManager.ConnectionStrings["ADOconnSQL"].ConnectionString;
            _paymentHubRepo = new PaymentHubRepo(conn);
            DomainController _controller = new DomainController(_paymentHubRepo);
            
            decimal bedrag = 10.00m;
            bool paymentGeslaagd = false;
            bool registreer = false;

            WebshopTest(_controller, bedrag, paymentGeslaagd, registreer);
        }
        
        public static void WebshopTest(DomainController _controller, decimal bedrag, bool paymentGeslaagd, bool registreer) {
            Console.WriteLine("Welkom bij de webshop");
            Console.WriteLine("---------------------");
            if (registreer) {
                Console.WriteLine("Optie 1: registreer");
                Console.Write("Email: ");
                string email = Console.ReadLine();
                Console.Write("Wachtwoord: ");
                string password = Console.ReadLine();
                Console.Write("Voornaam: ");
                string voornaam = Console.ReadLine();
                Console.Write("Achternaam: ");
                string achternaam = Console.ReadLine();
                Console.Write("Telefoonnummer: ");
                string telefoonNummer = Console.ReadLine();
                _controller.RegisterUser(email, password, voornaam, achternaam, telefoonNummer);
                Setup(_controller);
            } else {
                Console.WriteLine("Optie 2: login");
                Console.Write("Email: ");
                string email = Console.ReadLine();
                Console.Write("Wachtwoord: ");
                string password = Console.ReadLine();
                _controller.LoginUser(email, password);
            }
            Console.WriteLine("Je hebt een rekening van " + bedrag + " euro");
            Console.WriteLine("Je word nu doorgestuurd naar de payment hub");
            Console.WriteLine("Druk op enter");
            Console.ReadKey();
            Console.Clear();
            
            PaymentHubTest(bedrag, _controller, _controller.getWebshopByName("Webshop 1"), paymentGeslaagd);
        }
        public static void PaymentHubTest(decimal bedrag, DomainController _controller, Webshop ws, bool paymentGeslaagd) {
            User a = _controller.GetActiveUser();
            Bank bank = _controller.getBanks()[0];
            
            Console.WriteLine("Welkom bij de PaymentHub");
            Console.WriteLine("------------------------");
            Console.WriteLine("Je moest nog " + bedrag + " euro aan de webshop");
            Console.WriteLine("Welke bank kies je: ");
            List<Bank> banks = _controller.getBanks();
            foreach (Bank b in banks) {
                Console.WriteLine(b.Id + ": " + b.Naam);
            }
            Console.Write("Bank: ");
            int bankId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Welke van je rekeningnummers kies je ?");
            List<AccountNumber> bankAccounts = _controller.getAccountNumberByUserIdAndBankId(a.Id, bankId);
            foreach (AccountNumber an in bankAccounts) {
                Console.WriteLine(an.Id + ": " + an.IBAN);
            }
            Console.Write("Rekeningnummer: ");
            int accountNumberId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Je hebt gekozen voor " + _controller.GetAllAccountNumber().Find(id => id.Id == accountNumberId).IBAN + " bij bank " + _controller.GetBankById(bankId).Naam);
            _controller.AddPayment(bedrag, ws.Id, a.Id);
            Payment payment = new Payment(bedrag, ws.Id, a.Id);
            Console.WriteLine("Je gaat een betaling van " + payment.Bedrag + " euro doen naar " + ws.Naam);
            Console.WriteLine("Druk op enter om door te gaan");
            Console.ReadKey();
            Console.Clear();
            BankTest(_controller, payment, paymentGeslaagd);
        }
        public static void BankTest(DomainController _controller, Payment payment, bool paymentGeslaagd) {
            Console.WriteLine("Welkom bij de bank");
            Console.WriteLine("------------------");
            Console.WriteLine(payment.Bedrag + " euro wordt overgemaakt naar " + _controller.getWebshopById(payment.BegunstigeWebshopId).Naam);
            Console.WriteLine("Druk op enter");
            Console.ReadKey();
            Console.Clear();
            PaymentHubTest2(_controller, payment, paymentGeslaagd);
        }
        public static void PaymentHubTest2(DomainController _controller, Payment payment, bool paymentGeslaagd) {
            Console.WriteLine("Welkom bij de PaymentHub");
            Console.WriteLine("------------------------");
            if (paymentGeslaagd) {
                Console.WriteLine("Je hebt een betaling van " + payment.Bedrag + " euro gedaan naar " + _controller.getWebshopById(payment.BegunstigeWebshopId).Naam);
                _controller.BetaalPayment(_controller.ToonOpenPayments()[_controller.ToonOpenPayments().Count-1].Id);
            } else {
                Console.WriteLine("De betaling is mislukt");
            }
            Console.WriteLine("Druk op enter om door te gaan");
            Console.ReadKey();
            Console.Clear();
            WebshopTest2(_controller, paymentGeslaagd);
        }

        public static void WebshopTest2(DomainController _controller, bool paymentGeslaagd) {
            Console.WriteLine("Welkom bij de webshop");
            Console.WriteLine("---------------------");
            if ( paymentGeslaagd) {
                Console.WriteLine("Je betaling is geslaagd !");
                Console.WriteLine("Je pakketje word opgestuurd !");
            } else {
                Console.WriteLine("Je betaling is niet geslaagd !");
            }
            Console.WriteLine("Druk op enter om door te gaan");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("Dit was een demo van de webshop applicatie. Deze applicatie is gemaakt door team Nestor");
            Console.WriteLine("Tenslotte bekijken we nog het dashbord van de paymenthub");
            Console.ReadKey();
            Console.Clear();
            DashboardTest(_controller);
        }
        public static void DashboardTest(DomainController _controller) {
            Console.WriteLine("Welkom op het paymenthub Dashboard");
            Console.WriteLine("---------------------------------");
            List<Payment> payments = _controller.ToonOpenPayments();
            if (payments.Count != 0) {
                Console.WriteLine();
                Console.WriteLine("Onbetaald: ");
            }
            foreach (Payment payment in payments) {
                    Console.WriteLine("Er is een betaling van " + payment.Bedrag + " euro openstaand van " + _controller.getUserById(payment.UserId).Email + " naar " + _controller.getWebshopById(payment.BegunstigeWebshopId).Naam);
            }
            payments = _controller.ToonBetaaldePayments();
            if (payments.Count != 0) {
                Console.WriteLine();
                Console.WriteLine("Betaald: ");
            }
            foreach (Payment payment in payments) {
                Console.WriteLine("Er is een betaling van " + payment.Bedrag + " euro geslaagd van " + _controller.getUserById(payment.UserId).Email + " naar " + _controller.getWebshopById(payment.BegunstigeWebshopId).Naam);
            }
        }
    }
}
