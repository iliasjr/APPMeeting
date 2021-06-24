using API.entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class Data : DbContext
    {
        public Data(DbContextOptions options) : base(options)
        {
           
        }
        public DbSet<Utlisateurs> utlisateurs { get; set; }
    }
}
