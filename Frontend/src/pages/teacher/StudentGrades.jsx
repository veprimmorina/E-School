import React, { useEffect, useState } from 'react';
import adminLayout from "../../hoc/adminLayout";
import axios from 'axios';

const StudentGrades = () => {
  const [data, setStudentData] = useState([]);

  useEffect(() => {
    const fetchGrades = async () => {
      try {
        const response = await axios.get("https://localhost:7116/Student/GetMyGrades", { withCredentials: true });
        setStudentData(response.data.data);
        console.log(response.data.data);
      } catch (error) {
        console.error(error);
      }
    };
    fetchGrades();
  }, []);

  if (data.length === 0) {
    return <div>No grades available</div>;
  }

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
                  <th>Student</th>
                  {data[0].courses.map((course) => (
                    <th key={course.courseId} colSpan={Math.max(...course.grades.map(grade => grade.periodId), 0) + 2}>
                      {course.courseName}
                    </th>
                  ))}
                </tr>
                <tr>
                  <th></th>
                  {data[0].courses.map((course) => (
                    Array.from({ length: Math.max(...course.grades.map(grade => grade.periodId), 0) + 2 }).map((_, index) => (
                      <th key={`${course.courseId}-${index}`}>
                        {index === 0 ? "Final" : `Period ${index}`}
                      </th>
                    ))
                  ))}
                </tr>
              </thead>
              <tbody>
                {data.map((student) => (
                  <tr key={student.userId}>
                    <td>{`${student.firstName} ${student.lastName}`}</td>
                    {student.courses.map((course) => (
                      Array.from({ length: Math.max(...course.grades.map(grade => grade.periodId), 0) + 2 }).map((_, index) => (
                        <td key={`${student.userId}-${course.courseId}-${index}`}>
                          {course.grades.filter(grade => grade.periodId === index)
                            .map(grade => (
                              grade.isFinal && grade.periodId !== 0 ? <strong key={grade.grade}>{grade.grade}</strong> : grade.grade
                            ))
                            .join(" | ") || "-"}
                        </td>
                      ))
                    ))}
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

export default adminLayout(StudentGrades);