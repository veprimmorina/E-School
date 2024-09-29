import React, { useEffect, useState } from 'react';
import adminLayout from "../../hoc/adminLayout";
import axios from 'axios';
import ModalComponent from '../../components/ModalComponent';
import { Form, FormControl } from 'react-bootstrap';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';

const SubmittedReports = () => {
  
  const [reports, setReports] = useState([]);
  const [reportDetails, setReportDetails] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");

  useEffect(() => {
    axios.get("https://localhost:7116/Teacher/GetMyReports", { withCredentials: true })
      .then(response => {
        setReports(response.data.data);
        console.log(response.data.data);
      })
      .catch(ex => {
        console.log(ex);
      });
  }, []);

  const getReportDetails = (id) => {
    axios.get(`https://localhost:7116/Administration/GetReportDetails?reportId=${id}`, { withCredentials: true })
      .then(response => {
        setReportDetails(response.data.data);
        console.log(response.data.data);
      })
      .catch(ex => {
        console.log(ex);
      });
  };

  const printReport = () => {
    const element = document.getElementById('report-details');
    const width = element.offsetWidth; 
    element.style.visibility = 'visible';
    html2canvas(element, { width: width }).then(canvas => {
      const imgData = canvas.toDataURL('image/png');
      const pdf = new jsPDF('p', 'px', [width, canvas.height]);
      pdf.addImage(imgData, 'PNG', 0, 0, width, canvas.height);
      pdf.save('report.pdf');
    });
    element.style.visibility = 'hidden';
  };
  

  const modalFooterContent = () => { 
    return (
      <>
        <div style={{ width: "100%" }}>
          <button className="btn btn-default" onClick={() => printReport()}>Printo</button> 
        </div>
      </>
    );
  };

  const modalContent = () => {
    return (
      <Form.Group>
        <Form.Label>Pershkrimi: </Form.Label>
        {reportDetails?.map((report) => ( 
          <>
            <textarea type="text" className='form-control' value={report?.details} disabled></textarea>   
            <Form.Label>Mungesat: </Form.Label>
            {report.absences?.map((absence) => (
              absence.studentId != null ? 
                <select disabled className='form-control'>
                  <option value={absence.studentId}>{absence.studentName + " " + absence.studentLastName}</option>
                </select> : <p>Nuk ka mungesa</p>
            ))}
            <Form.Label>Verejtjet: </Form.Label>
            {report.remarks?.map((remark) => (
              remark.studentId != null ? 
                <>
                  <textarea type="text" className='form-control' value={remark?.description} disabled></textarea>
                  <p>Nxënësit</p>
                  <select disabled className='form-control'>
                    <option value={remark.studentId}>{remark.studentName + " " + remark.studentLastName}</option>
                  </select> 
                </> : <p>Nuk ka verejtje</p>
            ))}
          </>
        ))}
      </Form.Group>
    );
  };

  const handleSearch = (e) => {
    setSearchTerm(e.target.value);
  };

  const filteredReports = reports.filter(report =>
    report.courseName.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <>
      <div className="mb-2 d-flex justify-content-between align-items-center pt-5">
        <div className="position-relative">
          <span className="position-absolute search"><i className="fa fa-search"></i></span>
          <input className="form-control w-100" placeholder="Search by course name..."
            value={searchTerm}
            onChange={handleSearch}
          />
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
              <th scope="col" width="20%">Ora</th>
              <th scope="col" width="10%">Statusi</th>
              <th scope="col" width="20%">Lënda</th>
              <th scope="col" className="text-end" width="20%"><span>Veprimet</span></th>
            </tr>
          </thead>
          <tbody>
            {filteredReports.map((report, index) => (
              <tr key={index}>
                <th scope="row"><input className="form-check-input" type="checkbox" /></th>
                <td>{index + 1}</td>
                <td>{report?.date.split("T")[0]}</td>
                <td>{report?.date.split("T")[1]}</td>
                <td><i className="fa fa-check-circle-o green"></i><span className="ms-1">Përfunduar</span></td>
                <td>{report.courseName}</td>
                <td className="text-end">
                  <span className="fw-bolder"></span>
                  <button type="button" className="btn btn-primary" data-bs-toggle="modal" data-bs-target="#leftModalDefault" onClick={() => getReportDetails(report.id)}>
                    Detajet
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      <ModalComponent title="Detajet e raportit" footerContent={modalFooterContent()} content={modalContent()} className="left" dataBsBackdrop="static" id="leftModalDefault"/>
      <div id='report-details' style={{ visibility: "hidden" }}>
        <div className="card">
          <div className="card-header bg-black"></div>
          <div className="card-body">

            <div className="container">
              <div className="row">
                <div className="col-xl-12">
                  <i className="far fa-building text-danger fa-6x float-start"></i>
                </div>
              </div>

              <div className="row">
                <div className="col-xl-12">
                  <ul className="list-unstyled float-end">
                    <li style={{ fontSize: "30px", color: "red" }}>E-Shkolla</li>
                    <li>Platforma digjitale</li>
                    <li>123-456-789</li>
                    <li>eshkolla@gmail.com</li>
                  </ul>
                </div>
              </div>

              <div className="row text-center">
                <h3 className="text-uppercase text-center mt-3" style={{ fontSize: "40px" }}>Raporti</h3>
                <b className='pt-3'>{ reportDetails?.map((report) => ( report.courseName ))}</b>
              </div>
              <div className='text-center pt-3 pb-3'>
                <b>Përshkrimi</b>
                {reportDetails?.map((report) => ( 
                  <>
                    <textarea type="text" className='form-control' value={report?.details} disabled></textarea>   
                  </>
                ))}
              </div>
              <div className="row mx-3">
                <b className='text-center pt-3 pb-2'>Mungesat</b>
                {
                  reportDetails?.map((report) => ( 
                    report.absences?.map((absence) => (
                      absence.studentId != null ? 
                      ""
                      : "Nuk ka mungesa"))))
                }
                <table className="table">
                  <thead>
                    <tr>
                      <th scope="col">Emri</th>
                      <th scope="col">Mbiemri</th>
                    </tr>
                  </thead>
                  <tbody>
                    {
                      reportDetails?.map((report) => ( 
                        report.absences?.map((absence) => (
                          absence.studentId != null ? 
                          <>
                            <tr>
                              <td>{absence.studentName}</td>
                              <td><i className="fas fa-dollar-sign"></i>{absence.studentLastName}</td>
                            </tr>
                          </> : "Nuk ka mungesa"
                        ))
                      ))
                    }
                  </tbody>
                </table>
              </div>

              <div className="row mx-3">
                <b className='text-center pb-3'>Vërejte</b>
                {
                  reportDetails?.map((report) => ( 
                    report.remarks?.map((remark) => (
                      remark.studentId != null ? 
                      <textarea type="text" className='form-control' value={remark?.description} disabled></textarea> 
                      : "Nuk ka verejtje"
                    ))
                  ))
                }
              </div>

            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default adminLayout(SubmittedReports);