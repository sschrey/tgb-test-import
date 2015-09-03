using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tweddle.Commons
{
    public class CommandObj
    {
        private CommandOutput m_sResult;
        Process m_oProc;

        public CommandObj()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public class CommandOutput
        {
            public CommandOutput()
            {
                Output = new List<string>();
                Errors = new List<string>();
            }
            public List<string> Output { get; set; }
            public List<string> Errors { get; set; }
        }

        public CommandOutput Run(string sFilePath, string sArgs, string sInput, string sWorkingDir, int nWaitTime)
        {
            return Run(sFilePath, sArgs, sInput, sWorkingDir, nWaitTime, ProcessPriorityClass.Normal);
        }

        public CommandOutput Run(string sFilePath, string sArgs, string sInput, string sWorkingDir, int nWaitTime, ProcessPriorityClass priority)
        {
            m_sResult = new CommandOutput();
            m_oProc = new Process();
            ProcessStartInfo oInfo;
            if (sArgs == null || sArgs == "") oInfo = new ProcessStartInfo(sFilePath);
            else oInfo = new ProcessStartInfo(sFilePath, sArgs);
            if (sWorkingDir != null && sWorkingDir != "")
            {
                oInfo.WorkingDirectory = sWorkingDir;
            }
            oInfo.UseShellExecute = false;
            oInfo.RedirectStandardOutput = true;
            oInfo.RedirectStandardError = true;
            if (sInput != null && sInput != "")
            {
                oInfo.RedirectStandardInput = true;
            }
            m_oProc.StartInfo = oInfo;
            if (m_oProc.Start())
            {
                if (sInput != null && sInput != "")
                {
                    m_oProc.StandardInput.Write(sInput);
                    m_oProc.StandardInput.Close();
                }
                Thread oThread1 = new Thread(new ThreadStart(this.GetError));
                Thread oThread2 = new Thread(new ThreadStart(this.GetOutput));
                oThread1.Start();
                oThread2.Start();
                int nTotal = 0;
                int nPause = 50;
                while (oThread1.IsAlive || oThread2.IsAlive)
                {
                    Thread.Sleep(nPause);
                    nTotal += nPause;
                    if (nTotal > (nWaitTime > 0 ? nWaitTime : (1000 * 60 * 60))) // WE WILL WAIT FOR 3.600.000 milliseconds = 1 hour
                    {
                        if (oThread1.IsAlive) oThread1.Abort();
                        if (oThread2.IsAlive) oThread2.Abort();
                        break;
                    }
                }
                if (m_oProc.HasExited == false)
                {
                    m_oProc.Kill();
                    m_sResult.Output.Add("Error: Hung process terminated ...");
                }
                m_oProc.Close();
                m_oProc.Dispose();
                return m_sResult;
            }
            else return null;
        }
        private void GetError()
        {
            if (m_oProc != null && m_oProc.StandardError != null)
            {
                string sError = "";
                lock (m_oProc.StandardError)
                {
                    sError = m_oProc.StandardError.ReadToEnd();
                }
                if (sError != "")
                {
                    lock (this)
                    {
                        m_sResult.Errors.Add(sError);
                    }
                }
            }
        }
        private void GetOutput()
        {
            if (m_oProc != null && m_oProc.StandardOutput != null)
            {
                string sOutput = "";
                lock (m_oProc.StandardOutput)
                {
                    sOutput = m_oProc.StandardOutput.ReadToEnd();
                }
                if (sOutput != "")
                {
                    lock (this)
                    {
                        m_sResult.Output.Add(sOutput);
                    }
                }
            }
        }
    }
}
