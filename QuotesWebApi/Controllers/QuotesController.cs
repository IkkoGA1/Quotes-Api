using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuotesWebApi.Models;
using QuotesAPi.Models;
using Microsoft.AspNet.Identity;

namespace QuotesWebAPi.Controllers
{
    [Authorize]
    public class QuotesWebApiController : ApiController
    {
        ApplicationDbContext quotesDBContext = new ApplicationDbContext();
        // GET: api/Quotes
        [AllowAnonymous]
        [HttpGet] //we can simply put the "HTTPGET" attribute to map to the GET request if we have a custom method
        public IHttpActionResult LoadQuotes(string sort) //"Load Quotes" is the custom method name or you can have the attribute in front(i.e) GetQuotes
        {
            IQueryable<Quote> quotes; // I Queryable helps with performance because it send the exact items for search, not all
            switch (sort)// added sorting ability
            {
                case "desc":
                    quotes = quotesDBContext.Quotes.OrderByDescending(q => q.CreatedAt);
                    break;
                case "asc":
                    quotes = quotesDBContext.Quotes.OrderBy(q => q.CreatedAt);
                    break;
                default:
                    quotes = quotesDBContext.Quotes;
                    break;
            }
            return Ok(quotes);
        }

        [HttpGet]
        [Route("api/Quotes/PagingQuote/{pageNumber=1}/{pageSize=5}")]
       public IHttpActionResult PagingQuote(int pageNumber, int pageSize)
        {
            var quotes = quotesDBContext.Quotes.OrderBy(q => q.Id);
            return Ok (quotes.Skip((pageNumber - 1) * pageSize).Take(pageSize));//skip algorithm sends records in chunks
        }
        [HttpGet]
        [Route("api/Quotes/SearchQuote/{Type=}")]
        public IHttpActionResult SearchQuotes(string Type)
        {
            var quotes = quotesDBContext.Quotes.Where(q => q.Type.StartsWith(Type));
            return Ok(quotes);
        }

        // GET: api/Quotes/5
        [HttpGet]
        public IHttpActionResult LoadQuote(int Id)
        {
            var quotes = quotesDBContext.Quotes.Find(Id);
            if (quotes == null)
            {
                return NotFound();
            }
            return Ok(quotes);
        }

        // POST: api/Quotes
        public IHttpActionResult Post([FromBody]Quote quote)
        {
            string userId = User.Identity.GetUserId();
            quote.UserId = userId;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            quotesDBContext.Quotes.Add(quote);
            quotesDBContext.SaveChanges();
            return StatusCode(HttpStatusCode.Created);

        }

        // PUT: api/Quotes/5
        public IHttpActionResult Put(int Id, [FromBody]Quote quote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = quotesDBContext.Quotes.FirstOrDefault(q => q.Id == Id);
            if (entity == null)
            {
                return BadRequest("No such record exists in this database...!");
            }
            entity.Title = quote.Title;
            entity.Author = quote.Author;
            entity.Description = quote.Description;
            quotesDBContext.SaveChanges();
            return Ok("Record Updated Successfully..!");
        }

        // DELETE: api/Quotes/5
        public IHttpActionResult Delete(int Id)
        {
            var quote = quotesDBContext.Quotes.Find(Id);
            if (quote == null)
            {
                return BadRequest("No such record exists in this database...!");
            }
            quotesDBContext.Quotes.Remove(quote);
            quotesDBContext.SaveChanges();
            return Ok("The Record was deleted successfully....!");
            
        }
    }
}
