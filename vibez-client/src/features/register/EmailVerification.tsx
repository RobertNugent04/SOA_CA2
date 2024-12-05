import React from "react";
import "./register.css";
import logo from "../../assets/images/vibez_logo.jpg";
import { Link } from "react-router-dom";

export const EmailVerification = () => {
  return (
    <div className="login-container">
      <div className="login-image"></div>
      <div className="login-form">
        <img src={logo} alt="Vibez Logo" className="login-logo" />
        <form>
          <div className="form-group">
            <div className="info">We have sent a OTP to your email.</div>
            <label htmlFor="otp" className="form-label">OTP Code</label>
            <input
              id="otp"
              placeholder="Enter your OTP code"
              className="input-field"
            />
          </div>
          <button type="submit" className="login-button">
            Submit OTP 
          </button>
        </form>
      </div>
    </div>
  );
};
