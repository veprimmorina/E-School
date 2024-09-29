import React, { useEffect, useState } from 'react';
import administrationLayout from "../../hoc/administrationLayout";
import axios from 'axios';
import ModalComponent from '../../components/ModalComponent';

const ClassesWithoutFormTeachers = () => {
  const [classes, setClasses] = useState([]);
  const [teachers, setTeachers] = useState();  
  const [className, setClassName] = useState();
  const [teacherId, setTeacherId] = useState();
  const [classId, setClassId] = useState();

  useEffect(() => {
    axios.get("https://localhost:7116/Classes/ClassesWithoutFormTeachers", { withCredentials: true })
      .then(response => {
        setClasses(response.data.data);
      })
      .catch(ex => {
        console.log(ex);
      });
  }, []);

  const saveFormTeacher = () => {
    const FormTeacher = { teacherId, classId };
    axios.post("https://localhost:7116/Classes/CreateFormTeacher", FormTeacher, { withCredentials: true })
      .then(response => {
        console.log(response.data.data);
      })
      .catch(ex => {
        console.log(ex);
      });
  };

  const modalFooterContent = () => (
    <div style={{ width: "100%" }}>
      <button className="btn btn-default" onClick={() => saveFormTeacher()}>Ruaj</button>
    </div>
  );

  const modalContent = () => (
    <>
      <b className='text-danger'>VEREJTE! Mesuesit qe gjenden ne liste e qe emri dhe mbiemri i tyre shfaqet me ngjyre te kuqe veqse jane kujdestare</b>
      <br />
      <div className='mt-3'>
        <b>Zgjedh kujdestarin Per klasen {className}</b>
        <select className='form-control' onChange={(e) => setTeacherId(e.target.value)}>
          <option value="" className='text-dark'>Zgjedh kujdestarin</option>
          {teachers?.map(teacher => (
            <option key={teacher.id} value={teacher.id} className={teacher.isRecommended ? "text-dark" : "text-danger"}>
              {teacher.firstName + " " + teacher.lastName}
            </option>
          ))}
        </select>
      </div>
    </>
  );

  const getTeachers = (className, idOfClass) => {
    setClassName(className);
    setClassId(idOfClass);
    axios.get("https://localhost:7116/Administration/GetRecommendedFormTeachers", { withCredentials: true })
      .then(response => {
        setTeachers(response.data.data);
      })
      .catch(ex => {
        console.log(ex);
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
              <th scope="col" width="20%">Klasa</th>
              <th scope="col" width="20%">Viti shkollor</th>
              <th scope="col" width="20%">Veprime</th>
            </tr>
          </thead>
          <tbody>
            {classes?.map((classItem, index) => (
              <tr key={index}>
                <th scope="row">
                  <input className="form-check-input" type="checkbox" />
                </th>
                <td>{index + 1}</td>
                <td>{classItem.className}</td>
                <td>{classItem.schoolYearName}</td>
                <td>
                  <button type="button" onClick={() => getTeachers(classItem.className, classItem.classId)} className="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModalDefault">
                    Shto kujdestar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      <ModalComponent title="Zgjedh kujdestarin" footerContent={modalFooterContent()} content={modalContent()} dataBsBackdrop="static" id="exampleModalDefault" />
    </>
  );
};

export default administrationLayout(ClassesWithoutFormTeachers);