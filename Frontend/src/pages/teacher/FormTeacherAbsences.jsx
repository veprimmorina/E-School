import React, { useEffect, useState } from 'react';
import adminLayout from '../../hoc/adminLayout';
import axios from 'axios';
import { useParams } from 'react-router-dom';

const FormTeacherAbsences = () => {
  const [absences, setAbsences] = useState([]);
  const [selectedRows, setSelectedRows] = useState(new Set());
  const [studentAbsencesToEdit, setStudentAbsencesToEdit] = useState([]);
  const { id } = useParams();

  useEffect(() => {
    const fetchAbsences = async () => {
      try {
        const response = await axios.get(`https://localhost:7116/Classes/GetNewAbsencesForStudents?classId=${id}`, {
          withCredentials: true,
        });
        setAbsences(response.data.data);
      } catch (error) {
        console.error('Error fetching absences:', error);
      }
    };

    fetchAbsences();
  }, [id]);

  const toggleRowSelection = (id) => {
    const updatedSelectedRows = new Set(selectedRows);
    updatedSelectedRows.has(id) ? updatedSelectedRows.delete(id) : updatedSelectedRows.add(id);
    setSelectedRows(updatedSelectedRows);
  };

  const setAbsencesToEdit = (e, absenceId) => {
    const value = e.target.value === 'true' ? true : e.target.value === 'false' ? false : null;
    const updatedAbsences = studentAbsencesToEdit.filter(absence => absence.absenceId !== absenceId);
    updatedAbsences.push({ absenceId, value });
    setStudentAbsencesToEdit(updatedAbsences);
  };

  const saveChanges = async () => {
    const selectedAbsencesData = absences
      .filter(absence => selectedRows.has(absence.id))
      .map(absence => ({
        id: absence.id,
        reasonable: studentAbsencesToEdit.find(item => item.absenceId === absence.id)?.value,
      }));

    try {
      const response = await axios.post('https://localhost:7116/Classes/EditAbsence', selectedAbsencesData, {
        withCredentials: true,
      });
      console.log('Response from saving changes:', response.data);
    } catch (error) {
      console.error('Error saving changes:', error);
    }
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
        <table className="table table-borderless">
          <thead>
            <tr className="bg-light">
              <th scope="col" width="5%"><input className="form-check-input" type="checkbox" /></th>
              <th scope="col" width="5%">#</th>
              <th scope="col" width="20%">Emri</th>
              <th scope="col" width="20%">Mbiemri</th>
              <th scope="col" width="20%">Viti shkollorë</th>
              <th scope="col" width="20%">Lenda</th>
              <th scope="col" width="10%">Perioda</th>
              <th scope="col" width="20%">Statusi</th>
            </tr>
          </thead>
          <tbody>
            {absences.map((absence, index) => (
              <tr key={absence.id}>
                <th scope="row">
                  <input
                    className="form-check-input"
                    type="checkbox"
                    checked={selectedRows.has(absence.id)}
                    onChange={() => toggleRowSelection(absence.id)}
                  />
                </th>
                <td>{index + 1}</td>
                <td>{absence.firstName}</td>
                <td>{absence.lastName}</td>
                <td>{absence.schoolYear}</td>
                <td>{absence.subject}</td>
                <td>{absence.period}</td>
                <td>
                  <select onChange={(e) => setAbsencesToEdit(e, absence.id)}>
                    <option value=""></option>
                    <option value="false">Pa Arsyeshme</option>
                    <option value="true">Arsyeshme</option>
                  </select>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        <button className="btn btn-primary" onClick={saveChanges}>Ruaj Ndryshimet</button>
      </div>
    </>
  );
};

export default adminLayout(FormTeacherAbsences);