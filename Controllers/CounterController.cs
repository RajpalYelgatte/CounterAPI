using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CounterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CounterController : ControllerBase
    {
        [HttpGet()]
        public int GetIncrementCounter()
        {
            //read exising counter values from 'CounterData.json' file.
            string json = System.IO.File.ReadAllText(@"CounterData.json");
            Counter counter = JsonConvert.DeserializeObject<Counter>(json);
            counter.id++;

            //Update counter values by 1 and update 'CounterData.json' file.
            string updatedCounter = JsonConvert.SerializeObject(counter);
            System.IO.File.WriteAllText(@"CounterData.json", updatedCounter);

            return counter.id;
        }
    }

    public class Counter
    {
        public int id { get; set; }
    }
}
