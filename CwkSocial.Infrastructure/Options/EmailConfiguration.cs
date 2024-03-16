using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Infrastructure.Options;

public class EmailConfiguration
{
    public string From { get; init; } = null!;
    public string SmtpServer { get; init; } = null!;
    public int Port { get; init; }
    public string UserName { get; init; } = null!;
    public string Password { get; init; } = null!;
}
