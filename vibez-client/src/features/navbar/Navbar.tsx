import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './navbar.css';
import logo from '../../assets/images/vibez_logo.jpg';
import bell from '../../assets/images/notification_bell.png';
import search from '../../assets/images/search_icon.png';
import profilePic from '../../assets/images/default_pfp.png';
import { getNotificationsRequest } from '../../api/Notifications/getNotificationsRequest.ts'; 

type NavbarProps = {
  currentUserId: number | null;
  token: string | null; // Add token as a prop
};

export const Navbar: React.FC<NavbarProps> = ({ currentUserId, token }) => {
  const [notifications, setNotifications] = useState<any[]>([]);
  const [showNotifications, setShowNotifications] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const fetchNotifications = async () => {
    if (!token) {
      setError('No token provided');
      return;
    }

    try {
      const response = await getNotificationsRequest(token);
      if (response.success) {
        setNotifications(response.data);
      } else {
        setError(response.error || 'Failed to fetch notifications.');
      }
    } catch (err) {
      setError('An error occurred while fetching notifications.');
    }
  };

  const toggleNotifications = () => {
    setShowNotifications((prev) => !prev);
    if (!notifications.length) fetchNotifications(); // Fetch notifications if not already fetched
  };

  return (
    <nav className="navbar">
      <div className="navbar-brand">
        <Link to="/home">
          <img src={logo} alt="Vibez Logo" className="navbar-logo" />
        </Link>
      </div>
      <div className="navbar-right">
        <div className="navbar-icon" onClick={toggleNotifications}>
          <img src={bell} alt="notification bell" className="bell" />
          {showNotifications && (
            <div className="notifications-dropdown">
              {error && <p className="notification-error">{error}</p>}
              {notifications.length > 0 ? (
                <div className="notifications-list">
                  {notifications.map((notification) => (
                    <div key={notification.notificationId} className="notification-item">
                      <p className="notification-message">{notification.message}</p>
                      <span className="notification-date">
                        {new Date(notification.createdAt).toLocaleString()}
                      </span>
                    </div>
                  ))}
                </div>
              ) : (
                <p className="no-notifications">No notifications available.</p>
              )}
            </div>
          )}
        </div>
        <div className="navbar-search">
          <img src={search} alt="search icon" className="navbar-icon-image" />
          <input type="text" placeholder="Search" className="navbar-search-input" />
        </div>
        <div className="navbar-profile">
          <Link
            to="/user"
            state={{ token, userId: currentUserId }}
          >
            <img src={profilePic} alt="Profile" className="navbar-profile-pic" />
          </Link>
        </div>
      </div>
    </nav>
  );
};
