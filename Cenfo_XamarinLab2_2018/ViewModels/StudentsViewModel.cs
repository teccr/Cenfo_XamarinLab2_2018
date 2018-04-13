﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Cenfo_XamarinLab2_2018.Models;
using Cenfo_XamarinLab2_2018.Views;
using Xamarin.Forms;

namespace Cenfo_XamarinLab2_2018.ViewModels
{
    public class StudentsViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Interface Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName) && PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Singleton Definition

        private static StudentsViewModel instance = null;

        public static StudentsViewModel GetInstance()
        {
            if (instance == null)
            {

                instance = new StudentsViewModel();
            }

            return instance;
        }

        public static void DeleteInstance()
        {
            if (instance != null)
            {
                instance = null;
            }
        }

        public StudentsViewModel()
        {
            InitClass();
            InitCommands();
        }

        #endregion

        #region Initialization

        private async void InitClass()
        {
            Students = await LoadStudents();
            CurrentStudent = new Student();
        }

        private Task<ObservableCollection<Student>> LoadStudents()
        {
            Task<ObservableCollection<Student>> studentsTask = new Task<ObservableCollection<Student>>(() =>
            {
                ObservableCollection<Homework> homeworks = new ObservableCollection<Homework>();


                ObservableCollection<Student> students = new ObservableCollection<Student>();
                students.Add( new Student() { FirstName="Fulanito", Address="Carcosa", Grade=0.54f, 
                    Id=1001, LastName="Ramirez", MothersName="Jimenez", Phone="890781234", ProfilePicture="icon",
                    Homework = new ObservableCollection<Homework>() } );
                students.Add( new Student() { FirstName="Sultano", Address="Arkham", Grade=0.90f, 
                    Id=1002, LastName="Altamiranda", MothersName="Jimenez", Phone="890781234", ProfilePicture="icon",
                    Homework = new ObservableCollection<Homework>() } );
                students.Add( new Student() { FirstName="Mengano", Address="Innsmouth", Grade=0.89f, 
                    Id=1003, LastName="Gutt", MothersName="Jimenez", Phone="890781234", ProfilePicture="icon",
                    Homework = new ObservableCollection<Homework>() } );
                students.Add( new Student() { FirstName="Dieguito", Address="Dunwich", Grade=0.24f, 
                    Id=1004, LastName="De La Rosa", MothersName="Jimenez", Phone="890781234", ProfilePicture="icon",
                    Homework = new ObservableCollection<Homework>() } );

                Thread.Sleep(4000);

                return students;
            });

            studentsTask.Start();
            return studentsTask;
        }

        #endregion

        #region Commands

        private void InitCommands()
        {
            AddStudentCommand = new Command(AddStudent);
            SelectStudentCommand = new Command<int>(SelectStudent);
            AddHomeworkCommand = new Command(AddHomework);
            SaveHomeworkFileCommand = new Command(SaveHomeworkFile);
        }

        private void AddStudent()
        {
            System.Random random = new System.Random();
            CurrentStudent.Id = random.Next(100001, 10000000);
            Students.Add(CurrentStudent);
            CurrentStudent = null;
            MoveNext("Students");
        }

        public ICommand AddStudentCommand
        {
            get;
            set;
        }

        public ICommand SelectStudentCommand
        {
            get;
            set;
        }

        private void SelectStudent(int Id)
        {
            CurrentStudent = Students.Where( student => student.Id == Id ).FirstOrDefault();
            MoveNext("Student");
        }

        private void MoveNext(string pageName)
        {
            var tabbedPage = ((TabbedPage)App.Current.MainPage);
            tabbedPage.CurrentPage = tabbedPage.Children.Where( page => page.Title == pageName ).FirstOrDefault();
        }

        public ICommand AddHomeworkCommand
        {
            get;
            set;
        }

        private void AddHomework()
        {
            CurrentHomework = new Homework();
            ((TabbedPage)App.Current.MainPage).Navigation.PushModalAsync(new HomeworkFileView());
        }

        public ICommand SaveHomeworkFileCommand
        {
            get;
            set;
        }

        private void SaveHomeworkFile()
        {
            System.Random random = new System.Random();
            CurrentHomework.Id = random.Next(100001, 10000000);
            if (CurrentStudent.Homework == null) 
                CurrentStudent.Homework = new ObservableCollection<Homework>();
            CurrentStudent.Homework.Add(CurrentHomework);
            CurrentHomework = null;
            ((TabbedPage)App.Current.MainPage).Navigation.PopModalAsync();
        }

        #endregion

        #region Data

        private ObservableCollection<Student> _students;
        public ObservableCollection<Student> Students
        {
            get 
            {
                return _students;
            }
            set 
            {
                _students = value;
                OnPropertyChanged("Students");
            }
        }

        private Student _currentStudent;
        public Student CurrentStudent
        {
            get 
            {
                return _currentStudent;
            }
            set 
            {
                _currentStudent = value;
                OnPropertyChanged("CurrentStudent");
            }
        }

        private Homework _currentHomework;
        public Homework CurrentHomework
        {
            get 
            {
                return _currentHomework;
            }
            set
            {
                _currentHomework = value;
                OnPropertyChanged("CurrentHomework");
            }
        }

        #endregion
    }
}