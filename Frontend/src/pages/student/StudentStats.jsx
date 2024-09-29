import React, { useEffect, useState } from 'react';
import axios from 'axios';
import studentLayout from '../../hoc/studentLayout';

const StudentStats = () => {
  const [stats, setStats] = useState();

  useEffect(() => {
    const fetchStats = async () => {
      try {
        const response = await axios.get("https://localhost:7116/Student/GetStats", {
          withCredentials: true,
          credentials: 'include'
        });
        localStorage.setItem("User", response.data.data.studentName);
        setStats(response.data.data);
        console.log(response.data.data);
      } catch (ex) {
        console.error(ex);
      }
    };

    fetchStats();
  }, []);

  const statsCards = [
    { icon: "fas fa-shopping-cart", title: "Viti shkollor aktiv (aktual)", value: stats?.schoolYear, bgColor: "l-bg-cherry" },
    { icon: "fas fa-users", title: "Perioda aktuale (aktive)", value: stats?.period, bgColor: "l-bg-blue-dark" },
    { icon: "fas fa-ticket-alt", title: "Mungesa në periodën aktuale (aktive)", value: stats?.absences, bgColor: "l-bg-green-dark" },
    { icon: "fas fa-dollar-sign", title: "Klasa ime", value: stats?.class, bgColor: "l-bg-orange-dark" },
    { icon: "fas fa-users", title: "Kujdestari i klasës time", value: stats?.formTeacher, bgColor: "l-bg-blue-dark" },
    { icon: "fas fa-shopping-cart", title: "Numri lëndëve aktive në klasën time", value: stats?.courses, bgColor: "l-bg-cherry" }
  ];

  return (
    <div className="row mt-5 pt-5">
      {statsCards.map((card, index) => (
        <div className="col-xl-6 col-lg-6" key={index}>
          <div className={`excard ${card.bgColor} text-center`}>
            <div className="excard-statistic-3 p-4">
              <div className="excard-icon excard-icon-large"><i className={card.icon}></i></div>
              <div className="mb-4">
                <h5 className="excard-title mb-0">{card.title}</h5>
              </div>
              <div className="row align-items-center mb-2 d-flex">
                <div className="col-8">
                  <h1 className="d-flex align-items-center mb-0 mga">
                    {card.value}
                  </h1>
                </div>
              </div>
            </div>
          </div>
        </div>
      ))}
    </div>
  );
}

export default studentLayout(StudentStats);