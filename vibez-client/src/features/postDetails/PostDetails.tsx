import React, { useState } from 'react';
import './postDetails.css';
import '../posts/posts.css';
import back from '../../assets/images/back.png';
import send from '../../assets/images/send.png';
import { Posts } from '../posts/Posts.tsx';
import black_like from '../../assets/images/black_like.png';
import blue_like from '../../assets/images/blue_like.png';

export const PostDetails = ({ postId, posts }) => {
  const [goBack, setGoBack] = useState(false);
  const [likes, setLikes] = useState({});
  const [likeImages, setLikeImages] = useState({});

  const comments = [
    {
      id: 1,
      postId: 1,
      user: {
        name: 'Alice Johnson',
        profilePic: 'https://via.placeholder.com/40',
      },
      content: 'Great post! I really enjoyed reading it.',
      date: 'Dec 2, 2024',
    },
    {
      id: 2,
      postId: 1,
      user: {
        name: 'Michael Brown',
        profilePic: 'https://via.placeholder.com/40',
      },
      content: 'Thanks for sharing your thoughts!',
      date: 'Dec 3, 2024',
    },
    {
      id: 3,
      postId: 2,
      user: {
        name: 'Emily Davis',
        profilePic: 'https://via.placeholder.com/40',
      },
      content: 'Sounds like a wonderful experience!',
      date: 'Dec 5, 2024',
    },
  ];

  const post = posts.find((p) => p.id === postId);
  const filteredComments = comments.filter((comment) => comment.postId === postId);

  if (!post) {
    return <div>Post not found.</div>;
  }

  if (goBack) {
    // Render the MessageTab component if back arrow is clicked
    return <Posts />;
  }

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

  return (
    <div className="post-details-container">
      <img src={back} alt="Back" className="back" onClick={() => setGoBack(true)}/>
      <div className="post-details-header">
        <div className="post-header">
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
        <div className="post-content">{post.content}</div>
        {post.imageUrl && (
          <img src={post.imageUrl} alt="Post" className="post-image" />
        )}
        <div className="post-actions">
            <div className="comment-input-container">
            <input
            type="text"
            placeholder="Leave a comment..."
            />
            <img
            src={send}
            alt="Send"
            className="send-button"
            // onClick={() => handleSendComment(post.id)}
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

      {/* Display the comments */}
      <div className="comments-container">
        <h3 className="comments-header">Comments</h3>
        {filteredComments.map((comment) => (
          <div key={comment.id} className="comment-item">
            <img
              src={comment.user.profilePic}
              alt={comment.user.name}
              className="comment-profile-pic"
            />
            <div className="comment-content-wrapper">
              <div className="comment-author-date">
                <span className="comment-author">{comment.user.name}</span>
                <span className="comment-date">{comment.date}</span>
              </div>
              <div className="comment-content">{comment.content}</div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};
