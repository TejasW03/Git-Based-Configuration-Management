using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class GitController : ControllerBase
{
    private readonly IGitService _gitService;

    public GitController(IGitService gitService)
    {
        _gitService = gitService ?? throw new ArgumentNullException(nameof(gitService));
    }

    [HttpGet("clusters")]
    public async Task<ActionResult<IEnumerable<string>>> GetClusters()
    {
        try
        {
            var clusters = await _gitService.ListClusters();
            return Ok(clusters);
        }
        catch (GitServiceException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("clusters/{name}")]
    public async Task<ActionResult<Cluster>> GetCluster(string name)
    {
        try
        {
            var cluster = await _gitService.GetCluster(name);
            if (cluster == null)
            {
                return NotFound();
            }
            return Ok(cluster);
        }
        catch (GitServiceException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}