import React, { useEffect, useState } from 'react';
import axios from 'axios';
import studentLayout from "../../hoc/studentLayout";

const Grades = () => {
  const [data, setStudentData] = useState(null);
  const [isSuccess, setIsSuccess] = useState(false);

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
  }, [isSuccess]);

  if (!data) {
    return null; 
  }

  return (
    <>
      <div className="w-95 w-md-75 w-lg-60 w-xl-55 mx-auto mb-6 text-center">
        <div className="subtitle alt-font">
          <span className="text-primary">#04</span>
          <span className="title">Ditari</span>
        </div>
        <h2 className="display-18 display-md-16 display-lg-14 mb-0">
          Ditari im <span className="text-primary">{data.className}</span>
        </h2>
      </div>

      <div className="row">
        <div className="col-md-12">
          <div className="schedule-table">
            <table className="table bg-white">
              <thead>
                <tr>
                  <th>Student</th>
                  {data[0]?.courses?.map(course => (
                    <th key={course.courseId} colSpan={course.periods.length + 1}>
                      {course.courseName}
                    </th>
                  ))}
                </tr>
                <tr>
                  <th></th>
                  {data[0]?.courses?.flatMap(course => (
                    <>
                      {course.periods.map(period => (
                        <th key={`${course.courseId}-${period.id}`}>
                          {period.name}
                        </th>
                      ))}
                      <th key={`${course.courseId}-final`}>N.F</th>
                    </>
                  ))}
                </tr>
              </thead>
              <tbody>
                {data.map(student => (
                  <tr key={student.userId}>
                    <td>{`${student.firstName} ${student.lastName}`}</td>
                    {student.courses.flatMap(course => (
                      <>
                        {course.periods.map(period => (
                          <td key={`${student.userId}-${course.courseId}-${period.id}`}>
                            <div>
                              {period.grades.map((grade, i) => (
                                <div key={i}>
                                  {grade.isFinal ? (
                                    <b style={{ fontSize: '16px' }}>{grade.grade}</b>
                                  ) : (
                                    grade.grade
                                  )}
                                </div>
                              ))}
                            </div>
                          </td>
                        ))}
                        <td key={`${student.userId}-${course.courseId}-final`}>
                          {course.finalGrade !== 0 ? course.finalGrade : ""}
                        </td>
                      </>
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

export default studentLayout(Grades);