import React, { useEffect, useState } from 'react';
import administrationLayout from "../../hoc/administrationLayout";
import axios from 'axios';
import ModalComponent from '../../components/ModalComponent';

const SchoolYears = () => {
  
  const [schoolYears, setSchoolYears] = useState([]);
  const [number, setNumber] = useState();

  useEffect(() => {
    axios.get("https://localhost:7116/Administration/GetSchoolYears", { withCredentials: true })
      .then(response => {
        setSchoolYears(response.data.data);
      })
      .catch(ex => {
        console.log(ex);
      });
  }, [number]);
  
  const modalFooterContent = () => { 
    return (
      <div style={{ width: "100%" }}>
        <button className="btn btn-default">Ruaj</button> 
      </div>
    );
  }

  const modalContent = () => {
    return (
      <>
        <b className='text-danger'>VEREJTE! Mesuesit qe gjenden ne liste e qe emri dhe mbiemri i tyre shfaqet me ngjyre te kuqe veqse jane kujdestare</b>
        <br />
        <div className='mt-3'>
          <b>Zgjedh kujdestarin Per klasen </b>
        </div>
      </>
    );
  }

  const updateStatus = (id, status) => {
    const schoolYearToUpdate = {
      id: id,
      isActive: status
    }

    axios.post("https://localhost:7116/Administration/ChangeSchoolYearStatus", schoolYearToUpdate, { withCredentials: true })
      .then(response => {
        console.log(response.data.data);
      })
      .catch(ex => {
        console.log(ex);
      });

    setNumber(Math.random() * 1524);
  }

  return (
    <>
      <div className="container-fluid bg-gradient p-5">
        <div className="row m-auto text-center w-75">
          {schoolYears?.map((schoolYear, index) => (
            schoolYear.isActive ? 
              <div className="col-4 pricing-item blue" key={index}>
                <div className="pricing-divider">
                  <h3 className="text-light"> {schoolYear.categoryName}</h3>
                </div>
                <div className="card-body bg-white mt-0 shadow">
                  <ul className="list-unstyled mb-5 position-relative">
                    <li><b>100 </b>Nxenes</li>
                    <li><b>10</b> Mesues</li>
                    <li><b>Free</b>Klasa</li>
                    <li><b>Help center access</b></li>
                  </ul>
                  <div className='d-flex'>
                    <b>Aktiv</b>
                  </div>
                </div>
              </div>
            :
              <div className="col-4 pricing-item" key={index}>
                <div className="pricing-divider">
                  <h3 className="text-light">{schoolYear.categoryName}</h3>
                </div>
                <div className="card-body bg-white mt-0 shadow">
                  <ul className="list-unstyled mb-5 position-relative">
                    <li><b>{schoolYear.usersIncluded}</b> users included</li>
                    <li><b>{schoolYear.storage}</b> of storage</li>
                    <li><b>{schoolYear.emailSupport}</b> Email support</li>
                    <li><b>{schoolYear.helpCenterAccess}</b> Help center access</li>
                  </ul>
                  <button type="button" className="btn btn-lg btn-block btn-custom btn-primary" onClick={() => updateStatus(schoolYear.id, schoolYear.isActive)}>Aktivizo</button>
                </div>
              </div>
          ))}
        </div>
      </div>
      <ModalComponent title={"Ndrysho kujdestarin per "} footerContent={modalFooterContent()} content={modalContent()} dataBsBackdrop="static" id="exampleModalDefault"/>
    </>
  );
}

export default administrationLayout(SchoolYears);