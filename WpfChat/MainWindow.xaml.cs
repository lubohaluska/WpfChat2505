using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using WpfChat.ChatApp;
using Newtonsoft.Json;

namespace WpfChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<User_Controls.UsersContact> contacts = new List<User_Controls.UsersContact>();
        private Chat chat;

        private int _CurrentContactID;

        public MainWindow()
        {
            InitializeComponent();

            chat = new Chat();
            chat.OnUserLogin += OnUserLogin;

            RefreshData();

            //JSONExample();
        }

        private void RefreshData()
        {
            UsersContacts.Children.Clear();
            contacts.Clear();

            List<Contact> contactList = chat.GetContacts();

            for (int i = 0; i < contactList.Count; i++)
            {
                User_Controls.UsersContact control = new WpfChat.User_Controls.UsersContact();
                control.UpdateContact(contactList[i]);
                UsersContacts.Children.Add(control);
                contacts.Add(control);
                control.OnContactClicked += OnContactClicked;
            }
        }

        //tato metoda posiela ID kontaktu
        private void OnContactClicked(int pID)
        {
            _CurrentContactID = pID;
            RefreshData(pID);
        }

        private void RefreshData(int pID)
        {
            throw new NotImplementedException();
        }

        private void OnUserLogin(User pUser)
        {
            
        }

        private void JSONExample()
        {
            UserInfoData data = new UserInfoData()
            {
                _ID = 0,
                _Email = "ahoj",
            };

            //Tento radek prevede UserInfoData na typ string
            string s = JsonConvert.SerializeObject(data);

            Console.WriteLine(s);

            //Tento radek prevede string na typ UserInfoData
            UserInfoData newUserData = JsonConvert.DeserializeObject<UserInfoData>(s);

            Console.WriteLine(newUserData._ID.ToString());
            Console.WriteLine(newUserData._Email.ToString());
        }


        private void SendMSGTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && SendMSGTextBox.Text != "")  // neposle prazdnu medzeru
            {
                string s = SendMSGTextBox.Text;
                SendMSGTextBox.Text = "";

                MessageInfoData data = new MessageInfoData()
                {
                    _Content = s,
                    _Date = DateTime.Now,
                    _ID = 0,
                    _MessageDirectionType = MessageDirectionType.Outcome,
                    _MessageStateType = MessageStateType.Send,
                    _MessageType = MessageType.Text
                };

                Message message = new Message(data);
                chat.SendMessage(_CurrentContactID, null);
            }
        }
    }
}
