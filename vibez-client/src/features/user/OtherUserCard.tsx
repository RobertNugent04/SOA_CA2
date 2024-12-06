import React, { useEffect, useState } from "react";
import { getProfileRequest } from "../../api/Users/getProfile.ts";
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
  const [user_, setUser] = useState<UserProfileResponse | null>(null);
  const [isEditing, setIsEditing] = useState(false); // State to toggle EditUser component

  useEffect(() => {
    const fetchUser = async () => {
      setLoading(true);
      try {
        const response = await getProfileRequest(token, userId);
        if (response.success) {
          setUserProfile(response.data!);
          console.log("After setting user profile: ", userProfile);
        } else {
          setError(response.error || "Failed to fetch user data.");
        }
      } catch (err) {
        setError("An error occurred while fetching the profile.");
      }
      setLoading(false);
    };

    fetchUser();
  }, [token, userId]);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;

  if (!userProfile) return <p>No user data available.</p>;

  const { user, posts, friends } = userProfile;

  console.log("User profile: ", userProfile.user.fullName);

  const handleSave = (updatedData: { fullName: string; bio: string; profilePicture?: File | null }) => {
    console.log("Saved Data:", updatedData);
    setUser((prevUser) => ({
      ...prevUser!,
      fullName: updatedData.fullName,
      bio: updatedData.bio,
    }));
    setIsEditing(false);
  };

  const profilePictureUrl = user.profilePicturePath ? `${API_BASE_URL}${user.profilePicturePath}` : profilePic;
  console.log("Profile Picture URL:", profilePictureUrl);

  return (
    <div className="user-card">
      <img src={profilePictureUrl} alt="profile" className="profile-picture" />
      <div className="current-user-info">
        <h2 className="username">{user.userName}</h2>
        <p className="join-date">Joined {user.createdAt}</p>
        <p className="bio">{user.bio || "No bio available."}</p>
      </div>

      <div className="user-actions">
      <button className="edit-profile-button" onClick={() => setIsEditing(true)}>
        Send Friend Request
      </button>

      </div>
    </div>
  );
};