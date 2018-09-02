using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AutoMapper;
using Piranha.Extend;
using Piranha.Models;

namespace Piranha.Manager
{
    public class Module : IModule
    {
        /// <summary>
        /// Gets the Author
        /// </summary>
        public string Author => "Piranha";

        /// <summary>
        /// Gets the Name
        /// </summary>
        public string Name => "Piranha.Manager";

        /// <summary>
        /// Gets the Version
        /// </summary>
        public string Version => Piranha.Utils.GetAssemblyVersion(this.GetType().Assembly);

        /// <summary>
        /// Gets the release date
        /// </summary>
        public string ReleaseDate => "2018-05-30";

        /// <summary>
        /// Gets the description
        /// </summary>
        public string Description => "Manager panel for Piranha CMS for AspNetCore.";

        /// <summary>
        /// Gets the package url.
        /// </summary>
        public string PackageURL => "https://www.nuget.org/packages/Piranha.Manager";

        /// <summary>
        /// The currently registered custom scripts.
        /// </summary>
        public List<string> Scripts { get; private set; }

        /// <summary>
        /// The currently registered custom styles.
        /// </summary>
        public List<string> Styles { get; private set; }

        /// <summary>
        /// Gets the automapper for the module.
        /// </summary>
        public static IMapper Mapper { get; private set; }

        /// <summary>
        /// The assembly.
        /// </summary>
        internal static Assembly Assembly;

        /// <summary>
        /// Last modification date of the assembly.
        /// </summary>
        internal static DateTime LastModified;

        /// <summary>
        /// Initializes the module.
        /// </summary>
        public void Init() {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<PageBase, Models.PageEditModel>()
                    .ForMember(m => m.PageType, o => o.Ignore())
                    .ForMember(m => m.Blocks, o => o.Ignore())
                    .ForMember(m => m.Regions, o => o.Ignore())
                    .ForMember(m => m.PageContentType, o => o.Ignore())
                    .ForMember(m => m.PinnedRegions, o => o.Ignore());
                cfg.CreateMap<Models.PageEditModel, PageBase>()
                    .ForMember(m => m.Blocks, o => o.Ignore())
                    .ForMember(m => m.TypeId, o => o.Ignore())
                    .ForMember(m => m.Created, o => o.Ignore())
                    .ForMember(m => m.LastModified, o => o.Ignore());
            });

            config.AssertConfigurationIsValid();
            Mapper = config.CreateMapper();

            // Register permissions
            //foreach (var permission in _permissions)
            //{
            //    App.Permissions["Manager"].Add(permission);
            //}

            // Get assembly information
            Assembly = this.GetType().GetTypeInfo().Assembly;
            LastModified = new FileInfo(Assembly.Location).LastWriteTime;
        }        
    }
}