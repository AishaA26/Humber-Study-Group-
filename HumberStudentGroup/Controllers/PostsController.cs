using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HumberStudentGroup.ADO;

namespace HumberStudentGroup.Controllers
{
    public class PostsController : Controller
    {
        private HumberDBEntities db = new HumberDBEntities();

        public ActionResult Index()
        {
            // get the db posts to index
            return View(db.Posts.ToList());
        }

        public ActionResult Details(int? id)
        {
            // chck if the id is null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // find the posts from the database
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            //return the post
            return View(post);
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? groupId, [Bind(Include = "Id,Title,Body")] Post post)
        {
            // find the user from the session
            int userId = int.Parse(Session["UserId"].ToString());
            if (ModelState.IsValid)
            {
                // create the posts and save
                post.User = db.Users.Single(m => m.Id == userId);
                post.Groups.Add(db.Groups.Single(g => g.Id == groupId));
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // find the post from the id
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Body")] Post post)
        {
            // if the model is valid proform edits
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // find the post for deleting
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // if the post is confiremd to delete
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
