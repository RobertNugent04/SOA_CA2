import React, { useEffect, useState } from "react";
import "./posts.css";
import black_like from "../../assets/images/black_like.png";
import blue_like from "../../assets/images/blue_like.png";
import send from "../../assets/images/send.png";
import { PostDetails } from "../postDetails/PostDetails.tsx";
import { getProfileRequest } from "../../api/getProfile.ts";
import API_BASE_URL from "../../api/apiConsts.ts";
import profilePic from "../../assets/images/default_pfp.png";

type UserProfile = {
  userId: number;
  fullName: string;
  userName: string;
  email: string;
  bio: string | null;
  profilePicturePath: string | null;
  createdAt: string;
  isActive: boolean;
};

type Post = {
  postId: number;
  userId: number;
  content: string;
  imageUrl: string | null;
  createdAt: string;
};

type Friend = {
  friendshipId: number;
  userId: number;
  friendId: number;
  status: string;
};

type UserProfileResponse = {
  user: UserProfile;
  posts: Post[];
  friends: Friend[];
};

type PostsProps = {
  isUserPage: boolean;
  userId: number;
  token: string;
};

export const Posts: React.FC<PostsProps> = ({ isUserPage, userId, token }) => {
  const [userProfile, setUserProfile] = useState<UserProfileResponse | null>(null);
  const [posts, setPosts] = useState<Post[]>([]);
  const [comments, setComments] = useState({});
  const [likes, setLikes] = useState({});
  const [likeImages, setLikeImages] = useState({});
  const [selectedPostId, setSelectedPostId] = useState<number | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchPosts = async () => {
      setLoading(true);
      try {
        const response = await getProfileRequest(token, userId);
        if (response.success) {
          const profileData = response.data!;
          setPosts(isUserPage ? profileData.posts : []); // Only set user posts for user pages
          setUserProfile(response.data!);
        } else {
          setError(response.error || "Failed to fetch posts.");
        }
      } catch (err) {
        setError("An error occurred while fetching posts.");
      } finally {
        setLoading(false);
      }
    };

    if (isUserPage) {
      fetchPosts();
    }
  }, [isUserPage, token, userId]);

  if (loading) return <p>Loading posts...</p>;
  if (error) return <p>Error: {error}</p>;
  if (!posts.length) return <p>No posts available.</p>;

  if (selectedPostId !== null) {
    return <PostDetails postId={selectedPostId} posts={posts} />;
  }

  const { user, friends } = userProfile || { user: {}, friends: [] };

  const profilePictureUrl = user.profilePicturePath ? `${API_BASE_URL}${user.profilePicturePath}` : profilePic;
  console.log("Profile Picture URL:", profilePictureUrl);

  const getPostImageUrl = (imageUrl: string | null) =>
    imageUrl ? `${API_BASE_URL}${imageUrl}` : undefined;

  console.log("Post url: ", getPostImageUrl(posts[0].imageUrl));

  return (
    <div className="posts-container">
      <h2 className="posts-header">{isUserPage ? "User Posts" : "Posts"}</h2>
      {posts.map((post) => (
        <div
          key={post.postId}
          className="post-item"
          onClick={() => setSelectedPostId(post.postId)}
        >
          <div className="post-header">
            <div className="post-author-info">
              <img
                src={profilePictureUrl || ""}
                alt={`Post by user ${post.userId}`}
                className="post-profile-pic"
              />
              <div className="post-author-date">
                <span className="post-author">{user.userName}</span>
                <span className="post-date">
                  · {new Date(post.createdAt).toLocaleDateString()}
                </span>
              </div>
            </div>
          </div>
          <div className="post-content">{post.content}</div>

            <img src={getPostImageUrl(post.imageUrl)} alt="Post" className="post-image" />

          <div className="post-actions">
            <div className="comment-input-container">
              <input
                type="text"
                placeholder="Leave a comment..."
                value={comments[post.postId] || ""}
                onChange={(e) =>
                  setComments((prev) => ({
                    ...prev,
                    [post.postId]: e.target.value,
                  }))
                }
              />
              <img
                src={send}
                alt="Send"
                className="send-button"
                onClick={() =>
                  console.log(`Comment sent for post ${post.postId}`)
                }
              />
            </div>
            <div
              className="like-button"
              onClick={() =>
                setLikes((prev) => ({
                  ...prev,
                  [post.postId]: (prev[post.postId] || 0) + 1,
                }))
              }
            >
              <img
                src={likeImages[post.postId] || black_like}
                alt="Like"
                onClick={() =>
                  setLikeImages((prev) => ({
                    ...prev,
                    [post.postId]:
                      (prev[post.postId] || black_like) === black_like
                        ? blue_like
                        : black_like,
                  }))
                }
              />
              {likes[post.postId] || 0}
            </div>
          </div>
        </div>
      ))}
    </div>
  );
};
