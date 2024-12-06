import React, { useState } from "react";
import { useNavigate } from "react-router-dom"; // For redirecting after successful registration
import { registerRequest } from "../../api/registerRequest.ts"; 
import logo from "../../assets/images/vibez_logo.jpg";
import { Link } from "react-router-dom";
import { resetPasswordRequest } from "../../api/Users/resetPasswordRequest.ts";

export const ForgotPassword = () => {
  // States for handling form input
  const [email, setEmail] = useState("");
  const [loading, setLoading] = useState(false); 
  const [errorMessage, setErrorMessage] = useState(""); 

  const navigate = useNavigate(); 

  // Handle form submit
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    setLoading(true); 

    const payload = {
        email
    };

    try {
      const response = await resetPasswordRequest(payload); 

      if (response) {
        // On successful registration, redirect to email verification page
        //store email in state
        //navigate("/email-verification", { state: { email } });
        navigate("/change-password", {state: {email}}); 
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
        <img src={logo} alt="Vibez Logo" className="login-logo" />
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="email" className="form-label">E-mail</label>
            <input
              type="email"
              id="email"
              placeholder="Enter your email"
              className="input-field"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>

          {/* Show error message if any */}
          {errorMessage && <p className="error-message">{errorMessage}</p>}

          {/* Show loading spinner when submitting */}
          {loading ? (
            <button type="button" className="login-button" disabled>
              Requesting...
            </button>
          ) : (
            <button type="submit" className="login-button">
              Request Password Change
            </button>
          )}
        </form>
      </div>
    </div>
  );
};
