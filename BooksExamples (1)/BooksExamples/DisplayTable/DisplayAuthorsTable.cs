using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisplayTable
{
    public partial class DisplayAuthorsTable : Form
    {
        public DisplayAuthorsTable()
        {
            InitializeComponent();
        }

        //Entity Framework DbContext
        private BooksExamples.BooksEntities dbcontext = new BooksExamples.BooksEntities();
        //load data from database into DataGridView
        private void DisplayAuthorsTable_Load(object sender, EventArgs e)
        {
            //load Authors table ordered by LastName then FirstName
            dbcontext.Authors
                .OrderBy(author => author.LastName)
                .ThenBy(author => author.FirstName)
                .Load();
            //specify datasource for authorBindingSource
            authorBindingSource.DataSource = dbcontext.Authors.Local;
        }
        private void authorBindingNavigator_RefreshItems(object sender, EventArgs e)
        {
            //load Authors table ordered by LastName then FirstName
            dbcontext.Authors
                .OrderBy(author => author.LastName)
                .ThenBy(author => author.FirstName)
                .Load();
            //specify datasource for authorBindingSource
            authorBindingSource.DataSource = dbcontext.Authors.Local;
        }

        
        private void authorBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Validate();
            authorBindingSource.EndEdit();
            try
            {
                dbcontext.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException)
            {
                MessageBox.Show("FirstName and LastName must contain values", "Entity Validation Exception");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                string lname = textBox1.Text;
                //load Authors table ordered by LastName then FirstName
                var lastNameQuery =
                    from author in dbcontext.Authors
                    where author.LastName.Contains(lname)
                    orderby author.LastName, author.FirstName
                    select author;
                authorBindingSource.DataSource = lastNameQuery.ToList();
                authorDataGridView.DataSource = lastNameQuery.ToList();
                authorBindingSource.MoveFirst();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            //load Authors table ordered by LastName then FirstName
            dbcontext.Authors
                .OrderBy(author => author.LastName)
                .ThenBy(author => author.FirstName)
                .Load();
            //specify datasource for authorBindingSource
            authorBindingSource.DataSource = dbcontext.Authors.Local;
            authorDataGridView.DataSource = dbcontext.Authors.Local;
        }
    }
}
