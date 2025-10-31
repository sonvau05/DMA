import React from "react";
import Authors from "./Authors";
import Books from "./Books";
export default function App() {
  return (
    <div style={{padding:20,fontFamily:'Segoe UI'}}>
      <h1>Authors & Books</h1>
      <div style={{display:'flex',gap:24}}>
        <div style={{flex:1}}><Authors/></div>
        <div style={{flex:1}}><Books/></div>
      </div>
    </div>
  );
}