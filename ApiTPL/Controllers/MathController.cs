using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTPL.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MathController:ControllerBase
	{
		// GET api/values
		[HttpGet]
		public ActionResult<IEnumerable<string>> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/values/5
		[HttpGet("sin/{num}")]
		public ActionResult<string> Get(double num)
		{
			return "value:" + Math.Sin(num);
		}

		// GET api/values/5
		[HttpGet("cos/num={num}")]
		public ActionResult<string> GetCos(double num)
		{
			return "value:" + Math.Cos(num);
		}

		// POST api/values
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
