using GringottsBank.DataContracts;
using GringottsBank.Internal.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GringottsBank.Controllers
{
    [ApiController]
    [Route("/api/v1.0/customer")]
    public class CustomerController : ControllerBase
    {

        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;
        private readonly IAccountInfoService _accountInfoService;
        public CustomerController(ICustomerService customerService, IAccountInfoService accountInfoService)
        {
            _customerService = customerService;
            _accountInfoService = accountInfoService;
        }

        [HttpPost]
        [Route("new")]
        public async Task<ActionResult<Customer>> NewCustomer(NewCustomerRequest request)
        {
            var response = await _customerService.CreateAsync(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("{customerId}/allaccounts")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAccounts(string customerId)
        {
            var response = await _accountInfoService.GetAllAsync(customerId);
            return Ok(response);
        }
    }

    [ApiController]
    [Route("/api/v1.0/auth")]
    public class SessionController
    {
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login(LoginRequest loginRequest)
        {
            throw new System.Exception();
        }
    }

}
