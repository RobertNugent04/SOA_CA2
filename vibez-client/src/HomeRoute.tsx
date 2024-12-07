import React, { useEffect, useState } from "react";
import { Posts } from "./features/posts/Posts.tsx";
import { Navbar } from "./features/navbar/Navbar.tsx";
import { MessageTab } from "./features/messageTab/MessageTab.tsx";
import { getUserProfileRequest } from "./api/Users/userProfileRequest.ts"; // Assuming the getProfile API request is defined here
import { useLocation } from "react-router-dom";
import { createNotificationHubConnection } from "./signalRConnection.ts";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

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
          console.log(
            "User profile fetched successfully:",
            response.data.userId
          );
        } else {
          console.error("Failed to fetch user profile:", response.error);
        }
      } else {
        console.error("Token not found");
      }
    };

    fetchUserProfile();
  }, []);

  useEffect(() => {
    if (!token) return;

    // Initialize SignalR connection
    const connection = createNotificationHubConnection(token);

    connection.on("ReceiveNotification", (notification) => {
      toast.info(`📢 ${notification.message}`, {
        position: "top-right",
        autoClose: 5000, // Toast disappears after 5 seconds
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
        theme: "colored",
      });

      console.log(
        "Notification received from SignalR for user: {currentUserId}, notification: {notification}"
      );
    });

    connection
      .start()
      .then(() => console.log("Connected to Notification Hub"))
      .catch((err) => console.error("Connection failed:", err));

    return () => {
      connection.stop();
    };
  }, [token]);

  return (
    <div className="home-container">
      <Navbar token={token} currentUserId={currentUserId} />
      <div className="content-wrapper">
        <div className="posts-container">
          <Posts
            isUserPage={false}
            userId={currentUserId ? currentUserId : 0}
            token={token}
          />
        </div>
        <div className="messages-container">
          <MessageTab currentUserId={currentUserId ?? 0} token={token} />
        </div>
      </div>
      {/* Toast Container */}
      <ToastContainer />
    </div>
  );
};
