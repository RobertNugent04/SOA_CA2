import React from "react";
import "./userCard.css";
import profilePic from "../../assets/images/default_pfp.png";

type UserCardProps = {
  userId: string;
};

const users = [
  { id: "1", username: "JohnDoe", joinDate: "01 Dec", bio: "Loves coding and coffee." },
  { id: "2", username: "JaneSmith", joinDate: "15 Nov", bio: "Avid reader and writer." },
  { id: "3", username: "SamWilson", joinDate: "22 Oct", bio: "Travel enthusiast and foodie." },
];

export const UserCard: React.FC<UserCardProps> = ({userId}) => {
  const user = users.find((user) => user.id === userId);

  if (!user) {
    return <p>User not found.</p>;
  }

  return (
    <div className="user-card">
      <img src={profilePic} alt="profile" className="profile-picture" />
      <div className="current-user-info">
        <h2 className="username">{user.username}</h2>
        <p className="join-date">Joined {user.joinDate}</p>
        <p className="bio">{user.bio}</p>
      </div>
      <button className="edit-profile-button">
          Edit Profile
        </button>
    </div>
  );
};

