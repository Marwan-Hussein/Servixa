using System.Text;

namespace Servixa.Services.Helpers
{
    public static class EmailTemplateHelper
    {
        public static string BuildOtpTemplate(string displayName, string otpCode, DateTime expiresAt)
        {
            var greetingName = string.IsNullOrWhiteSpace(displayName) ? "there" : displayName;
            var expiryText = expiresAt.ToLocalTime().ToString("hh:mm tt");

            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Servixa verification code</title>
</head>
<body style='margin: 0; padding: 0; background-color: #f4f7fb; font-family: -apple-system, BlinkMacSystemFont, ""Segoe UI"", Roboto, Helvetica, Arial, sans-serif; color: #172033;'>
    <div style='padding: 36px 16px; background-color: #f4f7fb;'>
        <div style='max-width: 560px; margin: 0 auto; background: #ffffff; border: 1px solid #e5ebf3; border-radius: 18px; overflow: hidden; box-shadow: 0 18px 45px rgba(23, 32, 51, 0.08);'>
            <div style='background: linear-gradient(135deg, #0f766e 0%, #2563eb 100%); padding: 30px 28px;'>
                <div style='font-size: 28px; font-weight: 800; color: #ffffff; letter-spacing: 0.5px;'>Servixa</div>
                <div style='margin-top: 8px; color: #dbeafe; font-size: 14px;'>Secure email verification</div>
            </div>
            <div style='padding: 34px 28px 30px;'>
                <h1 style='font-size: 24px; line-height: 1.25; margin: 0 0 12px; color: #111827;'>Confirm your email</h1>
                <p style='font-size: 15px; line-height: 1.7; color: #526174; margin: 0 0 24px;'>Hi {greetingName}, use this one-time code to finish setting up your Servixa account.</p>
                <div style='background: #eef6ff; border: 1px solid #cfe4ff; border-radius: 16px; padding: 24px; text-align: center; margin-bottom: 22px;'>
                    <div style='font-size: 12px; font-weight: 700; letter-spacing: 1.8px; text-transform: uppercase; color: #2563eb; margin-bottom: 10px;'>Verification Code</div>
                    <div style='font-size: 38px; line-height: 1; font-weight: 800; letter-spacing: 10px; color: #0f172a;'>{otpCode}</div>
                </div>
                <p style='font-size: 14px; line-height: 1.6; color: #64748b; margin: 0;'>This code expires in 10 minutes, around <strong style='color: #172033;'>{expiryText}</strong>. If you did not create a Servixa account, you can safely ignore this email.</p>
            </div>
            <div style='background: #f8fafc; border-top: 1px solid #e5ebf3; padding: 18px 28px; text-align: center;'>
                <p style='font-size: 12px; color: #8a97a8; margin: 0;'>This is an automated message from Servixa. Please do not reply directly.</p>
                <p style='font-size: 12px; color: #8a97a8; margin: 8px 0 0;'>&copy; {DateTime.UtcNow.Year} Servixa. All rights reserved.</p>
            </div>
        </div>
    </div>
</body>
</html>";
        }

        public static string BuildHtmlTemplate(string title, string body, Dictionary<string, string> details)
        {
            var detailsRowsBuilder = new StringBuilder();
            if (details != null && details.Count > 0)
            {
                foreach (var detail in details)
                {
                    detailsRowsBuilder.Append($@"
                        <tr>
                            <td style='padding: 8px 0; color: #4b5563; font-weight: 500; font-size: 14px; width: 40%; vertical-align: top;'>{detail.Key}</td>
                            <td style='padding: 8px 0; color: #111827; font-weight: 600; font-size: 14px; text-align: right; vertical-align: top;'>{detail.Value}</td>
                        </tr>");
                }
            }

            string detailsSection = "";
            if (detailsRowsBuilder.Length > 0)
            {
                detailsSection = $@"
                    <div style='background-color: #f9fafb; border-radius: 12px; padding: 20px; border: 1px solid #f3f4f6; margin-bottom: 24px; text-align: left;'>
                        <h3 style='font-size: 12px; font-weight: 700; text-transform: uppercase; letter-spacing: 1px; color: #9ca3af; margin-top: 0; margin-bottom: 16px; border-bottom: 1px solid #e5e7eb; padding-bottom: 8px;'>Details</h3>
                        <table style='width: 100%; border-collapse: collapse;'>
                            <tbody>
                                {detailsRowsBuilder}
                            </tbody>
                        </table>
                    </div>";
            }

            var formattedBody = body.Replace("\n", "<br/>");

            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{title}</title>
</head>
<body style='margin: 0; padding: 0; background-color: #f3f4f6; font-family: -apple-system, BlinkMacSystemFont, ""Segoe UI"", Roboto, Helvetica, Arial, sans-serif; -webkit-font-smoothing: antialiased;'>
    <div style='background-color: #f3f4f6; padding: 40px 16px; min-height: 100%;'>
        <div style='max-width: 580px; margin: 0 auto; background-color: #ffffff; border-radius: 16px; overflow: hidden; box-shadow: 0 10px 15px -3px rgba(0,0,0,0.05), 0 4px 6px -2px rgba(0,0,0,0.05); border: 1px solid #e5e7eb;'>
            <div style='background: linear-gradient(135deg, #4f46e5 0%, #6366f1 100%); padding: 32px 24px; text-align: center;'>
                <span style='font-size: 28px; font-weight: 800; color: #ffffff; letter-spacing: 1.5px; display: inline-block; text-transform: uppercase;'>Servixa</span>
                <div style='margin-top: 6px; color: #e0e7ff; font-size: 13px; font-weight: 500; letter-spacing: 0.5px;'>Your trusted home services partner</div>
            </div>
            <div style='padding: 40px 32px; text-align: center;'>
                <h2 style='font-size: 22px; font-weight: 700; color: #111827; margin-top: 0; margin-bottom: 20px; line-height: 1.3;'>{title}</h2>
                <div style='font-size: 15px; line-height: 1.6; color: #4b5563; margin-bottom: 28px; text-align: left;'>
                    {formattedBody}
                </div>
                {detailsSection}
                <div style='margin-top: 32px; border-top: 1px solid #f3f4f6; padding-top: 24px; text-align: center;'>
                    <p style='font-size: 13px; color: #9ca3af; margin: 0;'>Have questions? Contact our support team at <a href='mailto:support@servixa.com' style='color: #4f46e5; text-decoration: none; font-weight: 500;'>support@servixa.com</a></p>
                </div>
            </div>
            <div style='background-color: #f9fafb; padding: 24px 32px; text-align: center; border-top: 1px solid #e5e7eb;'>
                <p style='font-size: 12px; color: #9ca3af; margin: 0 0 6px 0;'>This is an automated notification from Servixa. Please do not reply directly to this email.</p>
                <p style='font-size: 12px; color: #9ca3af; margin: 0;'>&copy; {DateTime.UtcNow.Year} Servixa. All rights reserved.</p>
            </div>
        </div>
    </div>
</body>
</html>";
        }
    }
}
