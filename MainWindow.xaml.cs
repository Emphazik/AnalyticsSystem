using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using AnalyticsSystem.AdminTools;
using AnalyticsSystem.ApplicationData;
using AnalyticsSystem.Models; // Обновите имя пространства имен при необходимости
using System.Reflection;
using Microsoft.Office.Interop.Excel;


namespace AnalyticsSystem
{
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            UsersDataGrid.ItemsSource = AppConnect.analyticsSystemEntities.Users.ToList();
            ProjectsDataGrid.ItemsSource = AppConnect.analyticsSystemEntities.Projects.ToList();
            TasksDataGrid.ItemsSource = AppConnect.analyticsSystemEntities.Tasks.ToList();
            SettingsDataGrid.ItemsSource = AppConnect.analyticsSystemEntities.Settings.ToList();
        }

        private void txtSearchUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = txtSearchUser.Text.ToLower();
            var filteredUsers = AppConnect.analyticsSystemEntities.Users
                .Where(u => u.Login.ToLower().Contains(filter) ||
                            u.Username.ToLower().Contains(filter) ||
                            u.Email.ToLower().Contains(filter) ||
                            u.Phone.ToLower().Contains(filter))
                .ToList();
            UsersDataGrid.ItemsSource = filteredUsers;
        }

        private void txtSearchTask_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = txtSearchTask.Text.ToLower();
            var filteredTasks = AppConnect.analyticsSystemEntities.Tasks
                .Where(t => t.TaskName.ToLower().Contains(filter) ||
                            t.Description.ToLower().Contains(filter))
                .ToList();
            TasksDataGrid.ItemsSource = filteredTasks;
        }

        private void txtSearchProject_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = txtSearchProject.Text.ToLower();
            var filteredProjects = AppConnect.analyticsSystemEntities.Projects
                .Where(p => p.ProjectName.ToLower().Contains(filter) ||
                            p.Description.ToLower().Contains(filter))
                .ToList();
            ProjectsDataGrid.ItemsSource = filteredProjects;
        }

        private void txtSearchSetting_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = txtSearchSetting.Text.ToLower();
            var filteredSettings = AppConnect.analyticsSystemEntities.Settings
                .Where(s => s.SettingName.ToLower().Contains(filter) ||
                            s.SettingValue.ToLower().Contains(filter))
                .ToList();
            SettingsDataGrid.ItemsSource = filteredSettings;
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var newUserWindow = new UserWindow(); 
            if (newUserWindow.ShowDialog() == true)
            {
                var newUser = newUserWindow.User;
                AppConnect.analyticsSystemEntities.Users.Add(newUser);
                AppConnect.analyticsSystemEntities.SaveChanges();
                LoadData();
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is Users selectedUser)
            {
                var editUserWindow = new UserWindow(selectedUser); 
                if (editUserWindow.ShowDialog() == true)
                {
                    var updatedUser = editUserWindow.User;
                    var user = AppConnect.analyticsSystemEntities.Users.Find(updatedUser.idUser);
                    if (user != null)
                    {
                        user.Login = updatedUser.Login;
                        user.Username = updatedUser.Username;
                        user.Email = updatedUser.Email;
                        user.Phone = updatedUser.Phone;
                        user.Role = updatedUser.Role;

                        if (!string.IsNullOrWhiteSpace(updatedUser.Password))
                        {
                            user.Password = updatedUser.Password; 
                        }

                        AppConnect.analyticsSystemEntities.SaveChanges();
                        LoadData();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to edit.");
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is Users selectedUser)
            {
                AppConnect.analyticsSystemEntities.Users.Remove(selectedUser);
                AppConnect.analyticsSystemEntities.SaveChanges();
                LoadData();
            }
            else
            {
                MessageBox.Show("Please select a user to delete.");
            }
        }

        private void AddProject_Click(object sender, RoutedEventArgs e)
        {
            //var newProjectWindow = new ProjectWindow(); 
            //if (newProjectWindow.ShowDialog() == true)
            //{
            //    var newProject = newProjectWindow.Project;
            //    AppConnect.analyticsSystemEntities.Projects.Add(newProject);
            //    AppConnect.analyticsSystemEntities.SaveChanges();
            //    LoadData();
            //}
        }

        private void EditProject_Click(object sender, RoutedEventArgs e)
        {
            //if (ProjectsDataGrid.SelectedItem is Projects selectedProject)
            //{
            //    var editProjectWindow = new ProjectWindow(selectedProject); 
            //    if (editProjectWindow.ShowDialog() == true)
            //    {
            //        var updatedProject = editProjectWindow.Project;
            //        var project = AppConnect.analyticsSystemEntities.Projects.Find(updatedProject.IdProject);
            //        if (project != null)
            //        {
            //            project.ProjectName = updatedProject.ProjectName;
            //            project.Description = updatedProject.Description;
            //            project.StartDate = updatedProject.StartDate;
            //            project.EndDate = updatedProject.EndDate;

            //            AppConnect.analyticsSystemEntities.SaveChanges();
            //            LoadData();
            //        }
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Please select a project to edit.");
            //}
        }

        private void DeleteProject_Click(object sender, RoutedEventArgs e)
        {
            //if (ProjectsDataGrid.SelectedItem is Projects selectedProject)
            //{
            //    AppConnect.analyticsSystemEntities.Projects.Remove(selectedProject);
            //    AppConnect.analyticsSystemEntities.SaveChanges();
            //    LoadData();
            //}
            //else
            //{
            //    MessageBox.Show("Please select a project to delete.");
            //}
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            //var newTaskWindow = new TaskWindow(); 
            //if (newTaskWindow.ShowDialog() == true)
            //{
            //    var newTask = newTaskWindow.Task;
            //    AppConnect.analyticsSystemEntities.Tasks.Add(newTask);
            //    AppConnect.analyticsSystemEntities.SaveChanges();
            //    LoadData();
            //}
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            //if (TasksDataGrid.SelectedItem is Tasks selectedTask)
            //{
            //    var editTaskWindow = new TaskWindow(selectedTask); 
            //    if (editTaskWindow.ShowDialog() == true)
            //    {
            //        var updatedTask = editTaskWindow.Task;
            //        var task = AppConnect.analyticsSystemEntities.Tasks.Find(updatedTask.IdTask);
            //        if (task != null)
            //        {
            //            task.TaskName = updatedTask.TaskName;
            //            task.Description = updatedTask.Description;
            //            task.ProjectId = updatedTask.ProjectId;
            //            task.StatusId = updatedTask.StatusId;
            //            task.AssignedTo = updatedTask.AssignedTo;
            //            task.StartDate = updatedTask.StartDate;
            //            task.EndDate = updatedTask.EndDate;

            //            AppConnect.analyticsSystemEntities.SaveChanges();
            //            LoadData();
            //        }
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Please select a task to edit.");
            //}
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            //if (TasksDataGrid.SelectedItem is Tasks selectedTask)
            //{
            //    AppConnect.analyticsSystemEntities.Tasks.Remove(selectedTask);
            //    AppConnect.analyticsSystemEntities.SaveChanges();
            //    LoadData();
            //}
            //else
            //{
            //    MessageBox.Show("Please select a task to delete.");
            //}
        }

        private void AddSetting_Click(object sender, RoutedEventArgs e)
        {
            //var newSettingWindow = new SettingWindow(); 
            //if (newSettingWindow.ShowDialog() == true)
            //{
            //    var newSetting = newSettingWindow.Setting;
            //    AppConnect.analyticsSystemEntities.Settings.Add(newSetting);
            //    AppConnect.analyticsSystemEntities.SaveChanges();
            //    LoadData();
            //}
        }

        private void EditSetting_Click(object sender, RoutedEventArgs e)
        {
            //if (SettingsDataGrid.SelectedItem is Settings selectedSetting)
            //{
            //    var editSettingWindow = new SettingWindow(selectedSetting); 
            //    if (editSettingWindow.ShowDialog() == true)
            //    {
            //        var updatedSetting = editSettingWindow.Setting;
            //        var setting = AppConnect.analyticsSystemEntities.Settings.Find(updatedSetting.IdSetting);
            //        if (setting != null)
            //        {
            //            setting.SettingName = updatedSetting.SettingName;
            //            setting.SettingValue = updatedSetting.SettingValue;

            //            AppConnect.analyticsSystemEntities.SaveChanges();
            //            LoadData();
            //        }
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Please select a setting to edit.");
            //}
        }

        private void DeleteSetting_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsDataGrid.SelectedItem is Settings selectedSetting)
            {
                AppConnect.analyticsSystemEntities.Settings.Remove(selectedSetting);
                AppConnect.analyticsSystemEntities.SaveChanges();
                LoadData();
            }
            else
            {
                MessageBox.Show("Please select a setting to delete.");
            }
        }

        private void ExportToExcel<T>(DataGrid dataGrid)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                FileName = "ExportedData.xlsx"
            };

            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                var application = new Microsoft.Office.Interop.Excel.Application();
                application.Visible = true;
                var workbook = application.Workbooks.Add(Missing.Value);
                var worksheet = (Worksheet)workbook.ActiveSheet;
                worksheet.Name = typeof(T).Name;

                for (int i = 0; i < dataGrid.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dataGrid.Columns[i].Header;
                }

                var itemsSource = dataGrid.ItemsSource.Cast<T>().ToList();
                for (int i = 0; i < itemsSource.Count; i++)
                {
                    for (int j = 0; j < dataGrid.Columns.Count; j++)
                    {
                        var bindingPath = ((Binding)dataGrid.Columns[j].ClipboardContentBinding).Path.Path;
                        var property = typeof(T).GetProperty(bindingPath);
                        worksheet.Cells[i + 2, j + 1] = property.GetValue(itemsSource[i], null);
                    }
                }

                workbook.SaveAs(saveFileDialog.FileName);
                workbook.Close(SaveChanges: false);
                application.Quit();
            }
        }



        private void ExportUsersToExcel_Click(object sender, RoutedEventArgs e)
        {
            ExportToExcel<Users>(UsersDataGrid);
        }

        private void ExportProjectsToExcel_Click(object sender, RoutedEventArgs e)
        {
            ExportToExcel<Projects>(ProjectsDataGrid);
        }

        private void ExportTasksToExcel_Click(object sender, RoutedEventArgs e)
        {
            ExportToExcel<Tasks>(TasksDataGrid);
        }

        private void ExportSettingsToExcel_Click(object sender, RoutedEventArgs e)
        {
            ExportToExcel<Settings>(SettingsDataGrid);
        }

        private void RefreshUsers_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void RefreshTasks_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void RefreshProjects_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void RefreshSettings_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
