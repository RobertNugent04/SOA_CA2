import React, { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import "./userCard.css";
import profilePic from "../../assets/images/default_pfp.png";
import { getUserProfileRequest } from "../../api/userProfileRequest.ts";

type UserCardProps = {
  token: string;
  userId: number;
};

// Function to fetch a user's profile by userId
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
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const location = useLocation();

  useEffect(() => {
    const fetchUser = async () => {
      setLoading(true);
      setError(null);

      try {
        console.log("UserCard token: ", token)
        const response = await getUserProfileRequest(token, userId); 
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
        setError(err);
      }

      setLoading(false);
    };

    fetchUser();
  }, [userId]); // Re-run effect if userId changes

  if (loading) {
    return <p>Loading...</p>;
  }

  if (error) {
    return <p>Error: {error}</p>;
  }

  if (!user) {
    return <p>User not found.</p>;
  }

  return (
    <div className="user-card">
      <img src={profilePic} alt="profile" className="profile-picture" />
      <div className="current-user-info">
        <h2 className="username">{user.userName}</h2>
        <p className="join-date">Joined {user.createdAt}</p>
        <p className="bio">{user.bio || "No bio available."}</p>
      </div>
      <button className="edit-profile-button">Edit Profile</button>
    </div>
  );
};
