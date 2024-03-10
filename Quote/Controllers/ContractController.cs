using Microsoft.AspNetCore.Mvc;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal.request;
using System.Diagnostics.Contracts;
using Quote.Models;

namespace Quote.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractController : ControllerBase
    {
        private readonly IRequestService _requestService;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public ContractController(IRequestService requestService, IWebHostEnvironment webHostEnvironment)
        {
            _requestService = requestService;
            _webHostEnvironment = webHostEnvironment;
        }
        private string GetContractFile(string code)
        {
            return this._webHostEnvironment.WebRootPath + "\\Upload\\contract\\" + code;
        }
        [NonAction]
        private string GetContractPath(int contractId, string fileName)
        {
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            return hosturl + "\\Upload\\contract\\" + contractId + "/" + fileName;
        }
        [HttpPost("CreateContract")]
        public async Task<ActionResult<Models.Contract>> CreateContract([FromForm] CreateContractModel Contractdata)
        {
            //if (userId == null)
            //{
            //    return Unauthorized();
            //}

            var contract = new Models.Contract
            {
                RequestId = Contractdata.RequestId,
                ConPrice = Contractdata.ConPrice,
                FinalPrice = Contractdata.FinalPrice,
                // Status = Contractdata.Status,
                //ContractFile =Contractdata.ContractFile,
                //Phone = requestdata.Phone,
            };

            var contractres = await _requestService.CreateContractUser(contract);
             string Filepath = GetContractFile(contractres.ContractId.ToString());
            if (!Directory.Exists(Filepath))
            {
                Directory.CreateDirectory(Filepath);
            }
            var fileName = Contractdata.ContractFile.FileName;
            var docPath = Filepath + "\\" + fileName;

            if (System.IO.File.Exists(docPath))
            {
                System.IO.File.Delete(docPath);
            }
            using (FileStream stream = System.IO.File.Create(docPath))
            {
                await Contractdata.ContractFile.CopyToAsync(stream);
                string docxFile = GetContractPath(contractres.ContractId, fileName);

                //contractres.ContractFile = contractres;
                var contdata =  await _requestService.UpdateContractUser(contractres);
                return Ok(docxFile);
            }

        }
    }
}
