import React, { useEffect, useState } from 'react';
import adminLayout from "../../hoc/adminLayout";
import axios from 'axios';
import ModalComponent from '../../components/ModalComponent';
import { Form } from 'react-bootstrap';
import { useParams } from 'react-router-dom';

const UnsubmittedReports = () => {
  
  const [unsubmittedReports, setUnsubmittedReports] = useState([]);
  const [students, setStudents] = useState([]);
  const [selects, setSelects] = useState([{ id: 0, value: '' }]);
  const [additionalSelects, setAdditionalSelects] = useState([{ id: 0, value: '' }]);
  const [reportId, setReportId] = useState();
  const [userAbsenceIds, setUserAbsenceIds] = useState([]);
  const [userRemarkIds, setUserRemarkIds] = useState([]);
  const [reportDescription, setReportDescription] = useState();
  const [remarkDescription, setRemarkDescription] = useState();
  const { id } = useParams();
  const [searchTerm, setSearchTerm] = useState("");

  useEffect(() => {
    axios.get("https://localhost:7116/Teacher/GetUnsubmittedReports", { withCredentials: true })
      .then(response => {
        setUnsubmittedReports(response.data.data);
        console.log(response.data);
      })
      .catch(ex => {
        console.log(ex);
      });
  }, []);

  const handleSearch = (e) => {
    setSearchTerm(e.target.value);
  };

  const getStudentsOfClass = (classId, reportId) => {
    setReportId(reportId);
    axios.get("https://localhost:7116/Classes/GetStudentsOfClass?classId=" + classId, { withCredentials: true })
      .then(response => {
        setStudents(response.data.data);
        console.log(response.data);
      })
      .catch(ex => {
        console.log(ex);
      });
  };

  const handleAddAdditionalSelect = () => {
    const newId = additionalSelects.length;
    setAdditionalSelects([...additionalSelects, { id: newId, value: '' }]);
  };

  const handleAddSelect = () => {
    const newId = selects.length;
    setSelects([...selects, { id: newId, value: '' }]);
  };

  const handleAdditionalSelectChange = (id, value) => {
    const updatedSelects = additionalSelects.map((select) =>
      select.id === id ? { ...select, value } : select
    );
    setAdditionalSelects(updatedSelects);

    setUserRemarkIds(prevIds => {
      if (!prevIds.includes(value)) {
        return [...prevIds, value];
      }
      return prevIds;
    });
  };

  const handleSelectChange = (id, value) => {
    const updatedSelects = selects.map((select) =>
      select.id === id ? { ...select, value } : select
    );
    setSelects(updatedSelects);

    setUserAbsenceIds(prevIds => {
      if (!prevIds.includes(value)) {
        return [...prevIds, value];
      }
      return prevIds;
    });
  };

  const saveReport = () => {
    const report = {
      reportId: reportId,
      description: reportDescription,
      date: new Date(),
      absences: userAbsenceIds?.map(userId => ({
        userId: userId,
      })),
      remark: {
        description: remarkDescription,
        userIds: userRemarkIds
      }
    };
    axios.post("https://localhost:7116/Report", report, { withCredentials: true })
      .then(response => {
        console.log(response);
      })
      .catch(ex => {
        console.log(ex);
      });
  };

  const modalFooterContent = () => { 
    return (
      <>
        <div style={{ width: "100%" }}>
          <button className="btn btn-default" onClick={saveReport}>Ruaj</button> 
        </div>
      </>
    );
  };

  const modalContent = () => {
    return (
      <>
        <Form.Group>
          <Form.Label>Pershkrimi: </Form.Label>
          <textarea type="text" className='form-control' placeholder="Shkruaj ketu" onChange={(e) => setReportDescription(e.target.value)}></textarea>    
          <Form.Label>Mungesat: </Form.Label>
          {selects.map((select) => (
            <select
              key={select.id}
              value={select.value}
              onChange={(e) => handleSelectChange(select.id, e.target.value)}
              className='form-control'
            >
              <option>Zgjedh Nxenesit</option>
              {students?.map((student) => (
                <option value={student.userId} key={student.userId}>{student.firstName + " " + student.lastName}</option>
              ))}
            </select>
          ))}
          <button onClick={handleAddSelect} className='btn btn-sm btn-primary text-center mb-3 border-bottom'>Shto mungesa</button>
          <br />
          <Form.Label>Verejtjet: </Form.Label>
          <textarea type="text" className='form-control' placeholder="Pershkrimi" onChange={(e) => setRemarkDescription(e.target.value)}></textarea> 
          <Form.Label>Verejtje per nxenesit: </Form.Label>
          {additionalSelects.map((select) => (
            <select
              key={select.id}
              value={select.value}
              onChange={(e) => handleAdditionalSelectChange(select.id, e.target.value)}
              className='form-control'
            >
              <option>Zgjedh Nxenesit</option>
              {students?.map((student) => (
                <option value={student.userId} key={student.userId}>{student.firstName + " " + student.lastName}</option>
              ))}
            </select>
          ))}
          <button onClick={handleAddAdditionalSelect} className='btn btn-sm btn-primary text-center'>Shto Nxenes ne verejte</button>
        </Form.Group>
      </>
    );
  };

  return (
    <>
      <div className="mb-2 d-flex justify-content-between align-items-center pt-4">
        <div className="position-relative">
          <span className="position-absolute search"><i className="fa fa-search"></i></span>
          <input className="form-control w-100" placeholder="Filtro" value={searchTerm} onChange={handleSearch} />
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
            {unsubmittedReports
              .filter(report => report.courseName.toLowerCase().includes(searchTerm.toLowerCase()))
              .map((report, index) => (
                <tr key={index} style={id != null && report.id === id ? { backgroundColor: "darkgrey" } : null}>
                  <th scope="row"><input className="form-check-input" type="checkbox" /></th>
                  <td>{index + 1}</td>
                  <td>{report.date.split(" ")[0]}</td>
                  <td><i className="fa fa-dot-circle-o text-danger"></i><span className="ms-1">Pa Përfunduar</span></td>
                  <td>{report.courseName}</td>
                  <td>{report.className}</td>
                  <td className="text-end">
                    <span className="fw-bolder"></span> 
                    <button type="button" className="btn btn-primary" data-bs-toggle="modal" data-bs-target="#leftModalDefault" onClick={() => getStudentsOfClass(report.classId, report.id)}>
                      Ploteso
                    </button>
                  </td>
                </tr>
              ))}
          </tbody>
        </table>
      </div>
      <ModalComponent title="Ploteso raportin" footerContent={modalFooterContent()} content={modalContent()} className="left" dataBsBackdrop="static" id="leftModalDefault" />
    </>
  );
};

export default adminLayout(UnsubmittedReports);
