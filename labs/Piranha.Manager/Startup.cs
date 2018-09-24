using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Piranha.Extend.Blocks;
using Piranha.Local;

namespace Piranha.Manager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc(config => 
            {
                config.ModelBinderProviders.Insert(0, new Piranha.Manager.Binders.AbstractModelBinderProvider());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddPiranhaFileStorage();
            services.AddPiranhaEF(options => options.UseSqlite("Filename=./piranha.labs.db"));

            // Should be packaged into a service extensions
            services.AddScoped<Services.PageEditService, Services.PageEditService>();
            App.Modules.Register<Module>();

            App.Init();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApi api)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            var pageTypeBuilder = new Piranha.AttributeBuilder.PageTypeBuilder(api)
                .AddType(typeof(TestPage));
            pageTypeBuilder.Build()
                .DeleteOrphans();            

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();

            if (api.Pages.GetAll().Count() == 0)
            {
                var site = api.Sites.GetDefault();

                var imageId = Guid.NewGuid();
                using (var stream = File.OpenRead("Assets/img/hyperlightdrifter.jpg")) {
                    api.Media.Save(new Piranha.Models.StreamMediaContent() {
                        Id = imageId,
                        Filename = "hyperlightdrifter.jpg",
                        Data = stream
                    });
                }

                var heroId = Guid.NewGuid();
                using (var stream = File.OpenRead("Assets/img/heroimage.png")) {
                    api.Media.Save(new Piranha.Models.StreamMediaContent() {
                        Id = heroId,
                        Filename = "heroimage.png",
                        Data = stream
                    });
                }

                var page = TestPage.Create(api);
                page.Id = new Guid("a47bc4f1-1722-4e09-b596-ab25d7657afb");
                page.SiteId = site.Id;
                page.Title = "Fermentum Amet Adipiscing";
                page.Blocks.Add(new HtmlBlock 
                {
                    Body = "<p>Nullam id dolor id nibh ultricies vehicula ut id elit. Maecenas sed diam eget risus varius blandit sit amet non magna. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Vestibulum id ligula porta felis euismod semper. Cras justo odio, dapibus ac facilisis in, egestas eget quam. Fusce dapibus, tellus ac cursus commodo, tortor mauris condimentum nibh, ut fermentum massa justo sit amet risus. Nullam quis risus eget urna mollis ornare vel eu leo.</p>"
                });
                page.Blocks.Add(new ImageBlock
                {
                    Body = imageId
                });
                page.Blocks.Add(new HtmlColumnBlock
                {
                    Column1 = "<p>Integer posuere erat a ante venenatis dapibus posuere velit aliquet. Nullam id dolor id nibh ultricies vehicula ut id elit. Aenean lacinia bibendum nulla sed consectetur. Vivamus sagittis lacus vel augue laoreet rutrum faucibus dolor auctor.</p>",
                    Column2 = "<p>Nullam quis risus eget urna mollis ornare vel eu leo. Donec sed odio dui. Nulla vitae elit libero, a pharetra augue. Donec id elit non mi porta gravida at eget metus.</p>"
                });
                page.Header.Title = "Tellus Tortor Consectetur";
                page.Header.SubTitle = "Ultricies Mattis Malesuada Purus";
                page.Header.Background = heroId;
                page.Header.Body = "<p>Integer posuere erat a ante venenatis dapibus posuere velit aliquet. Nullam id dolor id nibh ultricies vehicula ut id elit.</p>";
                page.Published = DateTime.Now;

                api.Pages.Save(page);
            }
        }
    }
}
