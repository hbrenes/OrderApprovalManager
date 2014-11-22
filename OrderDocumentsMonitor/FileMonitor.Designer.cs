namespace OrderDocumentsMonitor
{
    partial class FileMonitor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fswOrderInbox = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.fswOrderInbox)).BeginInit();
            // 
            // fswOrderInbox
            // 
            this.fswOrderInbox.EnableRaisingEvents = true;
            this.fswOrderInbox.Filter = "*.xml";
            //this.fswOrderInbox.Path = "C:\\OAM\\Documents\\OrderInbox";
            this.fswOrderInbox.Changed += new System.IO.FileSystemEventHandler(this.fswOrderInbox_Changed);
            this.fswOrderInbox.Created += new System.IO.FileSystemEventHandler(this.fswOrderInbox_Created);
            // 
            // FileMonitor
            // 
            this.ServiceName = "Service1";
            ((System.ComponentModel.ISupportInitialize)(this.fswOrderInbox)).EndInit();

        }

        #endregion

        private System.IO.FileSystemWatcher fswOrderInbox;
    }
}
