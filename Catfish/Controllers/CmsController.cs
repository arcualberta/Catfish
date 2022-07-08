using Catfish.Core.Services;
using Catfish.Helper;
using Catfish.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Piranha;
using Piranha.AspNetCore.Services;
using Piranha.Models;
using System;
using System.Threading.Tasks;

namespace Catfish.Controllers
{
    public class CmsController : Controller
    {
        private readonly IApi _api;
        private readonly IModelLoader _loader;
        private readonly IDb _db;
        private readonly IEmailService _email;
      

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="api">The current api</param>
        public CmsController(IApi api,IDb db,  IModelLoader loader, IEmailService email, IHttpContextAccessor httpContextAccessor)
        {
            _api = api;
            _loader = loader;
            _db = db;
            _email = email;
        }

        /// <summary>
        /// Gets the blog archive with the given id.
        /// </summary>
        /// <param name="id">The unique page id</param>
        /// <param name="year">The optional year</param>
        /// <param name="month">The optional month</param>
        /// <param name="page">The optional page</param>
        /// <param name="category">The optional category</param>
        /// <param name="tag">The optional tag</param>
        /// <param name="draft">If a draft is requested</param>
        [Route("archive")]
        public async Task<IActionResult> Archive(Guid id, int? year = null, int? month = null, int? page = null,
            Guid? category = null, Guid? tag = null, bool draft = false)
        {
            // var model = await _api.Pages.GetByIdAsync<Models.BlogArchive>(id);   
            //model.Archive = await _api.Archives.GetByIdAsync(id, page, category, tag, year, month);
            try
            {
                var model = await _loader.GetPageAsync<StandardArchive>(id, HttpContext.User, draft).ConfigureAwait(true);
                model.Archive = await _api.Archives.GetByIdAsync<PostInfo>(id, page, category, tag, year, month);

                return View(model);
            }
            catch(UnauthorizedAccessException ex)
            {
                return StatusCode(401);
            }
        }

        /// <summary>
        /// Gets the page with the given id.
        /// </summary>
        /// <param name="id">The unique page id</param>
        /// <param name="draft">If a draft is requested</param>
        [Route("page")]
        public async Task<IActionResult> Page(Guid id, bool draft = false)
        {
            
            try
            {
                var model = await _loader.GetPageAsync<StandardPage>(id, HttpContext.User, draft).ConfigureAwait(false);
                if (model != null && model.IsCommentsOpen)
                {
                    model.Comments = await _api.Pages.GetAllCommentsAsync(model.Id, true).ConfigureAwait(false);
                }
                return View(model);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(401);//401 --UnAuthorized
            }
            
        }

        /// <summary>
        /// Gets the post with the given id.
        /// </summary>
        /// <param name="id">The unique post id</param>
        /// <param name="draft">If a draft is requested</param>
        [Route("post")]
        public async Task<IActionResult> Post(Guid id, bool draft = false)
        {
            //// var model = await _loader.GetPost<BlogPost>(id, HttpContext.User, draft);
            // var model = await _loader.GetPostAsync<Models.BlogPost>(id, HttpContext.User, draft);

            // return View(model);
            try
            {
                var model = await _loader.GetPostAsync<StandardPost>(id, HttpContext.User, draft).ConfigureAwait(false);

                if (model.IsCommentsOpen)
                {
                    model.Comments = await _api.Posts.GetAllCommentsAsync(model.Id, true).ConfigureAwait(false);
                }
                return View(model);
            }
            catch(UnauthorizedAccessException ex)
            {
                return StatusCode(401);
            }
        }

        /// <summary>
        /// Gets the startpage with the given id.
        /// </summary>
        /// <param name="id">The unique page id</param>
        /// <param name="draft">If a draft is requested</param>
        [Route("start")]
        public async Task<IActionResult> Start(Guid id, bool draft = false)
        {
            var model = await _loader.GetPageAsync<StartPage>(id, HttpContext.User, draft).ConfigureAwait(false);

            return View(model);
        }

        /// <summary>
        /// Gets the startpage with the given id.
        /// </summary>
        /// <param name="id">The unique page id</param>
        /// <param name="draft">If a draft is requested</param>
        //[Route("main")]
        //public async Task<IActionResult> Main(Guid id, bool draft = false)
        //{
        //    try
        //    {
        //        var model = await _loader.GetPageAsync<MainPage>(id, HttpContext.User, draft);

        //        return View(model);
        //    }
        //    catch(UnauthorizedAccessException ex)
        //    {
        //        return StatusCode(401);//401 --UnAuthorized
        //    }
        //}

        [Route("mediapage")]
        public async Task<IActionResult> MediaPage(Guid id, bool draft = false)
        {

            var model = await _loader.GetPageAsync<MediaPage>(id, HttpContext.User, draft).ConfigureAwait(false);

            return View(model);

        }
        /// <summary>
        /// Gets the startpage with the given id.
        /// </summary>
        /// <param name="id">The unique page id</param>
        /// <param name="draft">If a draft is requested</param>
        //[Route("login")]
        //public async Task<IActionResult> Login(Guid id, bool draft = false)
        //{
        //   // var model = await _loader.GetPageAsync<LoginPage>(id, HttpContext.User, draft);

        //    return View(model);
        //}

        public JsonResult SendEmail(Email email)
        {
            try
            {
                _email.SendEmail(email);
            }
            catch (Exception ex)
            {
                throw;
            }

            return Json(true);
        }

        /// <summary>
        /// Saves the given comment and then redirects to the post.
        /// </summary>
        /// <param name="id">The unique post id</param>
        /// <param name="commentModel">The comment model</param>
        [HttpPost]
        [Route("post/comment")]
        public async Task<IActionResult> SavePostComment(SaveCommentModel commentModel)
        {
            try
            {
                var model = await _loader.GetPostAsync<StandardPost>(commentModel.Id, HttpContext.User).ConfigureAwait(true);

                // Create the comment
                var comment = new Comment
                {
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    UserAgent = Request.Headers.ContainsKey("User-Agent") ? Request.Headers["User-Agent"].ToString() : "",
                    Author = commentModel.CommentAuthor,
                    Email = commentModel.CommentEmail,
                    Url = commentModel.CommentUrl,
                    Body = commentModel.CommentBody
                };
                await _api.Posts.SaveCommentAndVerifyAsync(commentModel.Id, comment).ConfigureAwait(true);

                return Redirect(model.Permalink + "#comments");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Saves the given comment and then redirects to the post.
        /// </summary>
        /// <param name="id">The unique post id</param>
        /// <param name="commentModel">The comment model</param>
        [HttpPost]
        [Route("page/comment")]
        public async Task<IActionResult> SavePageComment(SaveCommentModel commentModel)
        {
            try
            {
                var model = await _loader.GetPageAsync<StandardPage>(commentModel.Id, HttpContext.User).ConfigureAwait(true);

                // Create the comment
                var comment = new Comment
                {
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    UserAgent = Request.Headers.ContainsKey("User-Agent") ? Request.Headers["User-Agent"].ToString() : "",
                    Author = commentModel.CommentAuthor,
                    Email = commentModel.CommentEmail,
                    Url = commentModel.CommentUrl,
                    Body = commentModel.CommentBody
                };
                await _api.Pages.SaveCommentAndVerifyAsync(commentModel.Id, comment).ConfigureAwait(true);
               // await _api.Posts.SaveCommentAndVerifyAsync(commentModel.Id, comment).ConfigureAwait(true);

                return Redirect(model.Permalink + "#comments");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
