using Microsoft.AspNetCore.Mvc;
using FuelManagementApi.Dtos;
using FuelManagementApi.Data;
using FuelManagementApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuelManagementApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FuelController : ControllerBase
    {
        private readonly FMDbContext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FuelController(FMDbContext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _dbcontext = dbcontext;
            _webHostEnvironment = webHostEnvironment;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public IActionResult GetFuelList()
        {
            var list = _dbcontext.fuels.ToList();
            //var groupList = list.GroupBy(m => m.VehicleName).ToList();
            ResponceModel responce = new ResponceModel();
            responce.Message = "List";
            responce.Status = true;
            responce.Model = list;
            return Ok(responce);
        }
        
        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFuelById(int id)
        {
            var model = await _dbcontext.fuels.FindAsync(id);
            ResponceModel responce = new ResponceModel();
            responce.Message = "Successfully";
            responce.Status = true;
            responce.Model = model;
            return Ok(responce);
        }
        [HttpGet("{Name}")]
        public async Task<IActionResult> SearchFuelByVehicleName(string Name)
        {
            var model = await _dbcontext.fuels.Where(m => ((!string.IsNullOrEmpty(Name) && Name != "0") ? m.VehicleName == Name : true)).ToListAsync();
            ResponceModel responce = new ResponceModel();
            responce.Message = "Successfully";
            responce.Status = true;
            responce.Model = model;
            return Ok(responce);
        }

        // POST api/<ValuesController>

        //[Consumes("multipart/form-data")]
        [HttpPost]
        public async Task<IActionResult> AddFuel([FromBody] FuelModel fuelModel)
        {
            try
            {
                if(fuelModel.Id == 0)
                {
                    Fuel fuel = new Fuel();
                    fuel.Amount = fuelModel.Amount;
                    fuel.VehicleName = fuelModel.VehicleName;
                    fuel.RefillDate = fuelModel.RefillDate;
                    fuel.Image = fuelModel.Image != null ? UploadedFile(fuelModel.Image) : "";
                    _dbcontext.Add(fuel);
                    _dbcontext.SaveChanges();
                }
                else
                {
                    var model = await _dbcontext.fuels.FindAsync(fuelModel.Id);
                    model.Amount = fuelModel.Amount;
                    model.VehicleName = fuelModel.VehicleName;
                    model.RefillDate = fuelModel.RefillDate;
                    model.Image = fuelModel.Image != null ? UploadedFile(fuelModel.Image) : "";
                    _dbcontext.Update(model);
                    _dbcontext.SaveChanges();
                }

                ResponceModel responce = new ResponceModel();
                responce.Message = "Added Successfully";
                responce.Status = true;
                responce.Model = null;
                return Ok(responce);
            }
            catch (Exception ex)
            {
                ResponceModel responce = new ResponceModel();
                responce.Message = "Some Error occurs";
                responce.Status = false;
                responce.Model = null;
                return Ok(responce);
            }
        }

        //// PUT api/<ValuesController>/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> EditFuel(int id, [FromBody] string value)
        //{
        //    try
        //    {
        //        Fuel fuel = new Fuel();
        //        fuel.Amount = fuelModel.Amount;
        //        fuel.VehicleName = fuelModel.VehicleName;
        //        fuel.RefillDate = fuelModel.RefillDate;
        //        fuel.Image = fuelModel.Image != null ? UploadedFile(fuelModel.Image) : "";
        //        _dbcontext.Add(fuel);
        //        _dbcontext.SaveChanges();

        //        ResponceModel responce = new ResponceModel();
        //        responce.Message = "Added Successfully";
        //        responce.Status = true;
        //        responce.Model = null;
        //        return Ok(responce);
        //    }
        //    catch (Exception ex)
        //    {
        //        ResponceModel responce = new ResponceModel();
        //        responce.Message = "Some Error occurs";
        //        responce.Status = false;
        //        responce.Model = null;
        //        return Ok(responce);
        //    }
        //}

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuel(int id)
        {
            var model = await _dbcontext.fuels.FindAsync(id);
            _dbcontext.fuels.Remove(model);
            _dbcontext.SaveChanges();
            ResponceModel responce = new ResponceModel();
            responce.Message = "Successfully";
            responce.Status = true;
            responce.Model = null;
            return Ok(responce);
        }

        private string UploadedFile(IFormFile file)
        {
            string uniqueFileName = "";
            string ext = "";
            if (file != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");
                ext = System.IO.Path.GetExtension(file.FileName);
                uniqueFileName = Guid.NewGuid().ToString();
                string filePath = Path.Combine(uploadsFolder, string.Format(uniqueFileName.ToString() + ext));
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return string.Format(uniqueFileName.ToString() + ext);
        }
    }
}
