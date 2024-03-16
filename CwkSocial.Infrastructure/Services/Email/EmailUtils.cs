
using MimeKit;

namespace CwkSocial.Infrastructure.Services.Email;

internal static class EmailUtils
{
    public static MimeEntity GenerateConfirmEmailMessageBody(string url)
    {
        var builder = new BodyBuilder();

        var image = "https://github.com/Faris-Abuali/Digital-Hippo/blob/main/public/hippo-email-sent.png?raw=true";

        var text = @$"<h1>Welcome to CwkSocial</h1>
            
                <p>In order to proceed with your registration, please click the link below to confirm your email address:</p>

                <p>
                    <a href=""{url}"">
                        <button 
                            style='background-color: #4CAF50; /* Green */
                            border: none;
                            color: white;
                            padding: 15px 32px;
                            text-align: center;
                            text-decoration: none;
                            display: inline-block;
                            font-size: 16px;
                            margin: 4px 2px;
                            cursor: pointer;'
                        >
                            Confirm Email
                        </button>
                    </a>
                </p>

                <center>
                    <img src=""{image}"" width=""200"" height=""auto"">
                </center>
            ";

        builder.HtmlBody = text;

        return builder.ToMessageBody();
    }
}
