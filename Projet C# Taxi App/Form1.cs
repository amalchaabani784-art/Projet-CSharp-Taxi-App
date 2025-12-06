using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Projet_C__Taxi_App.Models;
using Projet_C__Taxi_App.DAL;

namespace Projet_C__Taxi_App
{
    public partial class Form1 : Form
    {
        private Label lblTitle;
        private DataGridView dgvCourses;
        private ComboBox cbChauffeur, cbClient, cbStatut;
        private TextBox txtDistance, txtDuree;
        private Button btnAddCourse, btnUpdateStatus;

        private List<Chauffeur> chauffeurs;
        private List<Client> clients;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomControls();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize DB and seed data
            DatabaseSetup.InitializeDatabase();
            DatabaseSeeder.SeedTestData();

            // Load Chauffeurs and Clients
            chauffeurs = ChauffeurRepository.GetAllChauffeurs() ?? new List<Chauffeur>();
            clients = ClientRepository.GetAllClients() ?? new List<Client>();

            cbChauffeur.DataSource = chauffeurs;
            cbChauffeur.DisplayMember = "Nom";
            cbChauffeur.ValueMember = "Id";

            cbClient.DataSource = clients;
            cbClient.DisplayMember = "Nom";
            cbClient.ValueMember = "Id";

            cbStatut.DataSource = Enum.GetValues(typeof(StatutCourse));

            LoadCourses();
        }

        private void InitializeCustomControls()
        {
            this.Text = "Application Taxi - Gestion des Courses";
            this.Size = new Size(900, 500);

            // --- Title Label ---
            lblTitle = new Label
            {
                Text = "Table des Courses",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(20, 10),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // --- DataGridView ---
            dgvCourses = new DataGridView
            {
                Location = new Point(20, 50),
                Size = new Size(840, 200),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dgvCourses);

            int yLabel = 270; // labels above input boxes
            int yInput = 290;

            // --- Chauffeur ---
            Label lblChauffeur = new Label { Text = "Chauffeur:", Location = new Point(20, yLabel), AutoSize = true };
            this.Controls.Add(lblChauffeur);
            cbChauffeur = new ComboBox { Location = new Point(20, yInput), Size = new Size(150, 25) };
            this.Controls.Add(cbChauffeur);

            // --- Client ---
            Label lblClient = new Label { Text = "Client:", Location = new Point(200, yLabel), AutoSize = true };
            this.Controls.Add(lblClient);
            cbClient = new ComboBox { Location = new Point(200, yInput), Size = new Size(150, 25) };
            this.Controls.Add(cbClient);

            // --- Distance ---
            Label lblDistance = new Label { Text = "Distance (km):", Location = new Point(380, yLabel), AutoSize = true };
            this.Controls.Add(lblDistance);
            txtDistance = new TextBox { Location = new Point(380, yInput), Size = new Size(80, 25) };
            this.Controls.Add(txtDistance);

            // --- Duree ---
            Label lblDuree = new Label { Text = "Durée (min):", Location = new Point(480, yLabel), AutoSize = true };
            this.Controls.Add(lblDuree);
            txtDuree = new TextBox { Location = new Point(480, yInput), Size = new Size(80, 25) };
            this.Controls.Add(txtDuree);

            // --- Add Course Button ---
            btnAddCourse = new Button { Location = new Point(580, yInput), Size = new Size(150, 30), Text = "Ajouter Course" };
            btnAddCourse.Click += BtnAddCourse_Click;
            this.Controls.Add(btnAddCourse);

            // --- Statut ---
            Label lblStatut = new Label { Text = "Nouveau statut:", Location = new Point(20, yInput + 40), AutoSize = true };
            this.Controls.Add(lblStatut);
            cbStatut = new ComboBox { Location = new Point(140, yInput + 37), Size = new Size(150, 25) };
            cbStatut.DataSource = Enum.GetValues(typeof(StatutCourse));
            this.Controls.Add(cbStatut);

            // --- Update Status Button ---
            btnUpdateStatus = new Button { Location = new Point(320, yInput + 37), Size = new Size(150, 30), Text = "Mettre à jour statut" };
            btnUpdateStatus.Click += BtnUpdateStatus_Click;
            this.Controls.Add(btnUpdateStatus);

            this.Load += Form1_Load;
        }

        private void LoadCourses()
        {
            if (chauffeurs == null || clients == null)
            {
                MessageBox.Show("Erreur: Chauffeurs ou Clients non chargés !");
                return;
            }

            List<Course> courses = CourseRepository.GetAllCourses(chauffeurs, clients);

            var table = new DataTable();
            table.Columns.Add("ID");
            table.Columns.Add("Date");
            table.Columns.Add("Chauffeur");
            table.Columns.Add("Client");
            table.Columns.Add("Distance (km)");
            table.Columns.Add("Durée (min)");
            table.Columns.Add("Prix (€)");
            table.Columns.Add("Statut");

            foreach (var c in courses)
            {
                string chauffeurName = c.Chauffeur != null ? c.Chauffeur.Nom : "(Supprimé)";
                string clientName = c.Client != null ? c.Client.Nom : "(Supprimé)";

                table.Rows.Add(
                    c.Id,
                    c.DateCourse.ToString("dd/MM/yyyy HH:mm"),
                    chauffeurName,
                    clientName,
                    c.DistanceKm,
                    c.DureeMinutes,
                    c.Prix,
                    c.StatutToString()
                );
            }

            dgvCourses.DataSource = table;

            // Allow horizontal scrolling for all columns
            dgvCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvCourses.Columns[0].Width = 50;
            dgvCourses.Columns[1].Width = 120;
            dgvCourses.Columns[2].Width = 150;
            dgvCourses.Columns[3].Width = 150;
            dgvCourses.Columns[4].Width = 80;
            dgvCourses.Columns[5].Width = 80;
            dgvCourses.Columns[6].Width = 80;
            dgvCourses.Columns[7].Width = 100;

            dgvCourses.HorizontalScrollingOffset = 0;

            // Color rows by status
            foreach (DataGridViewRow row in dgvCourses.Rows)
            {
                string statut = row.Cells["Statut"].Value.ToString();
                switch (statut)
                {
                    case "Réservée": row.DefaultCellStyle.BackColor = Color.LightYellow; break;
                    case "En cours": row.DefaultCellStyle.BackColor = Color.LightBlue; break;
                    case "Terminée": row.DefaultCellStyle.BackColor = Color.LightGreen; break;
                }
            }
        }

        private void BtnAddCourse_Click(object sender, EventArgs e)
        {
            try
            {
                Chauffeur chauffeur = (Chauffeur)cbChauffeur.SelectedItem;
                Client client = (Client)cbClient.SelectedItem;
                double distance = double.Parse(txtDistance.Text);
                double duree = double.Parse(txtDuree.Text);

                Course newCourse = new Course(0, DateTime.Now, distance, duree, chauffeur, client, StatutCourse.Reservee);
                CourseRepository.InsertCourse(newCourse);

                MessageBox.Show("Course ajoutée !");
                LoadCourses();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout: " + ex.Message);
            }
        }

        private void BtnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (dgvCourses.SelectedRows.Count == 0) return;

            int courseId = Convert.ToInt32(dgvCourses.SelectedRows[0].Cells["ID"].Value);
            StatutCourse newStatut = (StatutCourse)cbStatut.SelectedItem;

            CourseRepository.UpdateCourseStatus(courseId, newStatut);
            MessageBox.Show("Statut mis à jour !");
            LoadCourses();
        }
    }
}
