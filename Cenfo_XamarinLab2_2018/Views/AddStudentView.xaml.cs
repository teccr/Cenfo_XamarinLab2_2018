using System;
using System.Collections.Generic;
using Cenfo_XamarinLab2_2018.ViewModels;
using Plugin.Media;
using Xamarin.Forms;

namespace Cenfo_XamarinLab2_2018.Views
{
    public partial class AddStudentView : ContentPage
    {
        public AddStudentView()
        {
            InitializeComponent();

            BindingContext = StudentsViewModel.GetInstance();
        }
    }
}
