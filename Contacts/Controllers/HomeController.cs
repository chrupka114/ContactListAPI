
using Contacts.Data;
using Contacts.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;

namespace Contacts.Controllers
{


	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		//loading databases and cache
		private readonly ApplicationDbContext _db;
		private readonly LoginDbContext _ldb;
		private readonly IMemoryCache _cache;
		private readonly string cachekey = "logged";
		public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, LoginDbContext ldb, IMemoryCache cache)
		{
			_logger = logger;
			_db = db;
			_ldb = ldb;
			_cache = cache;

		}
		//GETALL
		public IActionResult Index()
		{
			//display list of contacts
			IEnumerable<Contact> objContactsList = _db.Contacts.ToList();
			return View(objContactsList);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		//GET
		public IActionResult Create()
		{
			//redirecting to create viev
			if (_cache.TryGetValue(cachekey, out bool logged))
			{
				return View();
			}
			TempData["success"] = "you have to be logged in";
			return RedirectToAction("Index");

		}
		//POST

		[HttpPost]
		[ValidateAntiForgeryToken]

		public IActionResult Create(Contact obj)
		{
			//creating contact
			foreach (var otherobj in _db.Contacts)
			{
				if (obj.Email == otherobj.Email)
				{
					ModelState.AddModelError("CustomError", "email alredy in use");
				}
			}
			if (obj.Password.Any(char.IsDigit) == false)
			{
				ModelState.AddModelError("CustomError", "Password must contain at least one number");
			}
			if (obj.Password.Any(ch => !char.IsLetterOrDigit(ch)) == false)
			{
				ModelState.AddModelError("CustomError", "Password must contain at least one special character");
			}
			if (obj.Password.Any(char.IsUpper) == false)
			{
				ModelState.AddModelError("CustomError", "Password must contain at least one uppercase letter");
			}

			if (ModelState.IsValid)
			{
				_db.Contacts.Add(obj);
				_db.SaveChanges();
				TempData["success"] = "Contact created succesfully";
				return RedirectToAction("Index");
			}
			return View(obj);
		}
		//GET
		public IActionResult Info(int? id)
		{
			//redirecting to info viev
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var ContactFromDb = _db.Contacts.Find(id);


			if (ContactFromDb == null)
			{
				return NotFound();
			}


			return View(ContactFromDb);
		}
		//GET
		public IActionResult Edit(int? id)
		{
			//redirecting to edit viev
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var ContactFromDb = _db.Contacts.Find(id);


			if (ContactFromDb == null)
			{
				return NotFound();
			}

			if (_cache.TryGetValue(cachekey, out bool logged))
			{
				return View(ContactFromDb);
			}
			TempData["success"] = "you have to be logged in";
			return RedirectToAction("Index");
		}

		//PUT
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Contact obj)
		{
			//checking the compex of password
			if (obj.Password.Any(char.IsDigit) == false)
			{
				ModelState.AddModelError("CustomError", "Password must contain at least one number");
			}
			if (obj.Password.Any(ch => !char.IsLetterOrDigit(ch)) == false)
			{
				ModelState.AddModelError("CustomError", "Password must contain at least one special character");
			}
			if (obj.Password.Any(char.IsUpper) == false)
			{
				ModelState.AddModelError("CustomError", "Password must contain at least one uppercase letter");
			}

			//uppdating contact
			if (ModelState.IsValid)
			{
				_db.Contacts.Update(obj);
				_db.SaveChanges();
				TempData["success"] = "Contact updated succesfully";
				return RedirectToAction("Index");
			}
			return View(obj);
		}
		//GET
		public IActionResult Delete(int? id)
		{
			//redirecting to delete viev
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var ContactFromDb = _db.Contacts.Find(id);

			if (ContactFromDb == null)
			{
				return NotFound();
			}

			if (_cache.TryGetValue(cachekey, out bool logged))
			{
				return View(ContactFromDb);
			}
			TempData["success"] = "you have to be logged in";
			return RedirectToAction("Index");
		}
		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePOST(int? id)
		{
			//deleting contact from database
			var obj = _db.Contacts.Find(id);
			if (obj == null)
			{
				return NotFound();
			}


			_db.Contacts.Remove(obj);
			_db.SaveChanges();
			TempData["success"] = "Contact deleted succesfully";
			return RedirectToAction("Index");



		}
		//GET
		public IActionResult Register()
		{
			//redirecting to register viev
			return View();
		}
		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Register(LoginData obj)
		{
			//checking if email alredy exists in database
			foreach (var otherobj in _db.Contacts)
			{
				if (obj.Email == otherobj.Email)
				{
					ModelState.AddModelError("CustomError", "email alredy in use");
				}
			}
			//checking the compex of password
			if (obj.Password.Any(char.IsDigit) == false)
			{
				ModelState.AddModelError("CustomError", "Password must contain at least one number");
			}
			if (obj.Password.Any(ch => !char.IsLetterOrDigit(ch)) == false)
			{
				ModelState.AddModelError("CustomError", "Password must contain at least one special character");
			}
			if (obj.Password.Any(char.IsUpper) == false)
			{
				ModelState.AddModelError("CustomError", "Password must contain at least one uppercase letter");
			}
			//ading user to database
			if (ModelState.IsValid)
			{
				_ldb.Login.Add(obj);
				_ldb.SaveChanges();
				TempData["success"] = "registered succesfully";
				return RedirectToAction("Index");
			}
			return View(obj);
		}
		public IActionResult Login()
		{
			//redirecting to login viev
			return View();
		}

		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Login(LoginData obj)
		{




			if (obj.Password.Any(char.IsDigit) == false)
			{
				ModelState.AddModelError("CustomError", "Password must contain at least one number");
			}
			if (obj.Password.Any(ch => !char.IsLetterOrDigit(ch)) == false)
			{
				ModelState.AddModelError("CustomError", "Password must contain at least one special character");
			}
			if (obj.Password.Any(char.IsUpper) == false)
			{
				ModelState.AddModelError("CustomError", "Password must contain at least one uppercase letter");
			}
			foreach (var otherobj in _ldb.Login)
			{
				//checking email and password
				if (obj.Email.Equals(otherobj.Email) && obj.Password.Equals(otherobj.Password))
				{
					obj.Password.Equals(otherobj.Password);
					TempData["loginsuccess"] = "Logged succesfully";
					//changing logged status to true
					_cache.Set(cachekey, true);
					break;

				}
				else
				{
					ModelState.AddModelError("CustomError", "wrong password or email");
				}
			}



			if (ModelState.IsValid)
			{

				return RedirectToAction("Index");
			}
			return View(obj);
		}
	}
}
