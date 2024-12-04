import React, { useState } from 'react';
import './posts.css';
import black_like from '../../assets/images/black_like.png';
import blue_like from '../../assets/images/blue_like.png';
import send from '../../assets/images/send.png';
import { PostDetails } from '../postDetails/PostDetails.tsx';

export const Posts = () => {

    const [comments, setComments] = useState({});
    const [likes, setLikes] = useState({});
    const [likeImages, setLikeImages] = useState({});
    const [selectedPostId, setSelectedPostId] = useState<number | null>(null);


  const posts = [
    {
      id: 1,
      author: 'Wocc',
      profilePic: 'https://i.pinimg.com/736x/14/e1/c7/14e1c7bf5162b659c62dd57250373c07.jpg', // Replace with actual image URL
      date: 'Nov 29',
      content: "I think we're better off as friends",
      imageUrl: 'https://www.adobe.com/uk/products/firefly/features/media_179810889bf1ef34a453137e0387dd9e0f4e43f05.jpeg?width=750&format=jpeg&optimize=medium'
    },
    {
      id: 2,
      author: 'John Doe',
      profilePic: 'https://i.pinimg.com/736x/14/e1/c7/14e1c7bf5162b659c62dd57250373c07.jpg', // Replace with actual image URL
      date: 'Dec 5',
      content: "Had a great weekend camping!",
      imageUrl: 'https://www.bing.com/th?id=OIP.jmMrTb1h_-ITFtCqj9mXFwHaJQ&w=86&h=106&c=8&rs=1&qlt=90&o=6&dpr=1.3&pid=3.1&rm=2'
    },
    {
      id: 3,
      author: 'Jane Smith',
      profilePic: 'https://i.pinimg.com/736x/14/e1/c7/14e1c7bf5162b659c62dd57250373c07.jpg', // Replace with actual image URL
      date: 'Dec 7',
      content: 'Loving the new book I’m reading!',
      imageUrl: 'https://www.adobe.com/uk/products/firefly/features/media_179810889bf1ef34a453137e0387dd9e0f4e43f05.jpeg?width=750&format=jpeg&optimize=medium'
    }
  ];

  const handleSendComment = (postId) => {
    // Implement sending a comment
    console.log(`Comment sent for post ${postId}`);
  };

  const handleComment = (postId, comment) => {
    setComments((prevComments) => ({
      ...prevComments,
      [postId]: comment,
    }));
  };

  const handleLike = (postId) => {
    setLikes((prevLikes) => ({
      ...prevLikes,
      [postId]: (prevLikes[postId] || 0) + 1,
    }));
    setLikeImages((prevLikeImages) => ({
        ...prevLikeImages,
        [postId]: (prevLikeImages[postId] || black_like) === black_like ? blue_like : black_like,
      }));
  };
  
  if (selectedPostId !== null) {
    return <PostDetails postId={selectedPostId} posts={posts} />; // Render PostDetails if a post is clicked
  }

    return (
      <div className="posts-container">
        <h2 className="posts-header">Posts</h2>
        {posts.map((post) => (
          <div key={post.id} className="post-item" onClick={() => setSelectedPostId(post.id)}>
            <div className="post-header">
              <div className="post-author-info">
                <img
                  src={post.profilePic}
                  alt={post.author}
                  className="post-profile-pic"
                />
                <div className="post-author-date">
                  <span className="post-author">{post.author}</span>
                  <span className="post-date">· {post.date}</span>
                </div>
              </div>
            </div>
            <div className="post-content">{post.content}</div>
            <img src={post.imageUrl} alt={post.author} className="post-image" />
            <div className="post-actions">
            <div className="comment-input-container">
            <input
            type="text"
            placeholder="Leave a comment..."
            value={comments[post.id] || ''}
            onChange={(e) => handleComment(post.id, e.target.value)}
            />
            <img
            src={send}
            alt="Send"
            className="send-button"
            onClick={() => handleSendComment(post.id)}
            />
        </div>
        <div className="like-button" onClick={() => handleLike(post.id)}>
            <img
            src={likeImages[post.id] || black_like}
            alt="Like"
            />
            {likes[post.id] || 0}
        </div>
        </div>

          </div>
        ))}
      </div>
    );
  };
