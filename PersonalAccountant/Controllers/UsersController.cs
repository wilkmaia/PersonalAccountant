using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using PersonalAccountant.Models;
using Newtonsoft.Json;

namespace PersonalAccountant.Controllers
{
	[RoutePrefix("api/Users")]
	public class UsersController : ApiController
    {
        private UsersContext db = new UsersContext();

		private List<SafeUsers> ParseUsers(System.Data.Entity.DbSet<Users> users)
		{
			List<SafeUsers> safeUsers = new List<SafeUsers>();

			Parallel.ForEach(users, user =>
			{
				safeUsers.Add(user.GetSafeUser());
			});

			return safeUsers;
		}

        // GET: api/Users
		[Route("")]
        public List<SafeUsers> GetUsers()
        {
            return ParseUsers(db.Users);
        }

        // GET: api/Users/:id
		[Route("{id:int}")]
        [ResponseType(typeof(SafeUsers))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            Users user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.GetSafeUser());
        }

		// POST: api/Users
		[Route("")]
		[ResponseType(typeof(SafeUsers))]
		public async Task<IHttpActionResult> PostUsers(Users user)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			db.Users.Add(user);
			await db.SaveChangesAsync();

			return Ok(user.GetSafeUser());
		}

		// POST: api/Users/login
		[Route("login")]
		[ResponseType(typeof(SafeUsers))]
		public async Task<IHttpActionResult> PostUsersLogin(Users user)
		{
			int count = await db.Users.Where(u => u.Password == user.Password && u.Username == user.Username).CountAsync();
			if (count == 0)
			{
				return NotFound();
			}

			return Ok(true);
		}

        // PUT: api/Users/:id
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUsers(int id, Users user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(Users))]
        public async Task<IHttpActionResult> DeleteUsers(int id)
        {
            Users user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user.GetSafeUser());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsersExists(int id)
        {
            return db.Users.Count(user => user.UserId == id) > 0;
        }
    }
}