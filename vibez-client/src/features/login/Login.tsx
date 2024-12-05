import React, { useState } from "react";
import "./login.css";
import logo from "../../assets/images/vibez_logo.jpg";
import { Link, useNavigate } from "react-router-dom";
import { loginRequest } from "../../api/loginRequest.ts";

export const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const payload = { email, password };

    const result = await loginRequest(payload);

    if (result.success) {
      console.log("Login successful:", result.data);
      navigate("/home");
    } else {
      setError(result.message || "Login failed. Please try again.");
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
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>
          {error && <div className="error-message">{error}</div>}
          <button type="submit" className="login-button">
            Login
          </button>
        </form>
        <p className="signup-text">
          Haven't got an account? <Link to="/register" className="signup-link">Sign Up</Link>
        </p>
      </div>
    </div>
  );
};
