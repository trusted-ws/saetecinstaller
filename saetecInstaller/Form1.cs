using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using MySql.Data.MySqlClient;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;
using System.Net.Sockets;

namespace saetecInstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// Shadow
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
(
        int nLeftRect, // x-coordinate of upper-left corner
        int nTopRect, // y-coordinate of upper-left corner
        int nRightRect, // x-coordinate of lower-right corner
        int nBottomRect, // y-coordinate of lower-right corner
        int nWidthEllipse, // height of ellipse
        int nHeightEllipse // width of ellipse
        );

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        private bool m_aeroEnabled;                     // variables for box shadow
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        public struct MARGINS                           // struct for box shadow
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        private const int WM_NCHITTEST = 0x84;          // variables for dragging the form
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();

                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;

                return cp;
            }
        }

        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:                        // box shadow
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 1,
                            rightWidth = 1,
                            topHeight = 1
                        };
                        DwmExtendFrameIntoClientArea(this.Handle, ref margins);

                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)     // drag the form
                m.Result = (IntPtr)HTCAPTION;

        }

        /// Shadow End

        private void Form1_Load(object sender, EventArgs e)
        {
            metroTabPage1.Text = "Instalação";
            metroTabPage2.Text = "Configurações";
            labelDiretorio.Text = general.installPath;
            labelDiretorio2.Text = general.installPath;
            metroTabControl1.SelectedIndex = 0;
            metroComboBox1.SelectedIndex = 0; // 0 = Instalação por cópia (without download). 1 = Download from git
            //metroCheckBox3.Enabled = false; // wamp64
            metroCheckBox2.Enabled = false; // Iniciar servidor Apache
            metroCheckBox4.Checked = true;
            metroCheckBox6.Checked = true;
            metroCheckBox5.Enabled = true;
            metroComboBox1.Enabled = true;
            panel5.Visible = false;
            button2.Enabled = false;
            
           // metroTabControl1.Selecting += new TabControlCancelEventHandler(metroTabControl1.Selecting);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void metroTabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            labelDiretorio.Text = general.installPath;
            labelDiretorio2.Text = general.installPath;
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.Description = "Selecione o diretório para instalar o TCC (Ex.: c:\\wamp\\www)";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                general.installPath = folderBrowserDialog1.SelectedPath;
                labelDiretorio2.Text = general.installPath;
            }
        }

        private void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelDiretorio.Text = general.installPath;
            labelDiretorio2.Text = general.installPath;
        }


        // Functions Sectors
        private void runExec(string path)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = path;
            // Do you want to show a console window?
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;
            int exitCode;

            // Run the external process & wait for it to finish
            try
            {
                using (Process proc = Process.Start(start))
                {
                    proc.WaitForExit();

                    // Retrieve the app's exit code
                    exitCode = proc.ExitCode;
                }
            } catch(System.ComponentModel.Win32Exception)
            {

            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }


            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                try
                {
                    file.CopyTo(temppath, false);
                } catch(Exception)
                {

                }
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {

                foreach (DirectoryInfo subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }



        bool buttonInstallStat = true;
        private void metroButton1_Click(object sender, EventArgs e) 
        {
            DialogResult dialogResult = MessageBox.Show("Deseja prosseguir com a instalação?", "Deseja Continuar?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                metroButton1.Text = "Instalando...";
                metroButton1.Visible = false;
                Application.DoEvents();
                labelOutput.Text = "Iniciando instalação...";
                if (buttonInstallStat)
                {
                    metroButton1.Enabled = false;

                    buttonInstallStat = false;
                }
                labelOutput.Refresh();
                // Get TCC path roots
                if (general.installType == 0)
                {
                    FolderBrowserDialog folderBrowserDialog2 = new FolderBrowserDialog();
                    folderBrowserDialog2.Description = "Selecione o diretório em que o TCC se encontra. (Ex.: D:\\Saetec)";
                    if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
                    {
                        general.tccPath = folderBrowserDialog2.SelectedPath;
                        
                    }
                    else
                    {
                        metroButton1.Enabled = true;
                        metroButton1.Visible = true;
                        labelOutput.Text = "";
                        metroButton1.Text = "INSTALAR";
                        metroProgressBar1.Value = 0;
                        return;
                    }
                }
                metroProgressBar1.Value += 5;
                //metroButton1.Visible = false;
                System.Threading.Thread.Sleep(2000);

                metroProgressBar1.Value += 5;
                labelOutput.Refresh();

                if (general.startWamp)
                {
                    labelOutput.Text = "Iniciando o Wamp Server...";
                    System.Threading.Thread.Sleep(3050);

                    if (general.wamp64)
                    {
                        Thread t = new Thread(() => runExec(general.wamp64Executable));
                        t.Start();
                    }
                    else
                    {
                        Thread t = new Thread(() => runExec(general.wampExecutable));
                        t.Start();
                    }
                }
                metroProgressBar1.Value += 30;
                labelOutput.Refresh();

                // Execute Saetec's SQL Query
                if (general.sqlMount && general.startWamp)
                {
                    /* Syntax
                     * Server=myServerAddress; Port=1234; Database=myDataBase; Uid=myUsername; Pwd=myPassword;
                     */
                    var connString = "Server=" + general.dbHost + ";Port=" + general.dbPort + ";Uid=" + general.dbUsername + ";Pwd=" + general.dbPassword;
                    var connection = new MySqlConnection(connString);
                    var command = connection.CreateCommand();
                    MySqlScript script = new MySqlScript(connection, File.ReadAllText("database.sql"));
                    //System.Threading.Thread.Sleep(6000);
                    /* Check if MySQL door is opened */
                    int dbPort;
                    try
                    {
                        dbPort = Convert.ToInt32(general.dbPort);
                    } catch (Exception)
                    {
                        MessageBox.Show("A porta de conexão inserida está em formato incorreto.", "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dbPort = 3306;
                        labelOutput.Text = "Erro de conexão. Porta inválida - Continuando com 3306...";
                        System.Threading.Thread.Sleep(3000);
                    }
                checkIfPortIsOpened:
                    System.Threading.Thread.Sleep(2000);
                    labelOutput.Text = "Verificando serviço MySQL...";
                    using (TcpClient tcpClient = new TcpClient())
                    {
                        try
                        {
                            tcpClient.Connect(general.dbHost, dbPort);
                            //labelOutput.Text = "Verificando serviço MySQL...";
                            metroProgressBar1.Value += 10;
                            labelOutput.Refresh();
                            System.Threading.Thread.Sleep(420);
                        }
                        catch (Exception)
                        {
                            // Port is closed
                            labelOutput.Text = "Serviço MySQL não ativo. Tentando novamente...";
                            goto checkIfPortIsOpened;
                        }
                    }

                    try
                    {
                        connection.Open();
                        script.Execute();
                        labelOutput.Text = "Montando script SQL...";
                        System.Threading.Thread.Sleep(3000);
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                    }
                }
                metroProgressBar1.Value += 10;
                labelOutput.Refresh();

                // Execute Moving to DocumentRoot
                labelOutput.Text = "Movendo arquivos necessários...";
                System.Threading.Thread.Sleep(750);
                if (general.formatRootDirectory)
                {
                    labelOutput.Text = "Formatando diretório de instalação...";
                    System.Threading.Thread.Sleep(750);
                    // Formatting root directory
                    System.IO.DirectoryInfo di = new DirectoryInfo(general.installPath);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                }
                metroProgressBar1.Value += 5;
                labelOutput.Refresh();

                // Subdirectory
                if (general.installSubDirectory)
                {
                    general.installPath += @"\" + general.subdirectoryName;
                }

                // Download GitHub project
                metroProgressBar1.Value += 10;
                labelOutput.Refresh();
                if (general.installType == 1)
                {
                    labelOutput.Text = "Baixando arquivos necessários...";
                    labelOutput.Refresh();
                    //metroProgressBar1.Value = 0;
                    string fileName = general.installPath + @"\" + general.gitAddress.Split('/').Last();
                    general.finalPath = fileName;

                    System.Threading.Thread.Sleep(3000);
                    var githubToken = general.gitToken;
                    var url = general.gitAddress;
                    var path = general.installPath;


                    using (var client = new WebClient())
                    {
                        System.IO.Directory.CreateDirectory(path);
                        client.DownloadFile(url, fileName);
                    }

                    labelOutput.Text = "Extraindo arquivos necessários...";
                    System.Threading.Thread.Sleep(3000);
                    int exceptionCounter1 = 0;
                    try
                    {
                        ZipFile.ExtractToDirectory(fileName, general.installPath);
                    } catch(Exception)
                    {
                        labelOutput.Text += " [EGx0" + exceptionCounter1 + "] Arquivo pré existente";
                        labelOutput.Refresh();
                    }
                    string extractedDir = general.installPath + @"\" + general.gitRepName + "-master";

                    DirectoryCopy(extractedDir, general.installPath, true);
                    System.IO.DirectoryInfo dir = new DirectoryInfo(extractedDir);
                    foreach (FileInfo file in dir.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo di in dir.GetDirectories())
                    {
                        di.Delete(true);
                    }
                }

                metroProgressBar1.Value += 15;
                //Move files to target directory
                if (general.installType == 0)
                {
                    labelOutput.Text = "Copiando arquivos necessários...";
                    System.Threading.Thread.Sleep(3000);
                    DirectoryCopy(general.tccPath, general.installPath, true);
                }

                metroProgressBar1.Value += 10;
                labelOutput.Refresh();
                System.Threading.Thread.Sleep(2000);
                panel5.Visible = true;
                metroButton1.Text = "Instalação Concluída";
                labelOutput.Text = "Instalação Concluída";
                metroProgressBar1.Value += 10;
                labelOutput.Refresh();
                DialogResult dialogResult2 = MessageBox.Show("A instalação do TCC foi concluída com sucesso.\nDeseja abri-lo?", "Instalação Concluída", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult2 == DialogResult.Yes)
                {
                    // Open SAETEC in browser.
                    System.Diagnostics.Process.Start("http://127.0.0.1");
                }
            }
            /// FIM
        }


        private void metroButton3_Click(object sender, EventArgs e)
        {
            // Open MySQL Options Form
            databaseOptions dboptions = new databaseOptions();
            dboptions.ShowDialog();
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(metroComboBox1.SelectedIndex == 0)
            {
                general.installType = 0; // Move files
                button2.Enabled = false;
                //MessageBox.Show("Para utilizar esta opção é necessário que você execute este programa dentro do diretório raíz do projeto", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if(metroComboBox1.SelectedIndex == 1)
            {
                general.installType = 1; // Download git
                button2.Enabled = true;
            } else
            {
                MessageBox.Show("Error");
            }
        }

        private void metroCheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            if(metroCheckBox6.Checked == true)
            {
                general.sqlMount = true;
            } else
            {
                general.sqlMount = false;
            }
        }

        private void metroCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(metroCheckBox1.Checked == true)
            {
                general.formatRootDirectory = true;
            } else
            {
                general.formatRootDirectory = false;
            }
        }

        private void metroCheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            if(metroCheckBox5.Checked == true)
            {
                general.installSubDirectory = true;
                subdirectoryNameSet subname = new subdirectoryNameSet();
                general.subdirectoryClose = false;
                subname.ShowDialog();
                if(general.subdirectoryClose)
                    metroCheckBox5.Checked = false;
            } else
            {
                general.installSubDirectory = false;
            }
        }

        private void metroCheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            if(metroCheckBox4.Checked == true)
            {
                general.startWamp = true;
                metroCheckBox3.Enabled = true;
                metroCheckBox3.BackColor = Color.FromArgb(55, 63, 81);
                metroCheckBox6.Enabled = true;
                metroCheckBox6.BackColor = Color.FromArgb(55, 63, 81);

            } else
            {
                general.startWamp = false;
                metroCheckBox3.Enabled = false;
                metroCheckBox3.BackColor = Color.FromArgb(94, 108, 140);
                metroCheckBox6.Enabled = false;
                metroCheckBox6.BackColor = Color.FromArgb(94, 108, 140);
            }
        }

        private void metroCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if(metroCheckBox4.Checked)
            {
                if(metroCheckBox3.Checked)
                {
                    general.wamp64 = true;
                    labelDiretorio.Text = @"C:\wamp64\www";
                    labelDiretorio2.Text = @"C:\wamp64\www";
                    general.installPath = @"C:\wamp64\www";
                } else
                {
                    general.wamp64 = false;
                    labelDiretorio.Text = @"C:\wamp\www";
                    labelDiretorio2.Text = @"C:\wamp\www";
                    general.installPath = @"C:\wamp\www";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            externalDownloader downloadopt = new externalDownloader();
            downloadopt.ShowDialog();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
