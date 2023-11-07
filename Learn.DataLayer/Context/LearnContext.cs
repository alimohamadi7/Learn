using Learn.DataLayer.Entities.Course;
using Learn.DataLayer.Entities.Order;
using Learn.DataLayer.Entities.Permissions;
using Learn.DataLayer.Entities.User;
using Learn.DataLayer.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Learn.DataLayer.Context
{
    public class LearnContext : DbContext
    {
        public LearnContext(DbContextOptions<LearnContext> options) : base(options)
        {

        }
        #region User

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserDiscountCode> UserDiscountCodes { get; set; }
       

        #endregion
        #region Wallet

        public DbSet<WalletType> WalletTypes { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        #endregion
        #region Permission

        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }

        #endregion
        #region Course
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseGroup> CourseGroups { get; set; }
        public DbSet<CourseLevel> CourseLevels { get; set; }
        public DbSet<CourseStatus> CourseStatuses { get; set; }
        public DbSet<CourseEpisode> CourseEpisodes { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<CourseComment> CourseComments { get; set; }
        #endregion
        #region Order

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        //    modelBuilder.Entity<OrderDetail>()
        //    .HasOne(e => e.Order).WithMany(p => p.OrderDetails)
        //     .OnDelete(DeleteBehavior.Restrict);
        //    modelBuilder.Entity<UserCourse>()
        //    .HasOne(e => e.Course).WithMany(p => p.UserCourses)
        //     .OnDelete(DeleteBehavior.Restrict);
        //    modelBuilder.Entity<CourseComment>()
        //   .HasOne(e => e.Course).WithMany(p => p.CourseComments)
        //    .OnDelete(DeleteBehavior.Restrict);
        //    modelBuilder.Entity<CourseComment>()
        //.HasOne(e => e.User).WithMany(p => p.CourseComments)
        // .OnDelete(DeleteBehavior.Restrict);

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
         .SelectMany(t => t.GetForeignKeys())
         .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDelete);

            base.OnModelCreating(modelBuilder);
        }
    }
}
