import React, { useEffect, useState } from 'react';
import axios from 'axios';
import administrationLayout from '../../hoc/administrationLayout';

const AdministrationUnsubmittedReports = () => {
  const [reportId, setReportId] = useState();
  const [unsubmittedReports, setUnsubmittedReports] = useState();
  const [students, setStudents] = useState();

  useEffect(() => {
    fetchUnsubmittedReports();
  }, []);

  const fetchUnsubmittedReports = () => {
    axios.get("https://localhost:7116/Administration/GetUnsubmittedReports", { withCredentials: true })
      .then(response => {
        setUnsubmittedReports(response.data.data);
        console.log(response.data);
      })
      .catch(error => {
        console.log(error);
      });
  };

  const notifyTeacher = (id) => {
    axios.post(`https://localhost:7116/Administration/SendEmailToTeacher?courseId=${id}`, { withCredentials: true })
      .then(response => {
        console.log(response.data.data);
      })
      .catch(error => {
        console.log(error);
      });
  };

  return (
    <>
      <div className="mb-2 d-flex justify-content-between align-items-center pt-4">
        <div className="position-relative">
          <span className="position-absolute search"><i className="fa fa-search"></i></span>
          <input className="form-control w-100" placeholder="Filtro" />
        </div>
        <div className="px-2">
          <span>Filtrime <i className="fa fa-angle-down"></i></span>
          <i className="fa fa-ellipsis-h ms-3"></i>
        </div>
      </div>
      <div className="table-responsive">
        <table className="table table-responsive table-borderless">
          <thead>
            <tr className="bg-light">
              <th scope="col" width="5%"><input className="form-check-input" type="checkbox" /></th>
              <th scope="col" width="5%">#</th>
              <th scope="col" width="20%">Data</th>
              <th scope="col" width="10%">Statusi</th>
              <th scope="col" width="20%">Lënda</th>
              <th scope="col" width="20%">Klasa</th>
              <th scope="col" className="text-end" width="20%"><span>Veprimet</span></th>
            </tr>
          </thead>
          <tbody>
            {unsubmittedReports?.map((report, index) => (
              <tr key={index}>
                <th scope="row"><input className="form-check-input" type="checkbox" /></th>
                <td>{index + 1}</td>
                <td>{report.date.split(" ")[0]}</td>
                <td><i className="fa fa-dot-circle-o text-danger"></i><span className="ms-1">Pa Përfunduar</span></td>
                <td>{report.courseName}</td>
                <td>{report.className}</td>
                <td className="text-end">
                  <button type="button" className="btn btn-primary" onClick={() => notifyTeacher(report.courseId)}>
                    Njofto Arsimtarin
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </>
  );
};

export default administrationLayout(AdministrationUnsubmittedReports);