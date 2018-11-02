using Piranha.Extend;
using Piranha.Extend.Fields;

namespace Piranha.Manager.ExtraBlocks
{
    [BlockType(Name = "Slider Item", Category = "Content", Icon = "fas fa-cubes")]
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
}