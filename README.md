# E-School
Technologies: C#, ASP.NET, MSSQL, ReactJs

The application comprises three main modules, each contributing to the overall functionality of the system.

The first module integrates Open Educational Resources (OER) with Moodle, functioning as a management system that provides educational materials and features related to course activities. The second module operates as an attachment system within Moodle, utilizing data about subjects, students, and academic years. This module facilitates administrative management, including grade entry by teachers, class monitoring, and tracking student details such as attendance, comments, and grades.
The final module incorporates machine learning to predict student academic decline. By analyzing data from Moodle—such as completed assignments, interactions with OER, grades, and attendance—the system generates detailed predictions, which are then communicated back to the user interface.
The following chapters elaborate on the features and functionalities achieved through the integration of these three modules.

Three main roles are: Administrator, Teacher, Student
The administrator who is an authorized person of the school who has full control over the system where I can give access, add, modify or even delete users, school years, courses etc. Once the administrator access is given to the teacher of the corresponding subject, this teacher can add, modify and delete the content of the corresponding course. The administrator also decides on the registration of new teachers or students.
The administrator can decide to change the appearance of the user interface for the system with several stylized pages provided by the system. Full control of the system is in the hands of the administrator, where the Moodle platform provides several security mechanisms for unauthorized access, and this role is primarily responsible for managing administrative matters within the school, such as course management, access, statistics, assessments and services.
Through the school management module, the administration has the opportunity to be released from other responsibilities such as data storage and care, maintenance of various reports, etc. These features enable the administration to do more work with less effort, considering that the design of the interfaces and the completion of the functions have been done in such a way as to promote simplicity and at the same time reduce complexity. As can be seen in figure below, the administrator can also see the statistics regarding students, teachers, school years, periods, and classes.

![image](https://github.com/user-attachments/assets/618044ef-fd88-4eda-afb3-2ec0730383f7)

The system also offers the possibility of modifying the schedule in cases where a subject can be mistakenly added on a wrong day or hour, or even when there is a need to make changes in different periods of that school year. This add and update schedule feature.

![image](https://github.com/user-attachments/assets/1e3609cf-b1c4-4432-a4b8-691eba119cff)

In addition to creating reports, administrators can also track the progress of those created reports. A specific page is used only for displaying unfinished reports. These reports are displayed only for the current or active year in a data table. If the administrators want to see the same status reports for another school year, the same process must be repeated with the schedule, process described above (activation of that school year). These reports are presented with the corresponding dates of creation, the name of the subject, as well as the name of the teacher who teaches that subject. For the reports presented in the table, the possibility of filtering is also offered, where the reports can be filtered based on the name of the course or the teacher of the course. Administrators can also notify subject teachers of these incomplete reports. Along with this data, a button appears at the end of the row of the table. By clicking this button, a notification email is sent to that teacher with the details of the incomplete report

![image](https://github.com/user-attachments/assets/c0d6d091-de4d-4076-a50c-c065bfc708df)

As for unfinished reports, the same functionalities can be used for completed reports, excluding the possibility of notifying the teacher to complete the report. Other functionalities include printing reports, viewing various details and statistics, managing class guardians, etc.

![image](https://github.com/user-attachments/assets/eb9178b8-a0a7-48f1-831e-8fb8867a14e8)

The educator is the other important role within the system. This role is defined by the administrator and his responsibilities are divided within the subject. A given user can be a teacher in several different subjects in different school years. The teacher has special privileges within the subject in which he is qualified as a teacher. These teachers can set grades only for subjects that are registered as teachers in Moodle. If three grades are set for a certain period, then the fourth final grade is automatically set for that student. The same procedure also applies in the case when three final grades are set in one subject in three periods, then the final grade for that subject is also automatically set. Figure 23 presents a pictorial description of grading.

![image](https://github.com/user-attachments/assets/e4d2c46c-96e5-445c-b471-013cf6e0264a)

Educators have the opportunity to see in the same way the prediction of those students if they will fail or not.

![image](https://github.com/user-attachments/assets/0619f748-99ba-4cb0-86eb-8fc5652c4cf6)

Students can also view their schedule for the entire week. This part, which is managed by the administration, can also be seen by students. Considering the class in which that student is, the schedule for him is displayed in the format of the table with the corresponding days and hours

![image](https://github.com/user-attachments/assets/bafa00cc-e1b1-4074-a842-e83b8d953f22)
