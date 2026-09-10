using Infra.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Context
{
    public class ImageContext : DbContext
    {
        public DbSet<Image> Image { get; set; }

    }
}
