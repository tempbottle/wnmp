﻿/*
Copyright (C) Kurt Cancemi

This file is part of Wnmp.

    Wnmp is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Wnmp is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Wnmp.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml;
using System.Security.Permissions;

using Wnmp.Helpers;
using Wnmp.Internals;

namespace Wnmp.Programs
{
    /// <summary>
    /// Functions/Handlers releated to MariaDB
    /// </summary>
    class MariaDB
    {
        public static Process ps; // Avoid GC
        public static ContextMenuStrip cms = new ContextMenuStrip(); // Config button context menu
        public static ContextMenuStrip lms = new ContextMenuStrip(); // Log button context menu
        public static ToolTip MariaDB_start_Tip = new ToolTip(); // Start button ToolTip
        public static ToolTip MariaDB_stop_Tip = new ToolTip(); // Stop button ToolTip
        public static ToolTip MariaDB_opnshell_Tip = new ToolTip(); // Open Shell button ToolTip

        /// <summary>
        /// Starts an executable file
        /// </summary>
        public static void startprocess(string p, string args, bool shellexc, bool redirectso)
        {
            System.Threading.Thread.Sleep(100); // Wait
            ps = new Process(); // Create process
            ps.StartInfo.FileName = p; // p is the path and file name of the file to run
            ps.StartInfo.Arguments = args; // Parameters to pass to program
            ps.StartInfo.UseShellExecute = shellexc;
            ps.StartInfo.RedirectStandardOutput = redirectso; // Set output of program to be written to process output stream
            ps.StartInfo.WorkingDirectory = Application.StartupPath;
            ps.StartInfo.CreateNoWindow = true; // Excute with no window
            ps.Start(); // Start the process
        }

        internal static void mdb_start_Click(object sender, EventArgs e)
        {
            try
            {
                startprocess(Application.StartupPath + @"\mariadb\bin\mysqld.exe", "", false, true);
                Log.wnmp_log_notice("Attempting to start MariaDB", Log.LogSection.WNMP_MARIADB);
                Declarations.ToStartedLabel(Program.formInstance.mariadbrunning);
            }
            catch (Exception ex)
            {
                Log.wnmp_log_error(ex.Message, Log.LogSection.WNMP_MARIADB);
            }
        }

        internal static void mdb_stop_Click(object sender, EventArgs e)
        {
            try
            {
                // MariaDB
                Log.wnmp_log_notice("Attempting to stop MariaDB", Log.LogSection.WNMP_MARIADB);
                startprocess(Application.StartupPath + @"\mariadb\bin\mysqladmin.exe", "-u root -p shutdown", true, false);
                Declarations.ToStoppedLabel(Program.formInstance.mariadbrunning);
            }
            catch (Exception ex)
            {
                Log.wnmp_log_error(ex.Message, Log.LogSection.WNMP_MARIADB);
            }
        }
        private static bool MariaDBIsRunning()
        {
            Process[] ptcf = Process.GetProcessesByName("mysqld");
            if (ptcf.Length == 0)
                return false;
            else
                return true;
        }
        internal static void mdb_shell_Click(object sender, EventArgs e)
        {
            try
            {
                Log.wnmp_log_notice("Attempting to start MariaDB shell", Log.LogSection.WNMP_MARIADB);
                // MariaDB
                if (!MariaDBIsRunning())
                {
                    startprocess(Application.StartupPath + @"\mariadb\bin\mysqld.exe", "", false, true);
                }
                // MariaDB Shell
                startprocess(Application.StartupPath + @"\mariadb\bin\mysql.exe", "-u root -p", true, false);
            }
            catch (Exception ex)
            {
                Log.wnmp_log_error(ex.Message, Log.LogSection.WNMP_MARIADB);
            }
        }

        internal static void mdb_start_MouseHover(object sender, EventArgs e)
        {
            MariaDB_start_Tip.Show("Start MariaDB", Program.formInstance.mdb_start);
        }

        internal static void mdb_stop_MouseHover(object sender, EventArgs e)
        {
            MariaDB_stop_Tip.Show("Stop MariaDB", Program.formInstance.mdb_stop);
        }

        internal static void mdb_shell_MouseHover(object sender, EventArgs e)
        {
            MariaDB_opnshell_Tip.Show("Open MariaDB Shell", Program.formInstance.mdb_shell);
        }

        internal static void mdb_help_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The default login for MariaDB/phpMyAdmin is:" + "\n" + "Username: root" + "\n" + "Password: password");
        }

        internal static void mdb_cfg_Click(object sender, EventArgs e)
        {
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point(0, btnSender.Height);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            cms.Show(ptLowerLeft);
            cms.ItemClicked -= cms_ItemClicked;
            cms.ItemClicked += cms_ItemClicked;
        }

        static void cms_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Process.Start(Options.settings.editor, Application.StartupPath + "/mariadb/" + e.ClickedItem.Text);
        }

        internal static void mdb_log_Click(object sender, EventArgs e)
        {
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point(0, btnSender.Height);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            lms.Show(ptLowerLeft);
            lms.ItemClicked -= cms_ItemClicked;
            lms.ItemClicked += cms_ItemClicked;
        }

        static void lms_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Process.Start(Options.settings.editor, Application.StartupPath + "/mariadb/data/" + e.ClickedItem.Text);
        }
    }
}
