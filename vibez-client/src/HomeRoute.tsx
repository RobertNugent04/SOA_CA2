// HomeRoute.tsx
import React from 'react';
import { Posts } from './features/posts/Posts.tsx';
import { Navbar } from './features/navbar/Navbar.tsx';
import { MessageTab } from './features/messageTab/MessageTab.tsx';

export const HomeRoute: React.FC = () => {
  return (
    <div className="home-container">
      <Navbar />
      <div className="content-wrapper">
        <div className="posts-container">
          <Posts />
        </div>
        <div className="messages-container">
          <MessageTab currentUserId={1} />
        </div>
      </div>
    </div>
  );
};