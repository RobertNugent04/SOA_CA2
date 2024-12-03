import React, { useEffect, useRef, useState } from 'react';
import './messages.css';
import audio from '../../assets/images/audio.png';
import video from '../../assets/images/video.png';
import arrow from '../../assets/images/arrow.png';
import { MessageTab } from '../messageTab/MessageTab.tsx';

export const Messages = ({ currentUserId, otherUserId }) => {
    const [showMessagesTab, setShowMessagesTab] = useState(false);
    
    const messages = [
        {
            Message_Id: 1,
            Sender_User_ID: 2,
            Receiver_User_ID: 1,
            Content: 'Hey Bob, are you free for a meeting later?',
            Sent_At: '2024-12-03 10:15:00',
        },
        {
            Message_Id: 2,
            Sender_User_ID: 1,
            Receiver_User_ID: 2,
            Content: 'Sure, see you at 3 PM!',
            Sent_At: '2024-12-03 09:45:00',
        },
        {
            Message_Id: 3,
            Sender_User_ID: 2,
            Receiver_User_ID: 1,
            Content: 'I cant wait till 3!',
            Sent_At: '2024-12-02 17:30:00',
        },
        {
            Message_Id: 4,
            Sender_User_ID: 1,
            Receiver_User_ID: 2,
            Content: 'If you throw in an extra 100.',
            Sent_At: '2024-12-02 15:45:00',
        },
        {
            Message_Id: 5,
            Sender_User_ID: 2,
            Receiver_User_ID: 1,
            Content: 'Yeah ok',
            Sent_At: '2024-12-06 17:30:00',
        },
        {
            Message_Id: 6,
            Sender_User_ID: 1,
            Receiver_User_ID: 2,
            Content: 'Okay.',
            Sent_At: '2024-12-02 15:45:00',
        },
        {
            Message_Id: 7,
            Sender_User_ID: 1,
            Receiver_User_ID: 2,
            Content: 'Okay.',
            Sent_At: '2024-12-02 15:45:00',
        },
        {
            Message_Id: 8,
            Sender_User_ID: 1,
            Receiver_User_ID: 2,
            Content: 'Okay.',
            Sent_At: '2024-12-02 15:45:00',
        },
    ];

    const users = [

        {
            User_ID: 1,
            Name: 'Alice',
            Profile_Pic: 'https://www.shutterstock.com/image-photo/head-shot-portrait-close-smiling-600nw-1714666150.jpg',
        },
        {
            User_ID: 2,
            Name: 'Bob',
            Profile_Pic: 'https://www.shutterstock.com/image-photo/head-shot-portrait-close-smiling-600nw-1714666150.jpg',
        },
        {
            User_ID: 3,
            Name: 'Charlie',
            Profile_Pic: 'https://www.shutterstock.com/image-photo/head-shot-portrait-close-smiling-600nw-1714666150.jpg',
        },
        {
            User_ID: 4,
            Name: 'David',
            Profile_Pic: 'https://www.shutterstock.com/image-photo/head-shot-portrait-close-smiling-600nw-1714666150.jpg',
        },
        {
            User_ID: 5,
            Name: 'Eve',
            Profile_Pic: 'https://www.shutterstock.com/image-photo/head-shot-portrait-close-smiling-600nw-1714666150.jpg',
        },

    ];

  // Find the user associated with the otherUserId
  const otherUser = users.find((user) => user.User_ID === otherUserId);

  // Filter and sort messages
  const filteredMessages = messages
    .filter(
      (message) =>
        (message.Sender_User_ID === currentUserId && message.Receiver_User_ID === otherUserId) ||
        (message.Sender_User_ID === otherUserId && message.Receiver_User_ID === currentUserId)
    )
    .sort((a, b) => new Date(a.Sent_At).getTime() - new Date(b.Sent_At).getTime())
    // Reverse the order so that the latest message is at the bottom
    .reverse();

  // Ref to the messages container
  const messagesEndRef = useRef<HTMLDivElement>(null);

  // Scroll to the bottom of the message list on render or update
  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [filteredMessages]);

  if (showMessagesTab) {
    // Render the MessageTab component if back arrow is clicked
    return <MessageTab currentUserId={currentUserId} />;
  }

  return (
    <div className="messages-container">
      <div className="messages-header">
        <img src={arrow} alt="arrow" className="back-arrow" onClick={() => setShowMessagesTab(true)} />
        {otherUser && (
          <div className="user-info">
            <img
              src={otherUser.Profile_Pic}
              alt={otherUser.Name}
              className="user-profile-pic"
            />
            <span className="user-name">{otherUser.Name}</span>
          </div>
        )}
      </div>
      <div className="messages-list">
      <div ref={messagesEndRef} />
        {filteredMessages.map((message) => (
          <div
            key={message.Message_Id}
            className={`message ${
              message.Sender_User_ID === currentUserId ? 'sent' : 'received'
            }`}
          >
            <p className="message-content">{message.Content}</p>
            <span className="message-timestamp">{message.Sent_At}</span>
          </div>
        ))}
      </div>
      <div className="message-input-container">
        <input
          type="text"
          placeholder="Send a Message"
        />
        <img
          src={audio}
          alt="Audio"
          className="audio-button"
        />
        <img
          src={video}
          alt="Video"
          className="video-button"
        />
      </div>
    </div>
  );
};  