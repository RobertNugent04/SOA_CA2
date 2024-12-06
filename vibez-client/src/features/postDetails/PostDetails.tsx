import React, { useState, useEffect } from "react";
import "./postDetails.css";
import "../posts/posts.css";
import back from "../../assets/images/back.png";
import send from "../../assets/images/send.png";
import { Posts } from "../posts/Posts.tsx";
import black_like from "../../assets/images/black_like.png";
import blue_like from "../../assets/images/blue_like.png";
import { getPostRequest } from "../../api/getPostRequest.ts";  
import API_BASE_URL from "../../api/apiConsts.ts";

export const PostDetails = ({ postId, token }) => {
  const [goBack, setGoBack] = useState(false);
  const [likes, setLikes] = useState<{ [key: number]: number }>({});
  const [likeImages, setLikeImages] = useState<{ [key: number]: string }>({});
  const [post, setPost] = useState<any>(null); // State to store the fetched post data
  const [loading, setLoading] = useState<boolean>(true); // State for loading state
  const [error, setError] = useState<string | null>(null); // State for error message

  const comments = [
    {
      id: 1,
      postId: 1,
      user: {
        name: "Alice Johnson",
        profilePic: "https://via.placeholder.com/40",
      },
      content: "Great post! I really enjoyed reading it.",
      date: "Dec 2, 2024",
    },
    {
      id: 2,
      postId: 1,
      user: {
        name: "Michael Brown",
        profilePic: "https://via.placeholder.com/40",
      },
      content: "Thanks for sharing your thoughts!",
      date: "Dec 3, 2024",
    },
    {
      id: 3,
      postId: 2,
      user: {
        name: "Emily Davis",
        profilePic: "https://via.placeholder.com/40",
      },
      content: "Sounds like a wonderful experience!",
      date: "Dec 5, 2024",
    },
  ];

  // Fetch the post data from the API when the component mounts or when postId changes
  useEffect(() => {
    const fetchPost = async () => {
      setLoading(true);
      try {
        // Fetch post data
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
  }, [postId, token]); // Run the effect when postId or token changes

  if (loading) return <p>Loading post...</p>;
  if (error) return <p>Error: {error}</p>;

  if (!post) {
    return <div>Post not found.</div>;
  }

  if (goBack) {
    // Render the Posts component if back arrow is clicked
    return <Posts />;
  }

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

  const getPostImageUrl = (imageUrl: string | null) =>
    imageUrl ? `${API_BASE_URL}${imageUrl}` : undefined;


  return (
    <div className="post-details-container">
      <img
        src={back}
        alt="Back"
        className="back"
        onClick={() => setGoBack(true)}
      />
      <div className="post-details-header">
        <div className="post-header">
          <img
            src={getPostImageUrl(post.profilePicturePath) || "https://via.placeholder.com/40"}
            alt={post.userName}
            className="post-profile-pic"
          />
          <div className="post-author-date">
            <span className="post-author">{post.userName}</span>
            <span className="post-date">· {new Date(post.createdAt).toLocaleDateString()}</span>
          </div>
        </div>
        <div className="post-content">{post.content}</div>
        {post.imageUrl && (
          <img src={getPostImageUrl(post.imageUrl)} alt="Post" className="post-image" />
        )}
        <div className="post-actions">
          <div className="comment-input-container">
            <input type="text" placeholder="Leave a comment..." />
            <img src={send} alt="Send" className="send-button" />
          </div>
          <div className="like-button" onClick={() => handleLike(post.postId)}>
            <img src={likeImages[post.postId] || black_like} alt="Like" />
            {likes[post.postId] || 0}
          </div>
        </div>
      </div>

      {/* Display the comments */}
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
