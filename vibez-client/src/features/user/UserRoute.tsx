import React from "react";
import { Posts } from "../posts/Posts.tsx";
import { Navbar } from "../navbar/Navbar.tsx";
import { MessageTab } from "../messageTab/MessageTab.tsx";
import { UserCard } from "./UserCard.tsx";
import { useLocation } from "react-router-dom";

export const UserRoute: React.FC = (isCurrentUser) => {
  const location = useLocation();
  const { userId, token } = location.state || {}; // Extract userId and token from state

  console.log("User Route token: ", token);

  if (!userId) {
    return <p>User ID not provided.</p>;
  }

  return (
    <div className="home-container">
      <Navbar token={token} currentUserId={userId} />{" "}
      {/* Pass userId to Navbar */}
      <div className="content-wrapper">
        <div className="posts-container">
          <UserCard token={token} userId={userId.toString()} />{" "}
          {/* Pass userId to UserCard */}
          <div className="posts">
            <Posts isUserPage={true} userId={userId.toString()} token={token} />{" "}
            {/* Use dynamic userId */}
          </div>
        </div>
        <div className="messages-container">
          <MessageTab currentUserId={userId} token={token} />{" "}
          {/* Use dynamic userId */}
        </div>
      </div>
    </div>
  );
};
