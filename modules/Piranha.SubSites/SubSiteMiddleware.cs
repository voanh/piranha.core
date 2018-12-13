/*
 * Copyright (c) 2016-2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Piranha.AspNetCore;
using Piranha.Models;
using Piranha.Web;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Piranha.AspNetCore.Services;

namespace Piranha.SubSites
{
    public class SubSiteMiddleware
    {
        /// <summary>
        /// The next middleware in the pipeline.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// The optional logger.
        /// </summary>
        private ILogger _logger;
        
        /// <summary>
        /// Creates a new middleware instance.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline</param>
        /// <param name="factory">The logger factory</param>
        public SubSiteMiddleware(RequestDelegate next, ILoggerFactory factory = null)
        { 
            _next = next;

            if (factory != null)
                _logger = factory.CreateLogger(this.GetType().FullName);
        }

        /// <summary>
        /// Invokes the middleware.
        /// </summary>
        /// <param name="context">The current http context</param>
        /// <param name="api">The current api</param>
        /// <returns>An async task</returns>
        public async Task Invoke(HttpContext context, IApi api, IDb db, IApplicationService service)
        {
            if (!IsHandled(context) && !context.Request.Path.Value.StartsWith("/manager/assets/"))
            {
                var url = context.Request.Path.HasValue ? context.Request.Path.Value : "";
                var siteId = service.Site.Id;
                var authorized = true;
                bool isSubSite = false;

                // Get the available sub sites. This needs caching
                var subsites = db.Pages
                    .Where(p => p.ParentId == null && p.PageTypeId == nameof(Models.SubSitePage))
                    .Select(p => p.Id)
                    .ToArray();

                if (!String.IsNullOrWhiteSpace(url) && url.Length > 1)
                {
                    var response = PageRouter.Invoke(api, url, siteId);

                    if (response != null)
                    {
                        foreach (var item in service.Site.Sitemap.Where(p => subsites.Contains(p.Id)))
                        {
                            if (item.Id == response.PageId)
                            {
                                _logger?.LogInformation($"Requesting subsite root: {url}");

                                // We've hit the subsite root
                                var subsite = api.Pages.GetById<Models.SubSitePage>(item.Id);

                                // Update site info in the application service
                                UpdateSite(api, service, item.Id);

                                /*
                                // Store partial sitemap
                                service.Site.Sitemap = GetPartialSitemap(service.Site.Sitemap, item.Id);

                                // Check if we should reset the culture 
                                if (!string.IsNullOrEmpty(subsite.SiteInfo.Culture))
                                {
                                    service.Site.Culture = subsite.SiteInfo.Culture;
                                    var cultureInfo = new CultureInfo(service.Site.Culture);
                                    CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = cultureInfo;
                                }
                                */

                                if (subsite.SiteInfo.StartPage.HasValue)
                                {
                                    // Set specified startpage
                                    response = PageRouter.Invoke(api, $"/{subsite.SiteInfo.StartPage.Page.Slug}", siteId);
                                }
                                else 
                                {
                                    // Set the first page as startpage
                                    var firstChild = service.Site.Sitemap.FirstOrDefault();
                                    
                                    if (firstChild != null)
                                    {
                                        // Set specified startpage
                                        response = PageRouter.Invoke(api, firstChild.Permalink, siteId);
                                    }
                                    else {
                                        response = null;
                                    }
                                }
                                isSubSite = true;
                                break;
                            }
                            else if (item.HasChild(response.PageId))
                            {
                                _logger?.LogInformation($"Requesting page under subsite: {url}");

                                // Update site info in the application service
                                UpdateSite(api, service, item.Id);

                                /*
                                // The requested page is part of a subsite
                                var subsite = api.Pages.GetById<Models.SubSitePage>(item.Id);

                                // Store partial subsite
                                service.Site.Sitemap = GetPartialSitemap(service.Site.Sitemap, item.Id);

                                // Check if we should reset the culture 
                                if (!string.IsNullOrEmpty(subsite.SiteInfo.Culture))
                                {
                                    service.Site.Culture = subsite.SiteInfo.Culture;
                                    var cultureInfo = new CultureInfo(service.Site.Culture);
                                    CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = cultureInfo;
                                }
                                */
                                response = null;

                                isSubSite = true;
                                break;
                            }
                        }

                        // Handle request
                        if (isSubSite && response != null)
                        {
                            _logger?.LogInformation($"Found page\n  Route: {response.Route}\n  Params: {response.QueryString}");

                            if (!response.IsPublished)
                            {
                                if (!context.User.HasClaim(Security.Permission.PagePreview, Security.Permission.PagePreview))
                                {
                                    _logger?.LogInformation($"User not authorized to preview unpublished page");
                                    authorized = false;
                                }
                            }

                            if (authorized)
                            {
                                if (string.IsNullOrWhiteSpace(response.RedirectUrl))
                                {
                                    service.PageId = response.PageId;

                                    using (var config = new Config(api))
                                    {
                                        var headers = context.Response.GetTypedHeaders();
                                        var expires = config.CacheExpiresPages;

                                        // Only use caching for published pages
                                        if (response.IsPublished && expires > 0)
                                        {
                                            _logger?.LogInformation("Caching enabled. Setting MaxAge, LastModified & ETag");

                                            headers.CacheControl = new CacheControlHeaderValue
                                            {
                                                Public = true,
                                                MaxAge = TimeSpan.FromMinutes(expires),
                                            };

                                            headers.ETag = new EntityTagHeaderValue(response.CacheInfo.EntityTag);
                                            headers.LastModified = response.CacheInfo.LastModified;
                                        }
                                        else
                                        {
                                            headers.CacheControl = new CacheControlHeaderValue
                                            {
                                                NoCache = true
                                            };
                                        }
                                    }

                                    if (HttpCaching.IsCached(context, response.CacheInfo))
                                    {
                                        _logger?.LogInformation("Client has current version. Returning NotModified");

                                        context.Response.StatusCode = 304;
                                        return;
                                    }
                                    else
                                    {
                                        context.Request.Path = new PathString(response.Route);

                                        if (context.Request.QueryString.HasValue)
                                        {
                                            context.Request.QueryString = new QueryString(context.Request.QueryString.Value + "&" + response.QueryString);
                                        }
                                        else context.Request.QueryString = new QueryString("?" + response.QueryString);
                                    }
                                }
                                else
                                {
                                    _logger?.LogInformation($"Redirecting to url: {response.RedirectUrl}");

                                    context.Response.Redirect(response.RedirectUrl, response.RedirectType == RedirectType.Permanent);
                                    return;
                                }
                            }
                        }
                    }
                }

                if (!isSubSite && service.Site.Sitemap != null)
                {
                    _logger?.LogInformation($"Removing subsites from sitemap");

                    var pages = service.Site.Sitemap.Where(i => !subsites.Contains(i.Id)).ToArray();
                    var sitemap = new Sitemap();

                    foreach (var item in pages)
                        sitemap.Add(item);
                    service.Site.Sitemap = sitemap;
                }
            }
            await _next.Invoke(context);
        }

        /// <summary>
        /// Checks if the request has already been handled by another
        /// Piranha middleware.
        /// </summary>
        /// <param name="context">The current http context</param>
        /// <returns>If the request has already been handled</returns>
        private bool IsHandled(HttpContext context)
        {
            var values = context.Request.Query["piranha_handled"];
            if (values.Count > 0)
                return values[0] == "true";
            return false;
        }

        /// <summary>
        /// Gets a partial sitemap from the given page id.
        /// </summary>
        /// <param name="org">The original sitemap</param>
        /// <param name="id">The root of the partial</param>
        /// <returns>The sitemap</returns>
        private Sitemap GetPartialSitemap(Sitemap org, Guid id)
        {
            var sitemap = new Sitemap();

            sitemap.AddRange(org.GetPartial(id));

            return sitemap;
        }

        /// <summary>
        /// Updates the site info in the application service.
        /// </summary>
        /// <param name="api">The current api</param>
        /// <param name="service">The current application service</param>
        /// <param name="id">The subsite id</param>
        private void UpdateSite(IApi api, IApplicationService service, Guid id)
        {
            // Get the subsite
            var subsite = api.Pages.GetById<Models.SubSitePage>(id);

            // Store partial subsite
            service.Site.Sitemap = GetPartialSitemap(service.Site.Sitemap, id);

            // Check if we should reset the culture 
            if (!string.IsNullOrEmpty(subsite.SiteInfo.Culture))
            {
                service.Site.Culture = subsite.SiteInfo.Culture;
                var cultureInfo = new CultureInfo(service.Site.Culture);
                CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = cultureInfo;
            }
        }
    }
}
