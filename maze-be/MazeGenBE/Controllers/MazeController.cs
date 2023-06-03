using MazeGenBE.Logic;
using MazeGenBE.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[ApiController]
[Route("[controller]")]
public class MazeController : ControllerBase
{
    private readonly MazeGenerator mazeGenerator;

    public MazeController()
    {
        mazeGenerator = new MazeGenerator();
    }

    [HttpGet]
    [Route("generate")]
    public ActionResult<Maze> GenerateMaze(int rows, int columns, string? seed = null)
    {
        Maze maze = mazeGenerator.GenerateMaze(rows, columns, seed);
        if(maze != null)
        {
            return Ok(maze);
        }
        else
        {
            return BadRequest("The maze could not be generated");
        }
    }

    [HttpPost]
    [Route("solve")]
    public ActionResult<Maze> SolveMaze([FromBody] Maze maze)
    {
        MazeSolver solver = new MazeSolver(maze);
        if (solver.SolveMaze())
        {
            return Ok(solver.maze);
        }
        else
        {
            return BadRequest("The maze could not be solved");
        }
    }
}
