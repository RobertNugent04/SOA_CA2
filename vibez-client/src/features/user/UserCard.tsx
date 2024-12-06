import React, { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import "./userCard.css";
import profilePic from "../../assets/images/default_pfp.png";
import { getUserProfileRequest } from "../../api/userProfileRequest.ts";
import { EditUser } from "./EditUser.tsx";
import API_BASE_URL from "../../api/apiConsts.ts";
import { CreatePost } from "./CreatePost.tsx";

type UserCardProps = {
  token: string;
  userId: number;
};

type UserProfileResponse = {
  userId: number;
  fullName: string;
  userName: string;
  email: string;
  bio: string | null;
  profilePicturePath: string | null;
  createdAt: string;
  isActive: boolean;
};

export const UserCard: React.FC<UserCardProps> = ({ token, userId }) => {
  const [user, setUser] = useState<UserProfileResponse | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [isEditing, setIsEditing] = useState(false); 
  const [isCreatingPost, setIsCreatingPost] = useState(false);
  const location = useLocation();

  useEffect(() => {
    const fetchUser = async () => {
      setLoading(true);
      setError(null);

      try {
        console.log("UserCard token: ", token);
        const response = await getUserProfileRequest(token);
        console.log("UserCard response: ", response);

        if (response.success && response.data) {
          const userProfile: UserProfileResponse = response.data;
          setUser({
            userId: userProfile.userId,
            fullName: userProfile.fullName,
            userName: userProfile.userName,
            email: userProfile.email,
            bio: userProfile.bio,
            profilePicturePath: userProfile.profilePicturePath || profilePic,
            createdAt: new Date(userProfile.createdAt).toLocaleDateString(),
            isActive: userProfile.isActive,
          });
          
        } else {
          setError(response.error || "Failed to fetch user data.");
        }
      } catch (err) {
        setError(err as string);
      }

      setLoading(false);
    };

    fetchUser();
  }, [userId]);

  if (loading) {
    return <p>Loading...</p>;
  }

  if (error) {
    return <p>Error: {error}</p>;
  }

  if (!user) {
    return <p>User not found.</p>;
  }

  const handleSave = (updatedData: { fullName: string; bio: string; profilePicture?: File | null }) => {
    console.log("Saved Data:", updatedData);
    setUser((prevUser) => ({
      ...prevUser!,
      fullName: updatedData.fullName,
      bio: updatedData.bio,
    }));
    setIsEditing(false);
  };

  const handlePostCreated = (newPost: { content: string; imageUrl: string }) => {
    console.log("New Post Created:", newPost);
    setIsCreatingPost(false);
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
        Edit Profile
      </button>

      {isEditing && (
  <EditUser
          fullName={user.fullName}
          bio={user.bio}
          onSave={handleSave}
          onClose={() => setIsEditing(false)} 
          token={token}/>
      )}

<button
  className="edit-profile-button"
  onClick={() => setIsCreatingPost(true)}
>
  Create Post
</button>

{isCreatingPost && (
    <CreatePost
      token={token}
      onClose={() => setIsCreatingPost(false)}
      onPostCreated={handlePostCreated}
    />
  )}
</div>
    </div>
  );
};