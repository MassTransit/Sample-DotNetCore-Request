using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MessageContracts;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    [Route("api/item")]
    public class Item : Controller
    {
        readonly ISendEndpointProvider _sendEndpointProvider;

        public Item(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Submit(string id, CancellationToken cancellationToken)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:item-notification"));

            await endpoint.Send<OrderItem>(new
            {
                OrderId = id
            }, cancellationToken);

            return Ok(new {Orderid = id});
        }
    }
}