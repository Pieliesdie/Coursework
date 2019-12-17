using Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Data;

namespace subdClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isUpdate;

        //public string name => "You are log in as " + App.Name;

        public async void Init()
        {
            var x = await App.dataReader.GetSecurityLabelsAsync();
            var info = await App.dataReader.GetInfoAsync();

            var name = $"You are log in as {(await App.dataReader.GetInfoAsync()).Login}, Доступ - {x.Where(x => x.Id == info.Permission).FirstOrDefault()} ";

            InfoItem.Header = name;

            SecurityLabelAuthorTextBox.ItemsSource = x;

            SecurityLabelArticleTextBox.ItemsSource = x;

            SecurityLabelDocumentTextBox.ItemsSource = x;

            SelectAuthorTextBox.ItemsSource = await App.dataReader.GetAuthorsAsync();

            DocumentTypeTextBox.ItemsSource = await App.dataReader.GetDocumentTypesAsync();

            switch ((await App.dataReader.GetInfoAsync()).Role)
            {
                case "NotAuthorizedUser":
                case "AuthorizedUser":
                    MenuButton.IsEnabled = false;
                    AuthorsButton.IsEnabled = false;
                    dataGrid.ContextMenu.IsEnabled = false;
                    break;
                case "Admin":
                    ConsoleButton.Visibility = Visibility.Visible;
                    break;
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            Init();
            this.DataContext = this;
        }


        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = await App.dataReader.GetArticlesAsync();
        }


        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private async void AuthorsButton_Click(object sender, RoutedEventArgs e)
        {
            tabCntrl.SelectedItem = GridTab;
            dataGrid.ItemsSource = await App.dataReader.GetAuthorsAsync();
        }

        private async void ArticlesButton_Click(object sender, RoutedEventArgs e)
        {
            tabCntrl.SelectedItem = GridTab;
            dataGrid.ItemsSource = await App.dataReader.GetArticlesAsync();
        }

        private async void TypeButton_Click(object sender, RoutedEventArgs e)
        {
            tabCntrl.SelectedItem = GridTab;
            dataGrid.ItemsSource = await App.dataReader.GetDocumentTypesAsync();
        }

        private async void AddAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Authors author = new Authors();
                if (isUpdate)
                    author.Id = (dataGrid.SelectedItem as Authors).Id;
                author.Name = NameTextBox.Text;
                author.MiddleName = MiddleNameTextBox.Text;
                author.Surname = SurNameTextBox.Text;
                author.Age = int.Parse(AgeTextBox.Text);
                author.Email = EmailTextBox.Text;
                author.Description = DescriptionTextBox.Text;

                var x = await App.dataReader.GetSecurityLabelsAsync();
                author.SecurityLabel = x.Where(x => x.Name == SecurityLabelAuthorTextBox.Text).Select(x => x.Id).First();

                bool isok;
                if (isUpdate)
                {
                    isok = await App.dataReader.UpdateAuthorAsync(author);
                }
                else
                {
                    isok = await App.dataReader.AddAuthorAsync(author);
                }
                if (isok)
                {
                    NameTextBox.Text = string.Empty;
                    MiddleNameTextBox.Text = string.Empty;
                    SurNameTextBox.Text = string.Empty;
                    AgeTextBox.Text = string.Empty;
                    EmailTextBox.Text = string.Empty;
                    DescriptionTextBox.Text = string.Empty;
                    SecurityLabelAuthorTextBox.SelectedIndex = -1;
                    MessageBox.Show("Запись добавлена");
                }
                else
                {
                    MessageBox.Show("Запрещено");
                }
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так...");
            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            isUpdate = false;
            tabCntrl.SelectedItem = AddAuthorTab;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            isUpdate = false;
            tabCntrl.SelectedItem = AddArticlesTab;
        }

        private async void AddArticleButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Articles article = new Articles();

                if (isUpdate)
                    article.Id = (dataGrid.SelectedItem as Articles).Id;

                article.ShortName = ShortNameTextBox.Text;
                article.Decription = ArticleDescriptionTextBox.Text;

                if (!isUpdate)
                    article.CreateDate = DateTime.Now;

                article.LastChange = DateTime.Now;
                article.AuthorId = ((Authors)SelectAuthorTextBox.SelectedItem)?.Id;
                article.DocumentTypeId = (DocumentTypeTextBox.SelectedItem as DocumentTypes)?.Id;
                article.SecurityLabel = (SecurityLabelArticleTextBox.SelectedItem as SecurityLabels).Id;

                bool isok;
                if (isUpdate)
                {
                    isok = await App.dataReader.UpdateArticleAsync(article);
                }
                else
                {
                    isok = await App.dataReader.AddArticleAsync(article);
                }

                if (isok)
                {
                    ShortNameTextBox.Text = string.Empty;
                    ArticleDescriptionTextBox.Text = string.Empty;
                    SelectAuthorTextBox.SelectedIndex = -1;
                    DocumentTypeTextBox.SelectedIndex = -1;
                    SecurityLabelArticleTextBox.SelectedIndex = -1;
                    MessageBox.Show("Запись добавлена");
                }
                else
                {
                    MessageBox.Show("Запрещено");
                }
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так...");
            }
        }

        private async void AddDocumentTypeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DocumentTypes document = new DocumentTypes();


                document.Type = AddDocumentTypeTextBox.Text;
                document.SecurityLabel = (SecurityLabelDocumentTextBox.SelectedItem as SecurityLabels)?.Id;

                bool isok;

                if (isUpdate)
                {
                    document.Id = (dataGrid.SelectedItem as DocumentTypes).Id;
                    isok = await App.dataReader.UpdateDocumentTypeAsync(document);
                }
                else
                {
                    isok = await App.dataReader.AddDocumentTypeAsync(document);
                }
                if (isok)
                {
                    AddDocumentTypeTextBox.Text = string.Empty;
                    SecurityLabelDocumentTextBox.SelectedIndex = -1;
                    MessageBox.Show("Запись добавлена");
                }
                else
                {
                    MessageBox.Show("Запрещено");
                }

            }
            catch
            {
                MessageBox.Show("Что-то пошло не так...");
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            isUpdate = false;
            tabCntrl.SelectedItem = AddDocumentTypeTab;
        }

        private async void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dataGrid.SelectedItem as Articles != null)
                {
                    var isok = await App.dataReader.DeleteArticleAsync((dataGrid.SelectedItem as Articles).Id);
                    if (isok)
                    {
                        dataGrid.ItemsSource = await App.dataReader.GetArticlesAsync();
                        MessageBox.Show("Запись удалена");
                    }
                    else
                    {
                        MessageBox.Show("Запрещено");
                    }
                }
                else if (dataGrid.SelectedItem as Authors != null)
                {
                    var isok = await App.dataReader.DeleteAuthorAsync((dataGrid.SelectedItem as Authors).Id);
                    if (isok)
                    {
                        dataGrid.ItemsSource = await App.dataReader.GetAuthorsAsync();
                        MessageBox.Show("Запись удалена");
                    }
                    else
                    {
                        MessageBox.Show("Запрещено");
                    }
                }
                else if (dataGrid.SelectedItem as DocumentTypes != null)
                {
                    var isok = await App.dataReader.DeleteDocumentTypeAsync((dataGrid.SelectedItem as DocumentTypes).Id);
                    if (isok)
                    {
                        dataGrid.ItemsSource = await App.dataReader.GetDocumentTypesAsync();
                        MessageBox.Show("Запись удалена");
                    }
                    else
                    {
                        MessageBox.Show("Запрещено");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так...");
            }

        }

        private async void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            isUpdate = true;
            if (dataGrid.SelectedItem as Articles != null)
            {
                ShortNameTextBox.Text = (dataGrid.SelectedItem as Articles).ShortName;
                ArticleDescriptionTextBox.Text = (dataGrid.SelectedItem as Articles).Decription;
                tabCntrl.SelectedItem = AddArticlesTab;
            }
            else if (dataGrid.SelectedItem as Authors != null)
            {
                tabCntrl.SelectedItem = AddAuthorTab;
            }
            else if (dataGrid.SelectedItem as DocumentTypes != null)
            {
                AddDocumentTypeTextBox.Text = (dataGrid.SelectedItem as DocumentTypes).Type;
                tabCntrl.SelectedItem = AddDocumentTypeTab;
            }
        }

        private void ConsoleButton_Click(object sender, RoutedEventArgs e)
        {
            tabCntrl.SelectedItem = ConsoleTab;
        }

        private async void ConsoleExecButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConsoleResultTextBox.Text += $"{await App.dataReader.ExecCode(ConsoleTextBox.Text)}\n";
            }
            catch
            {
                ConsoleResultTextBox.Text += $"error\n";
            }
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            new AuthWindow().Show();
            App.dataReader.Dispose();
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GC.Collect();
        }

        private async void CreateBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await App.dataReader.ExecCode(@"BACKUP DATABASE NotConsultantv2" +
                   @" TO DISK = 'C:\backup\backup.bak'" +
                   "    WITH FORMAT;");
                ConsoleResultTextBox.Text += "BACKUP DATABASE was successful\n";
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private async void RestoreBackupButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               await App.dataReader.ExecCode(@"alter database  NotConsultantv2 set offline with rollback immediate;restore DATABASE NotConsultantv2 from DISK = 'C:\backup\backup.bak' with replace;");
                ConsoleResultTextBox.Text += "Restore DATABASE was successful\n";
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }
    }
}
