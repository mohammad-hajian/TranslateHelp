using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TranslateHelp.Models;

namespace TranslateHelp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult GetFile()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetFile2(List<IFormFile> files)
        {
            try
            {
                if (files.Count == 0)
                {
                    return BadRequest("چیزی برنداشتی");
                }
                else
                {
                        string str = string.Empty;
                        string strp = string.Empty;
                        if (files[0].Length > 0)
                        {
                            //تبدیل استریم به متن
                            Stream stream1 = files[0].OpenReadStream();
                            StreamReader text = new StreamReader(stream1);
                            while ((strp = text.ReadLine()) != null)
                            {
                                if (strp.StartsWith("00:") || strp.StartsWith("01:") || strp.StartsWith("02:"))
                                {
                                    strp = null;
                                }
                                str += strp + "\n";
                            }

                            int formFileLength = files[0].FileName.Split('.').Length;
                            string formFileName = string.Empty;
                            for (int i = 0; i < formFileLength - 1; i++)
                            {
                                formFileName += files[0].FileName.Split('.')[i];
                                if (formFileLength - i > 2)
                                {
                                    formFileName += ".";
                                }
                            }
                            formFileName += "_2" + "." + files[0].FileName.Split('.').Last();

                            //        //ساخت پوشه و ریپلیس کردن
                            //        try
                            //        {
                            //            if (!Directory.Exists(@"d:\TranslatePool\start\"))
                            //            {
                            //                Directory.CreateDirectory(@"d:\TranslatePool\start\");
                            //            }
                            //            System.IO.File.Delete(@"d:\TranslatePool\start\" + formFileName);
                            //        }
                            //        catch { }

                            //        StreamWriter oSW = new StreamWriter(@"d:\TranslatePool\start\" + formFileName, true, encoding: Encoding.UTF8);
                            //        try
                            //        {
                            //            oSW.Write(str);
                            //        }
                            //        catch (Exception)
                            //        {
                            //            if (oSW != null)
                            //            {
                            //                oSW.Close();
                            //                oSW.Dispose();
                            //            }
                            //        }
                            //        finally
                            //        {
                            //            if (oSW != null)
                            //            {
                            //                oSW.Close();
                            //                oSW.Dispose();
                            //            }
                            //        }


                            //تبدیل متن به استریم برای دانلود
                            byte[] data = Encoding.UTF8.GetBytes(str);
                            MemoryStream stm = new MemoryStream(data, 0, data.Length);
                            return File(stm, "har/har", fileDownloadName: formFileName);

                        }
                        else
                        {
                            return BadRequest("فایل خالی نفرست");
                        }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost]
        public IActionResult GetFile3(List<IFormFile> files)
        {
            try
            {
                if (files.Count != 2)
                {
                    return BadRequest("فقط دو تا بردار");
                }
                else
                {
                    string strpBase = string.Empty;
                    string strTranslated = string.Empty;
                    string strpTranslated = string.Empty;

                    IFormFile baseFile = null;
                    IFormFile translatedFile = null;
                    if (files[0].Length > 0 && files[1].Length > 0)
                    {



                        Stream stream = files[0].OpenReadStream();
                        StreamReader text = new StreamReader(stream);
                        if (text.ReadToEnd().Contains("\n00:"))
                        {
                            baseFile = files[0];
                            translatedFile = files[1];
                        }
                        else
                        {
                            baseFile = files[1];
                            translatedFile = files[0];
                        }



                        Stream streamBase = baseFile.OpenReadStream();
                        StreamReader textBase = new StreamReader(streamBase);
                        Stream streamTranslated = translatedFile.OpenReadStream();
                        StreamReader textTranslated = new StreamReader(streamTranslated);
                        while ((strpBase = textBase.ReadLine()) != null && (strpTranslated = textTranslated.ReadLine()) != null)
                        {
                            if (strpBase.StartsWith("00:") || strpBase.StartsWith("01:") || strpBase.StartsWith("02:"))
                            {
                                strpTranslated = strpBase;
                            }
                            strTranslated += strpTranslated + "\n";
                        }

                        //ساخت پوشه و ریپلیس کردن
                        //try
                        //{
                        //    if (!Directory.Exists(@"d:\TranslatePool\end\"))
                        //    {
                        //        Directory.CreateDirectory(@"d:\TranslatePool\end\");
                        //    }
                        //    System.IO.File.Delete(@"d:\TranslatePool\end\" + translatedFile.FileName);
                        //}
                        //catch { }

                        //StreamWriter oSW = new StreamWriter(@"d:\TranslatePool\end\" + translatedFile.FileName, true, encoding: Encoding.UTF8);
                        //try
                        //{
                        //    oSW.Write(strTranslated);
                        //}
                        //catch (Exception)
                        //{
                        //    if (oSW != null)
                        //    {
                        //        oSW.Close();
                        //        oSW.Dispose();
                        //    }
                        //}
                        //finally
                        //{
                        //    if (oSW != null)
                        //    {
                        //        oSW.Close();
                        //        oSW.Dispose();
                        //    }
                        //}

                        //تبدیل متن به استریم برای دانلود
                        byte[] data = Encoding.UTF8.GetBytes(strTranslated);
                        MemoryStream stm = new MemoryStream(data, 0, data.Length);
                        return File(stm, "har/har", fileDownloadName: baseFile.FileName);
                    }
                    else
                    {
                        return BadRequest("فایل خالی نفرست");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
