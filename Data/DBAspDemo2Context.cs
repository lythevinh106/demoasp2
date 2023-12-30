using demoAsp2.Constacts;
using demoAsp2.Models;
using demoAsp2.Models.Category.Module;
using demoAsp2.Models.CategoryDto.Category.Response;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace demoAsp2.Data
{
    public class DBAspDemo2Context : IdentityDbContext<ApplicationUser>
    {

        public DBAspDemo2Context(DbContextOptions<DBAspDemo2Context> options) : base(options)
        {


        }


        public DbSet<Category> categories { get; set; }

        public DbSet<Product> products { get; set; }

        public DbSet<ProductOrder> productOrders { get; set; }

        public DbSet<Order> orders { get; set; }

        public DbSet<UserRefreshToken> refreshToken { get; set; }


        public DbSet<CategoryResponseDto> catRp { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.id);
                entity.HasMany(c => c.products).WithOne(p => p.category);



            });


            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.id);

                //relative category
                entity.HasOne(p => p.category)
                .WithMany(p => p.products).HasForeignKey(p => p.categoryId);

                // relative producrOrder
                entity.HasMany(p => p.productOrders)
                .WithOne(po => po.product);
            });

            modelBuilder.Entity<ProductOrder>(entity =>
            {
                entity.HasKey(po => new { po.productId, po.orderId });

                entity.HasOne(po => po.product)
                .WithMany(p => p.productOrders)
                .HasForeignKey(po => po.productId);

                entity.HasOne(po => po.order)
               .WithMany(o => o.productOrders).HasForeignKey(po => po.orderId);


            });


            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.id);


                //relative user
                //entity.HasOne(o => o.user)
                //.WithMany(u => u.orders).HasForeignKey(o => o.userId);

                // relative producrOrder
                entity.HasMany(o => o.productOrders)
                .WithOne(po => po.order);


            });

            //1-1
            modelBuilder.Entity<UserRefreshToken>(entity =>
            {
                entity.HasKey(urt => urt.Id);

                entity.HasOne<ApplicationUser>(urt => urt.user)
                .WithOne(u => u.refreshToken)
                .HasForeignKey<UserRefreshToken>(urt => urt.UserId);




            });


            //modelBuilder.Entity<User>(entity =>
            //{
            //    entity.HasKey(u => u.id);
            //    entity.HasMany(u => u.orders)
            //    .WithOne(o => o.user);


            //});



            ///seed

            var entities = typeof(ISeedable).Assembly.GetTypes().Where(x => typeof(ISeedable).IsAssignableFrom(x))
                                .Where(x => !x.IsInterface).ToList();

            foreach (var m in entities)
            {
                var entity = modelBuilder.Entity(m);

                Console.WriteLine(m);

                if (typeof(ISeedable).IsAssignableFrom(m))
                {
                    var inst = Activator.CreateInstance(m) as ISeedable;
                    var data = inst?.Seed();

                    foreach (var d in data)
                        entity.HasData(d);
                }
            }













        }
    }
}
