using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSE_Hankers.Controllers
{
    public class UserFollowingController : Controller
    {
        // GET: UserFollowingController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserFollowingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserFollowingController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserFollowingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserFollowingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserFollowingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserFollowingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserFollowingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
