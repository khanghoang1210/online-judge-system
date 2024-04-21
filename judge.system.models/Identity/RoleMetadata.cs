﻿using judge.system;
using System.ComponentModel.DataAnnotations;

namespace StarOJ.Core.Identity
{
    public class RoleMetadata : IHasId<string>
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string NormalizedName { get; set; }
    }
}
