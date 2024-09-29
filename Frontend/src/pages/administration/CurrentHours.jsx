import React, { useEffect, useState } from 'react';
import administrationLayout from "../../hoc/administrationLayout";
import axios from 'axios';

const CurrentHours = () => {
  const [hours, setHours] = useState([]);

  useEffect(() => {
    axios.get("https://localhost:7116/Classes/GetCurrentHours", { withCredentials: true })
      .then(response => {
        setHours(response.data.data);
        console.log(response.data.data);
      })
      .catch(ex => {
        console.log(ex);
      });
  }, []);

  return (
    <>
      <div className='pt-5'>
        {hours.length > 0 ? 
          hours.map(hour => (
            <div className="card mb-3" style={{ maxWidth: "540px" }} key={hour.id}>
              <div className="row g-0">
                <div className="col-md-4">
                  <img
                    src="https://cdn5.vectorstock.com/i/1000x1000/57/34/computer-background-vector-3235734.jpg"
                    alt="Class Hour"
                    className="img-fluid rounded-start"
                  />
                </div>
                <div className="col-md-8">
                  <div className="card-body">
                    <h5 className="card-title">{hour.FullName}</h5>
                    <p className="card-text">{hour.name}</p>
                    <p className="card-text">
                      <small className="text-muted">Ora {hour.hour}, Dita: {hour.day}</small>
                    </p>
                  </div>
                </div>
              </div>
            </div>
          ))
        : 
          <div className='text-center'>
            <b>Nuk po mbahet asnjë orë në intervalin momental</b>
          </div>
        }
      </div>
    </>
  );
};

export default administrationLayout(CurrentHours);