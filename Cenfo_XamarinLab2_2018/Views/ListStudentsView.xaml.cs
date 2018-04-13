using System;
using System.Collections.Generic;
using Cenfo_XamarinLab2_2018.ViewModels;
using Xamarin.Forms;

namespace Cenfo_XamarinLab2_2018.Views
{
    public partial class ListStudentsView : ContentPage
    {
        public ListStudentsView()
        {
            InitializeComponent();

            BindingContext = StudentsViewModel.GetInstance();
        }
    }
}
