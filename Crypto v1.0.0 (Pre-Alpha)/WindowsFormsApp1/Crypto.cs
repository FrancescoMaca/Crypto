using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Security.Cryptography;

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006: Naming Styles")]
namespace WindowsFormsApp1
{
    public partial class Crypto : Form
    {
        //main encryption object
        readonly RijndaelManaged AES = new RijndaelManaged();

        //mode type for ProgressBar value
        enum Mode { Set, Add, Subtract, SetMaximum };

        /// <summary>
        /// Default constructor
        /// </summary>
        public Crypto()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the as soon as the application opens.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Crypto_Load(object sender, EventArgs e)
        {
            //setting max password length
            tbPassword.MaxLength = 32;

            //setting shown character
            tbPassword.UseSystemPasswordChar = true;

            //disables the "Start" button
            btStart.Enabled = false;
        }

        /// <summary>
        /// Opens a window to select the file you want to encrypt or decrypt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btBrowseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Multiselect = true
            };


            if (dialog.ShowDialog() == DialogResult.OK)
            {
                tbFilePath.Text = dialog.FileName;
            }

            dialog.Dispose();
        }

        /// <summary>
        /// Start encrypting or decrypting the file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btStart_Click(object sender, EventArgs e)
        {
            //it shows the tooltip when the password's field is empty
            if (string.IsNullOrEmpty(tbPassword.Text))
            {
                ToolTipError.Show("The password cannot be empty!", tbPassword, 0, 30, 1500);
                return;
            }

            //if the user wants to encode a folder and a file at the same time
            if (!string.IsNullOrEmpty(tbFilePath.Text) && !string.IsNullOrEmpty(tbFolderPath.Text))
            {
                ToolTipError.Show("You cannot encode a folder and a file simultaneously!", tbFolderPath, 175, 30, 1500);
                return;
            }

            //Sets progressBar to 0
            UpdateProgressBarValue(pbLoading, 0, Mode.Set);

            UpdateLabel(lbStatus, "Status: N/D");

            //if the data is a file
            if (!string.IsNullOrEmpty(tbFilePath.Text))
            {
                //encoding
                if (rbEncode.Checked)
                {
                    //calls the method on a new thread so the UI thread doesn't freeze
                    new Task(() =>
                    {
                        int result_code = EncodeFile(tbFilePath.Text, tbPassword.Text);

                        //succesfuly executed
                        if (result_code == 0)
                            MessageBox.Show("Encoding has been completed successfully!", "Task Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                        {
                            //tries to delete the not completed encrypted file if an error occurs
                            try
                            {
                                File.Delete(tbFilePath.Text + ".aes");
                            }
                            catch (Exception) { }

                            UpdateProgressBarValue(pbLoading, 0, Mode.Set);
                            UpdateLabel(lbStatus, "Status: Error");
                        }
                    }).Start();
                }
                //decoding
                else
                {
                    new Task(() =>
                    {
                        int result_code = DecodeFile(tbFilePath.Text, tbPassword.Text);

                        //succesfuly executed
                        if (result_code == 0)
                            MessageBox.Show("Decoding has been completed successfully!", "Task Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                        {
                            //tries to delete the not completed encrypted file if an error occurs
                            try
                            {
                                File.Delete(tbFilePath.Text.Substring(0, tbFilePath.Text.Length - 4));
                            }
                            catch (Exception) { }

                            UpdateProgressBarValue(pbLoading, 0, Mode.Set);
                            UpdateLabel(lbStatus, "Status: Error");
                        }
                    }).Start();
                }
            }
            //if the data is a folder
            else if (!string.IsNullOrEmpty(tbFolderPath.Text))
            {
                //contains all file's path in a given folder
                string[] filePaths = Directory.GetFiles(tbFolderPath.Text);

                if (rbEncode.Checked)
                {
                    //calls the method on a new thread so the UI thread doesn't freeze
                    new Task(() =>
                    {
                        int errorNumber = 0;
                        foreach(string filePath in filePaths)
                        {
                            //setting up loading bar maximum value
                            UpdateProgressBarValue(pbLoading, (int)new FileInfo(filePath).Length / 1048576, Mode.SetMaximum);
                            if (pbLoading.Maximum % 1048576 != 0)
                                UpdateProgressBarValue(pbLoading, pbLoading.Maximum + 1, Mode.SetMaximum);
                            
                            int result_code = EncodeFile(filePath, tbPassword.Text);

                            //error handler
                            if (result_code == 0)
                            {
                                UpdateProgressBarValue(pbLoading, 0, Mode.Set);
                            }
                            else
                            {
                                try
                                {
                                    File.Delete(filePath);
                                }
                                catch (Exception) { }
                                MessageBox.Show($"An error occurred with the file \"{filePath}\".");
                                UpdateProgressBarValue(pbLoading, 0, Mode.Set);
                                UpdateLabel(lbStatus, "Status: Error");
                                errorNumber++;
                            }
                        }

                        if (errorNumber != 0)
                        {
                            MessageBox.Show($"{errorNumber} errors have occurred in the encoding! Try to encode\nthem again one by one.");
                        }
                        else
                        {
                            MessageBox.Show("No problem occurred! All files have been encrypted successfully!");
                        }
                        
                    }).Start();
                }
                //decoding
                else
                {
                    new Task(() =>
                    {
                        int errorNumber = 0;
                        foreach (string filePath in filePaths)
                        {
                            //setting up loading bar maximum value
                            UpdateProgressBarValue(pbLoading, (int)new FileInfo(filePath).Length / 1048576, Mode.SetMaximum);
                            if (pbLoading.Maximum % 1048576 != 0)
                                UpdateProgressBarValue(pbLoading, pbLoading.Maximum + 1, Mode.SetMaximum);

                            int result_code = DecodeFile(filePath, tbPassword.Text);

                            //error handler
                            if (result_code == 0)
                            {
                                UpdateProgressBarValue(pbLoading, 0, Mode.Set);
                            }
                            else
                            {
                                try
                                {
                                    File.Delete(filePath);
                                }
                                catch (Exception) { }
                                MessageBox.Show($"An error occurred with the file \"{filePath}\".");
                                UpdateProgressBarValue(pbLoading, 0, Mode.Set);
                                UpdateLabel(lbStatus, "Status: Error");
                                errorNumber++;
                            }
                        }

                        if (errorNumber != 0)
                        {
                            MessageBox.Show($"{errorNumber} errors have occurred in the decoding! Try to decode\nthem again one by one.");
                        }
                        else
                        {
                            MessageBox.Show("No problem occurred! All files have been encrypted successfully!");
                        }
                    }).Start();
                }
            }
        }

        /* I'm very sure that there is another way other than using if() else if() else 
         * in tbFilePath_TextChanged() and tbFolderPath_TextChanged()  */
        /// <summary>
        /// Checks if the selected file exists and outputs a error if it doesn't
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbFilePath_TextChanged(object sender, EventArgs e)
        {
            //checks if the file exist
            if (!File.Exists(tbFilePath.Text))
            {
                //makes the program unable to start
                btStart.Enabled = false;

                //sets the text messages
                lbFileSize.Text = "File Size: Unknown";
                return;
            }

            //if none of the errors above are triggered then it enables the start button
            btStart.Enabled = true;

            //sets the file size label
            long size = new FileInfo(tbFilePath.Text).Length;
            string[] units = new string[5] { "B", "Kb", "Mb", "Gb", "Tb" };
            int place = Convert.ToInt32(Math.Floor(Math.Log(size, 1024)));
            double num = Math.Round(size / Math.Pow(1024, place), 1);

            lbFileSize.Text = "File Size: " + num + units[place];

            //setting progress bar maxmimum value (so we don't get stuck on 99% processBar value)
            pbLoading.Maximum = (int)new FileInfo(tbFilePath.Text).Length / 1048576;
            if (pbLoading.Maximum % 1048576 != 0)
                pbLoading.Maximum++;
        }

        /// <summary>
        /// Checks if the selected folder exists and outputs a error if it doesn't
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbFolderPath_TextChanged(object sender, EventArgs e)
        {
            if (!Directory.Exists(tbFolderPath.Text))
            {
                //makes the program unable to start
                btStart.Enabled = false;
            }
            else
            {
                //if none of the errors above activates then its good
                btStart.Enabled = true;

                string[] files = Directory.GetFiles(tbFolderPath.Text);
                long size = 0;
                foreach (string file in files)
                {
                    size += new FileInfo(file).Length;
                }
                //sets the file size label
                string[] units = new string[5] { "B", "Kb", "Mb", "Gb", "Tb" };
                int place = Convert.ToInt32(Math.Floor(Math.Log(size, 1024)));
                double num = Math.Round(size / Math.Pow(1024, place), 1);

                lbFileSize.Text = "File Size: " + num + units[place];

                if (pbLoading.Maximum % 1048576 != 0)
                    pbLoading.Maximum++;
            }
        }

        /// <summary>
        /// Copies the dragged folder's path into the textBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbFolderPath_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (Directory.Exists(files[0]))
                {
                    tbFolderPath.Text = files[0];
                }
                else
                {
                    tbFolderPath.Text = Path.GetDirectoryName(files[0]);
                }
            }
        }

        /// <summary>
        /// Shows the Drag effets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbFolderPath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Copies the dragged file's path into the textBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbFilePath_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                tbFilePath.Text = files[0];
            }
        }
        
        /// <summary>
        /// Shows the dragEnter copy effect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbFilePath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Updates the ProgressBar value with the encryption/decryption status.
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="amountEachLoop"></param
        /// <param name="mode"></param>
        private void UpdateProgressBarValue(ProgressBar bar, int amount, Mode mode)
        {
            //if the progressBar is declared on a different thread, this will
            //call the function in the UI thread
            if (bar.InvokeRequired)
            {
                bar.Invoke((Action)(() => UpdateProgressBarValue(bar, amount, mode)));
                return;
            }

            //depending on the mode it sets, subtracts or adds the given amount
            if (mode == Mode.Add)
                bar.Value += amount;
            else if (mode == Mode.Subtract)
                bar.Value -= amount;
            else if (mode == Mode.Set)
                bar.Value = amount;
            else if (mode == Mode.SetMaximum)
                bar.Maximum = amount;
        }

        /// <summary>
        /// Updates a label with the text given.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="text"></param>
        private void UpdateLabel(Label label, string text)
        {
            //if the Label is declared on a different thread, this will
            //call the function in the UI thread
            if (label.InvokeRequired)
            {
                label.Invoke((Action)(() => UpdateLabel(label, text)));
                return;
            }

            label.Text = text;
        }

        /// <summary>
        /// Sets the mode to decode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbDecode_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDecode.Checked)
            {
                rbEncode.Checked = false;
            }
        }

        /// <summary>
        /// Sets the mode to encode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbEncode_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEncode.Checked)
            {
                rbDecode.Checked = false;
            }
        }

        /// <summary>
        /// Shows the password in plain text to the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowPassword.Checked)
            {
                tbPassword.UseSystemPasswordChar = false;
            }
            else
            {
                tbPassword.UseSystemPasswordChar = true;
            }
        }

        /// <summary>
        /// Encodes the file selected in tbFilePath with an AES encryption
        /// with a password chosen by the user.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="password"></param>
        /// <returns>0 = no errors, 1 = not found, 3 = other exception</returns>
        private int EncodeFile(string FilePath, string password)
        {
            //check if the file exists
            if (!File.Exists(FilePath))
            {
                MessageBox.Show("Selected file doesn't exist!", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }

            if (new FileInfo(FilePath).IsReadOnly)
            {
                DialogResult result = MessageBox.Show("The file is read-only, do you wanna change that?\nIf you select \"No\" the task will terminate.", "File attribute error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    new FileInfo(FilePath).IsReadOnly = false;
                }
                else
                {
                    return 1;
                }
            }

            byte[] salt = new byte[32];

            //creating salt and using it to create IV and Key
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    // Fille the buffer with the generated data
                    rng.GetBytes(salt);
                }

                //mixes the password with the salt
                var rfc2898DeriveBytes = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), salt, 100000);
                AES.Key = rfc2898DeriveBytes.GetBytes(AES.KeySize / 8);
                AES.IV = rfc2898DeriveBytes.GetBytes(AES.BlockSize / 8);
            }

            //create output file name
            using (FileStream CryptedFile = new FileStream(FilePath + ".aes", FileMode.Create))
            {
                //write salt to the begining of the output file, so in this case can be random every time
                CryptedFile.Write(salt, 0, salt.Length);

                using (CryptoStream cryptoStream = new CryptoStream(CryptedFile, AES.CreateEncryptor(), CryptoStreamMode.Write))
                using (FileStream InputFile = new FileStream(FilePath, FileMode.Open))
                {
                    //create 1Mb buffer so only this amount will allocate in the memory and not the whole file
                    byte[] buffer = new byte[1048576];

                    //crypting file
                    try
                    {
                        int read;

                        while ((read = InputFile.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            //writes encoded file
                            cryptoStream.Write(buffer, 0, read);

                            //call method to update texts in the UI
                            UpdateProgressBarValue(pbLoading, 1, Mode.Add);

                            //updating status text
                            UpdateLabel(lbStatus, "Status: " + pbLoading.Value + "Mb of " + pbLoading.Maximum + "Mb encrypted");

                            //updating status in percentage
                            UpdateLabel(lbProgress, "Progress: " + (int)(((float)pbLoading.Value / pbLoading.Maximum) * 100.0f) + "%");
                        }

                        //sets the status to "Completed" when the application finishes
                        UpdateLabel(lbStatus, "Status: Completed");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error: " + e.Message, "Unhandled Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return 1;
                    }
                }
            }

            try
            {
                //deletes the non-crypted file
                File.Delete(FilePath);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show($"Cannot delete the file \"{FilePath}\" since it cannot be found!", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 0;
        }

        /// <summary>
        /// Decodes the file selected in tbFilePath, after three times you decrypt
        /// the file with the wrong password, the files encrypts itself with a random
        /// password and a random salt.
        /// </summary>
        /// <param name="FilePath">The path of the file to decode.</param>
        /// <param name="password">The password you want to encode the file with.</param>
        /// <returns>0 = no errors, 1 = not found, 2 = cryptographic error, 3 = other exception</returns>
        private int DecodeFile(string FilePath, string password)
        {
            //check if the file exists
            if (!File.Exists(FilePath))
            {
                MessageBox.Show("Selected file doesn't exist!", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }

            if (new FileInfo(FilePath).IsReadOnly)
            {
                DialogResult result = MessageBox.Show("The file is read-only, do you wanna change that?\nIf you select \"No\" the task will terminate.", "File attribute error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    new FileInfo(FilePath).IsReadOnly = false;
                }
                else
                {
                    return 1;
                }
            }

            using (FileStream CryptedFile = new FileStream(FilePath, FileMode.Open))
            {
                byte[] salt = new byte[32];
                CryptedFile.Read(salt, 0, salt.Length);

                using (Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), salt, 100000))
                {
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;
                }

                using (CryptoStream cryptoStream = new CryptoStream(CryptedFile, AES.CreateDecryptor(), CryptoStreamMode.Read))
                using (FileStream OutputFile = new FileStream(FilePath.Substring(0, FilePath.Length - 4), FileMode.OpenOrCreate))
                {
                    //creating 1Mb buffer to read/write the decoded file
                    byte[] buffer = new byte[1048576];

                    //decrypting file
                    try
                    {
                        int read;

                        while ((read = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            //writes decoded file
                            OutputFile.Write(buffer, 0, read);

                            //call method to update texts in the UI
                            UpdateProgressBarValue(pbLoading, 1, Mode.Add);

                            //updating status text
                            UpdateLabel(lbStatus, "Status: " + pbLoading.Value + "Mb of " + pbLoading.Maximum + "Mb decrypted");

                            //updating status in percentage
                            UpdateLabel(lbProgress, "Progress: " + (int)(((float)pbLoading.Value / pbLoading.Maximum) * 100.0f) + "%");
                        }

                        //sets the status to "Completed" when the application finishes
                        UpdateLabel(lbStatus, "Status: Completed");
                    }
                    catch (CryptographicException)
                    {
                        MessageBox.Show("The password inserted is wrong", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //deletes the empty file if the password is wrong
                        File.Delete(FilePath.Substring(0, FilePath.Length - 4));
                        return 1;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Exception not handled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return 1;
                    }
                }
            }

            try
            {
                File.Delete(FilePath);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show($"The file \"{FilePath}\" coudln't be found.", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return 0;
        }

        /// <summary>
        /// Opens the dialog for the user to choose a folder to encrypt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btBrowseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog()
            {
                RootFolder = Environment.SpecialFolder.Desktop
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                tbFolderPath.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// Opens a help form to show the user how to program works.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Crypto_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            if (!isFormOpen("Crypto_Help"))
            {
                new Crypto_Help().Show();
            }
            else
            {
                GetForm("Crypto_Help").BringToFront();
            }
        }

        /// <summary>
        /// Returns if a form is opened.
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        private bool isFormOpen(string formName)
        {
            foreach(Form form in Application.OpenForms)
            {
                if (form.Name == formName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a form from the opened forms, if it doesn't exist it returns null
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        private Form GetForm(string formName)
        {
            foreach (Form form in Application.OpenForms)
                if (form.Name == formName)
                    return form;

            return null;
        }
    }
}
