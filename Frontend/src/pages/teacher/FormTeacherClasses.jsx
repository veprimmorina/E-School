import React, { useEffect, useState } from 'react';
import adminLayout from "../../hoc/adminLayout";
import axios from 'axios';
import { Link } from 'react-router-dom';

const FormTeacherClasses = () => {
  const [myFormClasses, setMyFormClasses] = useState([]);

  useEffect(() => {
    const fetchClasses = async () => {
      try {
        const response = await axios.get("https://localhost:7116/Teacher/GetMyFormClasses", { withCredentials: true });
        console.log(response.data);
        setMyFormClasses(response.data.data);
      } catch (error) {
        console.error(error);
      }
    };
    fetchClasses();
  }, []);

  return (
    <section className="pricing-section pt-4">
      <div className="container">
        <div className="row justify-content-md-center">
          <div className="col-xl-5 col-lg-6 col-md-8">
            <div className="section-title text-center title-ex1">
              <h2>Kujdestaria Ime</h2>
              <p>Klasat ne te cilat une jam kujdestar.</p>
            </div>
          </div>
        </div>
        <div className="row">
          {myFormClasses.map((classItem) => (
            <div className="col-md-4" key={classItem.classId}>
              <div className="price-card featured">
                <h2>Klasa</h2>
                <p>Kujdestari</p>
                <p className="price"><span>{classItem.className}</span></p>
                <ul className="pricing-offers">
                  <li>{classItem.schoolYear}</li>
                </ul>
                <Link 
                  to={`/classDetails/${classItem.classId}`} 
                  className="btn btn-primary btn-mid"
                  style={linkStyle}
                >
                  Hyr brenda
                </Link>
              </div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
};

const linkStyle = {
  display: 'flex',
  justifyContent: 'center',
  alignItems: 'center',
  textAlign: 'center',
};

export default adminLayout(FormTeacherClasses);