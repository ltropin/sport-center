using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SportCenter.Data
{
    public class SportContext : IdentityDbContext
    {
        public SportContext(DbContextOptions<SportContext> options)
            : base(options)
        {
        }
    }
}
