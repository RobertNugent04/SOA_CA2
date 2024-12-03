// HomeRoute.tsx
import React from 'react';
import { Posts } from './features/posts/Posts.tsx';
import { Navbar } from './features/navbar/Navbar.tsx';
import { Messages } from './features/messages/MessageTab.tsx';

export const HomeRoute: React.FC = () => {
  return (
    <div className="home-container">
      <Navbar />
      <div className="content-wrapper">
        <div className="posts-container">
          <Posts />
        </div>
        <div className="messages-container">
          <Messages />
        </div>
      </div>
    </div>
  );
};