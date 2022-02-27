using Microsoft.AspNetCore.Mvc;
using GringottsBank.Internal.Contracts;
using GringottsBank.DataContracts;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GringottsBank.Controllers
{
    [ApiController]
    [Route("/api/v1.0/account")]
    public class AccountController: ControllerBase 
    {
        private readonly IAccountInfoService _accountInfoService;
        private readonly ITransactionService _transactionService;
        private readonly ITransactionInfoService _transactionInfoService;

        public AccountController(IAccountInfoService accountInfoService, 
                                 ITransactionService transactionService, 
                                 ITransactionInfoService transactionInfoService)
        {
            _accountInfoService = accountInfoService;
            _transactionService = transactionService;
            _transactionInfoService = transactionInfoService;
        }

        [HttpGet]
        [Route("{accNo}/details")]
        public async Task<ActionResult<Account>> GetAccountDetails(string accNo)
        {
            var response = await _accountInfoService.GetAsync(accNo);
            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Account>> CreateAccount(NewAccountRequest newAccountRequest)
        {
            var response = await _accountInfoService.CreateAsync(newAccountRequest);
            return Ok(response);
        }

        [HttpPut]
        [Route("transact/withdraw")]
        public async Task<ActionResult<Transaction>> Withdraw(TransactionRequest txnRequest)
        {
            var response = await _transactionService.Withdraw(txnRequest.AccountNumber, txnRequest.Amount);
            return Ok(response);
        }

        [HttpPut]
        [Route("transact/deposit")]
        public async Task<ActionResult<Transaction>> Deposit(TransactionRequest txnRequest)
        {
            var response = await _transactionService.Deposit(txnRequest.AccountNumber, txnRequest.Amount);
            return Ok(response);
        }

        [HttpPost]
        [Route("transactions")]
        public async Task<ActionResult<IEnumerable<Transaction>>> ListTransactions(TransactionDetailsRequest txnDetailsRequest)
        {
            var response = await _transactionInfoService.FetchTransactionsAsync(txnDetailsRequest);
            return Ok(response);
        }
    }

}
