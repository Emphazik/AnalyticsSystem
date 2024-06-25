using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AnalyticsSystem.AdminTools;
using AnalyticsSystem.ApplicationData;
using AnalyticsSystem.Models; // Обновите имя пространства имен при необходимости

namespace AnalyticsSystem
{
    public partial class MainWindow : Window
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
                .Where(t => t.ProjectName.ToLower().Contains(filter) ||
                            t.Description.ToLower().Contains(filter))
                .ToList();
            TasksDataGrid.ItemsSource = filteredProjects;
        }
        private void txtSearchSetting_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = txtSearchSetting.Text.ToLower();
            var filteredSetting = AppConnect.analyticsSystemEntities.Settings
                .Where(t => t.SettingName.ToLower().Contains(filter) ||
                            t.SettingValue.ToLower().Contains(filter))
                .ToList();
            TasksDataGrid.ItemsSource = filteredSetting;
        }
        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var newUserWindow = new UserWindow(); // Assuming UserWindow is a window for user input
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
                var editUserWindow = new UserWindow(selectedUser); // Assuming UserWindow is a window for user input
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
                            user.Password = updatedUser.Password; // Update password if provided
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
            //var newProject = new Project
            //{
            //    ProjectName = "New Project",
            //    Description = "New Project Description",
            //    StartDate = DateTime.Now,
            //    EndDate = DateTime.Now.AddMonths(1)
            //};
            //AppConnect.analyticsSystemEntities.Projects.Add(newProject);
            //AppConnect.analyticsSystemEntities.SaveChanges();
            //LoadData();
        }

        private void EditProject_Click(object sender, RoutedEventArgs e)
        {
            //if (ProjectsDataGrid.SelectedItem is Project selectedProject)
            //{
            //    selectedProject.ProjectName = "Updated Project";
            //    selectedProject.Description = "Updated Project Description";
            //    selectedProject.StartDate = DateTime.Now;
            //    selectedProject.EndDate = DateTime.Now.AddMonths(1);
            //    AppConnect.analyticsSystemEntities.SaveChanges();
            //    LoadData();
            //}
            //else
            //{
            //    MessageBox.Show("Please select a project to edit.");
            //}
        }

        private void DeleteProject_Click(object sender, RoutedEventArgs e)
        {
            //if (ProjectsDataGrid.SelectedItem is Project selectedProject)
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
            //var newTask = new Task
            //{
            //    TaskName = "New Task",
            //    Description = "New Task Description",
            //    ProjectId = 1,
            //    StatusId = 1,
            //    AssignedTo = "User1",
            //    StartDate = DateTime.Now,
            //    EndDate = DateTime.Now.AddDays(7)
            //};
            //AppConnect.analyticsSystemEntities.Tasks.Add(newTask);
            //AppConnect.analyticsSystemEntities.SaveChanges();
            //LoadData();
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            //if (TasksDataGrid.SelectedItem is Task selectedTask)
            //{
            //    selectedTask.TaskName = "Updated Task";
            //    selectedTask.Description = "Updated Task Description";
            //    selectedTask.ProjectId = 1;
            //    selectedTask.StatusId = 1;
            //    selectedTask.AssignedTo = "User1";
            //    selectedTask.StartDate = DateTime.Now;
            //    selectedTask.EndDate = DateTime.Now.AddDays(7);
            //    AppConnect.analyticsSystemEntities.SaveChanges();
            //    LoadData();
            //}
            //else
            //{
            //    MessageBox.Show("Please select a task to edit.");
            //}
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            //if (TasksDataGrid.SelectedItem is Task selectedTask)
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
            //var newSetting = new Setting
            //{
            //    SettingName = "New Setting",
            //    SettingValue = "New Setting Value"
            //};
            //AppConnect.analyticsSystemEntities.Settings.Add(newSetting);
            //AppConnect.analyticsSystemEntities.SaveChanges();
            //LoadData();
        }

        private void EditSetting_Click(object sender, RoutedEventArgs e)
        {
            //if (SettingsDataGrid.SelectedItem is Setting selectedSetting)
            //{
            //    selectedSetting.SettingName = "Updated Setting";
            //    selectedSetting.SettingValue = "Updated Setting Value";
            //    AppConnect.analyticsSystemEntities.SaveChanges();
            //    LoadData();
            //}
            //else
            //{
            //    MessageBox.Show("Please select a setting to edit.");
            //}
        }

        private void DeleteSetting_Click(object sender, RoutedEventArgs e)
        {
            //if (SettingsDataGrid.SelectedItem is Setting selectedSetting)
            //{
            //    AppConnect.analyticsSystemEntities.Settings.Remove(selectedSetting);
            //    AppConnect.analyticsSystemEntities.SaveChanges();
            //    LoadData();
            //}
            //else
            //{
            //    MessageBox.Show("Please select a setting to delete.");
            //}
        }
    }
}
