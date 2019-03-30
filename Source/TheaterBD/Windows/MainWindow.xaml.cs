using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TheaterBD.Class;

namespace TheaterBD
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatePicker h, h2, h3;
        SqlDataAdapter adapter, adapterSpectacle, adapterShedule, adapterTicket, adapterACTSPEC;
        DataTable personalTable, spectacleTable, sheduleTable, ticketTable, actpecTable;
        bool checkHoliday = false;
        char gen, acc, acc2;

        public MainWindow()
        {
            InitializeComponent();
            DisplayPerson();
            DisplaySpectacle();
            DisplayShedule();
            DisplayTickets();
            DisplayPersonToSpectacle();
            h = (DatePicker)FindName("dtp_birthday");
            h2 = (DatePicker)FindName("dtp_dateOfSpectacle");
            h3 = (DatePicker)FindName("dtp_searchDateOfSpectacle");

            persons = new List<Person_>();
            spectacles = new List<Spectacle_>();
            shedules = new List<Shedule_>();

            Actors.ItemsSource = persons;
            SpecActor.ItemsSource = spectacles;
            cmb_Spectacle.ItemsSource = spectacles;
            cmb_SpectacleForTickets.ItemsSource = spectacles;
            cmb_SpectacleForTickets.ItemsSource = shedules;

            StartWorking();
        }

        public List<Person_> persons { get; private set; } // свойтсво
        public List<Spectacle_> spectacles { get; private set; } // свойтсво
        public List<Shedule_> shedules { get; private set; } // свойтсво

        const string patternMobile = @"\d{1,20}"; // регулярка для проверки на цифры

        public SqlConnection sqlCon = new SqlConnection(@"Data Source=NOTEBOOK-ASUS\SQLEXPRESS;Initial Catalog=TheaterDB;Integrated Security=True;Connect Timeout=30; Column Encryption Setting = Enabled"); // подключение бд

        void StartWorking()
        {
            #region bd подключение сотрудников combobox

            //подключить

            try
            {

                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();

                SqlCommand command = new SqlCommand("ob_PersonalComboBoxItem", sqlCon);     //указываем какую команду вызываем
                command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int Id = reader.GetInt32(0);
                    string Name = reader.GetString(1);
                    string Surname = reader.GetString(2);
                    string Patronymic = reader.GetString(3);

                    Person_ person = new Person_(Id, Surname + " " + Name + " " + Patronymic);
                    persons.Add(person);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                sqlCon.Close();
            }
            #endregion

            #region bd подключение спектаклей combobox

            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();

                SqlCommand command = new SqlCommand("ob_SpectacleComboBoxItem", sqlCon);     //указываем какую команду вызываем
                command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int Id = reader.GetInt32(0);
                    string Name = reader.GetString(1);

                    Spectacle_ spectacle = new Spectacle_(Id, Name);
                    spectacles.Add(spectacle);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
            #endregion

            #region bd подключение билетов combobox

            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();

                SqlCommand command = new SqlCommand("ob_TicketComboBoxItem", sqlCon);     //указываем какую команду вызываем
                command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int Id = reader.GetInt32(0);
                    string Name = reader.GetString(1);

                    Shedule_ shedule = new Shedule_(Id, Name);
                    shedules.Add(shedule);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
            #endregion
        }

        #region ===Personal===

        public void DisplayPerson() // обновить
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {

                    SqlCommand command = new SqlCommand("ob_PersonalSelectAll", sqlCon);     //указываем какую команду вызываем
                    command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                    sqlCon.Open();

                    //SqlDataReader result = command.ExecuteReader();//выполняем процедуру, попутно записывая результат

                    personalTable = new DataTable();
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(personalTable);
                    personalGrid.ItemsSource = personalTable.DefaultView;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private void displayPerson_Click(object sender, RoutedEventArgs e) // обновить datagrid
        {
            DisplayPerson();
        }

        private void holiday_Checked(object sender, RoutedEventArgs e)  // true
        {
            checkHoliday = true;
        }


        private void holiday_Unchecked(object sender, RoutedEventArgs e)  // false
        {
            checkHoliday = false;
        }

        private void genderM_Checked(object sender, RoutedEventArgs e)  // value M for person
        {
            string m = "M";
            gen = Char.Parse(m);
        }

        private void genderW_Checked(object sender, RoutedEventArgs e)
        {
            string m = "W";
            gen = Char.Parse(m);
        }

        private void addPerson_Click(object sender, RoutedEventArgs e) // добавление сотрудника 
        {
            string txtName = txt_name.Text, txtSurname = txt_surname.Text, txtPatronymic = txt_patronymic.Text, nameOfRole = txt_role.Text, mobile = txt_mobile.Text, bankcard = txt_bankcard.Text;
            int exp, sal;

            DateTime? dateTime = new DateTime();
            dateTime = h.SelectedDate;

            //
            if (
                Regex.IsMatch(txt_experience.Text, patternMobile, RegexOptions.IgnoreCase) &&
                Regex.IsMatch(txt_salary.Text, patternMobile, RegexOptions.IgnoreCase) &&
                Regex.IsMatch(txt_mobile.Text, patternMobile, RegexOptions.IgnoreCase) &&
                Regex.IsMatch(txt_bankcard.Text, patternMobile, RegexOptions.IgnoreCase))
            {
                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        if (txt_name.Text != "" && txt_surname.Text != "" && txt_patronymic.Text != "" && txt_role.Text != "" && txt_mobile.Text != "" && txt_experience.Text != "" && txt_salary.Text != "" && txt_bankcard.Text != "")
                        {
                            //Int32.TryParse(txt_bankcard.Text, out bankcard) $$ Int32.TryParse(txt_mobile.Text, out mobile) &&
                            if (Int32.TryParse(txt_experience.Text, out exp) && Int32.TryParse(txt_salary.Text, out sal))
                            {
                                if(mobile.Length == 12){
                                    if(bankcard.Length == 16)
                                    {
                                        SqlCommand command = new SqlCommand("sp_InsertPerson", sqlCon);     //указываем какую команду вызываем
                                        command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                                        sqlCon.Open();                                                      // открываем подключение к бд                            

                                        command.Parameters.AddWithValue("@Name", txtName);
                                        command.Parameters.AddWithValue("@Surname", txtSurname);
                                        command.Parameters.AddWithValue("@Patronymic", txtPatronymic);
                                        command.Parameters.AddWithValue("@Gender", gen);
                                        command.Parameters.AddWithValue("@Birthday", dateTime);
                                        command.Parameters.AddWithValue("@Mobile", mobile);
                                        command.Parameters.AddWithValue("@NameOfRole", nameOfRole);
                                        command.Parameters.AddWithValue("@Experience", exp);
                                        command.Parameters.AddWithValue("@On_holiday", checkHoliday);
                                        command.Parameters.AddWithValue("@Salary", sal);
                                        command.Parameters.AddWithValue("@Bankcard_number", bankcard);

                                        SqlDataReader result = command.ExecuteReader();//выполняем процедуру, попутно записывая результат
                                        MessageBox.Show("Сотрудник добавлен!");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Номер банковской карты в формате XXXX-XXXX-XXXX-XXXX (16 цифр)");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Телефон должен содержать XXX-XX-XXXXXXX (12 цифр)");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Введите корректный формат для числовых полей!");
                            }

                        }
                        else
                        {
                            MessageBox.Show("Все поля должны быть заполнены!");
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlCon.Close();
                    DisplayPerson();
                }
            }
            else
            {
                MessageBox.Show("Ошибка данных!");
            }
        }

        private void updatePerson_Click(object sender, RoutedEventArgs e) // изменить записи сотрудника
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);
            adapter.Update(personalTable);
            MessageBox.Show("Изменил!");
            DisplayPerson();
        }

        private void deletePerson_Click(object sender, RoutedEventArgs e) // удалить сотрудника
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    SqlCommand command = new SqlCommand("ob_PersobnalDeleteAll", sqlCon);     //указываем какую команду вызываем
                    command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                    sqlCon.Open();

                    SqlDataReader result = command.ExecuteReader();//выполняем процедуру, попутно записывая результат
                    MessageBox.Show("Все сотрудники удалены!");
                    
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
                DisplayPerson();
            }
        }

        private void deletePersonItem_Click(object sender, RoutedEventArgs e)  // удаляем один элемент
        {
            try
            {
                SqlCommand command = new SqlCommand("ob_PersonalDeleteItem", sqlCon);     //указываем какую команду вызываем
                command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру
                DataRowView index = (DataRowView)personalGrid.SelectedItem;
                var id = index.Row[0];
                command.Parameters.AddWithValue("@id", id);

                sqlCon.Open();

                SqlDataReader result = command.ExecuteReader();//выполняем процедуру, попутно записывая результат

                MessageBox.Show("Сотрудник удален!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
                DisplayPerson();
            }
        }

        private void ClearTxtPerson_Click(object sender, RoutedEventArgs e)  // очистить поля в сотрудниках
        {
            txt_surname.Text = "";
            txt_name.Text = "";
            txt_patronymic.Text = "";
            txt_mobile.Text = "";
            txt_role.Text = "";
            txt_experience.Text = "";
            txt_bankcard.Text = "";
            txt_salary.Text = "";
        }

        #region ===Search===

        private void SearchPersonal_Click(object sender, RoutedEventArgs e) // поиск персонала по ФИО
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    SqlCommand command = new SqlCommand("ob_PersonalSearchItem", sqlCon);     //указываем какую команду вызываем
                    command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                    sqlCon.Open();

                    command.Parameters.AddWithValue("@Name", txt_searchName.Text);
                    command.Parameters.AddWithValue("@Surname", txt_searchSurname.Text);
                    command.Parameters.AddWithValue("@Patronymic", txt_searchPatronymic.Text);

                    // command.ExecuteReader();//выполняем процедуру, попутно записывая результат

                    personalTable = new DataTable();
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(personalTable);
                    personalGrid.ItemsSource = personalTable.DefaultView;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        #endregion

        #endregion

        #region===Spectacle===

        public void DisplaySpectacle() // обновить spectacle
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {

                    SqlCommand command = new SqlCommand("ob_SpectacleSelectAll", sqlCon);     //указываем какую команду вызываем
                    command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                    sqlCon.Open();

                    spectacleTable = new DataTable();
                    adapterSpectacle = new SqlDataAdapter(command);
                    adapterSpectacle.Fill(spectacleTable);
                    spectacleGrid.ItemsSource = spectacleTable.DefaultView;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private void AccRU_Check(object sender, RoutedEventArgs e)  // RU
        {
            string b = "R";
            acc = Char.Parse(b);
        }

        private void AccEN_Click(object sender, RoutedEventArgs e)  //EN
        {
            string b = "E";
            acc = Char.Parse(b);
        }

        private void Acc2R_Click(object sender, RoutedEventArgs e) //r
        {
            string b = "R";
            acc2 = Char.Parse(b);
        }

        private void Acc2E_Click(object sender, RoutedEventArgs e) //e
        {
            string b = "E";
            acc2 = Char.Parse(b);
        }

        private void RefuseSpectacle_Click(object sender, RoutedEventArgs e) // обновить
        {
            DisplaySpectacle();
        }

        private void ClearSpec_Click(object sender, RoutedEventArgs e) // очистить
        {
            txt_spectacleName.Text = "";
            txt_spectacleDesc.Text = "";
        }

        private void SearchSpectacle_Click(object sender, RoutedEventArgs e) // поиск спектакля
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    SqlCommand command = new SqlCommand("ob_SpectacleSearchItem", sqlCon);     //указываем какую команду вызываем
                    command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                    sqlCon.Open();

                    command.Parameters.AddWithValue("@NameOfSpectacle", txt_searchSpectacleName.Text);
                    command.Parameters.AddWithValue("@AccountingSpectacle", acc2);

                    spectacleTable = new DataTable();
                    adapterSpectacle = new SqlDataAdapter(command);
                    adapterSpectacle.Fill(spectacleTable);
                    spectacleGrid.ItemsSource = spectacleTable.DefaultView;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private void SpectacleInsert_Click(object sender, RoutedEventArgs e) // добавить спектакль
        {
            string txtspectacleName = txt_spectacleName.Text, txtspectacleDesc = txt_spectacleDesc.Text;
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    SqlCommand command = new SqlCommand("ob_SpectacleInsert", sqlCon);     //указываем какую команду вызываем
                    command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                    sqlCon.Open();

                    command.Parameters.AddWithValue("@NameOfSpectacle", txtspectacleName);
                    command.Parameters.AddWithValue("@Description", txtspectacleDesc);
                    command.Parameters.AddWithValue("@AccountingSpectacle", acc);

                    SqlDataReader result = command.ExecuteReader();//выполняем процедуру, попутно записывая результат
                    MessageBox.Show("Спектакль добавлен!");
                   
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
                DisplaySpectacle();
            }
        }

        private void UpdateSpectacle_Click(object sender, RoutedEventArgs e) // update спектакли
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapterSpectacle);
            adapterSpectacle.Update(spectacleTable);
            MessageBox.Show("Изменил!");
            DisplaySpectacle();
        }

        private void DeleteSpectacle_Click(object sender, RoutedEventArgs e) // удалить спектакль
        {
            try
            {
                SqlCommand command = new SqlCommand("ob_SpectacleDeleteItem", sqlCon);     //указываем какую команду вызываем
                command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру
                DataRowView index = (DataRowView)spectacleGrid.SelectedItem;
                var id = index.Row[0];
                command.Parameters.AddWithValue("@id", id);

                sqlCon.Open();

                SqlDataReader result = command.ExecuteReader();//выполняем процедуру, попутно записывая результат

                MessageBox.Show("Спектакль удален!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
                DisplaySpectacle();
            }
        }

        #endregion

        #region===Shedule===

        public void DisplayShedule() // обновить расписание
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {

                    SqlCommand command = new SqlCommand("ob_SheduleSelectAll", sqlCon);     //указываем какую команду вызываем
                    command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                    sqlCon.Open();

                    sheduleTable = new DataTable();
                    adapterShedule = new SqlDataAdapter(command);
                    adapterShedule.Fill(sheduleTable);
                    sheduleGrid.ItemsSource = sheduleTable.DefaultView;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private void RefuseShedule_Click(object sender, RoutedEventArgs e) // обновить кнопка
        {
            DisplayShedule();
        }

        private void InsertShedule_Click(object sender, RoutedEventArgs e) // добавить расписание
        {

            DateTime? dateTime2 = new DateTime();
            dateTime2 = h2.SelectedDate;

            int dc = 0;
            for (int i = 0; i < spectacles.Count; i++)
            {
                if (spectacles[i].NNN == cmb_Spectacle.Text)
                    dc = spectacles[i].Id;
            }

            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    SqlCommand command = new SqlCommand("sp_InsertShedule", sqlCon);     //указываем какую команду вызываем
                    command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                    sqlCon.Open();

                    command.Parameters.AddWithValue("@ID_Spec", dc);
                    command.Parameters.AddWithValue("@DateSpec", dateTime2);

                    SqlDataReader result = command.ExecuteReader();//выполняем процедуру, попутно записывая результат
                    MessageBox.Show("Расписание добавлено!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
                DisplayShedule();
            }
        }

        //private void UpdateShedule_Click(object sender, RoutedEventArgs e) // изменить расписание
        //{
        //    SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapterShedule);
        //    adapterShedule.Update(sheduleTable);
        //    MessageBox.Show("Изменил!");
        //    DisplayShedule();
        //}

        private void DeleteShedule_Click(object sender, RoutedEventArgs e) // удалить расписание
        {

            DateTime? dateTime2 = new DateTime();
            dateTime2 = h2.SelectedDate;

            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    SqlCommand command = new SqlCommand("ob_SheduleDeleteItem", sqlCon);     //указываем какую команду вызываем
                    command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                    DataRowView index = (DataRowView)sheduleGrid.SelectedItem;
                    var id = index.Row[0];
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@DateSpec", dateTime2);

                    sqlCon.Open();

                    SqlDataReader result = command.ExecuteReader();//выполняем процедуру, попутно записывая результат

                    MessageBox.Show("Расписание удалено!");
                }
                    
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
                DisplayShedule();
            }
        }

        private void SearchSheduleItem_Click(object sender, RoutedEventArgs e) // поиск расписания по дате
        {
            DateTime? dateTime3 = new DateTime();
            dateTime3 = h3.SelectedDate;
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    SqlCommand command = new SqlCommand("ob_SheduleSearchItem", sqlCon);     //указываем какую команду вызываем
                    command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                    sqlCon.Open();

                    command.Parameters.AddWithValue("@DateSpec", dateTime3);

                    // command.ExecuteReader();//выполняем процедуру, попутно записывая результат

                    sheduleTable = new DataTable();
                    adapterShedule = new SqlDataAdapter(command);
                    adapterShedule.Fill(sheduleTable);
                    sheduleGrid.ItemsSource = sheduleTable.DefaultView;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        #endregion

        #region===Ticket===

        public void DisplayTickets()
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {

                    SqlCommand command = new SqlCommand("ob_TicketSelectAll", sqlCon);     //указываем какую команду вызываем
                    command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                    sqlCon.Open();

                    ticketTable = new DataTable();
                    adapterTicket = new SqlDataAdapter(command);
                    adapterTicket.Fill(ticketTable);
                    ticketsGrid.ItemsSource = ticketTable.DefaultView;

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private void RefuseTickets_Click(object sender, RoutedEventArgs e) // обновить билеты кнопка
        {
            DisplayTickets();
        }

        private void InsertTicket_Click(object sender, RoutedEventArgs e) // добавление билета
        {
            string pr = txt_ticketPrice.Text, cou = txt_ticketCount.Text;
            int dc = 0;
            for (int i = 0; i < shedules.Count; i++)
            {
                if (shedules[i].SH == cmb_SpectacleForTickets.Text)
                    dc = shedules[i].Id;
            }

            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    SqlCommand command = new SqlCommand("sp_InsertTicket", sqlCon);     //указываем какую команду вызываем
                    command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                    sqlCon.Open();

                    command.Parameters.AddWithValue("@ID_Spec", dc);
                    command.Parameters.AddWithValue("@Price", pr);
                    command.Parameters.AddWithValue("@Count", cou);

                    SqlDataReader result = command.ExecuteReader();//выполняем процедуру, попутно записывая результат
                    MessageBox.Show("Билет добавлен!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
                DisplayTickets();
            }
        }

        private void DeleteTicket_Click(object sender, RoutedEventArgs e) // удаление билета
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    SqlCommand command = new SqlCommand("ob_TicketDeleteItem", sqlCon);     //указываем какую команду вызываем
                    command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                    DataRowView index = (DataRowView)ticketsGrid.SelectedItem;
                    var id = index.Row[0];
                    command.Parameters.AddWithValue("@id", id);

                    sqlCon.Open();

                    SqlDataReader result = command.ExecuteReader();//выполняем процедуру, попутно записывая результат

                    MessageBox.Show("Билет удален!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
                DisplayTickets();
            }
        }

        #endregion

        #region===AddPersonToSpectacle===

        public void DisplayPersonToSpectacle() // вывод
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {

                    SqlCommand command = new SqlCommand("ob_ActorToSpectacleSelectAll", sqlCon);     //указываем какую команду вызываем
                    command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                    sqlCon.Open();

                    actpecTable = new DataTable();
                    adapterACTSPEC = new SqlDataAdapter(command);
                    adapterACTSPEC.Fill(actpecTable);
                    specACTGrid.ItemsSource = actpecTable.DefaultView;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }

        private void addACTSPEC_Click(object sender, RoutedEventArgs e) // добавить
        {
            int a1 = 0, a2 = 0;
            for (int i = 0; i < persons.Count; i++)
            {
                if (persons[i].FIO == Actors.Text)
                    a1 = persons[i].Id;
            }
            for (int i = 0; i < spectacles.Count; i++)
            {
                if (spectacles[i].NNN == SpecActor.Text)
                    a2 = spectacles[i].Id;
            }

            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    SqlCommand command = new SqlCommand("sp_InsertPersonToSpectacle", sqlCon);     //указываем какую команду вызываем
                    command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                    sqlCon.Open();

                    command.Parameters.AddWithValue("@ID_Actor", a1);
                    command.Parameters.AddWithValue("@ID_Spec", a2);

                    SqlDataReader result = command.ExecuteReader();//выполняем процедуру, попутно записывая результат
                    MessageBox.Show("Добавлен!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
                DisplayPersonToSpectacle();
            }
        }

        private void delACTSPEC_Click(object sender, RoutedEventArgs e) // удалить
        {
            int a1 = 0, a2 = 0;
            for (int i = 0; i < persons.Count; i++)
            {
                if (persons[i].FIO == Actors.Text)
                    a1 = persons[i].Id;
            }
            for (int i = 0; i < spectacles.Count; i++)
            {
                if (spectacles[i].NNN == SpecActor.Text)
                    a2 = spectacles[i].Id;
            }

            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    SqlCommand command = new SqlCommand("ob_ActorFromSpectacleDeleteItem", sqlCon);     //указываем какую команду вызываем
                    command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                    //DataRowView index = (DataRowView)specACTGrid.SelectedItem;
                    //var id = index.Row[0];
                    command.Parameters.AddWithValue("@idA", a1);
                    command.Parameters.AddWithValue("@idSP", a2);

                    sqlCon.Open();

                    SqlDataReader result = command.ExecuteReader();//выполняем процедуру, попутно записывая результат

                    MessageBox.Show("Удален!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
                DisplayPersonToSpectacle();
            }
        }

        private void refuseACTSPEC(object sender, RoutedEventArgs e) // обновить
        {
            DisplayPersonToSpectacle();
        }

        private void searchACTSPEC_Click(object sender, RoutedEventArgs e) // поиск
        {
            int a2 = 0;
            for (int i = 0; i < spectacles.Count; i++)
            {
                if (spectacles[i].NNN == SpecActor.Text)
                    a2 = spectacles[i].Id;
            }
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    SqlCommand command = new SqlCommand("ob_ACTSpecSearchItem", sqlCon);     //указываем какую команду вызываем
                    command.CommandType = System.Data.CommandType.StoredProcedure;       // указываем, что команда представляет хранимую процедуру

                    sqlCon.Open();

                    command.Parameters.AddWithValue("@idSP", a2);

                    actpecTable = new DataTable();
                    adapterACTSPEC = new SqlDataAdapter(command);
                    adapterACTSPEC.Fill(actpecTable);
                    specACTGrid.ItemsSource = actpecTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }
        #endregion

    }
}