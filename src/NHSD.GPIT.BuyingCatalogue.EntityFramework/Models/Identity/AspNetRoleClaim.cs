﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

#nullable disable

namespace NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.Identity
{    
    [Table("AspNetRoleClaims")]
    public partial class AspNetRoleClaim : IdentityRoleClaim<string>
    {
        public virtual AspNetRole Role { get; set; }
    }
}