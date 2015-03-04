﻿using System.Web.Mvc;
using uProject.Models;
using uProject.Services;
using uProject.uVersion;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace uProject.Controllers
{
    public class ProjectController : SurfaceController
    {
        [ChildActionOnly]
        public ActionResult CompatibilityReport(int projectId, int fileId)
        {
            var compatReport = new VersionCompatibilityReport(projectId);

            var currentMember = Members.IsLoggedIn() ? Members.GetCurrentMember() : null;

            return PartialView("~/Views/Partials/Projects/CompatibilityReport.cshtml", 
                new VersionCompatibilityReportModel
                {
                    VersionCompatibilities = compatReport.GetCompatibilityReport(),
                    CurrentMemberHasDownloaded = currentMember != null && uProject.library.HasDownloaded(currentMember.Id, projectId),
                    CurrentMemberIsLoggedIn = currentMember != null,
                    FileId = fileId,
                    ProjectId = projectId,
                    AllVersions = UVersion.GetAllVersions()
                });
        }
    }
}