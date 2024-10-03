# E-School

**Technologies:** C#, ASP.NET, ML.NET MSSQL, ReactJs

The application comprises three main modules, each contributing to the overall functionality of the system.

The first module integrates **Open Educational Resources (OER)** with **Moodle**, functioning as a management system that provides educational materials and features related to course activities. 

The second module operates as an attachment system within Moodle, utilizing data about subjects, students, and academic years. This module facilitates administrative management, including grade entry by teachers, class monitoring, and tracking student details such as attendance, comments, and grades.

The final module incorporates **machine learning** to predict student academic decline. By analyzing data from Moodle—such as completed assignments, interactions with OER, grades, and attendance—the system generates detailed predictions, which are then communicated back to the user interface.

The following chapters elaborate on the features and functionalities achieved through the integration of these three modules.

---

### Main Roles

**Administrator**, **Teacher**, **Student**

#### Administrator
The administrator is an authorized person of the school who has full control over the system where they can give access, add, modify, or even delete users, school years, courses, etc. Once the administrator access is given to the teacher of the corresponding subject, this teacher can add, modify, and delete the content of the corresponding course. The administrator also decides on the registration of new teachers or students.

The administrator can decide to change the appearance of the user interface for the system with several stylized pages provided by the system. Full control of the system is in the hands of the administrator, where the Moodle platform provides several security mechanisms for unauthorized access. This role is primarily responsible for managing administrative matters within the school, such as course management, access, statistics, assessments, and services.

Through the school management module, the administration has the opportunity to be relieved from other responsibilities such as data storage and maintenance of various reports. These features enable the administration to perform more tasks with less effort. The design of the interfaces and functions promotes simplicity and reduces complexity. As shown in the figure below, the administrator can also see statistics regarding students, teachers, school years, periods, and classes.

![image](https://github.com/user-attachments/assets/618044ef-fd88-4eda-afb3-2ec0730383f7)

The system also offers the possibility of modifying the schedule in cases where a subject has been mistakenly added on the wrong day or hour, or when changes are needed in different periods of the school year.

![image](https://github.com/user-attachments/assets/1e3609cf-b1c4-4432-a4b8-691eba119cff)

In addition to creating reports, administrators can track the progress of created reports. A specific page is dedicated to displaying unfinished reports, which are shown only for the current or active year in a data table. If administrators wish to view status reports from another school year, they must activate that school year. These reports are presented with the corresponding creation dates, the name of the subject, and the name of the teacher. Reports can also be filtered by the name of the course or teacher. Administrators can notify teachers about incomplete reports via a notification email, which is sent by clicking a button at the end of the row in the table.

![image](https://github.com/user-attachments/assets/c0d6d091-de4d-4076-a50c-c065bfc708df)

As for unfinished reports, the same functionalities apply to completed reports, except for notifying the teacher to complete the report. Other functionalities include printing reports, viewing various details and statistics, and managing class guardians.

![image](https://github.com/user-attachments/assets/eb9178b8-a0a7-48f1-831e-8fb8867a14e8)

#### Educator
The educator is the other key role within the system. This role is defined by the administrator, and responsibilities are assigned within subjects. A given user can be a teacher in several different subjects across different school years. Teachers can set grades only for the subjects they are registered to teach in Moodle. If three grades are set for a certain period, the fourth final grade is automatically calculated. Similarly, if three final grades are set for a subject across three periods, the overall final grade for that subject is also automatically set. The figure below shows a visual description of the grading process.

![image](https://github.com/user-attachments/assets/e4d2c46c-96e5-445c-b471-013cf6e0264a)

Educators can also see predictions for students, indicating whether they are at risk of failing.

![image](https://github.com/user-attachments/assets/0619f748-99ba-4cb0-86eb-8fc5652c4cf6)

#### Student
Students can view their schedule for the entire week, which is managed by the administration. The schedule is displayed in a table format with corresponding days and hours, based on the class the student is enrolled in.

![image](https://github.com/user-attachments/assets/bafa00cc-e1b1-4074-a842-e83b8d953f22)
