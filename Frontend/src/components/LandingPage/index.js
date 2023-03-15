import React from "react";
import "./LandingPage.css";
import { Link } from "react-router-dom";

const LandingPage = () => {
  return (
    <div className="landing-page">
      <a href="/" className="logo-link">
        <img
          src={`${process.env.PUBLIC_URL}/Paylocity-logo.png`}
          alt="Paylocity Logo"
        />
      </a>
      <div>
        <img
          className="banner"
          src={`${process.env.PUBLIC_URL}/Paylocity-banner.png`}
          alt="Paylocity Logo"
        />
        <h1 className="credit">Assessment by Dillion Babin</h1>
      </div>
      <div className="buttons-container">
        <Link to="/user-demo">
          <button className="demo-button">User Demo</button>
        </Link>
        <Link to="/admin-demo">
          <button className="demo-button2">Admin Demo</button>
        </Link>
      </div>
    </div>
  );
};

export default LandingPage;
