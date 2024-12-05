// HomeRoute.tsx
import React from 'react';
import { Posts } from '../posts/Posts.tsx';
import { Navbar } from '../navbar/Navbar.tsx';
import { MessageTab } from '../messageTab/MessageTab.tsx';
import { UserCard } from './UserCard.tsx';

export const UserRoute: React.FC = () => {
  return (
    <div className="home-container">
      <Navbar />
      <div className="content-wrapper">
        <div className="posts-container">
          <UserCard userId = "1"/>
          <div className="posts">
          <Posts isUserPage={true} userId="1" />
          </div>
        </div>
        <div className="messages-container">
          <MessageTab currentUserId={2} />
        </div>
      </div>
    </div>
  );
};