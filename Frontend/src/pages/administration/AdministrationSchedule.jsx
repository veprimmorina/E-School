import React, { useEffect, useState } from 'react';
import axios from 'axios';
import administrationLayout from '../../hoc/administrationLayout';
import ModalComponent from '../../components/ModalComponent';

const Schedule = () => {
  const [schedule, setSchedule] = useState();
  const [courses, setCourses] = useState();
  const [addedHours, setAddedHours] = useState([]);
  const [days, setDays] = useState(["E Hene", "E Marte", "E Merkure", "E Enjte", "E Premte"]);
  const [selectedData, setSelectedData] = useState([]);

  useEffect(() => {
    axios.get("https://localhost:7116/Administration/GetSchedule", { withCredentials: true })
      .then(response => setSchedule(response.data.data))
      .catch(ex => console.log(ex));

    getAllCourses();
  }, []);

  const GetAllCourses = () => {
    axios.get("https://localhost:7116/Administration/GetAllCoursesOfSchoolYear", { withCredentials: true })
      .then(response => setCourses(response.data.data))
      .catch(ex => console.log(ex));
  };

  const SaveChanges = () => {
    const formattedSchedule = selectedData.map(data => ({
      day: data.day,
      hour: data.hour,
      courseId: data.courseId,
      categoryId: data.classId,
      id: data.id
    }));
    
    axios.post("https://localhost:7116/Classes/CreateSchedule", formattedSchedule, { withCredentials: true })
      .then(response => console.log(response.data.data))
      .catch(ex => console.log(ex));
  };

  const handleAddHourClick = (day, hour) => {
    setAddedHours([...addedHours, { day, hour }]);
  };

  const handleSelectChange = (e, day, hour, id) => {
    const { value } = e.target;
    const [courseId, classId] = value.split('-');
    setSelectedData([...selectedData, { courseId, classId, hour, day, id }]);
  };

  const modalFooterContent = () => (
    <div style={{ width: "100%" }}>
      <button className="btn btn-default" onClick={SaveChanges}>Ruaj Ndryshimet</button>
    </div>
  );

  const modalContent = () => (
    <>
      {days.map((day, dayIndex) => (
        <React.Fragment key={dayIndex}>
          <div className='text-center' style={{ borderBottom: "1px solid black", borderTop: "1px solid black" }}><b>{day}</b></div>
          <div className='row'>
            {[1, 2, 3, 4, 5, 6].map((hour) => {
              const scheduleDetails = schedule?.find(scheduleItem => scheduleItem.day === day)?.schedules.filter(schedule => schedule.hour === hour);
              return (
                <div className='col-md' key={`${day}-${hour}`} style={{ borderRight: "1px solid black" }}>
                  <p>Ora {hour}</p>
                  {scheduleDetails?.map((detail, index) => (
                    <React.Fragment key={index}>
                      <select style={{ width: '150px', marginRight: '10px' }} onChange={(e) => handleSelectChange(e, day, hour, detail.id)}>
                        {courses?.map(course => (
                          <option key={course.id} value={`${course.id}-${course.classId}`} selected={`${detail.courseId}-${detail.classId}` === `${course.id}-${course.classId}`}>{course.courseName + "-" + course.className}</option>
                        ))}
                      </select>
                    </React.Fragment>
                  ))}
                  <button className='btn btn-success' onClick={() => handleAddHourClick(day, hour)}>Shto Orë</button>
                  {addedHours.map((addedHour, index) => addedHour.day === day && addedHour.hour === hour && (
                    <select key={index} style={{ width: '150px', marginRight: '10px' }} onChange={(e) => handleSelectChange(e, day, hour)}>
                      <option value="">Zgjedh Lëndën</option>
                      {courses?.map(course => (
                        <option key={course.id} value={`${course.id}-${course.classId}`}>{course.courseName + "-" + course.className}</option>
                      ))}
                    </select>
                  ))}
                </div>
              );
            })}
          </div>
        </React.Fragment>
      ))}
    </>
  );

  return (
    <>
      <div className="w-95 w-md-75 w-lg-60 w-xl-55 mx-auto mb-6 text-center">
        <div className="subtitle alt-font">
          <span className="text-primary">#04</span>
          <span className="title">Orari</span>
        </div>
        <h2 className="display-18 display-md-16 display-lg-14 mb-0">
          Orari javorë 
          <span className="text-primary">
            <button type="button" className="btn btn-primary" data-bs-toggle="modal" data-bs-target="#fullScreenModalDefault">Ndrysho</button>
          </span>
        </h2>
      </div>
      <div className="row">
        <div className="col-md-12">
          <div className="schedule-table">
            <table className="table bg-white">
              <thead>
                <tr>
                  <th>Orët</th>
                  <th>E parë</th>
                  <th>E Dytë</th>
                  <th>E Tretë</th>
                  <th>E Katërt</th>
                  <th>E Pestë</th>
                  <th className="last">E Gjashtë</th>
                </tr>
              </thead>
              <tbody>
                {days.map((day, dayIndex) => (
                  <tr key={dayIndex}>
                    <td className='day'>{day}</td>
                    {[1, 2, 3, 4, 5, 6].map((hour) => {
                      const scheduleDetails = schedule?.find(scheduleItem => scheduleItem.day === day)?.schedules.filter(schedule => schedule.hour === hour);
                      return (
                        <td key={`${day}-${hour}`}>
                          {scheduleDetails?.map((detail, detailIndex) => (
                            <div key={detailIndex}>
                              <h5>{detail.className}</h5>
                              <p>{detail.start_time} - {detail.end_time}</p>
                              <p>{detail.courseName}</p>
                              <div className="hover">
                                <h4>{detail.courseName}</h4>
                                <p>{detail.start_time} - {detail.end_time}</p>
                              </div>
                            </div>
                          ))}
                        </td>
                      );
                    })}
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>
      <ModalComponent title="Ndrysho orarin" footerContent={modalFooterContent()} content={modalContent()} fullScreen="true" dataBsBackdrop="static" id="fullScreenModalDefault" />
    </>
  );
};

export default administrationLayout(Schedule);