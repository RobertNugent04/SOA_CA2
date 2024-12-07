import React, { useEffect, useState } from "react";
import { getProfileRequest } from "../../api/Users/getProfile.ts";
import { sendFriendRequest } from "../../api/Friends/sendFriendRequest.ts"; // Import the sendFriendRequest API
import "./userCard.css";
import profilePic from "../../assets/images/default_pfp.png";
import { EditUser } from "./EditUser.tsx";
import API_BASE_URL from "../../api/apiConsts.ts";

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

type UserCardProps = {
  token: string;
  userId: number;
};

export const UserCard: React.FC<UserCardProps> = ({ token, userId }) => {
  const [userProfile, setUserProfile] = useState<UserProfileResponse | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [isRequesting, setIsRequesting] = useState<boolean>(false); // State to handle request status
  const [requestStatus, setRequestStatus] = useState<string | null>(null); // State for request feedback

  useEffect(() => {
    const fetchUser = async () => {
      setLoading(true);
      try {
        const response = await getProfileRequest(token, userId);
        if (response.success) {
          setUserProfile(response.data!);
        } else {
          setError(response.error || "Failed to fetch user data.");
        }
      } catch (err) {
        setError("An error occurred while fetching the profile.");
      } finally {
        setLoading(false);
      }
    };

    fetchUser();
  }, [token, userId]);

  const handleSendFriendRequest = async () => {
    if (isRequesting) return; // Prevent multiple requests
    setIsRequesting(true);
    setRequestStatus(null);

    try {
      const response = await sendFriendRequest({ friendId: userId }, token);
      if (response.success) {
        setRequestStatus("Friend request sent successfully!");
      } else {
        setRequestStatus("Failed to send friend request.");
        console.error("Friend Request Error:", response.error);
      }
    } catch (error) {
      setRequestStatus("An error occurred while sending the friend request.");
      console.error("Error sending friend request:", error);
    } finally {
      setIsRequesting(false);
    }
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;
  if (!userProfile) return <p>No user data available.</p>;

  const { user } = userProfile;
  const profilePictureUrl = user.profilePicturePath ? `${API_BASE_URL}${user.profilePicturePath}` : profilePic;

  return (
    <div className="user-card">
      <img src={profilePictureUrl} alt="profile" className="profile-picture" />
      <div className="current-user-info">
        <h2 className="username">{user.userName}</h2>
        <p className="join-date">Joined {new Date(user.createdAt).toLocaleDateString()}</p>
        <p className="bio">{user.bio || "No bio available."}</p>
      </div>

      <div className="user-actions">
        <button
          className="edit-profile-button"
          onClick={handleSendFriendRequest}
          disabled={isRequesting}
        >
          {isRequesting ? "Sending..." : "Send Friend Request"}
        </button>
        {requestStatus && <p className="request-status">{requestStatus}</p>}
      </div>
    </div>
  );
};
