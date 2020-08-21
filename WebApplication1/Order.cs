using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MessageContracts;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    [Route("api/order")]
    public class Order : Controller
    {
        readonly IRequestClient<SubmitOrder> _requestClient;

        public Order(IRequestClient<SubmitOrder> requestClient)
        {
            _requestClient = requestClient;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Submit(string id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _requestClient.GetResponse<OrderAccepted>(new {OrderId = id}, cancellationToken);

                return Accepted(new {result.Message.OrderId});
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int) HttpStatusCode.RequestTimeout);
            }
        }
    }
}