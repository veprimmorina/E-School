import 'font-awesome/css/font-awesome.min.css';
import './assets/css/app.css';
import DashboardPage from './pages/DashboardPage';
import TypographyPage from './pages/TypographyPage'
import LoginPage from './pages/auth/LoginPage'
import ResetPassword from './pages/auth/ResetPassword';
import ProfilePage from './pages/profile/ProfilePage';
import ChangePasswordPage from './pages/profile/ChangePasswordPage';
import UserPreferencesPage from './pages/profile/UserPreferencesPage'
import AdminBlankPage from './pages/AdminBlankPage';
import {BrowserRouter as Router, Route, Routes} from 'react-router-dom';
import UnsubmittedReports from './pages/teacher/UnsubmittedReports';
import SubmittedReports from './pages/teacher/SubmittedReports';
import Schedule from './pages/teacher/Schedule';
import StudentGrades from './pages/teacher/StudentGrades';
import Ditari from './pages/teacher/Ditari';
import FormTeacherAbsences from './pages/teacher/FormTeacherAbsences';
import FormTeacherClasses from './pages/teacher/FormTeacherClasses';
import FormTeacherClassDetails from './pages/teacher/FormTeacherClassDetails';
import ClassesFormTeacher from './pages/administration/classesFormTeacher';
import ClassesWithoutFormTeacher from './pages/administration/classesWithoutFormTeacher';
import SchoolYears from './pages/administration/schoolYears';
import PeriodsTable from './pages/administration/periodsTable';
import SchoolYearsTable from './pages/administration/schoolYearsTable';
import AdministrationSchedule from './pages/administration/administrationSchedule';
import AdministrationUnsubmittedReports from './pages/administration/administrationUnsubmittedReports';
import Stats from './pages/administration/stats';
import Grades from './pages/student/Grades';
import StudentSchedule from './pages/student/studentSchedule';
import StudentAbsences from './pages/student/StudentAbsences';
import StudentStats from './pages/student/StudentStats';
import CurrentHours from './pages/administration/currentHours';

function App() {
  return (
        <Router>
            <Routes>
                <Route exact path='/' element={<DashboardPage/>} />
                <Route exact path='/login' element={<LoginPage/>} />
                <Route exact path='/reset-password' element={<ResetPassword/>} />
                <Route exact path='/profile' element={<ProfilePage/>} />
                <Route exact path='/change-password' element={<ChangePasswordPage/>} />
                <Route exact path='/preferences' element={<UserPreferencesPage/>} />
                <Route exact path='/typography' element={<TypographyPage/>} />
                <Route exact path='/blank-page' element={<AdminBlankPage/>} />
                <Route exact path='/unsubmitted-reports' element={<UnsubmittedReports/>} />
                <Route exact path='/unsubmitted-reports/:id' element={<UnsubmittedReports/>} />
                <Route exact path='/submitted-reports' element={<SubmittedReports/>} />
                <Route exact path='/schedule' element={<Schedule/>} />
                <Route exact path='/students' element={<StudentGrades/>} />
                <Route exact path='/ditari/:id' element={<Ditari/>} />
                <Route exact path='/absences/:id' element={<FormTeacherAbsences/>} />
                <Route exact path='/formTeacher' element={<FormTeacherClasses/>} />
                <Route exact path='/classDetails/:id' element={<FormTeacherClassDetails />} />
                <Route exact path='/classesFormTeacher' element={<ClassesFormTeacher />} />
                <Route exact path='/classesWithoutFormTeacher' element={<ClassesWithoutFormTeacher />} />
                <Route exact path='/schoolYears' element={<SchoolYears />} />
                <Route exact path='/schoolYearsTable' element={<SchoolYearsTable />} />
                <Route exact path='/periods' element={<PeriodsTable />} />
                <Route exact path='/administrationSchedule' element={<AdministrationSchedule />} />
                <Route exact path='/administrationCurrentHours' element={<CurrentHours />} />
                <Route exact path='/administrationUnsubmittedReports' element={<AdministrationUnsubmittedReports />} />
                <Route exact path='/administrationStats' element={<Stats />} />
                <Route exact path='/studentGrades' element={<Grades />} />
                <Route exact path='/studentSchedule' element={<StudentSchedule />} />
                <Route exact path='/studentAbsences' element={<StudentAbsences />} />
                <Route exact path='/studentStats' element={<StudentStats />} />
            </Routes>  
        </Router>
    )
}

export default App;
