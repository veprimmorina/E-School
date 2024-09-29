import React, { useEffect, useState } from 'react';
import administrationLayout from "../../hoc/administrationLayout";
import axios from 'axios';
import ModalComponent from '../../components/ModalComponent';

const ClassesFormTeachers = () => {
  const [classes, setClasses] = useState([]);
  const [teachers, setTeachers] = useState();
  const [className, setClassName] = useState();
  const [teacherId, setTeacherId] = useState();
  const [currentTeacherId, setCurrentTeacherId] = useState();
  const [classId, setClassId] = useState();
  const [id, setId] = useState();

  useEffect(() => {
    fetchFormTeachers();
  }, []);

  const fetchFormTeachers = () => {
    axios.get("https://localhost:7116/Administration/GetFormTeachers", { withCredentials: true })
      .then(response => {
        setClasses(response.data.data);
        console.log(response.data.data);
      })
      .catch(error => {
        console.log(error);
      });
  };

  const fetchTeachers = (className, classId, id, teacherId) => {
    setId(id);
    setClassName(className);
    setClassId(classId);
    setCurrentTeacherId(teacherId);
    
    axios.get("https://localhost:7116/Administration/GetRecommendedFormTeachers", { withCredentials: true })
      .then(response => {
        setTeachers(response.data.data);
      })
      .catch(error => {
        console.log(error);
      });
  };

  const updateFormTeacher = () => {
    const formTeacher = {
      id: id,
      teacherId: teacherId,
      classId: classId
    };

    axios.post("https://localhost:7116/Classes/EditFormTeacher", formTeacher, { withCredentials: true })
      .then(response => {
        console.log(response.data.data);
      })
      .catch(error => {
        console.log(error);
      });
  };

  const modalFooterContent = () => { 
    return (
      <div style={{ width: "100%" }}>
        <button className="btn btn-default" onClick={updateFormTeacher}>Ruaj</button> 
      </div>
    );
  };

  const modalContent = () => {
    return (
      <>
        <b className='text-danger'>
          VEREJTE! Mesuesit qe gjenden ne liste e qe emri dhe mbiemri i tyre shfaqet me ngjyre te kuqe veqse jane kujdestare
        </b>
        <br />
        <div className='mt-3'>
          <b>Zgjedh kujdestarin Per klasen {className}</b>
          <select className='form-control' onChange={(e) => setTeacherId(e.target.value)}>
            <option value="" className='text-dark'>Zgjedh kujdestarin</option>
            {teachers?.map(teacher => (
              <option
                key={teacher.id}
                value={teacher.id}
                className={teacher.isRecommended ? "text-dark" : "text-danger"}
                selected={teacher.id === currentTeacherId}
              >
                {teacher.firstName + " " + teacher.lastName}
              </option>
            ))}
          </select>
        </div>
      </>
    );
  };

  return (
    <>
      <div className="container text-center">
        <div className="logo">
          <h1><b>Klasat</b></h1>
        </div>
        <h1>Klasat dhe kujdestaret per kete vit shkollor</h1>
      </div>

      <div className="container">
        <div className="row">
          {classes?.map((classItem, index) => (
            <div className="col-md-4" key={index}>
              <h4 className="text-center"><strong>{classItem.className}</strong></h4>
              <div className="profile-card-4 text-center">
                <img src="" style={{ maxWidth: "100%", height: "auto" }} className="img img-responsive" alt="Profile" />
                <div className="profile-content">
                  <div className="row">
                    <div className="col-xs-4">
                      <div className="profile-overview">
                        <p>Kujdestari</p>
                        <h4>{classItem.teacherName + " " + classItem.teacherLastName}</h4>
                      </div>
                    </div>
                    <div className="col-xs-4">
                      <div className="profile-overview">
                        <p>Nxenes</p>
                        <h4>2</h4>
                      </div>
                      <button
                        type="button"
                        onClick={() => fetchTeachers(classItem.className, classItem.classId, classItem.id, classItem.teacherId)}
                        className="btn btn-primary"
                        data-bs-toggle="modal"
                        data-bs-target="#exampleModalDefault"
                      >
                        Ndrysho kujdestarin
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>
      
      <ModalComponent 
        title={`Ndrysho kujdestarin per ${className}`} 
        footerContent={modalFooterContent()} 
        content={modalContent()} 
        dataBsBackdrop="static" 
        id="exampleModalDefault" 
      />
    </>
  );
}

export default administrationLayout(ClassesFormTeachers);