﻿using LearningWpfProject.Services;
using LearningWpfProject.ViewModel;
using System.Windows;

namespace LearningWpfProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel([new JsonTaskRepository(), new LiteDbTaskRepository()]);
            DataContext = mainWindowViewModel;
        }
    }
}