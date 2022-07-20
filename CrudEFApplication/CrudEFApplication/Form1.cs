using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace CrudEFApplication
{

    public partial class Form1 : Form
    {
        public enum TransactionType
        {
            Create,
            Read,
            Update,
            Delete,
        }

        TransactionType _transactionType = TransactionType.Read;


        Customer model = new Customer();
        public Form1()
        {
            InitializeComponent();
        }

        private void Cancel(object sender, EventArgs e)
        {
            Cancel();
        }
        void Cancel()
        {

            txtBoxFirstName.Text = txtBoxLastName.Text = txtBoxAdd.Text = txtBoxAge.Text = "";

            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.CustomerID = 0;
        }
        void New()
        {
            txtBoxFirstName.Text = txtBoxLastName.Text = txtBoxAdd.Text = txtBoxAge.Text = "";

            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.CustomerID = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Cancel();
            this.ActiveControl = txtBoxFirstName;
            PopulateDataGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            model.FirstName = txtBoxFirstName.Text;
            model.LastName = txtBoxLastName.Text;
            model.Address = txtBoxAdd.Text;
            model.Age = Convert.ToInt32(txtBoxAge.Text);

            using (DBEntities db = new DBEntities())

            {
                if (String.IsNullOrWhiteSpace(txtBoxFirstName.Text) || String.IsNullOrWhiteSpace(txtBoxLastName.Text) ||
                    String.IsNullOrWhiteSpace(txtBoxAdd.Text) || String.IsNullOrWhiteSpace(txtBoxAge.Text))
                {
                    MessageBox.Show("Required Missing Field!", "EF CRUD Operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (model.CustomerID == 0/*_transactionType == TransactionType.Create*/)
                        db.Customers.Add(model);
                    else
                        db.Entry(model).State = EntityState.Modified;

                    db.SaveChanges();
                    MessageBox.Show("Submitted Successfully", "EF CRUD Operation");
                }
            }
            Cancel();
            PopulateDataGridView();
        }
        void PopulateDataGridView()
        {
            dgvData.AutoGenerateColumns = false;
            using (DBEntities db = new DBEntities())
            {
                dgvData.DataSource = db.Customers.ToList<Customer>();
            }
        }
        private void dgvData_DoubleClick(object sender, EventArgs e)
        {
            _transactionType = TransactionType.Update;

            if (dgvData.CurrentRow.Index != -1)
            {
                model.CustomerID = Convert.ToInt32(dgvData.CurrentRow.Cells["CustomerID"].Value);
                using (DBEntities db = new DBEntities())
                {
                    model = db.Customers.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
                    txtBoxFirstName.Text = model.FirstName;
                    txtBoxLastName.Text = model.LastName;
                    txtBoxAdd.Text = model.Address;
                    txtBoxAge.Text = model.Age == null ? "" : model.Age.ToString();
                    //if (model.Age == null)
                    //{
                    //    txtBoxAge.Text = "";
                    //}
                    //else
                    //{
                    //    txtBoxAge.Text = model.Age.ToString();
                    //}
                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            _transactionType = TransactionType.Delete;

            if (MessageBox.Show("Are you sure to delete this Data?", "EF CRUD Operation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                using (DBEntities db = new DBEntities())
                {
                    var customer = db.Customers.FirstOrDefault(d => d.CustomerID == model.CustomerID);
                    db.Customers.Remove(customer);
                    db.SaveChanges();

                    /*var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                        db.Customers.Attach(model);
                    db.Customers.Remove(model);

                    db.SaveChanges();*/
                    PopulateDataGridView();
                    Cancel();

                    MessageBox.Show("Deleted Succesfully", "EF CRUD Operation");
                }
        }
        private void txtBoxFirstName_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtBoxFirstName.Text, "^[a-zA-Z]+$"))
            {
            }
            else
            {
                txtBoxFirstName.Text = txtBoxFirstName.Text.Remove(txtBoxFirstName.Text.Length - 1);    
                MessageBox.Show("Enter only Alphabets", "EF CRUD Operation", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void txtBoxAge_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtBoxAge.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers", "EF CRUD Operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBoxAge.Text = txtBoxAge.Text.Remove(txtBoxAge.Text.Length - 1);
            }


        }
        private void txtBoxLastName_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtBoxLastName.Text, "^[a-zA-Z]+$"))
            {
            }
            else
            {
                txtBoxLastName.Text = txtBoxLastName.Text.Remove(txtBoxLastName.Text.Length - 1);
                MessageBox.Show("Enter only Alphabets", "EF CRUD Operation", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
        }
        private void txtBoxAdd_TextChanged(object sender, EventArgs e)
        {
        }
        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void dgvData_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            _transactionType = TransactionType.Update;
            if (dgvData.CurrentRow.Index != -1)
            {
                model.CustomerID = Convert.ToInt32(dgvData.CurrentRow.Cells["CustomerID"].Value);
                using (DBEntities db = new DBEntities())
                {
                    model = db.Customers.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
                    txtBoxFirstName.Text = model.FirstName;
                    txtBoxLastName.Text = model.LastName;
                    txtBoxAdd.Text = model.Address;
                    txtBoxAge.Text = model.Age.ToString();
                    //if (model.Age == null)
                    //{
                    //    txtBoxAge.Text = "";
                    //}
                    //else
                    //{
                    //    txtBoxAge.Text = model.Age.ToString();
                    //}
                }
                btnSave.Text = "Update";
                btnCancel.Text = "Cancel";
                btnDelete.Enabled = true;

            }
        }


        private void btnNew_Click_1(object sender, EventArgs e)
        {

            New();

            {

            }
        }
    }
}