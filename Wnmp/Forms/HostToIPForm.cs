﻿/*
Copyright (C) Kurt Cancemi 2014

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
using System.Net;
using System.Text;
using System.Windows.Forms;
using Wnmp.Internals;

namespace Wnmp.Forms
{
    public partial class HostToIPForm : Form
    {
        public HostToIPForm()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.Style = myCp.Style & ~Declarations.WS_THICKFRAME; // Remove WS_THICKFRAME (Disables resizing)
                return myCp;
            }
        }

        private void HostToIP(string host, out IPAddress[] ip)
        {
            ip = Dns.GetHostAddresses(host);
        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Go_Click(object sender, EventArgs e)
        {
            IPAddresses.Items.Clear();
            if (host.Text != String.Empty)
            {
                try
                {
                    if (host.Text.Contains("http://") || host.Text.Contains("https://"))
                    { MessageBox.Show("Invalid Format"); }
                    else
                    {
                        IPAddress[] ips;
                        HostToIP(host.Text, out ips);

                        foreach (IPAddress ip in ips)
                        {
                            IPAddresses.Items.Add(ip.ToString());
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

    }
}
