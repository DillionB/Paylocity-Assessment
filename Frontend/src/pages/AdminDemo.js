import React from "react";
import Payroll from "../components/payroll";

const AdminDemo = () => {
  return (
    <div>
      <a href="/" className="logo-link">
        <img
          src={`${process.env.PUBLIC_URL}/Paylocity-logo.png`}
          alt="Paylocity Logo"
        />
      </a>
      <h1>Admin Demo</h1>

      <Payroll />
    </div>
  );
};

export default AdminDemo;
