using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

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

        public static async Task<ObservableCollection<StudentAPI>> GetAllStudents()
        {
            ObservableCollection<StudentAPI> students = new ObservableCollection<StudentAPI>();
            using(HttpClient client = new HttpClient())
            {
                var uri = new Uri("http://50cf4af0.ngrok.io/Student/getAllStudents");
                var json = JsonConvert.SerializeObject( new { ID = 1 } );
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(uri, content).ConfigureAwait(false);
                string ans = await responseMessage.Content.ReadAsStringAsync();
                students = JsonConvert.DeserializeObject<ObservableCollection<StudentAPI>>(ans);

            }

            return students;
        }
    }
}
