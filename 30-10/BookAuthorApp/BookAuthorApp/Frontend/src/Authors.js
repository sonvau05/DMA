import React, { useState, useEffect } from "react";
import axios from "axios";
export default function Authors() {
  const [authors, setAuthors] = useState([]);
  const [name, setName] = useState("");
  const [bio, setBio] = useState("");
  useEffect(() => { load(); }, []);
  function load() { axios.get("http://localhost:5000/api/authors").then(r => setAuthors(r.data)); }
  function create(e) { e.preventDefault(); axios.post("http://localhost:5000/api/authors", { name, biography: bio }).then(() => { setName(""); setBio(""); load(); }); }
  function remove(id) { if (!window.confirm('Delete?')) return; axios.delete(`http://localhost:5000/api/authors/${id}`).then(() => load()); }
  return (
    <div>
      <h2>Authors</h2>
      <form onSubmit={create}>
        <div><input placeholder="Name" value={name} onChange={e=>setName(e.target.value)} required style={{width:'100%'}}/></div>
        <div><input placeholder="Biography" value={bio} onChange={e=>setBio(e.target.value)} style={{width:'100%'}}/></div>
        <div><button type="submit">Add Author</button></div>
      </form>
      <ul>
        {authors.map(a=> (
          <li key={a.authorId}>
            <b>{a.name}</b> (<button onClick={()=>remove(a.authorId)}>Delete</button>)
            <div>{a.biography}</div>
            <div>Books: {a.books?.length ?? 0}</div>
          </li>
        ))}
      </ul>
    </div>
  );
}