using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Cenfo_XamarinLab2_2018.Models;
using Cenfo_XamarinLab2_2018.Views;
using Newtonsoft.Json;
using Plugin.Media;
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
            Students = await Student.GetAllStudents();
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
            TakePictureCommand = new Command(TakePicture);
            PickFileCommand = new Command(PickFile);
        }

        private async void TakePicture()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            //await App.Current.MainPage.DisplayAlert("File Location", file.Path, "OK");

            CurrentStudent.ProfilePicture = file.Path;
        }

        public ICommand TakePictureCommand
        {
            get;
            set;
        }

        private async void AddStudent()
        {
            System.Random random = new System.Random();
            CurrentStudent.Id = random.Next(100001, 10000000);
            if (string.IsNullOrEmpty(CurrentStudent.ProfilePicture)) 
                CurrentStudent.ProfilePicture = "icon";
            using (HttpClient client = new HttpClient())
            {
                var uri = new Uri(Utils.Utils.STUDENTS_URl);
                var json = JsonConvert.SerializeObject( CurrentStudent );
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(uri, content).ConfigureAwait(false);
                string ans = await responseMessage.Content.ReadAsStringAsync();
                CurrentStudent = JsonConvert.DeserializeObject<Student>( ans );
                Students.Add(CurrentStudent);
            }
            CurrentStudent = null;
            //MoveNext("Students");
        }

        public ICommand PickFileCommand
        {
            get;
            set;
        }

        public async void PickFile()
        {
            await CrossMedia.Current.Initialize();

            /*if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }*/

            CurrentHomework = new Homework();
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
            });

            if (file == null)
                return;

            //await App.Current.MainPage.DisplayAlert("File Location", file.Path, "OK");

            CurrentHomework.FilePath = file.Path;
            System.Random random = new System.Random();
            CurrentHomework.Id = random.Next(100001, 10000000);

            // REST Call
            using (HttpClient client = new HttpClient())
            {
                var uri = new Uri(Utils.Utils.HOMEWORK_URl + CurrentStudent.Id.ToString());
                var json = JsonConvert.SerializeObject(CurrentHomework);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(uri, content).ConfigureAwait(false);
                string ans = await responseMessage.Content.ReadAsStringAsync();
            }

            if (CurrentStudent.Homework == null)
                CurrentStudent.Homework = new ObservableCollection<Homework>();
            CurrentStudent.Homework.Add(CurrentHomework);
            CurrentHomework = null;
            OnPropertyChanged("CurrentStudent");
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
