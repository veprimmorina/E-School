import React, { useEffect, useState } from 'react';
import adminLayout from "../../hoc/adminLayout";
import axios from 'axios';
import { useParams } from 'react-router-dom';

const Ditari = () => {
  const [data, setStudentData] = useState([]);
  const [isEditable, setIsEditable] = useState(false);
  const [newGrades, setNewGrades] = useState([]);
  const [activePeriod, setActivePeriod] = useState(null);
  const [gradesToPut, setGradesToPut] = useState([]);
  const [isSuccess, setIsSuccess] = useState(false);
  const [predictions, setPredictions] = useState({});
  
  const { id } = useParams();

  useEffect(() => {
    const fetchStudents = async () => {
      try {
        const response = await axios.get(`https://localhost:7116/Classes/GetStudentsOfClass?classId=${id}`, { withCredentials: true });
        setStudentData(response.data.data);
      } catch (error) {
        console.error('Error fetching student data:', error);
      }
    };

    fetchStudents();
  }, [id, isSuccess]);

  const handleEdit = async () => {
    try {
      const response = await axios.get("https://localhost:7116/Administration/GetActivePeriod", { withCredentials: true });
      setActivePeriod(response.data.data);
      setIsEditable(true);
    } catch (error) {
      console.error('Error fetching active period:', error);
    }
  };

  const handleCancel = () => {
    setIsEditable(false);
    setGradesToPut([]);
    setNewGrades([]);
  };

  const predict = async (userId, name, lastName) => {
    const studentDto = getStudentDto(userId, name, lastName);
    if (!studentDto) return;

    try {
      const response = await axios.post('https://localhost:7126/Prediction/Predict', [studentDto]);
      setPredictions(prev => ({
        ...prev,
        [userId]: response.data[0].willDrop
      }));
    } catch (error) {
      console.error('Error predicting:', error);
    }
  };

  const getStudentDto = (userId, name, lastName) => {
    switch (userId) {
      case 9:
        return createStudentDto(name, lastName, 4, 5, 40, 10, 4, 5, 100, 100);
      case 5:
        return createStudentDto(name, lastName, 5, 4, 10, 20, 5, 4, 50, 50);
      case 7:
        return createStudentDto(name, lastName, 4, 5, 10, 5, 4, 5, 90, 80);
      default:
        console.error('Invalid userId:', userId);
        return null;
    }
  };

  const createStudentDto = (name, lastName, avgPrevious, avgCurrent, absencesPrevious, absencesCurrent, behaviorGradePrevious, behaviorGradeCurrent, oerInteraction, taskSubmission) => ({
    Name: name,
    LastName: lastName,
    MesatarjaENotaveVitinParaprak: avgPrevious,
    MesatarjaTani: avgCurrent,
    MungesatVitinEKaluar: absencesPrevious,
    MungesatTani: absencesCurrent,
    NotaESjelljesVitinEKaluar: behaviorGradePrevious,
    NotaESjelljesTani: behaviorGradeCurrent,
    NderveprimiMeOER: oerInteraction,
    DorezimiIDetyrave: taskSubmission
  });

  const handleSave = async () => {
    const grades = gradesToPut.map(grade => ({
      UserId: grade.studentId,
      ClassId: 3,
      Grade: grade.grade,
      GradeId: grade.gradeId,
      PeriodId: activePeriod,
      CourseId: grade.courseId
    }));

    try {
      await axios.post("https://localhost:7116/Teacher/PutGrade", grades, { withCredentials: true });
      setIsSuccess(true);
      setIsEditable(false);
      setGradesToPut([]);
    } catch (error) {
      console.error('Error saving grades:', error);
    }
  };

  const handleAddGrade = (studentId, courseId, periodId) => {
    setNewGrades(prev => [...prev, { studentId, courseId, periodId }]);
  };

  const handleChange = (event, studentId, courseId, gradeId, periodId) => {
    const grade = event.target.value;
    setGradesToPut(prev => [...prev, { studentId, courseId, grade, gradeId }]);
  };

  const handleExport = async () => {
    const students = [
      createStudentDto('Sadik', 'Gashi', 4, 5, 40, 10, 4, 5, 100, 100),
      createStudentDto('student', 'test', 5, 4, 10, 20, 5, 4, 50, 50),
      createStudentDto('Test', 'Test', 4, 5, 10, 5, 4, 5, 90, 80)
    ];

    try {
      const response = await axios.post('https://localhost:7126/Prediction/ExportToExcel', students, {
        responseType: 'blob',
        headers: {
          'Content-Type': 'application/json'
        }
      });

      const blob = new Blob([response.data], { type: response.headers['content-type'] });
      const link = document.createElement('a');
      link.href = window.URL.createObjectURL(blob);
      link.download = 'Predictions.xlsx';
      document.body.appendChild(link);
      link.click();
      link.remove();
    } catch (error) {
      console.error('Error exporting to Excel:', error);
    }
  };

  return (
    <>
      {data.length === 0 ? null : (
        <>
          <div className="w-95 w-md-75 w-lg-60 w-xl-55 mx-auto mb-6 text-center">
            <div className="subtitle alt-font">
              <span className="text-primary">#04</span>
              <span className="title">Ditari</span>
            </div>
            <h2 className="display-18 display-md-16 display-lg-14 mb-0">
              Ditari për klasen <span className="text-primary">{data.className}</span>
            </h2>
            {isEditable ? (
              <>
                <button className='btn btn-primary' onClick={handleSave}>Ruaj</button>
                <button className='btn btn-secondary' onClick={handleCancel}>Anulo</button>
              </>
            ) : (
              <>
                <button className='btn btn-primary' onClick={handleEdit}>Veprime</button>
                <button className='btn btn-success' onClick={handleExport}>Shkarko</button>
              </>
            )}
          </div>
          <div className="row">
            <div className="col-md-12">
              <div className="schedule-table">
                <table className="table bg-white">
                  <thead>
                    <tr>
                      <th>Nxenesi</th>
                      {data[0]?.courses?.map(course => (
                        <th key={course.courseId} colSpan={course.periods.length + 1}>
                          {course.courseName}
                        </th>
                      ))}
                      <th>Parashiko</th>
                    </tr>
                    <tr>
                      <th></th>
                      {data[0]?.courses?.map(course => (
                        <>
                          {course.periods.map(period => (
                            <th key={`${course.courseId}-${period.id}`}>{period.name}</th>
                          ))}
                          <th key={`${course.courseId}-final`}>N.F</th>
                        </>
                      ))}
                      <th></th>
                    </tr>
                  </thead>
                  <tbody>
                    {data.map(student => (
                      <tr key={student.userId}>
                        <td>{`${student.firstName} ${student.lastName}`}</td>
                        {student.courses?.map(course => (
                          <>
                            {course.periods.map(period => (
                              <td key={`${student.userId}-${course.courseId}-${period.id}`}>
                                {isEditable && period.id === activePeriod && course.isMyCourse ? (
                                  <>
                                    {period.grades.filter(grade => !grade.isFinal).map((grade, i) => (
                                      <input
                                        key={i}
                                        type="text"
                                        size={1}
                                        defaultValue={grade.grade}
                                        onChange={(event) => handleChange(event, student.userId, course.courseId, grade.gradeId, period.id)}
                                      />
                                    ))}
                                  </>
                                ) : (
                                  <div>
                                    {period.grades.map((grade, i) => (
                                      <div key={i}>{grade.isFinal ? <b style={{ fontSize: '16px' }}>{grade.grade}</b> : grade.grade}</div>
                                    ))}
                                  </div>
                                )}
                                {isEditable && period.id === activePeriod && course.isMyCourse && period.grades.filter(grade => !grade.isFinal).length < 3 && (
                                  <>
                                    <button onClick={() => handleAddGrade(student.userId, course.courseId, period.id)}>+</button>
                                  </>
                                )}
                              </td>
                            ))}
                            <td>{period.grades.filter(grade => grade.isFinal).map((grade, i) => <b key={i}>{grade.grade}</b>)}</td>
                          </>
                        ))}
                        <td>
                          <button onClick={() => predict(student.userId, student.firstName, student.lastName)}>Parashiko</button>
                          {predictions[student.userId] !== undefined && (
                            <span className="badge rounded-pill bg-warning">{predictions[student.userId] ? 'Do të braktisë' : 'Nuk do të braktisë'}</span>
                          )}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        </>
      )}
    </>
  );
};

export default adminLayout(Ditari);