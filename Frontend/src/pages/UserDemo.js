import { useState, useEffect } from "react";
import styles from "./UserDemo.css";
import { PieChart, Pie, Cell, Tooltip } from "recharts";

function UserDemo() {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [dependents, setDependents] = useState([]);
  const [dependentsToDelete, setDependentsToDelete] = useState([]);
  const calculateDeductions = () => {
    const baseEmployeeDeduction = firstName.startsWith("A") ? 37.49 : 41.66;
    const baseDependentDeduction = firstName.startsWith("A") ? 18.74 : 20.83;

    const totalDeductions =
      baseEmployeeDeduction + dependents.length * baseDependentDeduction;
    return totalDeductions;
  };

  const salary = 2000;
  const deductions = calculateDeductions();
  const takeHome = salary - deductions;

  useEffect(() => {
    fetch("https://localhost:7281/Employee/1")
      .then((response) => response.json())
      .then((data) => {
        const [firstName, ...lastNameParts] = data.name.split(" ");
        setFirstName(firstName);
        setLastName(lastNameParts.join(" "));
        setDependents(data.dependents);
      });
  }, []);

  const addDependent = (e) => {
    e.preventDefault();
    setDependents([...dependents, { type: "spouse" }]);
  };

  const updateDependent = (index, field, value) => {
    const newDependents = [...dependents];
    newDependents[index][field] = value;
    setDependents(newDependents);
  };
  const handleDependentTypeChange = (e, index) => {
    const newDependents = [...dependents];
    newDependents[index].type = e.target.value;
    setDependents(newDependents);
  };
  const handleDependentNameChange = (e, index) => {
    const newDependents = [...dependents];
    newDependents[index].name = e.target.value;
    setDependents(newDependents);
  };
  const removeDependent = (e, index) => {
    e.preventDefault();
    setDependents(dependents.filter((_, i) => i !== index));
  };

  const saveChanges = () => {
    const updatedEmployee = {
      name: `${firstName} ${lastName}`,
      dependents,
    };

    fetch(`https://localhost:7281/Employee/1`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(updatedEmployee),
    }).then(() => {
      dependentsToDelete.forEach((dependentId) => {
        fetch(`https://localhost:7281/Dependent/${dependentId}`, {
          method: "DELETE",
        });
      });
    });
  };
  const calculateDeduction = (dependents) => {
    const employeeRate = firstName.startsWith("A") ? 37.49 : 41.66;
    const dependentRate = firstName.startsWith("A") ? 18.74 : 20.83;

    const totalDeduction = employeeRate + dependents.length * dependentRate;
    return totalDeduction;
  };

  const getChartData = () => {
    const totalPay = 2000;
    const totalDeduction = calculateDeduction(dependents);
    const totalTakeHome = totalPay - totalDeduction;

    return [
      { name: "Deduction", value: totalDeduction },
      { name: "Take Home", value: totalTakeHome },
    ];
  };

  const COLORS = ["#FF8042", "#82CA9D"];
  return (
    <div className={styles.container}>
      <a href="/" className="logo-link">
        <img
          src={`${process.env.PUBLIC_URL}/Paylocity-logo.png`}
          alt="Paylocity Logo"
        />
      </a>
      <div class="pie-chart-container">
        <div className="total-paid">Benefits Cost</div>
        <div className="total-paid-ammount">
          ${parseFloat(deductions).toFixed(2)}
        </div>

        <PieChart width={400} height={400}>
          <Pie
            data={getChartData()}
            cx={200}
            cy={200}
            innerRadius={60}
            outerRadius={80}
            fill="#82CA9D"
            dataKey="value"
          >
            {getChartData().map((entry, index) => (
              <Cell
                key={`cell-${index}`}
                fill={COLORS[index % COLORS.length]}
              />
            ))}
          </Pie>
          <Tooltip />
        </PieChart>
      </div>
      <div className="form-container">
        <form>
          <label>
            First Name:
            <input
              type="text"
              value={firstName}
              onChange={(e) => setFirstName(e.target.value)}
            />
          </label>
          <label>
            Last Name:
            <input
              type="text"
              value={lastName}
              onChange={(e) => setLastName(e.target.value)}
            />
          </label>
          <div className="deps"> Dependents: </div>
          {dependents.map((dependent, index) => (
            <div key={index} className="dependent-info">
              <label>Name:</label>
              <input
                type="text"
                value={dependent.name}
                onChange={(e) => handleDependentNameChange(e, index)}
              />
              <select
                value={dependent.type}
                onChange={(e) => handleDependentTypeChange(e, index)}
              >
                <option value="child">Child</option>
                <option value="spouse">Spouse</option>
              </select>
              <button onClick={(e) => removeDependent(e, index)}>Remove</button>
            </div>
          ))}
          <button onClick={addDependent}>Add Dependent</button>
        </form>
        <button onClick={saveChanges}>Save Changes</button>
      </div>
    </div>
  );
}

export default UserDemo;
