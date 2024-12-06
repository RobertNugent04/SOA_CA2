import React from 'react';
import { Posts } from '../posts/Posts.tsx';
import { Navbar } from '../navbar/Navbar.tsx';
import { MessageTab } from '../messageTab/MessageTab.tsx';
import { UserCard } from './OtherUserCard.tsx';
import { useLocation } from 'react-router-dom';

export const OtherUserRoute: React.FC = () => {
  const location = useLocation();
  const { userId, token, otherUserId} = location.state || {}; // Extract userId and token from state

  console.log("User Route token: ", token);

  if (!otherUserId) {
    return <p>User ID not provided.</p>;
  }

  console.log("Other User Route id: ", otherUserId);

  return (
    <div className="home-container">
      <Navbar token={token} currentUserId={otherUserId} /> {/* Pass userId to Navbar */}
      <div className="content-wrapper">
        <div className="posts-container">
          <UserCard token={token} userId={otherUserId} /> {/* Pass userId to UserCard */}
          <div className="posts">
            <Posts isUserPage={true} userId={otherUserId.toString()} token={token} /> {/* Use dynamic userId */}
          </div>
        </div>
        <div className="messages-container">
          <MessageTab currentUserId={otherUserId} /> {/* Use dynamic userId */}
        </div>
      </div>
    </div>
  );
};
