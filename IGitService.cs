public interface IGitService
{
    Task<List<string>> ListClusters();
    Task<Cluster> GetCluster(string clusterName);
}