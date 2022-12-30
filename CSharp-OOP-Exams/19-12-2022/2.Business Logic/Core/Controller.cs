using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityCompetition.Core.Contracts;
using UniversityCompetition.Models;
using UniversityCompetition.Repositories;

namespace UniversityCompetition.Core
{
    public class Controller : IController
    {
        private SubjectRepository subjects;
        private StudentRepository students;
        private UniversityRepository universities;
        private int subjectId = 1;
        private int universityId = 1;
        private int studentId = 1;

        public Controller()
        {
            subjects = new SubjectRepository();
            students = new StudentRepository();
            universities = new UniversityRepository();
        }

        public string AddSubject(string subjectName, string subjectType)
        {
            if (subjectType != "TechnicalSubject" && subjectType != "EconomicalSubject" && subjectType != "HumanitySubject")
                return string.Format(Utilities.Messages.OutputMessages.SubjectTypeNotSupported, subjectType);

            if (subjects.FindByName(subjectName) != default)
                return string.Format(Utilities.Messages.OutputMessages.AlreadyAddedSubject, subjectName);

            Subject subject = null;

            if (subjectType == "TechnicalSubject")
                subject = new TechnicalSubject(subjectId, subjectName);

            if (subjectType == "EconomicalSubject")
                subject = new EconomicalSubject(subjectId, subjectName);

            if (subjectType == "HumanitySubject")
                subject = new HumanitySubject(subjectId, subjectName);

            subjects.AddModel(subject);
            subjectId++;

            return string.Format(Utilities.Messages.OutputMessages.SubjectAddedSuccessfully, subjectType, subjectName, "SubjectRepository");
        }
        public string AddUniversity(string universityName, string category, int capacity, List<string> requiredSubjects)
        {
            if (universities.FindByName(universityName) != default)
                return string.Format(Utilities.Messages.OutputMessages.AlreadyAddedUniversity, universityName);

            List<int> requiredSubjectsInt = new List<int>();

            foreach (var subject1 in requiredSubjects)
                requiredSubjectsInt.Add(subjects.FindByName(subject1).Id);

            University university = new University(universityId, universityName, category, capacity, requiredSubjectsInt);

            universities.AddModel(university);
            universityId++;

            return string.Format(Utilities.Messages.OutputMessages.UniversityAddedSuccessfully, universityName, "UniversityRepository");
        }
        public string AddStudent(string firstName, string lastName)
        {
            if (students.FindByName(firstName + " " + lastName) != default)
                return string.Format(Utilities.Messages.OutputMessages.AlreadyAddedStudent, firstName, lastName);

            students.AddModel(new Student(studentId, firstName, lastName));
            studentId++;

            return string.Format(Utilities.Messages.OutputMessages.StudentAddedSuccessfully, firstName, lastName, "StudentRepository");
        }
        public string TakeExam(int studentId, int subjectId)
        {
            if (students.FindById(studentId) == default)
                return Utilities.Messages.OutputMessages.InvalidStudentId;

            if (subjects.FindById(subjectId) == default)
                return Utilities.Messages.OutputMessages.InvalidSubjectId;

            var student = students.FindById(studentId);

            if (student.CoveredExams.Any(x => x == subjectId))
            {
                var subject = student.CoveredExams.First(x => x == subjectId);
                return string.Format(Utilities.Messages.OutputMessages.StudentAlreadyCoveredThatExam, student.FirstName, student.LastName, subjects.FindById(subjectId).Name);
            }

            var subject1 = subjects.FindById(subjectId);
            student.CoverExam(subjects.FindById(subjectId));

            return string.Format(Utilities.Messages.OutputMessages.StudentSuccessfullyCoveredExam, student.FirstName, student.LastName, subject1.Name);
        }
        public string ApplyToUniversity(string studentName, string universityName)
        {
            string[] splittedName = studentName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string firstName = splittedName[0];
            string lastName = splittedName[1];

            if (students.FindByName(firstName + " " + lastName) == default)
                return string.Format(Utilities.Messages.OutputMessages.StudentNotRegitered, firstName, lastName);

            if (universities.FindByName(universityName) == default)
                return string.Format(Utilities.Messages.OutputMessages.UniversityNotRegitered, universityName);

            var student = students.FindByName(firstName + " " + lastName);
            var university = universities.FindByName(universityName);

            foreach (var requiredSubject in university.RequiredSubjects)
                if (!student.CoveredExams.Contains(requiredSubject))
                    return string.Format(Utilities.Messages.OutputMessages.StudentHasToCoverExams, studentName, universityName);

            if (student.University != null && student.University.Name == universityName)
                return string.Format(Utilities.Messages.OutputMessages.StudentAlreadyJoined, firstName, lastName, universityName);

            student.JoinUniversity(university);

            return string.Format(Utilities.Messages.OutputMessages.StudentSuccessfullyJoined, firstName, lastName, universityName);
        }
        public string UniversityReport(int universityId)
        {
            var university = universities.FindById(universityId);
            int count = 0;

            foreach (var student in students.Models)
                if (student.University != null && student.University.Name == university.Name)
                    count++;

            StringBuilder sb = new StringBuilder($"*** {university.Name} ***").AppendLine();
            sb.AppendLine($"Profile: {university.Category}");
            sb.AppendLine($"Students admitted: {count}");
            sb.AppendLine($"University vacancy: {university.Capacity - count}");
            return sb.ToString().TrimEnd();
        }
    }
}