CREATE TABLE Employees (
    EmployeeId INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    DiscountApplied BIT NOT NULL DEFAULT 0,
    PayrollPreview MONEY NOT NULL DEFAULT 0
);

CREATE TABLE Dependents (
    DependentId INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    Name VARCHAR(50) NOT NULL,
    FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId)
);