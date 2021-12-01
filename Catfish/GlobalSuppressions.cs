﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "<Pending>", Scope = "member", Target = "~F:Catfish.Pages.CatfishPageModelModel._authorizationSertvice")]
[assembly: SuppressMessage("Design", "CA1055:Uri return values should not be strings", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Helper.ICatfishAppConfiguration.GetLogoUrl~System.String")]
[assembly: SuppressMessage("Design", "CA1055:Uri return values should not be strings", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Helper.ReadAppConfiguration.GetLogoUrl~System.String")]
[assembly: SuppressMessage("Design", "CA1055:Uri return values should not be strings", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Helper.ReadAppConfiguration.GetSolrUrl~System.String")]
[assembly: SuppressMessage("Design", "CA1055:Uri return values should not be strings", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Helper.ICatfishAppConfiguration.GetSolrUrl~System.String")]
[assembly: SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.AuthorizationService.EnsureUserRoles(System.Collections.Generic.List{System.String})")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:Catfish.Pages.Workflow.StartPage.ItemTemplates")]
[assembly: SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.AuthorizationService.EnsureSystemRoles")]
[assembly: SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.CatfishInitializationService.EnsureSystemRoles")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Helper.ReadAppConfiguration.GetValue(System.String,System.Int32)~System.Int32")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Areas.Manager.Pages.GroupModel.OnGet(System.Nullable{System.Guid})")]
[assembly: SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "<Pending>", Scope = "type", Target = "~T:Catfish.Areas.Manager.Access.AuthorizationRequirements.CrudOperations")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Areas.Manager.Access.AuthorizationHandlers.EntityTemplateAuthorizationHandler.HandleRequirementAsync(Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext,Microsoft.AspNetCore.Authorization.Infrastructure.OperationAuthorizationRequirement,Catfish.Core.Models.EntityTemplate)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Areas.Manager.Access.AuthorizationHandlers.EntityTemplateAuthorizationHandler.HandleRequirementAsync(Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext,Microsoft.AspNetCore.Authorization.Infrastructure.OperationAuthorizationRequirement,Catfish.Core.Models.EntityTemplate)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.AuthorizationService.GetRole(System.String,System.Boolean)~Piranha.AspNetCore.Identity.Data.Role")]
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Areas.Manager.Access.AuthorizationHandlers.EntityTemplateAuthorizationHandler.HandleRequirementAsync(Microsoft.AspNetCore.Authorization.AuthorizationHandlerContext,Microsoft.AspNetCore.Authorization.Infrastructure.OperationAuthorizationRequirement,Catfish.Core.Models.EntityTemplate)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Api.Controllers.SearchController.Keywords(System.String[])~System.Collections.Generic.IList{Catfish.Core.Models.Solr.SolrEntry}")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:Catfish.Models.Fields.ControlledKeywordsField.AllowedKeywords")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.CatfishSiteService.UpdateBlockSettingsAsync(Piranha.Models.PageBase)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.CatfishSiteService.UpdateBlockSettingsAsync(Piranha.Models.PostBase)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.CatfishSiteService.UpdateSettingsAsync(Piranha.Models.PostBase)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.CatfishSiteService.UpdateSettingsAsync(Piranha.Models.PageBase)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.CatfishSiteService.UpdateKeywordVocabularyAsync(Piranha.Models.PageBase)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.CatfishSiteService.UpdateKeywordVocabularyAsync(Piranha.Models.PostBase)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Api.Controllers.SearchController.Keywords(System.String[],System.String)~System.Collections.Generic.IList{Catfish.Core.Models.Solr.SolrEntry}")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Api.Controllers.SearchController.FreeText(System.String)~System.Collections.Generic.IList{Catfish.Core.Models.Solr.SolrEntry}")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.PageIndexingService.IndexPage(Piranha.Models.PageBase)")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Controllers.Api.ItemsController.Post(Catfish.Core.Models.Contents.Data.DataItem,System.Guid,System.Guid,System.String)~Catfish.Core.Models.ApiResult")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:Catfish.Models.Blocks.VueCarousel.Slides")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:Catfish.Models.Blocks.SubmissionForm.AvailableGroups")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Models.Regions.PublishSettings.GetStartDate~System.Nullable{System.DateTime}")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:Catfish.Models.Blocks.SubmissionForm.SelectedGroupIds")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Models.Regions.PublishSettings.ToDateTime(Piranha.Extend.Fields.DateField,Piranha.Extend.Fields.StringField)~System.Nullable{System.DateTime}")]
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Models.Blocks.VueComponent.GetVueTemplateId~System.String")]
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Models.Blocks.VueComponentGroup.GetVueTemplateId~System.String")]
[assembly: SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.IAppService.AddScript(System.String)")]
[assembly: SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.IAppService.AddStylesheet(System.String)")]
[assembly: SuppressMessage("Performance", "CA1820:Test for empty strings using string length", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.SubmissionService.SetSubmission(Catfish.Core.Models.Contents.Data.DataItem,System.Guid,System.Guid,System.Nullable{System.Guid},System.String)~Catfish.Core.Models.Item")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Pages.TemplateEditModel.OnPost(System.Guid)")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Pages.CustomStylesModel.OnPost")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Controllers.Api.ItemsController.GetItemList(System.Guid,System.Nullable{System.Guid},System.Nullable{System.DateTime},System.Nullable{System.DateTime})~System.Collections.Generic.IList{System.String}")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Controllers.Api.ItemsController.GetItemList(System.Guid,System.Nullable{System.Guid},System.Nullable{System.DateTime},System.Nullable{System.DateTime})~System.String")]
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.AppService.RegisterOnLoadFunction(System.String)")]
[assembly: SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Controllers.Api.ItemsController.GetItemList(System.Guid,System.Nullable{System.Guid},System.Nullable{System.DateTime},System.Nullable{System.DateTime})~System.String")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Controllers.Api.ItemsController.GetItemList(System.Guid,System.Nullable{System.Guid},System.Nullable{System.DateTime},System.Nullable{System.DateTime},System.Nullable{System.Guid})~System.String")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Areas.Manager.Pages.SchemaPageModel.OnPost(System.Guid)~Microsoft.AspNetCore.Mvc.IActionResult")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.AuthorizationService.CreateRole(System.String,System.Guid)~Piranha.AspNetCore.Identity.Data.Role")]
[assembly: SuppressMessage("Usage", "CA2234:Pass system uri objects instead of strings", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.SolrService.PostAsync(System.String,System.String)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.SolrService.GetSampleDoc~System.Xml.Linq.XElement")]
[assembly: SuppressMessage("Usage", "CA2234:Pass system uri objects instead of strings", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.SolrService.CommitAsync")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.SolrService.#ctor(Microsoft.Extensions.Configuration.IConfiguration,Catfish.Core.Models.AppDbContext)")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.SolrService.Search(Catfish.Core.Models.Solr.SearchFieldConstraint[],System.Int32,System.Int32)~Catfish.Core.Models.Solr.SearchResult")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.SolrService.Search(System.String)~Catfish.Core.Models.Solr.SearchResult")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.SolrService.Search(System.String,System.Int32,System.Int32)~Catfish.Core.Models.Solr.SearchResult")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Services.SolrService.ExecuteSearchQuery(System.String,System.Int32,System.Int32,System.Int32)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Minor Code Smell", "S2219:Runtime type checking should be simplified", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Helper.BlockHelper.GetVueComponentNames(System.Collections.Generic.List{Piranha.Extend.Block})~System.Collections.Generic.IList{System.String}")]
[assembly: SuppressMessage("Minor Code Smell", "S2219:Runtime type checking should be simplified", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Helper.BlockHelper.GetVueComponentNames(System.Collections.Generic.IList{Piranha.Extend.Block})~System.Collections.Generic.IList{System.String}")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:Catfish.Models.Blocks.TileGrid.TileGrid.Tiles")]
[assembly: SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "<Pending>", Scope = "member", Target = "~P:Catfish.Models.Blocks.TileGrid.Tile.ObjectUrl")]
[assembly: SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "<Pending>", Scope = "member", Target = "~F:Catfish.Areas.Applets.Services.AssetRegistry.Stylesheets")]
[assembly: SuppressMessage("Minor Code Smell", "S1104:Fields should not have public accessibility", Justification = "<Pending>", Scope = "member", Target = "~F:Catfish.Areas.Applets.Services.AssetRegistry.Scripts")]
[assembly: SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Areas.Applets.Controllers.KeywordSearchController.GetItems(System.Guid,System.Guid,System.String,System.Int32,System.Int32)~System.Threading.Tasks.Task{Catfish.Areas.Applets.Models.Blocks.KeywordSearchModels.SearchOutput}")]
[assembly: SuppressMessage("Blocker Bug", "S2275:Composite format strings should not lead to unexpected behavior at runtime", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Startup.RegisterCustomBlocks")]
[assembly: SuppressMessage("Usage", "CA2241:Provide correct arguments to formatting methods", Justification = "<Pending>", Scope = "member", Target = "~M:Catfish.Startup.RegisterCustomBlocks")]
