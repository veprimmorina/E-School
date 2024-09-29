import React, { useEffect, useState } from 'react';
import studentLayout from "../../hoc/studentLayout";
import axios from 'axios';

const StudentAbsences = () => {
  const [absences, setAbsences] = useState([]);
  const [searchQuery, setSearchQuery] = useState("");
  const [statusFilter, setStatusFilter] = useState("");

  useEffect(() => {
    const fetchAbsences = async () => {
      try {
        const response = await axios.get("https://localhost:7116/Student/GetMyAbsences", { withCredentials: true });
        setAbsences(response.data.data);
        console.log(response.data.data);
      } catch (error) {
        console.error(error);
      }
    };

    fetchAbsences();
  }, []);

  const handleSearch = (event) => {
    setSearchQuery(event.target.value);
  };

  const handleStatusFilter = (event) => {
    setStatusFilter(event.target.value);
  };

  const filteredAbsences = absences.filter(absence =>
    absence.courseName.toLowerCase().includes(searchQuery.toLowerCase()) &&
    (statusFilter === "" || (absence.reasonable !== null && absence.reasonable.toString() === statusFilter))
  );

  return (
    <>
      <div className="mb-2 d-flex justify-content-between align-items-center pt-4">
        <div className="position-relative">
          <span className="position-absolute search"><i className="fa fa-search"></i></span>
          <input
            className="form-control w-100"
            placeholder="Search by course name..."
            value={searchQuery}
            onChange={handleSearch}
          />
        </div>
        <div className="px-2">
          <select
            className="form-select"
            value={statusFilter}
            onChange={handleStatusFilter}
          >
            <option value="">Filtro sipas statusit</option>
            <option value="true">E Arsyeshme</option>
            <option value="false">E Paarsyeshme</option>
          </select>
        </div>
      </div>
      <div className="table-responsive">
        <table className="table table-borderless">
          <thead>
            <tr className="bg-light">
              <th scope="col" width="5%">#</th>
              <th scope="col" width="20%">Viti shkollorë</th>
              <th scope="col" width="20%">Lënda</th>
              <th scope="col" width="10%">Perioda</th>
              <th scope="col" width="20%">Statusi</th>
            </tr>
          </thead>
          <tbody>
            {filteredAbsences.map((absence, index) => (
              <tr key={index}>
                <td>{index + 1}</td>
                <td>{absence.schoolYear}</td>
                <td>{absence.courseName}</td>
                <td>{absence.periodName}</td>
                <td>
                  {absence.reasonable === null ? "" : absence.reasonable ? "E Arsyeshme" : "E Paarsyeshme"}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </>
  );
};

export default studentLayout(StudentAbsences);