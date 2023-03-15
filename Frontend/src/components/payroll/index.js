import { useState, useEffect } from "react";
import axios from "axios";
import styles from "./payroll.css";

function Payroll() {
  const [payroll, setPayroll] = useState([]);

  useEffect(() => {
    async function fetchData() {
      const result = await axios.get("https://localhost:7281/Payroll");
      setPayroll(result.data);
    }
    fetchData();
  }, []);

  return (
    <div className={styles.container}>
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Payroll Preview</th>
          </tr>
        </thead>
        <tbody>
          {payroll.map((employee) => (
            <tr key={employee.employeeId}>
              <td>{employee.name}</td>
              <td>${parseFloat(employee.payrollPreview).toFixed(2)}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default Payroll;
