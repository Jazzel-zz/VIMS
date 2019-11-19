using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VIMS.Models;

namespace WebApplication19.Models
{
    public class AnotherDbContext : DbContext
    {
        public AnotherDbContext()
           : base("Connection")
        {
        }
       

    }
}