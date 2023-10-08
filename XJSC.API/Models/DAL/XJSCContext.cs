using Microsoft.EntityFrameworkCore;
using XJSC.API.Models.EN;

namespace XJSC.API.Models.DAL
{
    public class XJSCContext : DbContext
    {
        public XJSCContext(DbContextOptions<XJSCContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
    }
}
