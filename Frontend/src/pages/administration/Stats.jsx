import React, { useEffect, useState } from 'react';
import axios from 'axios';
import administrationLayout from '../../hoc/administrationLayout';

const Stats = () => {
  const [stats, setStats] = useState();

  useEffect(() => {
    axios.get("https://localhost:7116/Administration/GetStats", { withCredentials: true })
      .then(response => {
        localStorage.setItem("User", response.data.data.userDetails);
        setStats(response.data.data);
        console.log(response.data);
      })
      .catch(ex => {
        console.log(ex);
      });
  }, []);

  return (
    <>
      <div className="row mt-5 pt-5">
        <div className="col-xl-6 col-lg-6">
          <div className="excard l-bg-cherry text-center">
            <div className="excard-statistic-3 p-4">
              <div className="excard-icon excard-icon-large"><i className="fas fa-shopping-cart"></i></div>
              <div className="mb-4">
                <h5 className="excard-title mb-0">Viti shkollor aktiv (aktual)</h5>
              </div>
              <div className="row align-items-center mb-2 d-flex">
                <div className="col-8">
                  <h1 className="d-flex align-items-center mb-0 justify-content-center mga">
                    {stats?.currentYear}
                  </h1>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-xl-6 col-lg-6">
          <div className="excard l-bg-blue-dark text-center">
            <div className="excard-statistic-3 p-4">
              <div className="excard-icon excard-icon-large"><i className="fas fa-users"></i></div>
              <div className="mb-4">
                <h5 className="excard-title mb-0">Nxënës në vitin aktual</h5>
              </div>
              <div className="row align-items-center mb-2 d-flex">
                <div className="col-8">
                  <h1 className="d-flex align-items-center mb-0 justify-content-center mga">
                    {stats?.totalStudents}
                  </h1>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-xl-6 col-lg-6">
          <div className="excard l-bg-green-dark text-center">
            <div className="excard-statistic-3 p-4">
              <div className="excard-icon excard-icon-large"><i className="fas fa-ticket-alt"></i></div>
              <div className="mb-4">
                <h5 className="excard-title mb-0">Perioda aktuale (aktive)</h5>
              </div>
              <div className="row align-items-center mb-2 d-flex">
                <div className="col-8">
                  <h1 className="d-flex align-items-center mb-0 justify-content-center mga">
                    {stats?.currentPeriod}
                  </h1>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-xl-6 col-lg-6">
          <div className="excard l-bg-orange-dark text-center">
            <div className="excard-statistic-3 p-4">
              <div className="excard-icon excard-icon-large"><i className="fas fa-dollar-sign"></i></div>
              <div className="mb-4">
                <h5 className="excard-title mb-0">Arsimtar aktiv në këtë vit shkollorë</h5>
              </div>
              <div className="row align-items-center mb-2 d-flex">
                <div className="col-8">
                  <h1 className="d-flex align-items-center mb-0 justify-content-center mga">
                    {stats?.totalTeachers}
                  </h1>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-xl-6 col-lg-6">
          <div className="excard l-bg-blue-dark text-center">
            <div className="excard-statistic-3 p-4">
              <div className="excard-icon excard-icon-large"><i className="fas fa-users"></i></div>
              <div className="mb-4">
                <h5 className="excard-title mb-0">Vite shkollore te regjistruara</h5>
              </div>
              <div className="row align-items-center mb-2 d-flex">
                <div className="col-8">
                  <h1 className="d-flex align-items-center mb-0 justify-content-center mga">
                    {stats?.totalSchoolYears}
                  </h1>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-xl-6 col-lg-6">
          <div className="excard l-bg-cherry text-center">
            <div className="excard-statistic-3 p-4">
              <div className="excard-icon excard-icon-large"><i className="fas fa-shopping-cart"></i></div>
              <div className="mb-4">
                <h5 className="excard-title mb-0">Numri klasave aktive këtë vit</h5>
              </div>
              <div className="row align-items-center mb-2 d-flex">
                <div className="col-8">
                  <h1 className="d-flex align-items-center mb-0 justify-content-center mga">
                    {stats?.totalClasses}
                  </h1>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default administrationLayout(Stats);