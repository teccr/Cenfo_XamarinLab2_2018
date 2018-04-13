using System;
using System.Collections.ObjectModel;

namespace Cenfo_XamarinLab2_2018.Models
{
    public class Student
    {
        public int Id
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string MothersName
        {
            get;
            set;
        }

        public float Grade
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }

        public string Phone
        {
            get;
            set;
        }

        public string ProfilePicture
        {
            get;
            set;
        }

        public ObservableCollection<Homework> Homework
        {
            get;
            set;
        }
    }
}
