using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading;

namespace IsTheMicInUse
{
    public class Poller
    {
        private int _sleepSeconds;
        private string _tasmotaHostname;

        public void PollMicrophone()
        {
            initialize();
            MicrophoneInfo helper = new MicrophoneInfo();
            bool turnedOffOnce = false;

            while (true)
            {
                if (helper.IsAnyMicInUse())
                {
                    clearTasmotaBacklog(); // we get flickering if we do not clear the backlog first
                    turnOnTasmota();
                    turnedOffOnce = false;
                }
                else
                {
                    if (!turnedOffOnce)
                    {
                        turnOffTasmota();
                        turnedOffOnce = true;
                    }
                }

                Thread.Sleep(_sleepSeconds * 1000);
            }
        }

        private void initialize()
        {
            if (!int.TryParse(ConfigurationManager.AppSettings["CheckIntervalSeconds"], out int sleepSeconds))
            {
                sleepSeconds = 5;
            }
            this._sleepSeconds = sleepSeconds;
            this._tasmotaHostname = ConfigurationManager.AppSettings["TasmotaHostname"] ?? "";
        }

        private void clearTasmotaBacklog()
        {
            string commandUrl = String.Format($"http://{_tasmotaHostname}/cm?cmnd=Backlog");
            sendRequest(commandUrl);
        }

        private void turnOnTasmota()
        {
            string turnPowerOnWithTimeout = Uri.EscapeDataString("Backlog Power1 ON; Delay 600; Power1 OFF"); // delay is in 0.1 second increment
            string commandUrl = String.Format($"http://{_tasmotaHostname}/cm?cmnd={turnPowerOnWithTimeout}");
            sendRequest(commandUrl);
        }

        private void turnOffTasmota()
        {   
            string commandUrl = String.Format($"http://{_tasmotaHostname}/cm?cmnd=Power1%20OFF");
            sendRequest(commandUrl);
        }

        private void sendRequest(string commandUrl)
        {
            WebRequest request = WebRequest.Create(commandUrl);
            request.Method = "POST";
            request.Timeout = 5000;

            try
            {
                WebResponse response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    // make sure request processing has finished before sending next command:
                    reader.ReadToEnd();
                }
            }
            catch(Exception e)
            {
                var notification = new System.Windows.Forms.NotifyIcon()
                {
                    Visible = true,
                    Icon = System.Drawing.SystemIcons.Information,
                    //BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info,
                    BalloonTipTitle = e.GetType().Name,
                    BalloonTipText = e.Message,
                };
                notification.ShowBalloonTip(5000);
                Thread.Sleep(5000);
                notification.Dispose();
            }
        }
    }
}
