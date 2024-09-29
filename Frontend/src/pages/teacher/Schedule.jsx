import React, { useEffect, useState } from 'react';
import adminLayout from "../../hoc/adminLayout";
import axios from 'axios';

const Schedule = () => {
  const [schedule, setSchedule] = useState([]);

  useEffect(() => {
    const fetchSchedule = async () => {
      try {
        const response = await axios.get("https://localhost:7116/Teacher/GetMySchedule", { withCredentials: true });
        setSchedule(response.data.data);
        console.log(response.data.data);
      } catch (error) {
        console.error(error);
      }
    };
    fetchSchedule();
  }, []);

  return (
    <>
      <div className="w-95 w-md-75 w-lg-60 w-xl-55 mx-auto mb-6 text-center">
        <div className="subtitle alt-font">
          <span className="text-primary">#04</span>
          <span className="title">Orari</span>
        </div>
        <h2 className="display-18 display-md-16 display-lg-14 mb-0">
          Orari javorë <span className="text-primary">#Orari</span>
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
                {schedule.map((scheduleItem, index) => (
                  <ScheduleRow key={index} scheduleItem={scheduleItem} />
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </>
  );
};

const ScheduleRow = ({ scheduleItem }) => (
  <tr>
    <td className="day">{scheduleItem.day}</td>
    {[1, 2, 3, 4, 5, 6].map((hour) => {
      const scheduleDetails = scheduleItem.schedules.find((schedule) => schedule.hour === hour);
      return (
        <td key={hour}>
          {scheduleDetails ? (
            <>
              <h5>{scheduleDetails.className}</h5>
              <p>{scheduleDetails.start_time} - {scheduleDetails.end_time}</p>
              <p>{scheduleDetails.courseName}</p>
              <div className="hover">
                <h4>{scheduleDetails.courseName}</h4>
                <p>{scheduleDetails.start_time} - {scheduleDetails.end_time}</p>
              </div>
            </>
          ) : null}
        </td>
      );
    })}
  </tr>
);

export default adminLayout(Schedule);