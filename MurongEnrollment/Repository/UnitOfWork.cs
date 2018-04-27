

using System.Linq.Expressions;
using MurongEnrollment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace MurongEnrollment
{
    public class UnitOfWork : IDisposable
    {
        private EnrollmentEntities context = new EnrollmentEntities();


        #region Login Management

        private GenericRepository<AspNetUserDetails> aspNetUserDetailsRepo;
        public GenericRepository<AspNetUserDetails> AspNetUserDetailsRepo
        {
            get
            {
                if (this.aspNetUserDetailsRepo == null)
                    this.aspNetUserDetailsRepo = new GenericRepository<AspNetUserDetails>(context);
                return aspNetUserDetailsRepo;
            }
            set { aspNetUserDetailsRepo = value; }
        }
        private GenericRepository<AspNetUsers> userRepository;
        public GenericRepository<AspNetUsers> UserRepository
        {
            get
            {

                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<AspNetUsers>(context);
                }
                return userRepository;
            }
        }
        private GenericRepository<AspNetRoles> roleRepository;
        public GenericRepository<AspNetRoles> RoleRepository
        {
            get
            {

                if (this.roleRepository == null)
                {
                    this.roleRepository = new GenericRepository<AspNetRoles>(context);
                }
                return roleRepository;
            }
        }
        #endregion


        private GenericRepository<Subjects> subjectRepo;
        public GenericRepository<Subjects> SubjectRepo
        {
            get
            {
                if (this.subjectRepo == null)
                    this.subjectRepo = new GenericRepository<Subjects>(context);
                return subjectRepo;
            }
            set { subjectRepo = value; }
        }


        private GenericRepository<Schedules> scheduleRepo;
        public GenericRepository<Schedules> ScheduleRepo
        {
            get
            {
                if (this.scheduleRepo == null)
                    this.scheduleRepo = new GenericRepository<Schedules>(context);
                return scheduleRepo;
            }
            set { scheduleRepo = value; }
        }

        private GenericRepository<GradeLevels> gradeLevelRepo;
        public GenericRepository<GradeLevels> GradelevelRepo
        {
            get
            {
                if (this.gradeLevelRepo == null)
                    this.gradeLevelRepo = new GenericRepository<GradeLevels>(context);
                return gradeLevelRepo;
            }
            set { gradeLevelRepo = value; }
        }

        private GenericRepository<Sections> sectionRepo;
        public GenericRepository<Sections> SectionRepo
        {
            get
            {
                if (this.sectionRepo == null)
                    this.sectionRepo = new GenericRepository<Sections>(context);
                return sectionRepo;
            }
            set { sectionRepo = value; }
        }

        private GenericRepository<Teachers> teacherRepo;
        public GenericRepository<Teachers> TeacherRepo
        {
            get
            {
                if (this.teacherRepo == null)
                    this.teacherRepo = new GenericRepository<Teachers>(context);
                return teacherRepo;
            }
            set { teacherRepo = value; }
        }

        private GenericRepository<SchoolYears> schoolYearRepo;
        public GenericRepository<SchoolYears> SchoolYearRepo
        {
            get
            {
                if (this.schoolYearRepo == null)
                    this.schoolYearRepo = new GenericRepository<SchoolYears>(context);
                return schoolYearRepo;
            }
            set { schoolYearRepo = value; }
        }


        private GenericRepository<Students> studentRepo;
        public GenericRepository<Students> StudentRepo
        {
            get
            {
                if (this.studentRepo == null)
                    this.studentRepo = new GenericRepository<Students>(context);
                return studentRepo;
            }
            set { studentRepo = value; }
        }

        private GenericRepository<StandardFees> feeRepo;
        public GenericRepository<StandardFees> StandardFeeRepo
        {
            get
            {
                if (this.feeRepo == null)
                    this.feeRepo = new GenericRepository<StandardFees>(context);
                return feeRepo;
            }
            set { feeRepo = value; }
        }

        private GenericRepository<Enrollments> enrollmentRepo;
        public GenericRepository<Enrollments> EnrollmentsRepo
        {
            get
            {
                if (this.enrollmentRepo == null)
                    this.enrollmentRepo = new GenericRepository<Enrollments>(context);
                return enrollmentRepo;
            }
            set { enrollmentRepo = value; }
        }

        private GenericRepository<EnrolledSubjects> enrolledSubjectRepo;
        public GenericRepository<EnrolledSubjects> EnrolledSubjectsRepo
        {
            get
            {
                if (this.enrolledSubjectRepo == null)
                    this.enrolledSubjectRepo = new GenericRepository<EnrolledSubjects>(context);
                return enrolledSubjectRepo;
            }
            set { enrolledSubjectRepo = value; }
        }

        private GenericRepository<Payments> paymentsRepo;
        public GenericRepository<Payments> PaymentsRepo
        {
            get
            {
                if (this.paymentsRepo == null)
                    this.paymentsRepo = new GenericRepository<Payments>(context);
                return paymentsRepo;
            }
            set { paymentsRepo = value; }
        }

        private GenericRepository<Alumni> alumniRepo;
        public GenericRepository<Alumni> AlumniRepo
        {
            get
            {
                if (this.alumniRepo == null)
                    this.alumniRepo = new GenericRepository<Alumni>(context);
                return alumniRepo;
            }
            set { alumniRepo = value; }
        }

        private GenericRepository<Grades> gradesRepo;
        public GenericRepository<Grades> GradesRepo
        {
            get
            {
                if (this.gradesRepo == null)
                    this.gradesRepo = new GenericRepository<Grades>(context);
                return gradesRepo;
            }
            set { gradesRepo = value; }
        }

        private GenericRepository<Gradings> gradingsRepo;
        public GenericRepository<Gradings> GradingsRepo
        {
            get
            {
                if (this.gradingsRepo == null)
                    this.gradingsRepo = new GenericRepository<Gradings>(context);
                return gradingsRepo;
            }
            set { gradingsRepo = value; }
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Test()
        {
            //  this.enrollmentRepo.Fetch(m => new { m.Payments, m.SchoolYears });
        }
    }

}