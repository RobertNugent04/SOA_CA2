import React, { useState } from "react";
import { useNavigate } from "react-router-dom"; // For redirecting after successful registration
import { registerRequest } from "../../api/registerRequest.ts"; 
import "./register.css";
import logo from "../../assets/images/vibez_logo.jpg";
import { Link } from "react-router-dom";

export const Register = () => {
  // States for handling form input
  const [fullName, setFullName] = useState("");
  const [userName, setUserName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false); // For loading state
  const [errorMessage, setErrorMessage] = useState(""); // For error messages

  const navigate = useNavigate(); // Hook for redirection

  // Handle form submit
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    setLoading(true); // Set loading state to true when submitting

    const payload = {
      fullName,
      userName,
      email,
      password,
    };

    try {
      const response = await registerRequest(payload); // Call the API function

      if (response) {
        // On successful registration, redirect to email verification page
        navigate("/email-verification"); // Assuming your route is `/email-verification`
      }
    } catch (error) {
      setLoading(false);
      setErrorMessage("An error occurred. Please try again."); // Set error message on failure
    }
  };

  return (
    <div className="login-container">
      <div className="login-image"></div>
      <div className="login-form">
        <img src={logo} alt="Vibez Logo" className="login-logo" />
        <form onSubmit={handleSubmit}>
          <div className="names">
            <div className="form-group">
              <label htmlFor="fullname" className="form-label">Full Name</label>
              <input
                id="fullname"
                placeholder="Enter your full name"
                className="input-field"
                value={fullName}
                onChange={(e) => setFullName(e.target.value)} // Handle input change
                required
              />
            </div>
            <div className="form-group">
              <label htmlFor="username" className="form-label">Username</label>
              <input
                id="username"
                placeholder="Enter your username"
                className="input-field"
                value={userName}
                onChange={(e) => setUserName(e.target.value)} // Handle input change
                required
              />
            </div>
          </div>
          <div className="form-group">
            <label htmlFor="email" className="form-label">E-mail</label>
            <input
              type="email"
              id="email"
              placeholder="Enter your email"
              className="input-field"
              value={email}
              onChange={(e) => setEmail(e.target.value)} // Handle input change
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="password" className="form-label">Password</label>
            <input
              type="password"
              id="password"
              placeholder="Enter your password"
              className="input-field"
              value={password}
              onChange={(e) => setPassword(e.target.value)} // Handle input change
              required
            />
          </div>

          {/* Show error message if any */}
          {errorMessage && <p className="error-message">{errorMessage}</p>}

          {/* Show loading spinner when submitting */}
          {loading ? (
            <button type="button" className="login-button" disabled>
              Registering...
            </button>
          ) : (
            <button type="submit" className="login-button">
              Register
            </button>
          )}
        </form>
        <p className="signup-text">
          Already have an account? <Link to="/" className="signup-link">Log In</Link>
        </p>
      </div>
    </div>
  );
};
