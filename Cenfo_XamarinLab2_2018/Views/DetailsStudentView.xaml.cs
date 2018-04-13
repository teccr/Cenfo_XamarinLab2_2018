using System;
using System.Collections.Generic;
using Cenfo_XamarinLab2_2018.ViewModels;
using Xamarin.Forms;

namespace Cenfo_XamarinLab2_2018.Views
{
    public partial class DetailsStudentView : ContentPage
    {
        public DetailsStudentView()
        {
            InitializeComponent();

            BindingContext = StudentsViewModel.GetInstance();
        }
    }
}
