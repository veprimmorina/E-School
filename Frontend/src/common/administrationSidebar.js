import React, { useEffect, useState } from "react";
import 'react-perfect-scrollbar/dist/css/styles.css';
import PerfectScrollbar from 'react-perfect-scrollbar';
import { Link, useNavigate } from 'react-router-dom';
import axios from "axios";

const AdministrationSidebar = () => {
    const navigate = useNavigate();
    const [user, setUser] = useState(null);
    const [a, setA] = useState(1);

    const logOut = () => {
        axios.get("https://localhost:7116/Authentication/logout", { withCredentials: true, credentials: 'include' })
            .then(() => {
                localStorage.setItem("User", "");
                navigate("/login");
            })
            .catch(ex => {
                console.log(ex);
            });
    };

    useEffect(() => {
        const storedUser = localStorage.getItem("User");
        const storedAdditionalData = localStorage.getItem("AdditionalData");

        if (storedUser) {
            setUser(storedUser);
        }

        setTimeout(() => {
            if (storedAdditionalData) {
                setUser(prevData => prevData + ' ' + storedAdditionalData);
            }
        }, 1000);

        changeA();
    }, [a]);

    const changeA = () => {
        setA(a + 1);
    };

    return (
        <div className="border-end sidenav" id="sidebar-wrapper">
            <div className="sidebar-heading border-bottom">
                <Link to="/">
                    <img alt="User Icon" src='https://static.vecteezy.com/system/resources/previews/019/879/186/non_2x/user-icon-on-transparent-background-free-png.png' width={100} />
                </Link>
            </div>
            <PerfectScrollbar className="sidebar-items">
                <ul className="list-unstyled ps-0">
                    <li className="mb-1">
                        <Link to="/administrationStats">
                            <i className="fa fa-dashboard"></i> Statistikat
                        </Link>
                    </li>
                    <li className="mb-1">
                        <Link to="/administrationUnsubmittedReports">
                            <i className="fa fa-file-o"></i> Raportet e paperfunduara
                        </Link>
                    </li>
                    <li className="mb-1">
                        <Link to="/classesWithoutFormTeacher">
                            <i className="fa fa-address-book-o"></i> Klasat pa kujdestarë
                        </Link>
                    </li>
                    <li className="mb-1">
                        <Link to="/classesFormTeacher">
                            <i className="fa fa-address-book-o"></i> Kujdestaria
                        </Link>
                    </li>
                    <li className="mb-1">
                        <Link to="/periods">
                            <i className="fa fa-file-o"></i> Periodat
                        </Link>
                    </li>
                    <li className="mb-1">
                        <Link to="/schoolYearsTable">
                            <i className="fa fa-clock-o"></i> Vitet shkollore
                        </Link>
                    </li>
                    <li className="mb-1">
                        <Link to="/administrationSchedule">
                            <i className="fa fa-clock-o"></i> Orari
                        </Link>
                    </li>
                    <li className="mb-1">
                        <Link to="/administrationCurrentHours">
                            <i className="fa fa-clock-o"></i> Orët tani
                        </Link>
                    </li>
                    <li className="border-top my-3"></li>
                </ul>
            </PerfectScrollbar>
            <div className="dropdown fixed-bottom-dropdown">
                <a href="#" className="d-flex align-items-center text-decoration-none dropdown-toggle" id="dropdownUser2" data-bs-toggle="dropdown" aria-expanded="false">
                    <img src="https://via.placeholder.com/50" alt="" width="32" height="32" className="rounded-circle me-2" />
                    <span>{user}</span>
                </a>
                <ul className="dropdown-menu text-small shadow" aria-labelledby="dropdownUser2">
                    <li><Link className="dropdown-item" to="/profile"><i className="fa fa-user-circle" aria-hidden="true"></i> Profili</Link></li>
                    <li><hr className="dropdown-divider" /></li>
                    <li><Link className="dropdown-item" onClick={logOut}><i className="fa fa-sign-out" aria-hidden="true"></i> Ckycu</Link></li>
                </ul>
            </div>
        </div>
    );
};

export default AdministrationSidebar;