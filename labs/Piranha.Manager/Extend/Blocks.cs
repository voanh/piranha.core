using Piranha.Extend;
using Piranha.Extend.Blocks;
using Piranha.Extend.Fields;

namespace Piranha.Manager.ExtraBlocks
{
    [BlockType(Name = "Slider Item", Category = "Content", Icon = "fas fa-cubes", IsUnlisted = true)]
    public class SliderItemBlock : Block
    {
        public ImageField BackgroundImage { get; set; }
        public StringField Title { get; set; }
        public HtmlField SliderContent { get; set; }

        public override string GetTitle(Piranha.IApi api)
        {
            if (!string.IsNullOrEmpty(Title.Value))
            {
                return Title.Value;
            }
            return "Empty Item";
        }
    }

    [BlockGroupType(Name = "Slider", Category = "Groups", Icon = "fas fa-cubes")]
    [BlockItemType(Type = typeof(SliderItemBlock))]
    public class SliderBlock : BlockGroup
    {
    }

    [BlockGroupType(Name = "Gallery", Category = "Groups", Icon = "fas fa-images")]
    [BlockItemType(Type = typeof(ImageBlock))]
    public class GalleryBlock : BlockGroup
    {
        /// <summary>
        /// Main gallery title.
        /// </summary>
        public StringField Title { get; set; }

        /// <summary>
        /// Optional description.
        /// </summary>
        public HtmlField Description { get; set; }
    }
}