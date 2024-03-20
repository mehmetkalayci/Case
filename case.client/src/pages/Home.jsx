import { Link } from "react-router-dom";

export default function Home() {
  return (
    <>
      <h1>home</h1>
      <Link to={`/about`}>about</Link>
      <Link to={`/login`}>login</Link>
      <Link to={`/register`}>register</Link>
    </>
  );
}
