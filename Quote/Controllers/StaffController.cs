﻿using AutoMapper;
using Azure.Core;
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
        private readonly IMapper _mapper;
        public StaffController(IWebHostEnvironment webHostEnvironment, IProductService productService, IOptionService optionService, IImageService imageService, IMapper mapper)
        {
            _webHostEnvironment = webHostEnvironment;
            _productService = productService;
            _optionService = optionService;
            _imageService = imageService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductModal product)
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
                            Description = product.Description,
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

            return Ok();
        }

        [NonAction]
        private string GetFilepath(string code)
        {
            return this._webHostEnvironment.WebRootPath + "\\Upload\\product\\" + code;
        }
        [NonAction]
        private string GetImageProductPath(int productId, string fileName)
        {
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            return hosturl + "\\Upload\\product\\" + productId + "/" + fileName;

        }

        [HttpPost("AddOptions/{productID}")]
        public async Task<IActionResult> AddOptions([FromBody] Options[] options, int productID)
        {
            int count = 0;
            foreach (var opt in options)
            {
                var newOpt = new Option()
                {
                    ProductId = productID,
                    OptionName = opt.OptionName,
                    Quantity = int.Parse(opt.OptionQuantity),
                };
                await _optionService.AddOptions(newOpt);
                count++;
            }
            return Ok(count);
        }
        [HttpDelete("Delete_Product")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var pro = await _productService.DeleteProduct(productId);
            if (pro != null)
            {
                return Ok("Delete Product Success");

            }
            else { return BadRequest(); }

        }

        [HttpPut("Update_Options/{productId}")]
        public async Task<IActionResult> Update_Option(int productId, [FromBody] Options[] options)
        {
            try
            {
                var opt = await _optionService.Update(productId, options);
                if (opt != null)
                {
                    return Ok(opt);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

        //                    };
        //                    string imagepath = Path.Combine(Filepath, file.FileName);
        //                    if (System.IO.File.Exists(imagepath))
        //                    {
        //                        System.IO.File.Delete(imagepath);
        //                    }
        //                    using (FileStream stream = System.IO.File.Create(imagepath))
        //                    {
        //                        await file.CopyToAsync(stream);
        //                        passcount++;
        //                    }
        //                    await _imageService.AddImage(img);
        //                    imageCount++;
        //                }
        //                res.Result = "Update Success";
        //            }
        //        }
        //                }
        //                catch (Exception ex)
        //                {
        //                    errorcount++;
        //                    res.Errormessage = ex.Message;
        //                }
        //                res.ResponseCode = 200;


        //                return Ok(res);
        //            }
    }
}