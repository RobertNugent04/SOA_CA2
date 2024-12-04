using Microsoft.Extensions.Logging;
using SOA_CA2.Infrastructure;
using SOA_CA2.Interfaces;
using SOA_CA2.Repositories;

namespace SOA_CA2
{
    /// <summary>
    /// Implements the Unit of Work pattern for managing repositories and database transactions.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">Database context.</param>
        /// <param name="loggerFactory">Factory for creating logger instances.</param>
        public UnitOfWork(AppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            Users = new UserRepository(context, loggerFactory.CreateLogger<UserRepository>());
        }

        /// <inheritdoc />
        public IUserRepository Users { get; }

        /// <inheritdoc />
        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving changes to the database.", ex);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
