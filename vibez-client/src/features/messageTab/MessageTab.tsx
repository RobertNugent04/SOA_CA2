import React, { useState } from 'react';
import './messageTab.css';
import { Messages } from '../messages/Messages.tsx';

export const MessageTab = ({ currentUserId }) => {
  const [selectedUserId, setSelectedUserId] = useState(null);

  currentUserId = 1;

  const messages = [
    {
      id: 1,
      user_id: 2,
      sender: 'Benjamin Smith',
      timestamp: '10:30 AM',
      content: 'How are you?',
      profilePic: 'https://www.shutterstock.com/image-photo/head-shot-portrait-close-smiling-600nw-1714666150.jpg',
    },
    {
      id: 2,
      user_id: 3,
      sender: 'Evelyn',
      timestamp: '9:22 AM',
      content: 'Thank you so much!!',
      profilePic: 'https://www.shutterstock.com/image-photo/head-shot-portrait-close-smiling-600nw-1714666150.jpg',
    },
    {
      id: 3,
      user_id: 4,
      sender: 'Joseph Brown',
      timestamp: '9:02 AM',
      content: 'Ok, great! See you Sunday!',
      profilePic: 'https://www.shutterstock.com/image-photo/head-shot-portrait-close-smiling-600nw-1714666150.jpg',
    },
    {
      id: 4,
      user_id: 5,
      sender: 'Patrick',
      timestamp: 'Yesterday',
      content: 'I will be there at 5 PM',
      profilePic: 'https://www.shutterstock.com/image-photo/head-shot-portrait-close-smiling-600nw-1714666150.jpg',
    },
  ];

  const handleClick = (user_id) => {
    // Update the selected user ID
    setSelectedUserId(user_id); 
  };

  return (
    <div className="message-tab-container">
      {!selectedUserId ? (
        // Render the message list if no user is selected
        <div className="messages-container">
          <div className="messages-header">Messages</div>
          {messages.map((message) => (
            <div
              key={message.id}
              className="message-item"
              onClick={() => handleClick(message.user_id)} 
            >
              <div className="message-profile-pic">
                <img src={message.profilePic} alt={message.sender} />
              </div>
              <div className="message-content-wrapper">
                <div className="message-sender">{message.sender}</div>
                <div className="message-content">{message.content}</div>
                <div className="message-timestamp">{message.timestamp}</div>
              </div>
            </div>
          ))}
        </div>
      ) : (
        // Render the Messages component for the selected user
        <Messages currentUserId={currentUserId} otherUserId={selectedUserId} />
      )}
    </div>
  );
};
