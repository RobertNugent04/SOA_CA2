import React, { useState, useEffect } from "react";
import "./postDetails.css";
import "../posts/posts.css";
import back from "../../assets/images/back.png";
import send from "../../assets/images/send.png";
import { Posts } from "../posts/Posts.tsx";
import black_like from "../../assets/images/black_like.png";
import blue_like from "../../assets/images/blue_like.png";
import { getPostRequest } from "../../api/Posts/getPostRequest.ts";
import { createCommentRequest } from "../../api/Comments/createCommentRequest.ts";
import { getCommentsRequest } from "../../api/Comments/getCommentsRequest.ts"; // Import API to fetch comments
import { likePostRequest } from "../../api/Likes/sendLikeRequest.ts"; // Import the like API
import API_BASE_URL from "../../api/apiConsts.ts";

export const PostDetails = ({ postId, token }) => {
  const [goBack, setGoBack] = useState(false);
  const [likes, setLikes] = useState<{ [key: number]: number }>({});
  const [likeImages, setLikeImages] = useState<{ [key: number]: string }>({});
  const [post, setPost] = useState<any>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [commentText, setCommentText] = useState<string>("");
  const [comments, setComments] = useState<any[]>([]);

  // Fetch post details
  useEffect(() => {
    const fetchPost = async () => {
      setLoading(true);
      try {
        const response = await getPostRequest(postId, token);
        if (response.success) {
          setPost(response.data);
          setLikes({ [postId]: 0 }); // Initialize likes count
          setLikeImages({ [postId]: black_like }); // Default like image
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

  // Fetch comments for the current post
  useEffect(() => {
    const fetchComments = async () => {
      try {
        const response = await getCommentsRequest(postId, token);
        if (response.success) {
          setComments(response.data); // Populate comments from API
        } else {
          console.error("Failed to fetch comments:", response.error);
        }
      } catch (err) {
        console.error("An error occurred while fetching comments:", err);
      }
    };

    fetchComments();
  }, [postId, token]);

  const handleLike = async (postId: number) => {
    try {
      // Make the API request to like the post
      const response = await likePostRequest({ postId }, token);

      if (response.success) {
        // Update the UI based on like status
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
      } else {
        console.error("Failed to like post:", response.error);
      }
    } catch (error) {
      console.error("An error occurred while liking the post:", error);
    }
  };

  const handleCommentSubmit = async () => {
    if (!commentText.trim()) return;

    const payload = { content: commentText, postId };
    const response = await createCommentRequest(payload, token);

    if (response.success) {
      console.log("Comment recieved successfully:", response);
      // Append the new comment to the comments array
      setComments((prevComments) => [
        ...prevComments,
        {
          commentId: response.data.id,
          postId,
          content: commentText,
          createdAt: new Date().toISOString(),
          userId: 0, // Placeholder for userId
          user: { name: "You", profilePic: "https://via.placeholder.com/40" },
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
    return <Posts isUserPage={false} userId={0} token={token} />;
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
        {comments.map((comment) => (
          <div key={comment.commentId} className="comment-item">
            <img
              src={getPostImageUrl(comment.profilePicturePath) || "https://via.placeholder.com/40"}
              alt={comment?.userName || "User"}
              className="comment-profile-pic"
            />
            <div className="comment-content-wrapper">
              <div className="comment-author-date">
                <span className="comment-author">{comment.userName}</span>
                <span className="comment-date">
                  {new Date(comment.createdAt).toLocaleDateString()}
                </span>
              </div>
              <div className="comment-content">{comment.content}</div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};
