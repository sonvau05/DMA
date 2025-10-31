import React, { useState, useEffect } from "react";
import axios from "axios";
export default function Books() {
  const [books, setBooks] = useState([]);
  const [title, setTitle] = useState("");
  const [genre, setGenre] = useState("");
  const [year, setYear] = useState(new Date().getFullYear());
  const [authorId, setAuthorId] = useState(0);
  const [authors, setAuthors] = useState([]);
  useEffect(()=>{ load(); }, []);
  function load(){ axios.get("http://localhost:5000/api/books").then(r=>setBooks(r.data)); axios.get("http://localhost:5000/api/authors").then(r=>setAuthors(r.data)); }
  function create(e){ e.preventDefault(); axios.post("http://localhost:5000/api/books", { title, genre, publicationYear: Number(year), authorId: Number(authorId) }).then(()=>{ setTitle(""); setGenre(""); setYear(new Date().getFullYear()); setAuthorId(0); load(); }); }
  function remove(id){ if(!window.confirm('Delete?')) return; axios.delete(`http://localhost:5000/api/books/${id}`).then(()=>load()); }
  return (
    <div>
      <h2>Books</h2>
      <form onSubmit={create}>
        <div><input placeholder="Title" value={title} onChange={e=>setTitle(e.target.value)} required style={{width:'100%'}}/></div>
        <div><input placeholder="Genre" value={genre} onChange={e=>setGenre(e.target.value)} style={{width:'100%'}}/></div>
        <div><input placeholder="Year" type="number" value={year} onChange={e=>setYear(e.target.value)} required style={{width:'100%'}}/></div>
        <div>
          <select value={authorId} onChange={e=>setAuthorId(e.target.value)} required style={{width:'100%'}}>
            <option value={0}>Select author</option>
            {authors.map(a=> <option key={a.authorId} value={a.authorId}>{a.name}</option>)}
          </select>
        </div>
        <div><button type="submit">Add Book</button></div>
      </form>
      <ul>
        {books.map(b=> (
          <li key={b.bookId}>
            <b>{b.title}</b> by {b.author?.name}
            <div>Genre: {b.genre} Year: {b.publicationYear}</div>
            <div><button onClick={()=>remove(b.bookId)}>Delete</button> <button onClick={()=>alert(JSON.stringify(b))}>Details</button></div>
          </li>
        ))}
      </ul>
    </div>
  );
}