using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace CounterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CounterController : ControllerBase
    {
        private readonly ICounterProcessor _counterDataSource;

        public CounterController(ICounterProcessor counterDataSource)
        {
            _counterDataSource = counterDataSource;
        }

        [HttpGet()]
        public async Task<ActionResult<int>> GetIncrementCounter()
        {
            try
            {
                //read exising counter values from 'CounterData.json' file.
                Counter counter = await _counterDataSource.GetCounterAsync();
                counter.id++;

                //Update counter values by 1 and update 'CounterData.json' file.
                await _counterDataSource.UpdateCounterAsync(counter);

                return counter.id;
            }
            catch (Exception ex)
            {
                // Log the error message
                Console.WriteLine($"An error occurred while updating the counter: {ex.Message}");
                return StatusCode(500);
            }
        }
    }

    public class Counter
    {
        public int id { get; set; }
    }

    public interface ICounterProcessor
    {
        Task<Counter> GetCounterAsync();
        Task UpdateCounterAsync(Counter counter);
    }

    public class CounterProcessor : ICounterProcessor
    {
        private readonly string _filePath= @"CounterData.json";

        public async Task<Counter> GetCounterAsync()
        {
            string json = await System.IO.File.ReadAllTextAsync(_filePath);
            return JsonConvert.DeserializeObject<Counter>(json);
        }

        public async Task UpdateCounterAsync(Counter counter)
        {
            string updatedCounter = JsonConvert.SerializeObject(counter);

            await System.IO.File.WriteAllTextAsync(_filePath, updatedCounter);
        }
    }
}
