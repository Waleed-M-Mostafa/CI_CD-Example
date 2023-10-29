using EventManagementApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EventsController : ControllerBase
  {
    private readonly IEventsRepository _eventsRepository;

    public EventsController(IEventsRepository eventsRepository) => _eventsRepository = eventsRepository;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(IEquatable<Event>))]
    public IActionResult GetAll() => Ok(_eventsRepository.GetAll());

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Event))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
      var existingEvent = _eventsRepository.GetById(id);
      return _eventsRepository != null ? Ok(existingEvent) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Event))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] //, Type = typeof(string)
    public IActionResult Add([FromBody] Event newEvent)
    {
      if (newEvent.Id < 1) return BadRequest("Invalid id");
      _eventsRepository.Add(newEvent);
      return Created("", newEvent);
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Delete(int id)
    {
      try
      {
        _eventsRepository.Delete(id);
      }
      catch(ArgumentException)
      {
        return NotFound();
      }
      return NoContent();
    }
  }
}