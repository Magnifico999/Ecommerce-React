using API.Data;
using API.Data.DTO;
using API.Entities;
using API.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
	[Route("api/[controller]")]
	public class BasketController : BaseApiController
	{
		private readonly StoreContext _context;

		public BasketController(StoreContext context)
		{
			_context = context;
		}

		[HttpGet(Name = "GetBasket")]
		public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await RetrieveBasket(GetBuyerId());

            if (basket == null) return NotFound();

            return basket.MapBasketToDto();
        }

       

        [HttpPost]
		public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
		{
			//get basket || create the basket
			var basket = await RetrieveBasket(GetBuyerId());
			if (basket == null) basket = CreateBasket();

			//get product
			var product = await _context.Products.FindAsync(productId);
			if (product == null) return NotFound();

			basket.AddItem(product, quantity);

			var result = await _context.SaveChangesAsync() > 0;
			if (result) return CreatedAtRoute("GetBasket", basket.MapBasketToDto());

			return BadRequest(new ProblemDetails { Title = "Problem saving Item to basket" });
		}

		[HttpDelete]
		public async Task<ActionResult> RemoveBasketItem(int productId, int quantity)
		{
			var basket = await RetrieveBasket(GetBuyerId());
			if (basket == null) return NotFound();

			var basketItem = basket.Items.FirstOrDefault(item => item.ProductId == productId);
			if (basketItem == null)
			{
				return NotFound("Item not found in the basket");
			}

			if (quantity > basketItem.Quantity)
			{
				return BadRequest("Quantity to remove exceeds the available quantity in the basket");
			}

			
			basket.RemoveItem(productId, quantity);

			var result = await _context.SaveChangesAsync() > 0;
			if (result) return Ok();

			return BadRequest(new ProblemDetails { Title = "Problem deleting item from the basket" });
		}


		private async Task<Basket> RetrieveBasket(string buyerId)
		{
			if (string.IsNullOrEmpty(buyerId))
			{
				Response.Cookies.Delete("buyerId");
				return null;
			}
			return await _context.Baskets
				.Include(i => i.Items)
				.ThenInclude(p => p.Product)
				.FirstOrDefaultAsync(x => x.BuyerId == buyerId);
		}

		private string GetBuyerId()
		{
			return User.Identity?.Name ?? Request.Cookies["buyerId"];
		}

		private Basket CreateBasket()
		{
			var buyerId = User.Identity?.Name;
			if (string.IsNullOrEmpty(buyerId))
			{
				buyerId = Guid.NewGuid().ToString();
				var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30)};
				Response.Cookies.Append("buyerId", buyerId, cookieOptions);
			}
			
			var basket = new Basket { BuyerId = buyerId };
			_context.Baskets.Add(basket);
			return basket;
		}
        
	}
}