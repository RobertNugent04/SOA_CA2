import React, { useEffect, useState } from 'react';
import { Posts } from './features/posts/Posts.tsx';
import { Navbar } from './features/navbar/Navbar.tsx';
import { MessageTab } from './features/messageTab/MessageTab.tsx';
import { getUserProfileRequest } from './api/userProfileRequest.ts'; // Assuming the getProfile API request is defined here
import { useLocation } from 'react-router-dom';

export const HomeRoute: React.FC = () => {
  const [currentUserId, setCurrentUserId] = useState<number | null>(null);
  const location = useLocation();
  const { token } = location.state || {}; 

  useEffect(() => {
    const fetchUserProfile = async () => {
      // Replace this with your token retrieval logic

      if (token) {
        const response = await getUserProfileRequest(token);

        if (response.success && response.data) {
          setCurrentUserId(response.data.userId); 
        } else {
          console.error('Failed to fetch user profile:', response.error);
        }
      } else {
        console.error('Token not found');
      }
    };

    fetchUserProfile();
  }, []);

  return (
    <div className="home-container">
      <Navbar token={token} currentUserId={currentUserId}/>
      <div className="content-wrapper">
        <div className="posts-container">
          <Posts isUserPage={false} userId={currentUserId ? currentUserId : 0} token={token} />
        </div>
        <div className="messages-container">
          <MessageTab currentUserId={currentUserId} />
        </div>
      </div>
    </div>
  );
};