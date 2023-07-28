using GlamourHub.Models;
using GlamourHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GlamourHub.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        


    }
}
