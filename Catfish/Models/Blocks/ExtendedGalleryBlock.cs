﻿using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.Extend.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    [BlockGroupType(Name = "ExtendedGallery", Category = "Media", Icon = "fas fa-images")]
    [BlockItemType(Type = typeof(ExtendedImageBlock))]
    public class ExtendedGalleryBlock : BlockGroup
    {
    }
}
