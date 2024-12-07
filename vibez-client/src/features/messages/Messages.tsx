import React, { useEffect, useRef, useState } from "react";
import "./messages.css";
import audio from "../../assets/images/audio.png";
import video from "../../assets/images/video.png";
import arrow from "../../assets/images/arrow.png";
import { MessageTab } from "../messageTab/MessageTab.tsx";
import { getConversation, Message } from "../../api/Messages/getConversationRequest.ts";
import API_BASE_URL from "../../api/apiConsts.ts";

export const Messages = ({
  currentUserId,
  otherUserId,
  token,
  users,
}: {
  currentUserId: number;
  otherUserId: number;
  token: string;
  users: Array<{ userId: number; userName: string; profilePicturePath: string }>;
}) => {
  const [showMessagesTab, setShowMessagesTab] = useState(false);
  const [messages, setMessages] = useState<Message[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const otherUser = users.find((user) => {
    console.log("user:", user); 
    return user.userId === otherUserId;
  });

  console.log("currentUserId: ", currentUserId);
  console.log("otherUserId: ", otherUserId);
  console.log("token: ", token);
  console.log("users: ", users);

  console.log("otherUser: ", otherUser);

  // Fetch messages from the API
  useEffect(() => {
    const fetchMessages = async () => {
      setLoading(true);
      setError(null);

      try {
        const response = await getConversation(token, otherUserId);
        if (response.success && response.data) {
          // Sort messages by createdAt timestamp
          const sortedMessages = response.data.sort(
            (a, b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()
          );
          setMessages(sortedMessages);
        } else {
          setError(response.error || "Failed to fetch messages.");
        }
      } catch (err) {
        console.error("Error fetching messages:", err);
        setError("An unexpected error occurred.");
      } finally {
        setLoading(false);
      }
    };

    fetchMessages();
  }, [token, otherUserId]);

  const messagesEndRef = useRef<HTMLDivElement>(null);

  // Scroll to the bottom of the message list
  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  if (showMessagesTab) {
    // Render the MessageTab component if back arrow is clicked
    return <MessageTab currentUserId={currentUserId} token={token}/>;
  }

  const getImageUrl = (imageUrl: string | null) =>
    imageUrl ? `${API_BASE_URL}${imageUrl}` : undefined;

  return (
    <div className="messages-container">
      <div className="messages-header">
        <img
          src={arrow}
          alt="arrow"
          className="back-arrow"
          onClick={() => setShowMessagesTab(true)}
        />
        {otherUser && (
          <div className="user-info">
            <img
              src={getImageUrl(otherUser.profilePicturePath)}
              alt={otherUser.userName}
              className="user-profile-pic"
            />
            <span className="user-name">{otherUser.userName}</span>
          </div>
        )}
      </div>
      <div className="messages-list">
        {loading ? (
          <p>Loading messages...</p>
        ) : error ? (
          <p className="error-message">{error}</p>
        ) : messages.length === 0 ? (
          <p>No messages to display.</p>
        ) : (
          messages.map((message) => (
            <div
              key={message.messageId}
              className={`message ${
                message.senderId === currentUserId ? "sent" : "received"
              }`}
            >
              <p className="message-content">{message.content}</p>
              <span className="message-timestamp">
                {new Date(message.createdAt).toLocaleTimeString()}
              </span>
            </div>
          ))
        )}
        <div ref={messagesEndRef} />
      </div>
      <div className="message-input-container">
        <input type="text" placeholder="Send a Message" />
        <img src={audio} alt="Audio" className="audio-button" />
        <img src={video} alt="Video" className="video-button" />
      </div>
    </div>
  );
};
