import React, { useEffect, useState } from 'react';
import adminLayout from "../../hoc/adminLayout";
import axios from 'axios';
import { Link, useParams } from 'react-router-dom';

const FormTeacherClassDetails = () => {
  const [myFormClasses, setMyFormClasses] = useState([]);
  const { id } = useParams();

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
              <p>Detajet e klases.</p>
            </div>
          </div>
        </div>
        <div className="row">
          {myFormClasses.map((classItem, index) => (
            <div className="col-md-4" key={index}>
              <div className="price-card featured">
                <h2>Klasa {classItem.className || 'N/A'}</h2>
                <p>Kujdestari</p>
                <p className="price"><span>Ditari</span></p>
                <ul className="pricing-offers">
                  <li>Ditari i klases {classItem.diary || 'N/A'}</li>
                </ul>
                <Link to={`/ditari/${id}`} className="btn btn-primary btn-mid" style={linkStyle}>
                  Hyr brenda
                </Link>
              </div>
            </div>
          ))}
          <div className="col-md-4">
            <div className="price-card featured">
              <h2>Klasa {}</h2>
              <p>Kujdestari</p>
              <p className="price"><span>Mungesat</span></p>
              <ul className="pricing-offers">
                <li>Mungesat dhe detajet {}</li>
              </ul>
              <Link to={`/absences/${id}`} className="btn btn-primary btn-mid" style={linkStyle}>
                Hyr brenda
              </Link>
            </div>
          </div>
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

export default adminLayout(FormTeacherClassDetails);