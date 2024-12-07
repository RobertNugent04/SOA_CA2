import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import './navbar.css';
import logo from '../../assets/images/vibez_logo.jpg';
import bell from '../../assets/images/notification_bell.png';
import searchIcon from '../../assets/images/search_icon.png';
import defaultProfilePic from '../../assets/images/default_pfp.png';
import { getNotificationsRequest } from '../../api/Notifications/getNotificationsRequest.ts';
import { searchRequest } from '../../api/Users/searchRequest.ts';
import { getUserProfileRequest } from '../../api/Users/userProfileRequest.ts'; // Import API request
import API_BASE_URL from '../../api/apiConsts.ts';

type NavbarProps = {
  currentUserId: number | null;
  token: string | null;
};

export const Navbar: React.FC<NavbarProps> = ({ currentUserId, token }) => {
  const [notifications, setNotifications] = useState<any[]>([]);
  const [showNotifications, setShowNotifications] = useState<boolean>(false);
  const [searchResults, setSearchResults] = useState<any[]>([]);
  const [showSearchDropdown, setShowSearchDropdown] = useState<boolean>(false);
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [error, setError] = useState<string | null>(null);
  const [profilePic, setProfilePic] = useState<string | null>(null); // State for profile picture

  const navigate = useNavigate();

  // Fetch notifications
  const fetchNotifications = async () => {
    if (!token) {
      setError('No token provided');
      return;
    }

    try {
      const response = await getNotificationsRequest(token);
      
      if (response.success) {
        console.log('Notifications:', response.data);
        setNotifications(response.data);
      } else {
        setError(response.error || 'Failed to fetch notifications.');
      }
    } catch (err) {
      setError('An error occurred while fetching notifications.');
    }
  };

  // Fetch user profile picture
  useEffect(() => {
    const fetchUserProfile = async () => {
      if (token) {
        const response = await getUserProfileRequest(token);
        if (response.success && response.data?.profilePicturePath) {
          setProfilePic(response.data.profilePicturePath);
        }
      }
    };

    fetchUserProfile();
  }, [token]);

  // Handle search input changes
  const handleSearchChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const query = e.target.value;
    setSearchQuery(query);

    if (query.length === 0) {
      setSearchResults([]);
      setShowSearchDropdown(false);
      return;
    }

    try {
      const results = await searchRequest(query);
      setSearchResults(results);
      setShowSearchDropdown(true);
    } catch (err) {
      console.error('Error fetching search results:', err);
      setSearchResults([]);
    }
  };

  // Navigate to user's profile when a search result is clicked
  const handleResultClick = (otherUserId: number) => {
    navigate('/other-user', {
      state: { userId: currentUserId, token, otherUserId },
    });

    setShowSearchDropdown(false);
    setSearchQuery('');
  };

  // Toggle notifications dropdown
  const toggleNotifications = () => {
    setShowNotifications((prev) => !prev);
    if (!notifications.length) fetchNotifications(); // Fetch only if not already fetched
  };

  const getImageUrl = (imageUrl: string | null) =>
    imageUrl ? `${API_BASE_URL}${imageUrl}` : undefined;

  return (
    <nav className="navbar">
      <div className="navbar-brand">
        <Link to="/home" state={{ token, userId: currentUserId }}>
          <img src={logo} alt="Vibez Logo" className="navbar-logo" />
        </Link>
      </div>
      <div className="navbar-right">
        {/* Notifications Icon */}
        <div className="navbar-icon" onClick={(e) => {
          e.stopPropagation(); // Prevent closing dropdown due to window click
          toggleNotifications();
        }}>
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

        {/* Search Bar */}
        <div className="navbar-search">
          <img src={searchIcon} alt="search icon" className="navbar-icon-image" />
          <input
            type="text"
            placeholder="Search"
            className="navbar-search-input"
            value={searchQuery}
            onChange={handleSearchChange}
          />
          {showSearchDropdown && (
            <div className="search-dropdown">
              {searchResults.length > 0 ? (
                <div className="search-results">
                  {searchResults.map((result) => (
                    <div
                      key={result.id}
                      className="search-item"
                      onClick={() => handleResultClick(result.userId)}
                    >
                      <img
                        src={result.profilePic || defaultProfilePic}
                        alt={result.userName}
                        className="search-profile-pic"
                      />
                      <span className="search-username">{result.userName}</span>
                    </div>
                  ))}
                </div>
              ) : (
                <p className="no-search-results">No results found.</p>
              )}
            </div>
          )}
        </div>

        {/* Profile Picture */}
        <div className="navbar-profile">
          <Link
            to="/user"
            state={{ token, userId: currentUserId }}
          >
            <img
              src={getImageUrl(profilePic) || defaultProfilePic}
              alt="Profile"
              className="navbar-profile-pic"
            />
          </Link>
        </div>
      </div>
    </nav>
  );
};
