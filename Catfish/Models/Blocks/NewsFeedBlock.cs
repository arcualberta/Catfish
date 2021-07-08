using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    public enum FeedSource
    {
        [Display(Description = "Twitter")]
        Twitter,
        [Display(Description = "Facebook")]
        Facebook
    }
    public enum EmbedOption
    {
        [Display(Description = "Embed")]
        Embed,
        [Display(Description = "Shared Button")]
        SharedButton
    }

    public enum ButtonPosition
    {
        [Display(Description = "Left")]
        Left,
        [Display(Description = "Bottom")]
        Bottom,
        [Display(Description = "Right")]
        Right,
        [Display(Description = "Top")]
        Top
    }

    [BlockType(Name = "News Feed Block", Category = "Content", Component = "news-feed-block-vue", Icon = "fas fa-newspaper")]
    public class NewsFeedBlock : VueComponent
    {
        [Display(Description = "For Twitter: go to https://publish.twitter.com/, enter your twitter url you want to embed.")]
        public TextField ReferenceUrl { get; set; }
        public TextField BlockTitle { get; set; }
        public SelectField<FeedSource> NewsSource { get; set; }
        public SelectField<EmbedOption> EmbedOption{ get; set; }
    }
}
