import React, {useEffect, useState} from "react";
import adminLayout from "../hoc/adminLayout"
import axios from "axios";
import { Link } from "react-router-dom";

const DashboardPage = () => {
  const [classes, setClasses] = useState([]);
  
  useEffect(() => {
    axios.get("https://localhost:7116/Classes/GetMyClasses", {withCredentials: true})
      .then(response => {
        setClasses(response.data.data);
        console.log(response.data);
        localStorage.setItem("User", response.data.data[0].userDetails)
      })
      .catch(ex => {
        console.log(ex)
      });
  }, []);

  return (
    <>
    <section class="pricing-section mt-5 pt-5">
    <div class="container">
		<div class="row justify-content-md-center">
			<div class="col-xl-5 col-lg-6 col-md-8">
				<div class="section-title text-center title-ex1">
					<h2>Klasat e mija</h2>
					<p>Klasat ne te cilat une ligjeroj.</p>
				</div>
			</div>
		</div>
		<div class="row">
    {classes.map((classItem, index) => (
       <div class="col-md-4">
       <div class="price-card featured">
         <h2>Klasa</h2>
         <p></p>
         <p class="price"><span>{classItem.className}</span></p>
         <ul class="pricing-offers">
           <li>Kujdestar: {classItem.formTeacherFirstName + " " + classItem.formTeacherLastName}</li>
         </ul>
         <Link to={'/ditari/' + classItem.classId} class="btn btn-primary btn-mid classbutton">
         Hyr brenda
        </Link>
       </div>
     </div> 
      ))}
		</div>
	</div>
</section>
    
    </>
  );
}  


export default adminLayout(DashboardPage);