namespace SOA_CA2.Interfaces
{
    /// <summary>
    /// Unit of Work interface for coordinating multiple repository operations.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }

        IPostRepository Posts { get; }

        ICommentRepository Comments { get; }

        ILikeRepository Likes { get; }

        IFriendshipRepository Friendships { get; }
        Task SaveChangesAsync();
    }
}
