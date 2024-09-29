import React, { useEffect, useState } from "react";
import axios from "axios"; // Import Axios
import { navigate, useNavigate } from 'react-router-dom'; // Import navigate function
import 'react-perfect-scrollbar/dist/css/styles.css';
import PerfectScrollbar from 'react-perfect-scrollbar';
import { Link } from 'react-router-dom';

const Sidebar = () => {

    const navigate = useNavigate()
    const [user, setUser] = useState(null);
    const [a, setA] = useState(1)

    const logOut = () => {
        axios.get("https://localhost:7116/Authentication/logout", { withCredentials: true, credentials: 'include' })
        .then(response=>{
            localStorage.setItem("User", "")
            navigate("/login")
        })
        .catch(ex=>{
            console.log(ex)
        })
    }
    
        useEffect(() => {
            const storedUser = localStorage.getItem("User");
            const storedAdditionalData = localStorage.getItem("AdditionalData");
        
            if (storedUser) {
              setUser(storedUser); // First update
            }
        
            // Simulate another async operation or state update
            setTimeout(() => {
              if (storedAdditionalData) {
                setUser(prevData => prevData + ' ' + storedAdditionalData); // Append additional data
              }
            }, 1000); // Delay to simulate asynchronous data fetching
            changeA()
          }, [a]); 
    
          const changeA = () => {
            setA(a+1)
          }


    return (
        <div className="border-end sidenav" id="sidebar-wrapper">
            <div className="sidebar-heading border-bottom ">
                <Link to="/">
                    <img alt="Alt content" src='https://static.vecteezy.com/system/resources/previews/019/879/186/non_2x/user-icon-on-transparent-background-free-png.png' width={100} />
                </Link>
            </div>
            <PerfectScrollbar className="sidebar-items">
                <ul className="list-unstyled ps-0">
                    <li className="mb-1">
                        <Link tag="a" className="" to="/">
                            <i className="fa fa-dashboard"></i> Klasat
                        </Link>
                    </li>
                    <li className="mb-1">
                        <Link tag="a" className="" to="/unsubmitted-reports">
                            <i className="fa fa-file-o"></i> Raportet e paperfunduara
                        </Link>
                    </li>
                    <li className="mb-1">
                        <Link tag="a" className="" to="/submitted-reports">
                            <i className="fa fa-file-text"></i> Raportet e mija
                        </Link>
                    </li>
                    <li className="mb-1">
                        <Link tag="a" className="" to="/formTeacher">
                            <i className="fa fa-address-book-o"></i> Klasat e mia
                        </Link>
                    </li>
                    <li className="mb-1">
                        <Link tag="a" className="" to="/schedule">
                            <i className="fa fa-clock-o"></i> Orari im 
                        </Link>
                    </li>
                    <li className="border-top my-3"></li>
                    <li className="mb-1">
                      
                    </li>
                </ul>
            </PerfectScrollbar>
            <div className="dropdown fixed-bottom-dropdown">
                <a href="#" className="d-flex align-items-center text-decoration-none dropdown-toggle" id="dropdownUser2" data-bs-toggle="dropdown" aria-expanded="false">
                    <img src="https://via.placeholder.com/50" alt="" width="32" height="32" className="rounded-circle me-2" />
                    <span>{user}</span>
                </a>
                <ul className="dropdown-menu text-small shadow" aria-labelledby="dropdownUser2">
                    <li><Link className="dropdown-item" to="/profile"><i className="fa fa-user-circle" aria-hidden="true"></i> Profile</Link></li>
                    <li><hr className="dropdown-divider" /></li>
                    <li><Link className="dropdown-item" onClick={()=> logOut()}><i className="fa fa-sign-out" aria-hidden="true"></i> Sign out</Link></li>
                </ul>
            </div>
        </div>
    );
}

export default Sidebar;
