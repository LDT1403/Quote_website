using Microsoft.AspNetCore.Mvc;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Modal.request;
using Quote.Models;

namespace Quote.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IProductService _productService;
        private readonly ITaskInterface _taskInterface;
        private readonly UserInterface _userInterface;
        public RequestController(IRequestService requestService, IProductService productService, ITaskInterface taskInterface)
        {
            _requestService = requestService;
            _productService = productService;
            _taskInterface = taskInterface;
        }

        [HttpPost("CreateRequest")]
        public async Task<ActionResult<Request>> CreateRequest([FromBody] CreateRequestModel requestdata)
        {

            if (requestdata.UserId == null)
            {
                return Unauthorized();
            }
            var request = new Request
            {
                Address = requestdata.Address,
                Email = requestdata.Email,
                ProductId = requestdata.ProductId,
                Date = DateTime.Parse(requestdata.Date),
                Status = requestdata.Status,
                UserId = requestdata.UserId,
                Phone = requestdata.Phone,
                UserName = requestdata.UserName,
            };
            var requestItem = await _requestService.CreateRequestUser(request);
            return Ok("Success");
        }
        [HttpPost("GetRequestStatus")]
        public async Task<ActionResult> GetRequestStatus([FromBody] GetRequestStatusModal data)
        {
            try
            {
                if (data.Status == "1")
                {
                    var dataResponse = new List<RequestStatusResqonse>();
                    var list = await _requestService.GetAllRequest();
                    var listRe = list.Where(r => r.UserId == data.Id && r.Status == data.Status).ToList();
                    foreach (var item in listRe)
                    {
                        var dataPro = await _productService.GetProductId((int)item.ProductId);
                        var proCate = await _productService.GetCategoryIdAsync((int)dataPro.CategoryId);
                        var proimg = await _productService.GetImageAsync();
                        var thumb = proimg.Where(i => i.ProductId == dataPro.ProductId).FirstOrDefault();
                        var dataAdd = new RequestStatusResqonse
                        {
                            address = item.Address,
                            dateSurvey = item.Date.ToString(),
                            requestId = item.RequestId,
                            status = "Đang Xử Lí",
                            UserData = new RequestUser
                            {
                                userEmail = item.Email,
                                userName = item.UserName,
                                userId = (int)item.UserId,
                                userPhone = item.Phone,
                            },
                            ProdcuctData = new RequestProdcuct
                            {
                                productId = dataPro.ProductId,
                                productCate = proCate.CateName,
                                productName = dataPro.ProductName,
                                productPrice = dataPro.Price,
                                productThumbnail = thumb.ImagePath
                            },
                            ContracData = new RequestContrac(),
                            StaffData = new RequestStaff()



                        };
                        dataResponse.Add(dataAdd);

                    }
                    return Ok(dataResponse);
                }
                else
                if (data.Status == "2")
                {
                    var dataResponse = new List<RequestStatusResqonse>();
                    var list = await _requestService.GetAllRequest();
                    var listRe = list.Where(r => r.UserId == data.Id && r.Status == data.Status).ToList();
                    foreach (var item in listRe)
                    {
                        var dataPro = await _productService.GetProductId((int)item.ProductId);
                        var proCate = await _productService.GetCategoryIdAsync((int)dataPro.CategoryId);
                        var proimg = await _productService.GetImageAsync();
                        var thumb = proimg.Where(i => i.ProductId == dataPro.ProductId).FirstOrDefault();
                        var taskL = await _taskInterface.GetTasks();
                        var taskData = taskL.Where(t => t.RequestId == item.RequestId && t.Status == "0").FirstOrDefault();
                        var staff = await _userInterface.GetUserIDAsync((int)taskData.UserId);
                        var dataAdd = new RequestStatusResqonse
                        {
                            address = item.Address,
                            dateSurvey = item.Date.ToString(),
                            requestId = item.RequestId,
                            status = "Chờ Khảo Sát",
                            UserData = new RequestUser
                            {
                                userEmail = item.Email,
                                userName = item.UserName,
                                userId = (int)item.UserId,
                                userPhone = item.Phone,
                            },
                            ProdcuctData = new RequestProdcuct
                            {
                                productId = dataPro.ProductId,
                                productCate = proCate.CateName,
                                productName = dataPro.ProductName,
                                productPrice = dataPro.Price,
                                productThumbnail = thumb.ImagePath
                            },
                            StaffData = new RequestStaff
                            {
                                staffEmail = staff.Email,
                                staffName = staff.UserName,
                                staffPhone = staff.Phone,
                                staffId = (int)taskData.UserId
                            },
                            ContracData = new RequestContrac(),

                        };
                        dataResponse.Add(dataAdd);
                    }
                    return Ok(dataResponse);
                }
                else
                if (data.Status == "3")
                {
                    var dataResponse = new List<RequestStatusResqonse>();
                    var list = await _requestService.GetAllRequest();
                    var listRe = list.Where(r => r.UserId == data.Id && r.Status == data.Status).ToList();
                    foreach (var item in listRe)
                    {
                        var dataPro = await _productService.GetProductId((int)item.ProductId);
                        var proCate = await _productService.GetCategoryIdAsync((int)dataPro.CategoryId);
                        var proimg = await _productService.GetImageAsync();
                        var thumb = proimg.Where(i => i.ProductId == dataPro.ProductId).FirstOrDefault();
                        var taskL = await _taskInterface.GetTasks();
                        var taskData = taskL.Where(t => t.RequestId == item.RequestId && t.Status == "0").FirstOrDefault();
                        var staff = await _userInterface.GetUserIDAsync((int)taskData.UserId);
                        var dataAdd = new RequestStatusResqonse
                        {
                            address = item.Address,
                            dateSurvey = item.Date.ToString(),
                            requestId = item.RequestId,
                            status = "Chờ Báo Giá",
                            UserData = new RequestUser
                            {
                                userEmail = item.Email,
                                userName = item.UserName,
                                userId = (int)item.UserId,
                                userPhone = item.Phone,
                            },
                            ProdcuctData = new RequestProdcuct
                            {
                                productId = dataPro.ProductId,
                                productCate = proCate.CateName,
                                productName = dataPro.ProductName,
                                productPrice = dataPro.Price,
                                productThumbnail = thumb.ImagePath
                            },
                            StaffData = new RequestStaff
                            {
                                staffEmail = staff.Email,
                                staffName = staff.UserName,
                                staffPhone = staff.Phone,
                                staffId = (int)taskData.UserId
                            },
                            ContracData = new RequestContrac(),

                        };
                        dataResponse.Add(dataAdd);
                    }
                    return Ok(dataResponse);
                }
                else if (data.Status == "4")
                {
                    var dataResponse = new List<RequestStatusResqonse>();
                    var list = await _requestService.GetAllRequest();
                    var listRe = list.Where(r => r.UserId == data.Id && r.Status == data.Status).ToList();
                    foreach (var item in listRe)
                    {
                        var dataPro = await _productService.GetProductId((int)item.ProductId);
                        var proCate = await _productService.GetCategoryIdAsync((int)dataPro.CategoryId);
                        var proimg = await _productService.GetImageAsync();
                        var thumb = proimg.Where(i => i.ProductId == dataPro.ProductId).FirstOrDefault();
                        var taskL = await _taskInterface.GetTasks();
                        var taskData = taskL.Where(t => t.RequestId == item.RequestId && t.Status == "0").FirstOrDefault();
                        var staff = await _userInterface.GetUserIDAsync((int)taskData.UserId);
                        var contract = await _requestService.GetContract();
                        var contractData = contract.Where(c => c.RequestId == item.RequestId).FirstOrDefault();
                        var dataAdd = new RequestStatusResqonse
                        {
                            address = item.Address,
                            dateSurvey = item.Date.ToString(),
                            requestId = item.RequestId,
                            status = "Đã Báo Giá",
                            UserData = new RequestUser
                            {
                                userEmail = item.Email,
                                userName = item.UserName,
                                userId = (int)item.UserId,
                                userPhone = item.Phone,
                            },
                            ProdcuctData = new RequestProdcuct
                            {
                                productId = dataPro.ProductId,
                                productCate = proCate.CateName,
                                productName = dataPro.ProductName,
                                productPrice = dataPro.Price,
                                productThumbnail = thumb.ImagePath
                            },
                            StaffData = new RequestStaff
                            {
                                staffEmail = staff.Email,
                                staffName = staff.UserName,
                                staffPhone = staff.Phone,
                                staffId = (int)taskData.UserId
                            },
                            ContracData = new RequestContrac
                            {
                                contractId = contractData.ContractId,
                                priceProduct = contractData.FinalPrice,
                                priceConstruc = contractData.ConPrice,
                                contracFile = contractData.ContractFile
                            },

                        };
                        dataResponse.Add(dataAdd);
                    }
                    return Ok(dataResponse);
                }
                else if (data.Status == "5")
                {
                    var dataResponse = new List<RequestStatusResqonse>();
                    var list = await _requestService.GetAllRequest();
                    var listRe = list.Where(r => r.UserId == data.Id && r.Status == data.Status).ToList();
                    foreach (var item in listRe)
                    {
                        var dataPro = await _productService.GetProductId((int)item.ProductId);
                        var proCate = await _productService.GetCategoryIdAsync((int)dataPro.CategoryId);
                        var proimg = await _productService.GetImageAsync();
                        var thumb = proimg.Where(i => i.ProductId == dataPro.ProductId).FirstOrDefault();
                        var taskL = await _taskInterface.GetTasks();
                        var taskData = taskL.Where(t => t.RequestId == item.RequestId && t.Status == "0").FirstOrDefault();
                        var staff = await _userInterface.GetUserIDAsync((int)taskData.UserId);
                        var contract = await _requestService.GetContract();
                        var contractData = contract.Where(c => c.RequestId == item.RequestId).FirstOrDefault();
                        var dataAdd = new RequestStatusResqonse
                        {
                            address = item.Address,
                            dateSurvey = item.Date.ToString(),
                            requestId = item.RequestId,
                            status = "Đã Đặt Cọc",
                            UserData = new RequestUser
                            {
                                userEmail = item.Email,
                                userName = item.UserName,
                                userId = (int)item.UserId,
                                userPhone = item.Phone,
                            },
                            ProdcuctData = new RequestProdcuct
                            {
                                productId = dataPro.ProductId,
                                productCate = proCate.CateName,
                                productName = dataPro.ProductName,
                                productPrice = dataPro.Price,
                                productThumbnail = thumb.ImagePath
                            },
                            StaffData = new RequestStaff
                            {
                                staffEmail = staff.Email,
                                staffName = staff.UserName,
                                staffPhone = staff.Phone,
                                staffId = (int)taskData.UserId
                            },
                            ContracData = new RequestContrac
                            {
                                contractId = contractData.ContractId,
                                priceProduct = contractData.FinalPrice,
                                priceConstruc = contractData.ConPrice,
                                contracFile = contractData.ContractFile,
                                status = contractData.Status,
                            },

                        };
                        dataResponse.Add(dataAdd);
                    }
                    return Ok(dataResponse);
                }

                return Ok("false");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllRequest")]
        public async Task<ActionResult> GetAllRequest()
        {
            try
            {
                var list = await _requestService.GetAllRequest();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("ConfirmAppointment/{requestId}")]
        public async Task<ActionResult> ConfirmAppointment(int requestId)
        {
            try
            {
                var re = await _requestService.Appoinment(requestId);
                if (re == null)
                {
                    return NotFound();
                }
                return Ok(re);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
