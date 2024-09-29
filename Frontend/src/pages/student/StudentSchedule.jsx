import React, { useEffect, useState } from 'react';
import axios from 'axios';
import studentLayout from '../../hoc/studentLayout';

const Schedule = () => {
  const [schedule, setSchedule] = useState([]);
  const [courses, setCourses] = useState([]);
  const [addedHours, setAddedHours] = useState([]);
  const daysOfWeek = ["E Hene", "E Marte", "E Merkure", "E Enjte", "E Premte"];
  const [selectedData, setSelectedData] = useState([]);

  useEffect(() => {
    fetchSchedule();
    fetchCourses();
  }, []);

  const fetchSchedule = async () => {
    try {
      const response = await axios.get("https://localhost:7116/Student/GetMySchedule", { withCredentials: true });
      setSchedule(response.data.data);
    } catch (error) {
      console.error('Error fetching schedule:', error);
    }
  };

  const fetchCourses = async () => {
    try {
      const response = await axios.get("https://localhost:7116/Administration/GetAllCoursesOfSchoolYear", { withCredentials: true });
      setCourses(response.data.data);
    } catch (error) {
      console.error('Error fetching courses:', error);
    }
  };

  const saveChanges = async () => {
    const scheduleData = selectedData.map(data => ({
      day: data.day,
      hour: data.hour,
      courseId: data.courseId,
      categoryId: data.classId,
      id: data.id
    }));

    try {
      const response = await axios.post("https://localhost:7116/Classes/CreateSchedule", scheduleData);
      console.log('Changes saved:', response.data.data);
    } catch (error) {
      console.error('Error saving schedule:', error);
    }
  };

  const handleAddHourClick = (day, hour) => {
    setAddedHours(prevHours => [...prevHours, { day, hour }]);
  };

  const handleSelectChange = (event, day, hour, id) => {
    const { value } = event.target;
    const [courseId, classId] = value.split('-');
    setSelectedData(prevData => [...prevData, { courseId, classId, hour, day, id }]);
  };

  const renderModalFooterContent = () => (
    <div style={{ width: "100%" }}>
      <button className="btn btn-default" onClick={saveChanges}>Ruaj Ndryshimet</button>
    </div>
  );

  const renderModalContent = () => (
    <>
      {daysOfWeek.map((day, dayIndex) => (
        <React.Fragment key={dayIndex}>
          <div className='text-center' style={{ borderBottom: "1px solid black", borderTop: "1px solid black" }}><b>{day}</b></div>
          <div className='row'>
            {[1, 2, 3, 4, 5, 6].map(hour => {
              const scheduleDetails = schedule.find(item => item.day === day)?.schedules.filter(schedule => schedule.hour === hour) || [];
              return (
                <div className='col-md' key={`${day}-${hour}`} style={{ borderRight: "1px solid black" }}>
                  <p>Ora {hour}</p>
                  {scheduleDetails.map((detail, index) => (
                    <React.Fragment key={index}>
                      <select style={{ width: '150px', marginRight: '10px' }} onChange={(e) => handleSelectChange(e, day, hour, detail.id)}>
                        {courses.map(course => (
                          <option key={course.id} value={`${course.id}-${course.classId}`} selected={detail.courseId === course.id && detail.classId === course.classId}>
                            {course.courseName + "-" + course.className}
                          </option>
                        ))}
                      </select>
                    </React.Fragment>
                  ))}
                  <button className='btn btn-success' onClick={() => handleAddHourClick(day, hour)}>Shto Orë</button>
                  {addedHours.map((addedHour, index) => {
                    if (addedHour.day === day && addedHour.hour === hour) {
                      return (
                        <select key={index} style={{ width: '150px', marginRight: '10px' }} onChange={(e) => handleSelectChange(e, day, hour)}>
                          <option value="">Zgjedh Lëndën</option>
                          {courses.map(course => (
                            <option key={course.id} value={`${course.id}-${course.classId}`}>{course.courseName + "-" + course.className}</option>
                          ))}
                        </select>
                      );
                    }
                    return null;
                  })}
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
        <div className="subtitle alt-font"><span className="text-primary">#04</span><span className="title">Orari im</span></div>
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
                {daysOfWeek.map((day, dayIndex) => (
                  <tr key={dayIndex}>
                    <td className='day'>{day}</td>
                    {[1, 2, 3, 4, 5, 6].map(hour => {
                      const scheduleDetails = schedule.find(item => item.day === day)?.schedules.filter(schedule => schedule.hour === hour) || [];
                      return (
                        <td key={`${day}-${hour}`}>
                          {scheduleDetails.map((detail, detailIndex) => (
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
    </>
  );
};

export default studentLayout(Schedule);