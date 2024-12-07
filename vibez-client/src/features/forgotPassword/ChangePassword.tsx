import React, { useState } from "react";
import { useNavigate, useLocation } from "react-router-dom"; // For redirecting after successful registration
import { registerRequest } from "../../api/registerRequest.ts"; 
import logo from "../../assets/images/vibez_logo.jpg";
import { Link } from "react-router-dom";
import { changePassword } from "../../api/Users/resetPasswordRequest.ts";
import "./changePassword.css";

export const ChangePassword = (payload: { email: string; otp: string; password: string; }) => {
  // States for handling form input
  const [newPassword, setPassword] = useState("");
  const [otp, setOtp] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [loading, setLoading] = useState(false); 
  const [errorMessage, setErrorMessage] = useState(""); 

  const location = useLocation();
  // Get email from state
  const { email } = location.state || {};

  const navigate = useNavigate(); 

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
  
    // Reset error message
    setErrorMessage("");

      // Check if the password fields are empty
  if (!newPassword || !confirmPassword) {
    setErrorMessage("Password fields cannot be empty.");
    return;
  }
  
    // Validate if passwords match
    if (newPassword !== confirmPassword) {
      setErrorMessage("Passwords do not match. Please try again.");
      setLoading(false);
      return;
    }
  
    setLoading(true);
  
    const payload = {
      email,
      otp,
      newPassword,
    };
  
    try {
      const response = await changePassword(payload);
  
      if (response.success) {
        // On successful password change, redirect to the home page
        navigate("/");
      }
    } catch (error) {
      setLoading(false);
      setErrorMessage("An error occurred. Please try again.");
    }
  };
  

  return (
    <div className="login-container">
      <div className="login-image"></div>
      
      <div className="login-form">
      {errorMessage && <p className="error-message">{errorMessage}</p>}
        <img src={logo} alt="Vibez Logo" className="login-logo" />
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="otp" className="form-label">OTP</label>
            <input
              id="otp"
              placeholder="Enter your OTP"
              className="input-field"
              value={otp}
              onChange={(e) => setOtp(e.target.value)}
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="password" className="form-label">New Password</label>
            <input
              type="password"
              id="password"
              placeholder="Enter your password"
              className="input-field"
              value={newPassword}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="confirm-password" className="form-label">Confirm Password</label>
            <input
              type="password"
              id="confirm-password"
              placeholder="Confirm your password"
              className="input-field"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
            />
          </div>

          {/* Show loading spinner when submitting */}
          {loading ? (
            <button type="button" className="login-button" disabled>
              Changing...
            </button>
          ) : (
            <button type="submit" className="login-button">
             Change Password
            </button>
          )}
        </form>
      </div>
    </div>
  );
};
