﻿using Microsoft.Extensions.Logging;
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
            Posts = new PostRepository(context, loggerFactory.CreateLogger<PostRepository>());
            Comments = new CommentRepository(context, loggerFactory.CreateLogger<CommentRepository>());
            Likes = new LikeRepository(context, loggerFactory.CreateLogger<LikeRepository>());
            Friendships = new FriendshipRepository(context, loggerFactory.CreateLogger<FriendshipRepository>());
            Messages = new MessageRepository(context, loggerFactory.CreateLogger<MessageRepository>());
            Notifications = new NotificationRepository(context, loggerFactory.CreateLogger<NotificationRepository>());
            Calls = new CallRepository(context, loggerFactory.CreateLogger<CallRepository>());
        }

        /// <inheritdoc />
        public IUserRepository Users { get; }

        /// <inheritdoc />
        public IPostRepository Posts { get; }

        /// <inheritdoc />
        public ICommentRepository Comments { get; }

        /// <inheritdoc />
        public ILikeRepository Likes { get; }

        /// <inheritdoc />
        public IFriendshipRepository Friendships { get; }

        /// <inheritdoc />
        public IMessageRepository Messages { get; }

        /// <inheritdoc />
        public INotificationRepository Notifications { get; }

        public ICallRepository Calls { get; }

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
