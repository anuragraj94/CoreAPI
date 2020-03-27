using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class DoctorDBContex:DbContext
    {
        public DoctorDBContex(DbContextOptions<DoctorDBContex> options):base(options)
        {

        }
        public DbSet<CandidateModel> CandidateModels { get; set; }
    }
}
