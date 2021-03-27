using Crunchy.Crunchyroll;
using FFmpeg.NET;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crunchy
{
    public partial class MainForm : Form
    {
        private Crunchyroll.Crunchyroll crunchyroll;
        private Dictionary<int, dynamic> Cache;

        public MainForm()
        {
            InitializeComponent();

            Cache = new Dictionary<int, dynamic>();
            crunchyroll = new Crunchyroll.Crunchyroll();
        }

        private async void btnFrmUrl_Click(object sender, EventArgs e)
        {
            Cache.Clear();

            var url = Interaction.InputBox("Please enter a valid crunchyroll URL", "Crunchy");
            Cache.Add(Cache.Count, await crunchyroll.SearchAsync(url.Substring(url.LastIndexOf('/'), (url.Length - url.LastIndexOf('/'))).Replace('-', ' ')));

            var series = Cache[0][0];
            lbSrsTtl.Text = $"{series.Name} ({series.Id})";

            cmbSeasons.Items.Clear();

            Cache.Add(Cache.Count, await crunchyroll.ListSeasonsAsync(series.Id));

            var seasons = Cache[1];
            foreach (SeasonInfo season in seasons)
            {
                cmbSeasons.Items.Add(season);
            }

            cmbSeasons.SelectedIndex = 0;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            if (File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}\\tokens"))
            {
                try
                {
                    crunchyroll.LoadTokens($"{AppDomain.CurrentDomain.BaseDirectory}\\tokens");
                    lbUname.Text = $"{crunchyroll.Tokens.Account.username}  ({crunchyroll.Tokens.Account.user_id} | {crunchyroll.Tokens.Account.premium})";
                    lbUid.Text = crunchyroll.Tokens.SessionId;
                    lbAT.Text = crunchyroll.Tokens.AuthToken;
                    lbATE.Text = crunchyroll.Tokens.AuthExpires.ToString();
                }
                catch (TokenInvalidException ex)
                {
                    switch (ex.ErrorCode)
                    {
                        case 0:
                            await crunchyroll.InitializeAsync(); lbUid.Text = crunchyroll.Tokens.SessionId;
                            break;

                        case 1:
                            using (var loginDiag = new LoginDialog())
                            {
                                if (loginDiag.ShowDialog() == DialogResult.OK)
                                {
                                    await crunchyroll.LoginAsync(loginDiag.Username, loginDiag.Password);
                                }
                            }
                            break;
                    }
                }
            }
            else
            {
                await crunchyroll.InitializeAsync();
                lbUid.Text = crunchyroll.Tokens.SessionId;
            }
        }

        private void btnBrwsPth_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog diag = new FolderBrowserDialog() { ShowNewFolderButton = true })
            {
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = diag.SelectedPath;
                }
            }
        }

        private async void btnLgn_Click(object sender, EventArgs e)
        {
            using (LoginDialog diag = new LoginDialog())
            {
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    await crunchyroll.LoginAsync(diag.Username, diag.Password);

                    lbUname.Text = $"{crunchyroll.Tokens.Account.username}  ({crunchyroll.Tokens.Account.user_id} | {crunchyroll.Tokens.Account.premium})";
                    lbUid.Text = crunchyroll.Tokens.SessionId;
                    lbAT.Text = crunchyroll.Tokens.AuthToken;
                    lbATE.Text = crunchyroll.Tokens.AuthExpires.ToString();

                    crunchyroll.SaveTokens($"{AppDomain.CurrentDomain.BaseDirectory}\\tokens");
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string path = string.Empty;

            if (!checkBox1.Checked)
            {
                var item = (SeasonInfo)cmbSeasons.SelectedItem;
                var series = Cache[0][0];
                path = $"{txtPath.Text}\\{series.Name}\\{item.Name}\\";
                Directory.CreateDirectory(path);

                Cache.Add(Cache.Count, await crunchyroll.ListEpisodesAsync(item));
            }
            else
            {
                var series = (SeriesInfo)Cache[0][0];
                path = $"{txtPath.Text}\\{series.Name}";
                Directory.CreateDirectory(path);

                Cache.Add(Cache.Count, await crunchyroll.ListEpisodesAsync(series));
            }

            var episodes = Cache[2];
            foreach (EpisodeInfo episode in episodes)
            {
                var streaminfo = await crunchyroll.GetStreamInfoAsync(episode);
                var filepath = $"{path}\\[{episode.EpisodeIndex}] {episode.Name.Replace(Path.GetInvalidFileNameChars(), '_')}.mp4";
                var stream = crunchyroll.Tokens.Account.premium != string.Empty
                     ? streaminfo.Streams.FirstOrDefault(a => a.Quality.Equals("ultra"))
                     : streaminfo.Streams.FirstOrDefault(a => a.Quality.Equals("adaptive"));
                var programs = await stream.GetStreamProgramsAsync();

                (string ProgramUrl, Size Quality) program = default;
                switch (cmbQlty.SelectedItem)
                {
                    case "360p":
                        program = programs.FirstOrDefault(a => a.Quality.Width == 360);
                        break;

                    case "480p":
                        program = programs.FirstOrDefault(a => a.Quality.Width == 480);
                        break;

                    case "720p":
                        program = programs.FirstOrDefault(a => a.Quality.Width == 720);
                        break;

                    case "1080p":
                        program = programs.FirstOrDefault(a => a.Quality.Width == 1080);
                        break;

                    case "Best":
                        program = programs.OrderByDescending(a => a.Quality.Width)
                            .FirstOrDefault();

                        break;

                    case "Poorest":
                        program = programs.OrderBy(a => a.Quality.Width)
                            .FirstOrDefault();

                        break;
                }

                var ffmpegPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\ffmpeg.exe";

                Clipboard.SetText(stream.Url);

                lbDownloadProgress.Text = $"Downloading: [{episode.EpisodeIndex}] {episode.Name}";
                await Task.Run(() => CopyStream(ffmpegPath, program.ProgramUrl, filepath));

                lbDownloadProgress.Text = "Complete!";
            }
        }

        private void CopyStream(string ffmpegPath, string input, string output)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo()
                {
                    FileName = ffmpegPath,
                    Arguments = $"-i {InQuotes(input)} -c copy -threads 4 -bsf:a aac_adtstoasc {InQuotes(output)}",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                process.Start();
                process.WaitForExit();
            }
        }

        private string InQuotes(string text)
        {
            return $"\"{text}\"";
        }

        private bool CheckIsNull(object obj)
        {
            return obj != null;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                cmbSeasons.Enabled = false;
            else
                cmbSeasons.Enabled = true;
        }

        private async void btnRfrshTkns_Click(object sender, EventArgs e)
        {
            await crunchyroll.InitializeAsync();

            using (var loginDiag = new LoginDialog())
            {
                if (loginDiag.ShowDialog() == DialogResult.OK)
                {
                    await crunchyroll.LoginAsync(loginDiag.Username, loginDiag.Password);
                }
            }

            lbUname.Text = $"{crunchyroll.Tokens.Account.username}  ({crunchyroll.Tokens.Account.user_id} | {crunchyroll.Tokens.Account.premium})";
            lbUid.Text = crunchyroll.Tokens.SessionId;
            lbAT.Text = crunchyroll.Tokens.AuthToken;
            lbATE.Text = crunchyroll.Tokens.AuthExpires.ToString();

            crunchyroll.SaveTokens($"{AppDomain.CurrentDomain.BaseDirectory}\\tokens");
        }

        private void btnDltTkns_Click(object sender, EventArgs e)
        {
            File.Delete($"{AppDomain.CurrentDomain.BaseDirectory}\\tokens");
        }
    }
}

public static class StringExtensions
{
    public static string Replace(this string text, char[] oldchars, char newchar)
    {
        foreach (var c in oldchars)
            text.Replace(c, newchar);

        return text;
    }
}