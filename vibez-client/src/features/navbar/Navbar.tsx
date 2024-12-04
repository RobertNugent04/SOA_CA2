import React from 'react';
import { Link } from 'react-router-dom';
import './navbar.css';
import logo from '../../assets/images/vibez_logo.jpg';
import bell from '../../assets/images/notification_bell.png';
import search from '../../assets/images/search_icon.png';
import profilePic from '../../assets/images/default_pfp.png'; // Import profile picture

export const Navbar = () => {
  return (
    <nav className="navbar">
      <div className="navbar-brand">
        <Link to="/">
          <img src={logo} alt="Vibez Logo" className="navbar-logo" />
        </Link>
      </div>
      <div className="navbar-right">
        <div className="navbar-icon">
          <img src={bell} alt="notification bell" className="bell" />
        </div>
        <div className="navbar-search">
          <img src={search} alt="search icon" className="navbar-icon-image" />
          <input type="text" placeholder="Search" className="navbar-search-input" />
        </div>
        <div className="navbar-profile">
          <img src={profilePic} alt="Profile" className="navbar-profile-pic" />
        </div>
      </div>
    </nav>
  );
};
