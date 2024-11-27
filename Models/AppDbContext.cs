using Microsoft.EntityFrameworkCore;

namespace SOA_CA2.Models
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}


		public DbSet<User> Users { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Like> Likes { get; set; }
		public DbSet<Friend> Friend { get; set; }
		public DbSet<Message> Message { get; set; }
		public DbSet<Notification> Notification { get; set; }

	}
}