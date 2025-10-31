import React, { useEffect, useState } from "react";
import apiClient from "./apiClient";

function App() {
  const [departments, setDepartments] = useState([]);
  const [lecturers, setLecturers] = useState([]);

  const [depName, setDepName] = useState("");
  const [lecName, setLecName] = useState("");
  const [lecDegree, setLecDegree] = useState("");
  const [lecDep, setLecDep] = useState("");

  // HTTP API base (make sure the API runs on this port)
  const apiBase = "http://localhost:5100/api";

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const depRes = await apiClient.get("/Departments");
      const lecRes = await apiClient.get("/Lecturers");
      setDepartments(depRes.data);
      setLecturers(lecRes.data);
    } catch (e) {
      console.error(e);
      alert("Cannot load data. Make sure API is running on http://localhost:5100");
    }
  };

  const addDepartment = async () => {
    if(!depName) return alert("Enter department name");
    await apiClient.post("/Departments", { departmentName: depName });
    setDepName("");
    loadData();
  };

  const addLecturer = async () => {
    if(!lecName || !lecDegree || !lecDep) return alert("Fill lecturer fields");
    await apiClient.post("/Lecturers", {
      fullName: lecName,
      degree: lecDegree,
      departmentId: parseInt(lecDep)
    });
    setLecName("");
    setLecDegree("");
    setLecDep("");
    loadData();
  };

  const deleteDepartment = async (id) => {
    if(!window.confirm("Delete department?")) return;
    await apiClient.delete(`/Departments/${id}`);
    loadData();
  };

  const deleteLecturer = async (id) => {
    if(!window.confirm("Delete lecturer?")) return;
    await apiClient.delete(`/Lecturers/${id}`);
    loadData();
  };

  return (
    <div style={{ margin: "20px" }}>
      <h2>📚 Departments</h2>
      <input placeholder="Department name" value={depName} onChange={e => setDepName(e.target.value)} />
      <button onClick={addDepartment}>Add</button>

      <ul>
        {departments.map(d => (
          <li key={d.departmentId}>
            {d.departmentName} <button onClick={() => deleteDepartment(d.departmentId)}>❌</button>
          </li>
        ))}
      </ul>

      <h2>👨‍🏫 Lecturers</h2>
      <input placeholder="Name" value={lecName} onChange={e => setLecName(e.target.value)} />
      <input placeholder="Degree" value={lecDegree} onChange={e => setLecDegree(e.target.value)} />
      <select value={lecDep} onChange={e => setLecDep(e.target.value)}>
        <option value="">Select Department</option>
        {departments.map(d => <option key={d.departmentId} value={d.departmentId}>{d.departmentName}</option>)}
      </select>
      <button onClick={addLecturer}>Add</button>

      <ul>
        {lecturers.map(l => (
          <li key={l.lecturerId}>
            {l.fullName} - {l.degree} ({l.department?.departmentName})
            <button onClick={() => deleteLecturer(l.lecturerId)}>❌</button>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;
