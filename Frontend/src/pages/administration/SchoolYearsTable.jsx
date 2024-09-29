import React, { useEffect, useState } from 'react';
import administrationLayout from "../../hoc/administrationLayout";
import axios from 'axios';
import ModalComponent from '../../components/ModalComponent';
import { Form } from 'react-bootstrap';
import { Link } from 'react-router-dom';

const SchoolYearsTable = () => {
  
  const [schoolYears, setSchoolYears] = useState();
  const [periodId, setPeriodId] = useState();
  const [number, setNumber] = useState();
  const [schoolYearNameToAdd, setSchoolYearNameToAdd] = useState();
  const [schoolYearNameToEdit, setSchoolYearNameToEdit] = useState();
  const [schoolYearIdToEdit, setSchoolYearToEdit] = useState();
  const [currentSchoolYearName, setCurrentSchoolYearName] = useState();

  useEffect(() => {
    axios.get("https://localhost:7116/Administration/GetSchoolYears",  { withCredentials: true})
      .then(response => {
        setSchoolYears(response.data.data);
        console.log(response.data.data);
      })
      .catch(ex => {
        console.log(ex);
      });
  }, [number]);

  const updateStatus = (id, status) => {
    setPeriodId(id);

    const periodToUpdate = {
      id: id,
      isActive: status
    };

    axios.post("https://localhost:7116/Administration/ChangePeriodStatus", periodToUpdate,  { withCredentials: true}).then(response => {
      console.log(response.data.data);
    }).catch(ex => {
      console.log(ex);
    });

    setNumber(Math.random() * 1524);
  };

  const savePeriod = () => {
    const periodToCreate = {
      name: schoolYearNameToAdd,
    };
    axios.post("https://localhost:7116/Administration/CreateSchoolYear", periodToCreate,  { withCredentials: true}).then(response => {
      setNumber(Math.random() * 5200);
    }).catch(ex => {
      console.log(ex);
    });
  };

  const modalFooterContent = () => { 
    return (
      <div style={{ width: "100%" }}>
        <button className="btn btn-default" onClick={() => savePeriod()}>Ruaj</button> 
      </div>
    );
  };

  const modalContent = () => {
    return (
      <>
        <b className='text-danger'></b>
        <br />
        <div className='mt-3'>
          <Form.Label>Viti shkollorë </Form.Label>
          <Form.Control placeholder='Sheno vitin shkollorë' onChange={(e) => setSchoolYearNameToAdd(e.target.value)} />
        </div>
      </>
    );
  };

  const setSchoolYearIdAndName = (periodId, name) => {
    setSchoolYearToEdit(periodId);
    setCurrentSchoolYearName(name);
  };

  const editSchoolYear = () => {
    const schoolYearToEdit = {
      id: schoolYearIdToEdit,
      schoolYear: schoolYearNameToEdit
    };

    axios.post("https://localhost:7116/Administration/EditSchoolYear", schoolYearToEdit,  { withCredentials: true}).then(response => {
      console.log(response.data.data);
      setNumber(Math.random() * 5000);
    }).catch(ex => {
      console.log(ex);
    });
  };

  const editModalContent = () => {
    return (
      <>
        <b className='text-danger'></b>
        <br />
        <div className='mt-3'>
          <Form.Label>Viti shkollorë </Form.Label>
          <Form.Control placeholder='Sheno vitin shkollorë' onChange={(e) => setSchoolYearNameToEdit(e.target.value)} defaultValue={currentSchoolYearName} />
        </div>
      </>
    );
  };

  const editModalFooterContent = () => { 
    return (
      <div style={{ width: "100%" }}>
        <button className="btn btn-default" onClick={() => editSchoolYear()}>Ruaj Ndryshimet</button> 
      </div>
    );
  };

  return (
    <>
      <div className="mb-2 d-flex justify-content-between align-items-center mt-3">
        <div className="position-relative mt-5">
          <span className="position-absolute search"><i className="fa fa-search"></i></span>
          <input className="form-control w-100" placeholder="Filtro" />
        </div>
        <div className="px-2">
          <Link tag="a" className="" to="/schoolYears"><span>Detajet</span></Link>
          <i className="fa fa-ellipsis-h ms-3"></i>
          <button type="button" className="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModalDefault">Shto Vit shkollorë</button>
        </div>
      </div>
      <div className="table-responsive">
        <table className="table table-responsive table-borderless">
          <thead>
            <tr className="bg-light">
              <th scope="col" width="5%"><input className="form-check-input" type="checkbox"/></th>
              <th scope="col" width="5%">#</th>
              <th scope="col" width="20%">Emri</th>
              <th scope="col" width="20%">Statusi</th>
              <th scope="col" width="20%">Veprime</th>
              <th scope="col" width="20%">Fshij</th>
            </tr>
          </thead>
          <tbody>
            {schoolYears?.map((schoolYear, index) => (
              <tr key={index}>
                <th scope="row">
                  <input
                    className="form-check-input"
                    type="checkbox"
                  />
                </th>
                <td>{index + 1}</td>
                <td>{schoolYear.schoolYear}</td>
                <td>{schoolYear.categoryName}</td>
                <td>{schoolYear.isActive ? "Aktive" : "Jo Aktive"}</td>
                <td><button className='btn btn-warning' data-bs-toggle="modal" data-bs-target="#editModalDefault" onClick={() => setSchoolYearIdAndName(schoolYear.id, schoolYear.schoolYear)}>Modifiko</button></td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      <ModalComponent title="Krijo Vit Shkollorë" footerContent={modalFooterContent()} content={modalContent()} dataBsBackdrop="static" id="exampleModalDefault"/>
      <ModalComponent title="Modifiko Vitin Shkollorë" footerContent={editModalFooterContent()} content={editModalContent()} dataBsBackdrop="static" id="editModalDefault"/>
    </>
  );
}

export default administrationLayout(SchoolYearsTable);