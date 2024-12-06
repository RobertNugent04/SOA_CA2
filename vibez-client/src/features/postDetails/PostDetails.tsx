import React, { useState, useEffect } from "react";
import "./postDetails.css";
import "../posts/posts.css";
import back from "../../assets/images/back.png";
import send from "../../assets/images/send.png";
import { Posts } from "../posts/Posts.tsx";
import black_like from "../../assets/images/black_like.png";
import blue_like from "../../assets/images/blue_like.png";
import { getPostRequest } from "../../api/Posts/getPostRequest.ts";
import { createCommentRequest } from "../../api/Comments/createCommentRequest.ts"; // Import the createCommentRequest
import API_BASE_URL from "../../api/apiConsts.ts";

export const PostDetails = ({ postId, token }) => {
  const [goBack, setGoBack] = useState(false);
  const [likes, setLikes] = useState<{ [key: number]: number }>({});
  const [likeImages, setLikeImages] = useState<{ [key: number]: string }>({});
  const [post, setPost] = useState<any>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [commentText, setCommentText] = useState<string>(""); // State for the new comment input
  const [comments, setComments] = useState<any[]>([]); // State to store all comments for the post

  useEffect(() => {
    const fetchPost = async () => {
      setLoading(true);
      try {
        const response = await getPostRequest(postId, token);
        if (response.success) {
          setPost(response.data);
        } else {
          setError(response.error || "Failed to fetch post.");
        }
      } catch (err) {
        setError("An error occurred while fetching the post.");
      } finally {
        setLoading(false);
      }
    };

    fetchPost();
  }, [postId, token]);

  const handleLike = (postId: number) => {
    setLikes((prevLikes) => ({
      ...prevLikes,
      [postId]: (prevLikes[postId] || 0) + 1,
    }));
    setLikeImages((prevLikeImages) => ({
      ...prevLikeImages,
      [postId]:
        (prevLikeImages[postId] || black_like) === black_like
          ? blue_like
          : black_like,
    }));
  };

  const handleCommentSubmit = async () => {
    if (!commentText.trim()) return; // Prevent empty comments

    const payload = { content: commentText, postId };
    const response = await createCommentRequest(payload, token);

    if (response.success) {
      // Update comments locally without refetching
      setComments((prevComments) => [
        ...prevComments,
        {
          id: Date.now(), // Temporary ID
          postId,
          user: { name: "You", profilePic: "https://via.placeholder.com/40" },
          content: commentText,
          date: new Date().toLocaleDateString(),
        },
      ]);
      setCommentText(""); // Clear the input field
    } else {
      console.error("Failed to post comment:", response.message);
    }
  };

  const getPostImageUrl = (imageUrl: string | null) =>
    imageUrl ? `${API_BASE_URL}${imageUrl}` : undefined;

  if (loading) return <p>Loading post...</p>;
  if (error) return <p>Error: {error}</p>;
  if (!post) return <div>Post not found.</div>;

  if (goBack) {
    return <Posts />;
  }

  return (
    <div className="post-details-container">
      <img src={back} alt="Back" className="back" onClick={() => setGoBack(true)} />
      <div className="post-details-header">
        <div className="post-header">
          <img
            src={getPostImageUrl(post.profilePicturePath) || "https://via.placeholder.com/40"}
            alt={post.userName}
            className="post-profile-pic"
          />
          <div className="post-author-date">
            <span className="post-author">{post.userName}</span>
            <span className="post-date">
              · {new Date(post.createdAt).toLocaleDateString()}
            </span>
          </div>
        </div>
        <div className="post-content">{post.content}</div>
        {post.imageUrl && (
          <img src={getPostImageUrl(post.imageUrl)} alt="Post" className="post-image" />
        )}
        <div className="post-actions">
          <div className="comment-input-container">
            <input
              type="text"
              placeholder="Leave a comment..."
              value={commentText}
              onChange={(e) => setCommentText(e.target.value)} // Update comment text
            />
            <img
              src={send}
              alt="Send"
              className="send-button"
              onClick={handleCommentSubmit} // Submit comment on click
            />
          </div>
          <div className="like-button" onClick={() => handleLike(post.postId)}>
            <img src={likeImages[post.postId] || black_like} alt="Like" />
            {likes[post.postId] || 0}
          </div>
        </div>
      </div>

      <div className="comments-container">
        <h3 className="comments-header">Comments</h3>
        {comments
          .filter((comment) => comment.postId === postId)
          .map((comment) => (
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
