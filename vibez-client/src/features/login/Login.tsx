import React from "react";
import "./login.css";
import logo from "../../assets/images/vibez_logo.jpg";
import { Link } from "react-router-dom";

export const Login = () => {
  return (
    <div className="login-container">
      <div className="login-image"></div>
      <div className="login-form">
        <img src={logo} alt="Vibez Logo" className="login-logo" />
        <form>
          <div className="form-group">
            <label htmlFor="email" className="form-label">E-mail</label>
            <input
              type="email"
              id="email"
              placeholder="Enter your email"
              className="input-field"
            />
          </div>
          <div className="form-group">
            <label htmlFor="password" className="form-label">Password</label>
            <input
              type="password"
              id="password"
              placeholder="Enter your password"
              className="input-field"
            />
          </div>
          <button type="submit" className="login-button">
            Login
          </button>
        </form>
        <p className="signup-text">
          Haven't got an account? <Link to="/" className="signup-link">Sign Up</Link>
        </p>
      </div>
    </div>
  );
};
