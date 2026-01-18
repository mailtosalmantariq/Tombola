using Microsoft.AspNetCore.Mvc;
using MT.Beans.API.Service.BeanDayService;
using MT.Beans.API.Service.BeansServices;
using MT.Tombola.Api.Data.Models;

namespace MT.Beans.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BeansController : ControllerBase
    {
        private readonly IBeanService _beanService;
        private readonly IBeanOfTheDayService _beanOfTheDayService;

        public BeansController(
            IBeanService beanService,
            IBeanOfTheDayService beanOfTheDayService)
        {
            _beanService = beanService;
            _beanOfTheDayService = beanOfTheDayService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Bean>>> GetAll([FromQuery] string? search)
        {
            try
            {
                var beans = await _beanService.GetAllAsync(search);
                return Ok(beans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to load beans. {ex.Message}");
            }
        }

        [HttpGet("{externalId}")]
        public async Task<ActionResult<Bean>> Get(string externalId)
        {
            try
            {
                var bean = await _beanService.GetByExternalIdAsync(externalId);
                return bean is null ? NotFound() : Ok(bean);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to load bean '{externalId}'. {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Bean>> Create(Bean bean)
        {
            try
            {
                var created = await _beanService.CreateAsync(bean);
                return CreatedAtAction(nameof(Get), new { externalId = created.ExternalId }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to create bean. {ex.Message}");
            }
        }

        [HttpPut("{externalId}")]
        public async Task<IActionResult> Update(string externalId, [FromBody] Bean bean)
        {
            try
            {
                await _beanService.UpdateAsync(externalId, bean);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update bean '{externalId}'. {ex.Message}");
            }
        }

        [HttpDelete("{externalId}")]
        public async Task<IActionResult> Delete(string externalId)
        {
            try
            {
                await _beanService.DeleteAsync(externalId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to delete bean '{externalId}'. {ex.Message}");
            }
        }

        [HttpGet("bean-of-the-day")]
        public async Task<ActionResult<Bean>> GetBeanOfTheDay()
        {
            try
            {
                var bean = await _beanOfTheDayService.GetTodayAsync();
                return Ok(bean);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to load Bean of the Day. {ex.Message}");
            }
        }
    }
}
