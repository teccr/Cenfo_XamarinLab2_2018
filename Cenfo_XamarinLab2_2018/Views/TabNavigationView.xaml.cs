using Cenfo_XamarinLab2_2018.Models;
using Cenfo_XamarinLab2_2018.ViewModels;
using Xamarin.Forms;

namespace Cenfo_XamarinLab2_2018.Views
{
    public partial class TabNavigationView : TabbedPage
    {
        public TabNavigationView()
        {
            InitializeComponent();
        }

        void Handle_Appearing(object sender, System.EventArgs e)
        {
            var viewModelInstance = StudentsViewModel.GetInstance();
            viewModelInstance.CurrentStudent = new Student();
        }
    }
}
