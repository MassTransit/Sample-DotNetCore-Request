using MassTransit;
using MessageContracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1
{
  [Route("api/order")]
  public class Order : Controller
  {
    private readonly IRequestClient<SubmitOrder, OrderAccepted> _requestClient;

    public Order(IRequestClient<SubmitOrder, OrderAccepted> requestClient)
    {
      _requestClient = requestClient;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Submit(string id, CancellationToken cancellationToken)
    {
      try
      {
        OrderAccepted result = await _requestClient.Request(new { OrderId = id }, cancellationToken);

        return Accepted(result.OrderId);
      }
      catch (RequestTimeoutException exception)
      {
        return StatusCode((int)HttpStatusCode.RequestTimeout);
      }
    }
  }
}