using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

public class GitService : IGitService
{
    private readonly HttpClient _httpClient;
    private readonly string _repoOwner = "TejasW03";
    private readonly string _repoName = "Git-Based-Configuration-Management";
    private readonly string _branch = "main";
    private readonly string _githubToken = "";
    private readonly string _baseApiUrl;

    public GitService(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _githubToken);

        _baseApiUrl = $"https://api.github.com/repos/{_repoOwner}/{_repoName}/contents/cluster-definitions";
    }

    public async Task<List<string>> ListClusters()
    {
        try
        {
            var response = await _httpClient.GetStringAsync(_baseApiUrl);
            var files = JsonSerializer.Deserialize<List<GitHubFile>>(
                response,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return files?
                .Where(file => file.Name.EndsWith(".yaml", StringComparison.OrdinalIgnoreCase))
                .Select(file => file.Name)
                .ToList() ?? new List<string>();
        }
        catch (HttpRequestException ex)
        {
            throw new GitServiceException("Failed to fetch cluster list", ex);
        }
    }

    public async Task<Cluster> GetCluster(string clusterName)
    {
        if (string.IsNullOrEmpty(clusterName))
        {
            throw new ArgumentNullException(nameof(clusterName));
        }

        try
        {
            string fileUrl = $"https://raw.githubusercontent.com/{_repoOwner}/{_repoName}/{_branch}/cluster-definitions/{clusterName}.yaml";
            string yamlContent = await _httpClient.GetStringAsync(fileUrl);
            return Cluster.FromYaml(yamlContent);
        }
        catch (HttpRequestException ex)
        {
            throw new GitServiceException($"Failed to fetch cluster {clusterName}", ex);
        }
    }
}