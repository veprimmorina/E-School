import React, { useEffect, useState } from 'react';
import administrationLayout from "../../hoc/administrationLayout";
import axios from 'axios';
import ModalComponent from '../../components/ModalComponent';
import { Form } from 'react-bootstrap';

const PeriodsTable = () => {
  
  const [periods, setPeriods] = useState([]);
  const [periodId, setPeriodId] = useState(null);
  const [number, setNumber] = useState(0);
  const [periodNameToAdd, setPeriodNameToAdd] = useState('');
  const [periodNameToEdit, setPeriodNameToEdit] = useState('');
  const [periodIdToEdit, setPeriodIdToEdit] = useState(null);
  const [currentPeriodName, setCurrentPeriodName] = useState('');

  useEffect(() => {
    axios.get("https://localhost:7116/Administration/GetPeriods",  { withCredentials: true})
      .then(response => {
        setPeriods(response.data.data);
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
      isActive: status,
    };

    axios.post("https://localhost:7116/Administration/ChangePeriodStatus", periodToUpdate, { withCredentials: true })
      .then(response => {
        console.log(response.data.data);
      })
      .catch(ex => {
        console.log(ex);
      });

    setNumber(Math.random() * 1524);
  };

  const savePeriod = () => {
    const periodToCreate = {
      name: periodNameToAdd,
    };
    axios.post("https://localhost:7116/Administration/CreatePeriod", periodToCreate, { withCredentials: true })
      .then(response => {
        setNumber(Math.random() * 5200);
      })
      .catch(ex => {
        console.log(ex);
      });
  };

  const modalFooterContent = () => (
    <div style={{ width: "100%" }}>
      <button className="btn btn-default" onClick={savePeriod}>Ruaj</button> 
    </div>
  );

  const modalContent = () => (
    <>
      <b className='text-danger'></b>
      <br />
      <div className='mt-3'>
        <Form.Label>Emri periodes</Form.Label>
        <Form.Control placeholder='Sheno emrin e periodes' onChange={(e) => setPeriodNameToAdd(e.target.value)} />
      </div>
    </>
  );

  const setPeriodIdAndName = (periodId, name) => {
    setPeriodIdToEdit(periodId);
    setCurrentPeriodName(name);
  };

  const editPeriod = () => {
    const periodToEdit = {
      id: periodIdToEdit,
      name: periodNameToEdit,
    };

    axios.post("https://localhost:7116/Administration/EditPeriod", periodToEdit, { withCredentials: true })
      .then(response => {
        console.log(response.data.data);
        setNumber(Math.random() * 5000);
      })
      .catch(ex => {
        console.log(ex);
      });
  };

  const deletePeriod = (id) => {
    axios.delete(`https://localhost:7116/Administration/DeletePeriod?id=${id}`, { withCredentials: true })
      .then(response => {
        console.log(response.data.data);
        setNumber(Math.random() * 5000);
      })
      .catch(err => {
        console.log(err);
      });
  };

  const editModalContent = () => (
    <>
      <b className='text-danger'></b>
      <br />
      <div className='mt-3'>
        <Form.Label>Emri periodes</Form.Label>
        <Form.Control 
          placeholder='Sheno emrin e periodes' 
          onChange={(e) => setPeriodNameToEdit(e.target.value)} 
          defaultValue={currentPeriodName} 
        />
      </div>
    </>
  );

  const editModalFooterContent = () => (
    <div style={{ width: "100%" }}>
      <button className="btn btn-default" onClick={editPeriod}>Ruaj Ndryshimet</button> 
    </div>
  );

  return (
    <>
      <div className="mb-2 d-flex justify-content-between align-items-center mt-3">
        <div className="position-relative mt-5">
          <span className="position-absolute search"><i className="fa fa-search"></i></span>
          <input className="form-control w-100" placeholder="Filtro" />
        </div>
        <div className="px-2">
          <span>Filtrime <i className="fa fa-angle-down"></i></span>
          <i className="fa fa-ellipsis-h ms-3"></i>
          <button type="button" className="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModalDefault">Shto period</button>
        </div>
      </div>
      <div className="table-responsive">
        <table className="table table-responsive table-borderless">
          <thead>
            <tr className="bg-light">
              <th scope="col" width="5%"><input className="form-check-input" type="checkbox" /></th>
              <th scope="col" width="5%">#</th>
              <th scope="col" width="20%">Emri</th>
              <th scope="col" width="20%">Statusi</th>
              <th scope="col" width="20%">Veprime</th>
              <th scope="col" width="20%">Fshij</th>
            </tr>
          </thead>
          <tbody>
            {periods?.map((period, index) => (
              <tr key={index}>
                <th scope="row">
                  <input className="form-check-input" type="checkbox" />
                </th>
                <td>{index + 1}</td>
                <td>{period.name}</td>
                <td>{period.is_active ? "Aktive" : "Jo Aktive"}</td>
                <td>
                  {period.is_active ? "" : 
                    <button type="button" className="btn btn-primary" onClick={() => updateStatus(period.id, true)}>Aktivizo</button>}
                  <button className='btn btn-warning' data-bs-toggle="modal" data-bs-target="#editModalDefault" onClick={() => setPeriodIdAndName(period.id, period.name)}>Modifiko</button>
                </td>
                <td>
                  <button className='btn btn-danger' onClick={() => deletePeriod(period.id)}>Fshij</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      <ModalComponent title="Krijo period" footerContent={modalFooterContent()} content={modalContent()} dataBsBackdrop="static" id="exampleModalDefault" />
      <ModalComponent title="Modifiko perioden" footerContent={editModalFooterContent()} content={editModalContent()} dataBsBackdrop="static" id="editModalDefault" />
    </>
  );
};

export default administrationLayout(PeriodsTable);