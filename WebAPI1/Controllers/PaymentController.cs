using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using WebAPI1.Controllers.Models.Payment;
using WebAPI1.Controllers.Models.Webshop;

namespace WebAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase {
        private IPaymentRepo _paymentRepo;

        public PaymentController(IPaymentRepo repo) {
            this._paymentRepo = repo;
        }

        // GET: api/Payment
        [HttpGet]
        [HttpHead]
        public IEnumerable<Payment> Get([FromQuery] string bedrag) {
            if (!string.IsNullOrWhiteSpace(bedrag)) {
                return _paymentRepo.GetAll(bedrag);
            }
            return _paymentRepo.GetAll();
        }


        /*
        // GET: api/payment/2
        [HttpGet("{id}", Name = "Get")]
        public Payment Get(int id) {
            try {
                return _paymentRepo.GetPayment(id);
            } catch {
                Response.StatusCode = 404;
                return null;
            }
        }
        */

        // GET: api/payment/5
        [HttpGet("{id}", Name = "Get")]
        [HttpHead("{id}")]
        public IActionResult Get(int id) {
            try {
                return Ok(_paymentRepo.GetPayment(id));
            } catch (WebshopException ex){
                return NotFound();
            }
        }

        /*
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<Payment> Get(int id) {
            try {
                return _paymentRepo.GetPayment(id);
            } catch (PaymentException ex)
            {
                return NotFound();
            }
        }
        */
            // POST: api/Payment
            [HttpPost]
        public void Post([FromBody] Payment payment) {
            _paymentRepo.AddPayment(payment);
        }
    }
}
