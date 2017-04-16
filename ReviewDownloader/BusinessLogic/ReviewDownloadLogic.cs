using AutoMapper;
using FileHelpers;
using ReviewCurator;
using ReviewCurator.Dto;
using ReviewCurator.Service;
using ReviewDownloader.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Hosting;

namespace ReviewDownloader.BusinessLogic
{
    public class ReviewDownloadLogic
    {
        private const string _confMaxResults = "MaxResults";
        private const string _confDownloadTimeoutSeconds = "DownloadTimeoutSeconds";
        private const string _confReviewsFolder = "ReviewsFolder";
        private const string _confReviewsUrl = "ReviewsUrl";

        private const string _confSmtpHost = "SmtpHost";
        private const string _confSmtpFrom = "SmtpFrom";
        private const string _confSmtpPort = "SmtpPort";
        private const string _confSmtpUsername = "SmtpUsername";
        private const string _confSmtpPassword = "SmtpPassword";
        private const string _confSmtpUseSsl = "SmtpUseSsl";
        private const string _confSmtpSender = "SmtpSender";

        private const string _confHomePage = "HomePage";

        private int _enforcedMaximum = Convert.ToInt32(ConfigurationManager.AppSettings[_confMaxResults]);
        private int _downloadTimeoutSeconds = Convert.ToInt32(ConfigurationManager.AppSettings[_confDownloadTimeoutSeconds]);
        private bool isTimeout = false;
        private string reviewsFolder = ConfigurationManager.AppSettings[_confReviewsFolder];
        private string reviewDownloadUrl = ConfigurationManager.AppSettings[_confReviewsUrl];

        public ReviewDownloadResponse DownloadReviews(ReviewDownloadRequest input)
        {
            IReviewService reviewService = null;
            string productUrl = input.ProductUrl != null ? input
                .ProductUrl
                .Trim()
                .Replace("http://", "")
                .Replace("https://", "") : null;

            if (productUrl == null)
                return InvalidUrlResponse;

            if (productUrl.StartsWith("itunes.apple.com"))
                reviewService = new AppleStoreReviewService();

            else if (productUrl.StartsWith("play.google.com"))
                reviewService = new PlayStoreReviewService();

            else if (productUrl.StartsWith("www.amazon.com"))
                reviewService = new AmazonReviewService();

            else
                return InvalidUrlResponse;

            productUrl = $"https://{productUrl}";

            var theReviews = new List<Review>();


            var downloadTimeoutMilliSeconds = _downloadTimeoutSeconds * 1000;
            bool isJobDone = false;
            string errorMessage = null;
            string reviewsFileUrl = null;

            HostingEnvironment.QueueBackgroundWorkItem(ct =>
            {
                {
                    try
                    {
                        reviewService.GetReviewsFromUrl(theReviews, productUrl, 0, false, Math.Min(input.Count, _enforcedMaximum));
                        if (isTimeout)
                        {
                            reviewsFileUrl = GenerateAndSaveReviews(theReviews, reviewService.Name);

                            HostingEnvironment.QueueBackgroundWorkItem(cz =>
                            {
                                SendLinkToUser(input.Email, reviewsFileUrl, reviewService.Name);
                            });
                        }
                    }

                    catch (ReviewDownloadException ex)
                    {
                        //Log: There was an issue while downloading
                        errorMessage = ex.Message;
                    }

                    finally
                    {
                        isJobDone = true;
                    }
                }
            });

            var timer = new System.Timers.Timer(downloadTimeoutMilliSeconds);
            timer.Elapsed += SetTimeoutElasped;
            timer.Start();

            while (!isTimeout && !isJobDone)
            {
                // wait for the first of the above to occur
                System.Threading.Thread.Sleep(1000);
            }

            timer.Close();

            if (isTimeout)
            {
                return new ReviewDownloadResponse
                {
                    FirstFewReviews = theReviews.Take(20).ToList(),
                    ReviewSource = reviewService.Name
                };
            }

            // Generate the CSV
            // put the link in the response
            if (errorMessage != null)
            {
                return new ReviewDownloadResponse
                {
                    ErrorDisplayMessage = errorMessage
                };
            }

            reviewsFileUrl = GenerateAndSaveReviews(theReviews, reviewService.Name);
            return new ReviewDownloadResponse
            {
                FirstFewReviews = theReviews.Take(20).ToList(),
                ReviewsFileUrl = reviewsFileUrl,
                ReviewSource = reviewService.Name
            };
        }

        private string GenerateAndSaveReviews(List<Review> theReviews, string sourceName)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Review, ReviewForCsv>();
            });
            var converted = Mapper.Map<List<ReviewForCsv>>(theReviews);
            var feng = new FileHelperEngine<ReviewForCsv>();
            feng.HeaderText = "Username,Date,Star Rating,Title,Review Comment,Review Link,User Profile Link";
            string filePath = $"{DateTime.Today:dd-MM-yyyy}/{sourceName}-{DateTime.Now:HHmmss}.csv";
            var csvText = feng.WriteString(converted);
            var physicalPath = $"{AppDomain.CurrentDomain.BaseDirectory}{reviewsFolder}/{filePath}";
            Directory.CreateDirectory(Path.GetDirectoryName(physicalPath));
            File.WriteAllText(physicalPath, csvText);
            return $"{reviewDownloadUrl}/{filePath}";
        }

        private void SendLinkToUser(string email, string reviewsFileUrl, string serviceName)
        {
            var from = new MailAddress(ConfigurationManager.AppSettings[_confSmtpFrom], ConfigurationManager.AppSettings[_confSmtpSender]);
            var to = new MailAddress(email);
            MailMessage mail = new MailMessage(from, to);

            SmtpClient client = new SmtpClient();
            client.Port = Convert.ToInt32(ConfigurationManager.AppSettings[_confSmtpPort]);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings[_confSmtpUseSsl]);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings[_confSmtpUsername], ConfigurationManager.AppSettings[_confSmtpPassword]);
            client.Host = ConfigurationManager.AppSettings[_confSmtpHost];
            mail.Subject = $"Your {serviceName} reviews are ready";
            mail.IsBodyHtml = true;
            mail.Body = $"Dear User,<br/><br/>Download the requested {serviceName} reviews from {reviewsFileUrl}." +
                $"<br/><br/>Thank you for using the <a href='{ConfigurationManager.AppSettings[_confHomePage]}'>Review Downloader Service</a>.";
            client.Send(mail);
        }

        private void SetTimeoutElasped(object sender, EventArgs e)
        {
            this.isTimeout = true;
        }

        private ReviewDownloadResponse InvalidUrlResponse
        {
            get
            {
                return new ReviewDownloadResponse
                {
                    ErrorDisplayMessage = "Sorry. Only www.amazon.com, play.google.com and itunes.apple.com reviews are supported at the moment"
                };
            }
        }
    }
}