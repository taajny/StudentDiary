using StudentDiary.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace StudentDiary
{
    public partial class Main : Form
    {
        
        public bool isMaximize 
        {
            get
            {
                return Settings.Default.IsMaximize;
            }    
            set
            {
                Settings.Default.IsMaximize = value;
            }
        }

        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);
        public Main()
        {
            InitializeComponent();
            foreach (var item in Program.groups)
                cbGroups.Items.Add(item);
            cbGroups.SelectedIndex = 0;
            RefreshDiary();
            SetColumnHeader();
            if (isMaximize)
                WindowState = FormWindowState.Maximized;
        }
        public void RefreshDiary()
        {
            var students = _fileHelper.DeserializeFromFile();

            if (cbGroups.SelectedIndex == 0)
                dgvDiary.DataSource = students.OrderBy(x => x.Id).ToList();
            else
                dgvDiary.DataSource = students.Where(x => x.GroupId == cbGroups.SelectedIndex - 1).OrderBy(x => x.Id).ToList();

        }
        private void SetColumnHeader()
        {
            dgvDiary.Columns[0].HeaderText = "Numer";
            dgvDiary.Columns[1].HeaderText = "Imię";
            dgvDiary.Columns[2].HeaderText = "Nazwisko";
            dgvDiary.Columns[3].HeaderText = "Uwagi";
            dgvDiary.Columns[4].HeaderText = "Matematyka";
            dgvDiary.Columns[5].HeaderText = "Technologia";
            dgvDiary.Columns[6].HeaderText = "Fizyka";
            dgvDiary.Columns[7].HeaderText = "Język polski";
            dgvDiary.Columns[8].HeaderText = "Język obcy";
            dgvDiary.Columns[9].HeaderText = "Zajęcia dodatkowe";
            dgvDiary.Columns[10].Visible = false;
        }

        private void DeleteStudent(int id)
        {
            var students = _fileHelper.DeserializeFromFile();
            students.RemoveAll(x => x.Id == id);
            _fileHelper.SerializeToFile(students);
        }
        /*public void SerializeToFile(List<Student> students)
        {
            var serializer = new XmlSerializer(typeof(List<Student>));
            using (var streamWriter = new StreamWriter(_filePath))
            {
                serializer.Serialize(streamWriter, students);
                streamWriter.Close();
            }
        }

        public List<Student> DeserializeFromFile()
        {
            if( !File.Exists( _filePath ) )
                return new List<Student>(); 
           
            var serializer = new XmlSerializer(typeof(List<Student>));

            using (var streamReader = new StreamReader(_filePath))
            {
                var students = (List<Student>) serializer.Deserialize(streamReader);
                streamReader.Close();
                return students;
            }
           
        }*/
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
        }

        private void AddEditStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshDiary();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if ( dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznaczyć ucznia, którego chcesz edytować.", "Brak zaznaczonego ucznia", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            var addEditStudent = new AddEditStudent(Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznaczyć ucznia, którego chcesz usunąć.", "Brak zaznaczonego ucznia", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            var selectedStudent = dgvDiary.SelectedRows[0];

            var confirmDelete = MessageBox.Show($"Czy na pewno chcesz usunąć ucznia {selectedStudent.Cells[1].Value} {selectedStudent.Cells[2].Value} ?", "Potwierdź usunięcie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmDelete == DialogResult.Yes)
            {
                DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));
                RefreshDiary();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                isMaximize = true;
            else
                isMaximize = false;
            Settings.Default.Save();
        }
    }
}
