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
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Users = new UserRepository(context);
        }

        /// <inheritdoc />
        public IUserRepository Users { get; }

        /// <inheritdoc />
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
