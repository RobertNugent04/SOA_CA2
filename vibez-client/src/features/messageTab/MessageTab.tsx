import React, { useState, useEffect } from "react";
import "./messageTab.css";
import { Messages } from "../messages/Messages.tsx";
import { getProfileRequest } from "../../api/Users/getProfile.ts"; // Adjust the path as necessary
import defaultProfilePic from "../../assets/images/default_pfp.png";
import API_BASE_URL from "../../api/apiConsts.ts";

type MessageTabProps = {
  currentUserId: number;
  token: string;
};

export const MessageTab: React.FC<MessageTabProps> = ({
  currentUserId,
  token,
}) => {
  const [friends, setFriends] = useState<any[]>([]);
  const [selectedUserId, setSelectedUserId] = useState<number | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  // Fetch user's friends using the API
  useEffect(() => {
    const fetchFriends = async () => {
      setLoading(true);
      setError(null);

      console.log("Fetching friends for user:", currentUserId);

      try {
        // Fetch user's profile
        const response = await getProfileRequest(token, currentUserId);
        console.log("Profile Response:", response);

        if (response.success && response.data?.friends) {
          setFriends(response.data.friends);
        } else {
          console.error("Error in profile response:", response);
          setError(response.error || "Failed to fetch friends.");
        }
      } catch (err) {
        console.error("Error fetching friends:", err);
        setError("An error occurred while fetching friends.");
      } finally {
        setLoading(false);
      }
    };

    fetchFriends();
  }, [currentUserId, token]);

  const handleClick = (userId: number) => {
    setSelectedUserId(userId);
  };

  const getImageUrl = (imageUrl: string | null) =>
    imageUrl ? `${API_BASE_URL}${imageUrl}` : defaultProfilePic;

  return (
    <div className="message-tab-container">
      {loading ? (
        <p>Loading friends...</p>
      ) : error ? (
        <p className="error-message">{error}</p>
      ) : !selectedUserId ? (
        // Render the message list if no user is selected
        <div className="messages-container">
          <div className="messages-header">Messages</div>
          {friends.map((friend) => (
            <div
              key={friend.userId}
              className="message-item"
              onClick={() => handleClick(friend.userId)}
            >
              <div className="message-profile-pic">
                <img
                  src={getImageUrl(friend.profilePicturePath)}
                  alt={friend.fullName || "Friend"}
                />
              </div>
              <div className="message-content-wrapper">
                <div className="message-sender">
                  {friend.userName || "Unknown Friend"}
                </div>
                <div className="message-content">
                  Start a conversation! {/* Placeholder text */}
                </div>
              </div>
            </div>
          ))}
        </div>
      ) : (
        // Render the Messages component for the selected user
        <Messages
          currentUserId={currentUserId}
          otherUserId={selectedUserId}
          token={token}
          users={friends}
        />
      )}
    </div>
  );
};
