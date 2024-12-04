import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { Login } from "../src/features/login/Login.tsx";
import { HomeRoute } from "./HomeRoute.tsx";
import { UserRoute } from "../src/features/user/UserRoute.tsx";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Login/>} />
        <Route path="/home" element={<HomeRoute />} />
        <Route path="/user" element={<UserRoute />} />
      </Routes>
    </Router>
  );
}

export default App;
