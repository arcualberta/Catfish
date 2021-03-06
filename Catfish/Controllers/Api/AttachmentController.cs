﻿using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Helpers;
using Catfish.Core.Models;
using Catfish.Core.Models.Data;
using Catfish.Core.Services;
using Catfish.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Controllers.Api
{
    public class AttachmentController : CatfishController
    {
        // GET: Attachment
        [HttpPost]
        public JsonResult Upload(int maxPixelSize = 0)
        {
            try
            {
                List<CFDataFile> files = DataService.UploadTempFiles(Request, maxPixelSize);
                Db.SaveChanges(User.Identity);

                //Saving ids  of uploaded files in the session because these files and thumbnails
                //needs to be accessible by the user who is uploading them without restriction of any security rules.
                //This is because these files are stored in the temporary area without associating to any items.
                FileHelper.CacheGuids(Session, files);

                var ret = files.Select(f => new FileViewModel(f, f.Id, ControllerContext.RequestContext, "attachment"));
                return Json(ret);
            }
            catch (Exception ex)
            {
                //return 500 or something appropriate to show that an error occured.
                return Json(new { error = ex.Message + "/n" + ex.StackTrace });
            }
        }

        public ActionResult File(int id, string guid)
        {
            //This is an unprotected method so it only returns if the ids  of the file is in the session.
            // i.e. the file was uploaded during the current session
            if (!FileHelper.CheckGuidCache(Session, guid))
                return HttpNotFound("File not found");
            
            CFDataFile file = DataService.GetFile(id, guid);
            if (file == null)
                return HttpNotFound("File not found");

            string path_name = Path.Combine(file.Path, file.LocalFileName);
            return new FilePathResult(path_name, file.ContentType);
        }

        public ActionResult Thumbnail(int id, string name)
        {
            //This is an unprotected method so it only returns if the GUID name of the file is in the session.
            // i.e. the file was uploaded during the current session
            if (!FileHelper.CheckGuidCache(Session, name))
                return HttpNotFound("File not found");
            
            CFDataFile file = DataService.GetFile(id, name);
            if (file == null)
                return HttpNotFound("File not found");

            string path_name = file.ThumbnailType == CFDataFile.eThumbnailTypes.Shared
                ? Path.Combine(FileHelper.GetThumbnailRoot(Request), file.Thumbnail)
                : Path.Combine(file.Path, file.Thumbnail);

            return new FilePathResult(path_name, file.ContentType);
        }

        [HttpPost]
        public JsonResult DeleteCashedFile(string guid)
        {
            try
            {
                //Makes sure that the requested file is in the cache
                if (!FileHelper.CheckGuidCache(Session, guid))
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    Response.StatusDescription = "BadRequest: the file cannot be deleted -  NOT IN CACHE.";
                    return Json(string.Empty);
                }

                if (!DataService.DeleteStandaloneFile(guid))
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    Response.StatusDescription = "The file not found";
                    return Json(string.Empty);
                }

                Db.SaveChanges(User.Identity);
                return Json(new List<string>() { guid });
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                Response.StatusDescription = "BadRequest: an unknown error occurred.";
                return Json(string.Empty);
            }
        }
    }

}