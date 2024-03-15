using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using Quote.Helper;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Models;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Quote.Controllers
{
    public class StaffController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductService _productService;
        private readonly IOptionService _optionService;
        private readonly IImageService _imageService;
        private readonly UserInterface _userService;
        private readonly IMapper _mapper;
        public StaffController(IWebHostEnvironment webHostEnvironment, IProductService productService, IOptionService optionService, IImageService imageService, IMapper mapper, UserInterface userService)
        {
            _webHostEnvironment = webHostEnvironment;
            _productService = productService;
            _optionService = optionService;
            _imageService = imageService;
            _mapper = mapper;
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductModal product)
        {
            ApiResponse res = new ApiResponse();
            try

            {
                var newPro = new Product()
                {
                    ProductName = product.ProductName,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                    Price = product.Price,
                    IsDelete = product.isDelete,
                };
                var id = await _productService.AddProduct(newPro);
                res.Result = id.ToString();
                string Filepath = GetFilepath(id.ToString());
                if (!Directory.Exists(Filepath))
                {
                    Directory.CreateDirectory(Filepath);
                }
                foreach (var image in product.Imagess)
                {

                    var base64Data = Regex.Match(image.base24, @"data:image\/[a-zA-Z]+;base64,(?<data>.+)").Groups["data"].Value;
                    var imageBytes = Convert.FromBase64String(base64Data);
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        var fileName = Guid.NewGuid().ToString() + ".jpeg";
                        var filePath = Path.Combine(Filepath, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            ms.CopyTo(fileStream);

                        }
                        var img = new Models.Image()
                        {
                            ProductId = id,
                            Description = image.description,
                            ImagePath = GetImageProductPath(id, fileName),

                        };
                        await _imageService.AddImage(img);


                    }
                }
                foreach (var opt in product.Options)
                {
                    var newOpt = new Option()
                    {
                        ProductId = id,
                        OptionName = opt.OptionName,
                        Quantity = int.Parse(opt.OptionQuantity),
                    };
                    await _optionService.AddOptions(newOpt);
                }

            }
            catch (Exception ex)
            {
                res.Errormessage = ex.Message;
            }

            return Ok("Success");
        }

        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            try
            {
                var allPro = await _productService.GetAllProduct();

                var productWithCate = new List<ProductAllResponse>();
                foreach (var cate in allPro)
                {
                    var imgs = await _productService.GetImageAsync();
                    var img = imgs.Where(i => i.ProductId == cate.ProductId).FirstOrDefault();
                    var productCate = new ProductAllResponse
                    {
                        ProductId = cate.ProductId,
                        ImagePath = img?.ImagePath,
                        ProductName = cate.ProductName
                    };
                    productWithCate.Add(productCate);
                }
                return Ok(productWithCate);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetProductById/{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            try
            {
                var pro = await _productService.GetProductId(productId);
                var img = await _imageService.GetImgById(productId);
                var opt = await _optionService.GetOptionById(productId);

                ResProduct res = new ResProduct();

                if (pro != null && img != null && opt != null)
                {
                    res.Product = pro;
                    res.Images = img;
                    res.options = opt;
                    return Ok(res);
                }
                return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [NonAction]
        private string GetFilepath(string code)
        {
            return this._webHostEnvironment.WebRootPath + "//Upload//product//" + code;
        }
        [NonAction]
        private string GetImageProductPath(int productId, string fileName)
        {
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            return hosturl + "//Upload//product//" + productId + "/" + fileName;

        }

        [HttpDelete("Delete_Product")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            await _productService.DeleteProduct(productId);

            return Ok("Delete Product Success");



        }

        //[HttpPut("Update_Product")]

        //public async Task<IActionResult> UpdateProduct(ProductModal product, int productId)
        //{
        //    ApiResponse res = new ApiResponse();
        //    int passcount = 0;
        //    int errorcount = 0;
        //    int maxImageCount = 6;
        //    try
        //    {
        //        var pro = await _productService.UpdateProduct(productId, product);
        //        if (pro != null)
        //        {
        //            if (product.formFiles == null || product.formFiles.Length == 0)
        //            {
        //                await _productService.Save();

        //                return Ok("Add product successfully");
        //            }
        //            else
        //            {

        //                string Filepath = GetFilepath(pro.ProductId.ToString());
        //                if (System.IO.Directory.Exists(Filepath))
        //                {
        //                    DirectoryInfo directoryInfo = new DirectoryInfo(Filepath);
        //                    FileInfo[] fileInfos = directoryInfo.GetFiles();
        //                    foreach (FileInfo fileInfo in fileInfos)
        //                    {
        //                        fileInfo.Delete();
        //                    }

        //                }
        //                var imagesToRemove = await _imageService.DeleteImg(productId);
        //                string Filepath1 = GetFilepath(pro.ProductId.ToString());
        //                if (!Directory.Exists(Filepath1))
        //                {
        //                    Directory.CreateDirectory(Filepath1);
        //                }
        //                int imageCount = 0;

        //                foreach (var file in product.formFiles)
        //                {
        //                    if (imageCount >= maxImageCount)
        //                    {
        //                        break;
        //                    }

        //                    var img = new Models.Image()
        //                    {
        //                        ProductId = pro.ProductId,
        //                        Description = product.Description,
        //                        ImagePath = GetImageProductPath(pro.ProductId, file.FileName),

        //            };
        //            string imagepath = Path.Combine(Filepath, file.FileName);
        //            if (System.IO.File.Exists(imagepath))
        //            {
        //                System.IO.File.Delete(imagepath);
        //            }
        //            using (FileStream stream = System.IO.File.Create(imagepath))
        //            {
        //                await file.CopyToAsync(stream);
        //                passcount++;
        //            }
        //            await _imageService.AddImage(img);
        //            imageCount++;
        //        }
        //        res.Result = "Update Success";
        //    }
        //}
        //        }
        //        catch (Exception ex)
        //        {
        //            errorcount++;
        //            res.Errormessage = ex.Message;
        //        }
        //        res.ResponseCode = 200;


        //        return Ok(res);


    }

}
