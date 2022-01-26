using BookClient.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BookClient
{
    public partial class MainPage : ContentPage
    {
        readonly IList<Employee> employees = new ObservableCollection<Employee>();
        readonly EmployeeManager manager = new EmployeeManager();

        public MainPage()
        {
            BindingContext = employees;
            InitializeComponent();
        }

        async void OnRefresh(object sender, EventArgs e)
        {
            var employeesCollection = await manager.GetAll();

            foreach (Employee employee in employeesCollection)
            {
                if (employees.All(b => b.id != employee.id))
                    employees.Add(employee);
            }
        }

        async void OnAddNewBook(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(
                new AddEditEmployeePage(manager, employees));
        }

        async void OnEditBook(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushModalAsync(
                new AddEditEmployeePage(manager, employees, (Employee)e.Item));
        }

        async void OnDeleteBook(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            Employee employee = item.CommandParameter as Employee;
            if (employee != null)
            {
                if (await this.DisplayAlert("Delete Employee?",
                    "Are you sure you want to delete the employee '"
                        + employee.employee_name + "'?", "Yes", "Cancel") == true)
                {
                    await manager.Delete(employee.id);
                    employees.Remove(employee);
                }
            }
        }
    }
}
