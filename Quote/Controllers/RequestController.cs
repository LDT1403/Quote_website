using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Modal.request;
using Quote.Models;
using Quote.Services;
using System.Threading.Tasks;

namespace Quote.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IMailService _mailService;
        private readonly IProductService _productService;
        private readonly ITaskInterface _taskInterface;
        private readonly UserInterface _userInterface;

        public RequestController(IRequestService requestService, IMailService mailService, IProductService productService, UserInterface userInterface, ITaskInterface taskInterface)
        {
            _requestService = requestService;
            _mailService = mailService;
            _productService = productService;
            _userInterface = userInterface;
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
                Date = requestdata.Date,
                Status = requestdata.Status,
                UserId = requestdata.UserId,
                Phone = requestdata.Phone,
                UserName = requestdata.UserName,
                DateCre = DateTime.Now,
            };
            var requestItem = await _requestService.CreateRequestUser(request);
            var productItem = await _productService.GetProductId((int)requestItem.ProductId);
            var toEmail = requestItem.Email;

            var emailBody = $@"<div><h3>HỆ THỐNG ĐANG SẮP XẾP NHÂN VIÊN KHẢO SÁT</h3> 
                          <div>
                              <h3>Thông Tin Yêu Cầu</h3>    
                              <span>Tên Người Nhận: </span> <strong>{requestItem.UserName}</strong><br>
                              <span>Số Điện Thoại: </span> <strong>{requestItem.Phone:n0}</strong><br>
                              <span>Địa Chỉ Căn Hộ: </span> <strong>{requestItem.Address}</strong><br>
                              <span>Ngày Khảo Sát: </span> <strong>{requestItem.Date}</strong><br>

                                <h3>Danh Mục Bạn Đã Chọn </h3> 
                              <span>Tên Sản Phẩm: </span> <strong>{productItem.ProductName}</strong><br>
                              
                          </div>
                          <p>Xin trân trọng cảm ơn</p>
                      </div>";
            var mailRequest = new MailRequest()
            {
                ToEmail = toEmail,
                Subject = "[GOAT INTERIOR] CHÚNG TÔI ĐÃ NHẬN ĐƯỢC YÊU CẦU CỦA BẠN!",
                Body = emailBody
            };
            await _mailService.SendEmailAsync(mailRequest);
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
                    var listRe = list.Where(r => r.UserId == data.Id &&( r.Status == "2" || r.Status == "3") ).ToList();
                    foreach (var item in listRe)
                    {
                        var dataPro = await _productService.GetProductId((int)item.ProductId);
                        var proCate = await _productService.GetCategoryIdAsync((int)dataPro.CategoryId);
                        var proimg = await _productService.GetImageAsync();
                        var thumb = proimg.Where(i => i.ProductId == dataPro.ProductId).FirstOrDefault();
                        var taskL = _taskInterface.GetTasks().Result.Where(t => t.RequestId == item.RequestId).FirstOrDefault();
                        var staff = await _userInterface.GetUserIDAsync((int)taskL.UserId);
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
                                staffId = (int)staff.UserId
                            },
                            ContracData = new RequestContrac(),

                        };
                        dataResponse.Add(dataAdd);
                    }
                    return Ok(dataResponse);
                }
                else
                if (data.Status == "0")
                {
                    var dataResponse = new List<RequestStatusResqonse>();
                    var list = await _requestService.GetAllRequest();
                    var listRe = list.Where(r => r.UserId == data.Id).ToList();
                    foreach (var item in listRe)
                    {
                        var dataPro = await _productService.GetProductId((int)item.ProductId);
                        var proCate = await _productService.GetCategoryIdAsync((int)dataPro.CategoryId);
                        var proimg = await _productService.GetImageAsync();
                        var thumb = proimg.Where(i => i.ProductId == dataPro.ProductId).FirstOrDefault();
                        var taskL = await _taskInterface.GetTasks();
                        var taskData = taskL.Where(t => t.RequestId == item.RequestId && t.Status == "1").FirstOrDefault();
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
                        var taskData = taskL.Where(t => t.RequestId == item.RequestId && t.Status == "2").FirstOrDefault();
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
                                userPhone = item.Phone
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
                        var taskData = taskL.Where(t => t.RequestId == item.RequestId && t.Status == "2").FirstOrDefault();
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
        [HttpPost("ConfirmAppointment")]
        public async Task<ActionResult> ConfirmAppointment(int requestId, int staffId)
        {
            try
            {
                var user = await _userInterface.GetUserIDAsync(staffId);
                if(user == null)
                {
                    return NotFound();
                }
                else
                {
                    user.Status = "1";
                    await _userInterface.UpdateStatusStaff(user);
                }
                var request = await _requestService.Appoinment(requestId);
                if (request == null)
                {
                    return NotFound();

                }
                Models.Task newTask = new Models.Task();
                newTask.RequestId = request.RequestId; ;
                newTask.UserId = staffId;
                newTask.Status = "1";
                newTask.Location = request.Address;
                var item = await _taskInterface.CreateTasks(newTask);
                var staff = await _userInterface.GetUserIDAsync((int)item.UserId);
                var dataPro = await _productService.GetProductId((int)request.ProductId);
                var toEmail = request.Email;
                var emailBody = $@"<div><h3>THÔNG TIN LỊCH KHẢO SÁT </h3> 
                          <div>
                              <h3>Thông Tin Của Bạn </h3>    
                              <span>Tên Người Gửi: </span> <strong>{request.UserName}</strong><br>
                              <span>Số Điện Thoại: </span> <strong>{request.Phone:n0}</strong><br>
                                <br>

                                <h3>Thông Tin Nhân Viên Khảo Sát </h3> 
                              <span>Tên Nhân Viên: </span> <strong>{staff.UserName}</strong><br>
                              <span>Số Điện Thoại: </span> <strong>{staff.Phone:n0}</strong><br>
                                <span>Email: </span> <strong>{staff.Email}</strong><br>
                                <br>
                                <h3>Danh Mục Bạn Đã Chọn </h3> 
                              <span>Tên Sản Phẩm: </span> <strong>{dataPro.ProductName}</strong><br>
                                <br>
                            <h3>Thông Tin Khảo Sát </h3> 
                              <span>Địa Chỉ Khảo Sát: </span> <strong>{request.Address}</strong><br>
                              <span>Ngày Khảo Sát : </span> <strong>{request.Date}</strong><br>
                              
                          </div>
                          <p>Xin trân trọng cảm ơn</p>
                      </div>";
                var mailRequest = new MailRequest()
                {
                    ToEmail = toEmail,
                    Subject = "[GOAT INTERIOR] CHÚNG TÔI ĐÃ NHẬN ĐƯỢC YÊU CẦU CỦA BẠN!",
                    Body = emailBody
                };
                await _mailService.SendEmailAsync(mailRequest);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
