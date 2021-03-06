﻿using Kasim.Core.BLL.FileUploadWebApp;
using Kasim.Core.Common;
using Kasim.Core.FileUploadWebApp.Models;
using Kasim.Core.IBLL.FileUploadWebApp;
using Kasim.Core.Model.FileUploadWebApp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kasim.Core.FileUploadWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ConnectionStringOptions _conns;
        private readonly IHostingEnvironment _hostingEnvironment;
        private IFileBLL bll = null;
        public HomeController(ILogger<HomeController> logger, IOptions<ConnectionStringOptions> connsOptions, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _conns = connsOptions.Value;
            _hostingEnvironment = hostingEnvironment;
            bll = new FileBLL(_conns);
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "图片上传";
            if (Request != null && !string.IsNullOrEmpty(Request.QueryString.Value))
            {
                var req = Request.QueryString.Value.Remove(0, 1).Trim();
                var json = MySecurity.SDecryptString(req, "yss.yh");
                var _fileMode = JsonConvert.DeserializeObject<FileModel>(json);
                ViewData["Message"] = _fileMode.Message;
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
         
        public IActionResult Show(string id)
        {
            var json = MySecurity.SDecryptString(id, "yss.yh");
            var fileModel = JsonConvert.DeserializeObject<FileModel>(json);
            return View(fileModel);
        }

        /// <summary>
        /// 异步上传写入数据库时会重复写入相同数据，无法解决
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> FileSaveAsync()
        {
            FileModel fileMode = null;
            if (!string.IsNullOrEmpty(Request.Form["uid"]))
            {
                var req = Request.Form["uid"].ToString().Remove(0, 1).Trim();
                var json = MySecurity.SDecryptString(req, "yss.yh");
                fileMode = JsonConvert.DeserializeObject<FileModel>(json);
                fileMode.FileList = new List<Model.FileUploadWebApp.File>();
            }
            else
            {
                _logger.LogError("非法参数");
                return BadRequest("非法参数");
            }
            var now = DateTime.Now;
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            try
            {
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        string fileExt = FileOperate.GetPostfixStr(formFile.FileName).Remove(0, 1); //文件扩展名，不含“.”
                        long fileSize = formFile.Length; //获得文件大小，以字节为单位
                        string newFileName = Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名
                        var foldPath = "upload/" + now.Year + "/" + now.Month + "/" + now.Day;
                        var filePath = Path.Combine(webRootPath, foldPath, newFileName);
                        FileOperate.FolderCreate(Path.Combine(webRootPath, foldPath));
                        fileMode.FileList.Add(new Model.FileUploadWebApp.File
                        {
                            Name = newFileName,
                            Url = Path.Combine(foldPath, newFileName),
                            Time = now.ToString("yyyy-MM-dd HH:mm:ss"),
                            FullPath = filePath
                            //Md5 = MD5.GetFileMd5(filePath)
                        });
                        //fileMode = bll.AddFiles(fileMode);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Ok(new { count = files.Count, size, fileMode });
        }

        [HttpPost]
        public IActionResult FileSave()
        {
            FileModel fileMode = null;
            if (!string.IsNullOrEmpty(Request.Form["uid"]))
            {
                var req = Request.Form["uid"].ToString().Remove(0, 1).Trim();
                var json = MySecurity.SDecryptString(req, "yss.yh");
                fileMode = JsonConvert.DeserializeObject<FileModel>(json);
                fileMode.FileList = new List<Model.FileUploadWebApp.File>();
            }
            else
            {
                _logger.LogError("非法参数");
                return BadRequest("非法参数");
            }
            try
            {
                var now = DateTime.Now;
                var files = Request.Form.Files;
                long size = files.Sum(f => f.Length);
                string webRootPath = _hostingEnvironment.WebRootPath;
                string contentRootPath = _hostingEnvironment.ContentRootPath;
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        string fileExt = FileOperate.GetPostfixStr(formFile.FileName).Remove(0, 1); //文件扩展名，不含“.”
                        long fileSize = formFile.Length; //获得文件大小，以字节为单位
                        string newFileName = Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名
                        var foldPath = "upload/" + now.Year + "/" + now.Month + "/" + now.Day;
                        var filePath = Path.Combine(webRootPath, foldPath, newFileName);
                        FileOperate.FolderCreate(Path.Combine(webRootPath, foldPath));
                        fileMode.FileList.Add(new Model.FileUploadWebApp.File
                        {
                            Name = newFileName,
                            Url = Path.Combine(foldPath, newFileName),
                            Time = now.ToString("yyyy-MM-dd HH:mm:ss"),
                            FullPath = filePath
                            //Md5 = MD5.GetFileMd5(filePath)
                        });
                        //fileMode = bll.AddFiles(fileMode);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            formFile.CopyTo(stream);
                        }
                    }
                }
                fileMode = bll.AddFiles(fileMode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Ok(new { list = fileMode.FileList });
            //return Ok(new { count = files.Count, size, fileMode });
        }

        [HttpPost]
        public IActionResult SaveData()
        {
            var json = Request.Form["uid"];
            var fileMode = JsonConvert.DeserializeObject<FileModel>(json);
            try
            {
                fileMode = bll.AddFiles(fileMode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Ok(new { list = fileMode.FileList });
        }

        [HttpPost]
        public IActionResult GetData()
        {
            var json = MySecurity.SDecryptString(Request.Form["uid"], "yss.yh");
            var fileModel = JsonConvert.DeserializeObject<FileModel>(json);
            try
            {
                fileModel = bll.GetFiles(fileModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Ok(new { fileModel });
        }

        [HttpPost]
        public IActionResult Preview()
        {
            var json = MySecurity.SDecryptString(Request.Form["uid"], "yss.yh");
            var fileModel = JsonConvert.DeserializeObject<FileModel>(json);
            fileModel = bll.GetFiles(fileModel);
            return Ok(new { fileModel });
        }           
    }
}