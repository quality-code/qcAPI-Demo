using Xamarin.Forms;
using System;
using System.Linq;
using BookClient.Data;
using System.Collections.Generic;

namespace BookClient
{
    public class AddEditEmployeePage : ContentPage
    {
        readonly Employee existingEmployee;
        readonly EntryCell nameCell, ageCell, salaryCell;
        readonly IList<Employee> employees;
        readonly EmployeeManager manager;

        public AddEditEmployeePage(EmployeeManager manager, IList<Employee> employees, Employee existingEmployee = null)
        {
            this.manager = manager;
            this.employees = employees;
            this.existingEmployee = existingEmployee;

            var tableView = new TableView {
                Intent = TableIntent.Form,
                Root = new TableRoot(existingEmployee != null ? "Edit Employee" : "New Employee") {  
                    new TableSection("Details") {
                        new TextCell {
                            Text = "ID",
                            Detail = (existingEmployee != null) ? existingEmployee.id.ToString() : "Will be generated"
                        },
                        (nameCell = new EntryCell {
                            Label = "Name",
                            Placeholder = "add name",
                            Text = (existingEmployee != null) ? existingEmployee.employee_name : null,
                        }),
                        (ageCell = new EntryCell {
                            Label = "Age",
                            Placeholder = "add age",
                            Text = (existingEmployee != null) ? existingEmployee.employee_age.ToString() : null,
                        }),
                        (salaryCell = new EntryCell {
                            Label = "Profile",
                            Placeholder = "add profile",
                            Text = (existingEmployee != null) ? existingEmployee.profile_image : null,
                        }),
                    },
                }
            };

            Button button = new Button() {
                BackgroundColor = existingEmployee != null ? Color.Gray : Color.Green,
                TextColor = Color.White,
                Text = existingEmployee != null ? "Finished" : "Add Employee",
                BorderRadius = 0,
            };
            button.Clicked += OnDismiss;

            Content = new StackLayout
            {
                Spacing = 0,
                Children = { tableView, button },
            };
        }

        async void OnDismiss(object sender, EventArgs e)
        {
            Button button = (Button) sender;
            button.IsEnabled = false;
            this.IsBusy = true;
            try
            {
                string name = nameCell.Text;
                string salary = salaryCell.Text;
                int age = Int32.Parse(ageCell.Text);

                if (string.IsNullOrWhiteSpace(name)
                    || string.IsNullOrWhiteSpace(salary)
                    || age<=0)
                {
                    this.IsBusy = false;
                    await this.DisplayAlert("Missing Information",
                        "You must enter values for the Name, Age, and Salary.",
                        "OK");
                }
                else
                {
                    if (existingEmployee != null)
                    {
                        existingEmployee.employee_name = name;
                        existingEmployee.employee_age = age;
                        existingEmployee.employee_salary = salary;

                        await manager.Update(existingEmployee);
                        int pos = employees.IndexOf(existingEmployee);
                        employees.RemoveAt(pos);
                        employees.Insert(pos, existingEmployee);
                    }
                    else
                    {
                        Employee book = await manager.Add(name, age, salary);
                        employees.Add(book);
                    }

                    await Navigation.PopModalAsync();
                }

            }
            finally
            {
                this.IsBusy = false;
                button.IsEnabled = true;
            }
        }
    }
}

