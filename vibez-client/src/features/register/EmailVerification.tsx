import React, { useState } from "react";
import "./register.css";
import logo from "../../assets/images/vibez_logo.jpg";
import { useNavigate, useLocation } from "react-router-dom";
import { emailVerificationRequest } from "../../api/emailVerificationRequest.ts";

export const EmailVerification = () => {
  const [otp, setOtp] = useState("");
  const [error, setError] = useState("");
  const location = useLocation();
  const navigate = useNavigate();

  const { email } = location.state || {};

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const payload = { email, otp };

    try {
      const result = await emailVerificationRequest(payload);

      if (result) {
        console.log(email);
        console.log("OTP verified successfully.");
        console.log(result);

        navigate("/"); 
      } else {
        setError(result.message || "Failed to verify OTP.");
      }
    } catch (err) {
      setError("An unexpected error occurred.");
      console.error(err);
    }
  };

  return (
    <div className="login-container">
      <div className="login-image"></div>
      <div className="login-form">
        <img src={logo} alt="Vibez Logo" className="login-logo" />
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <div className="info">We have sent an OTP to your email.</div>
            <label htmlFor="otp" className="form-label">OTP Code</label>
            <input
              id="otp"
              placeholder="Enter your OTP code"
              className="input-field"
              value={otp}
              onChange={(e) => setOtp(e.target.value)}
            />
          </div>
          {error && <div className="error-message">{error}</div>}
          <button type="submit" className="login-button">
            Submit OTP
          </button>
        </form>
      </div>
    </div>
  );
};
