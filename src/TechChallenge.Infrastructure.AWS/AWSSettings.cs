using TechChallenge.Infrastructure.AWS.Cognito.Configuration;

namespace TechChallenge.Infrastructure.AWS;

public class AwsSettings
{
    public string Region { get; set; } = string.Empty;
    public string AccessKeyId { get; set; } = string.Empty;
    public string SecretAccessKey { get; set; } = string.Empty;
    public string SessionToken { get; set; } = string.Empty;
}
